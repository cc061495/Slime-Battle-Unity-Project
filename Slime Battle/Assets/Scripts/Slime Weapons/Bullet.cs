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

    void HitTarget(){
        GameObject effectIns = (GameObject)Instantiate(impactEffect, _transform.position, _transform.rotation);
        Destroy(effectIns, 0.5f);
        Destroy(gameObject);
        
        if(PhotonNetwork.isMasterClient)
		    tarHealth.TakeDamage(attackDamage);
    }
}