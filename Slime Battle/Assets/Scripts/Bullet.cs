using UnityEngine;

public class Bullet : MonoBehaviour{

    private SlimeHealth tarHealth;
    private Transform target, _transform;
    private float attackDamage;

    private float bulletSpeed = 20f;
    public GameObject impactEffect;

    public void Seek(Transform _target, float _attackDamage, SlimeHealth health){
        target = _target;
        attackDamage = _attackDamage;
        tarHealth = health;
        _transform = transform;
    }
    // Update is called once per frame
    void Update(){
        if (target == null){
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - _transform.position;
        float distanceThisFrame = bulletSpeed * GameManager.globalDeltaTime;

        if (dir.magnitude <= distanceThisFrame){
            HitTarget();
            return;
        }

        _transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget(){
        GameObject effectIns = (GameObject)Instantiate(impactEffect, _transform.position, _transform.rotation);
        Destroy(effectIns, 0.5f);
        Destroy(gameObject);
        
        if(PhotonNetwork.isMasterClient)
		    tarHealth.TakeDamage(attackDamage);
    }
}