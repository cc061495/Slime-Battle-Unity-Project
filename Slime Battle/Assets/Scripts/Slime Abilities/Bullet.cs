/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Bullet : MonoBehaviour{

    private SlimeHealth tarHealth;
    private Transform target, _transform;
    private float attackDamage, effectAreaRadius, slowDownPrecentage;

    private float bulletSpeed = 20f;
    private bool slowEffect;
    public GameObject impactEffect;

    public void Seek(Transform _target, float _attackDamage, SlimeHealth health){
        target = _target;
        tarHealth = health;
        attackDamage = _attackDamage;
        _transform = transform;
    }

    public void Seek(Transform _target, float _attackDamage, float _effectAreaRadius, float _slowDownPrecentage){
        target = _target;
        _transform = transform;
        attackDamage = _attackDamage;
        effectAreaRadius = _effectAreaRadius;
        slowDownPrecentage = _slowDownPrecentage;
        slowEffect = true;
    }
    // Update is called once per frame
    void Update(){
        if (target == null){
            Destroy(gameObject);
            return;
        }

        Vector3 dir;
        dir.x = target.position.x - _transform.position.x;
        dir.y = target.position.y - _transform.position.y;
        dir.z = target.position.z - _transform.position.z;

        float distanceThisFrame = bulletSpeed * Time.deltaTime;
        float dirMagnitude = dir.x * dir.x +
                             dir.y * dir.y +
                             dir.z * dir.z;

        if (dirMagnitude <= distanceThisFrame*distanceThisFrame){
            HitTarget();
            return;
        }
        
        float sqrtMagnitude = Mathf.Sqrt(dirMagnitude);
        Vector3 dirNormalized;
        dirNormalized.x = (dir.x / sqrtMagnitude) * distanceThisFrame;
        dirNormalized.y = (dir.y / sqrtMagnitude) * distanceThisFrame;
        dirNormalized.z = (dir.z / sqrtMagnitude) * distanceThisFrame;

        _transform.Translate(dirNormalized, Space.World);
    }

    private void HitTarget(){
        GameObject effectIns = (GameObject)Instantiate(impactEffect, _transform.position, _transform.rotation);
        Destroy(effectIns, 0.5f);
        Destroy(gameObject);

        if(PhotonNetwork.isMasterClient){
            /* Slow Effect */
            if(slowEffect)
                SlowDownEffect();
            else
		        tarHealth.TakeDamage(attackDamage);
        }
    }

    private void SlowDownEffect(){
		List<Transform> enemyTeam = GameManager.Instance.GetEnemies(transform)
										.Where(x => DistanceCalculate(target.position, x.position) <= effectAreaRadius*effectAreaRadius).ToList();

		for(int i=0;i<enemyTeam.Count;i++){
            Transform e = enemyTeam[i].root;
            SlimeMovement move = e.GetComponent<SlimeMovement>();
            if(move != null)
                move.ChangeTheMovementSpeed(slowDownPrecentage);
			e.GetComponent<SlimeHealth>().TakeDamage(attackDamage);
		}
    }
    
    private float DistanceCalculate(Vector3 pos1, Vector3 pos2){
		Vector3 distance;
		distance.x = pos1.x - pos2.x;
		distance.y = pos1.y - pos2.y;
		distance.z = pos1.z - pos2.z;

		float magnitude = distance.x * distance.x +
						  distance.y * distance.y +
						  distance.z * distance.z;
		return magnitude;
	}
}