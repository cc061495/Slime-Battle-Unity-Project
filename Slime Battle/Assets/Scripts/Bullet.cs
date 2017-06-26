using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Transform target;
    private float attackDamage;

    private float bulletSpeed = 20f;
    public GameObject impactEffect;

    public void Seek(Transform _target, float _attackDamage)
    {
        target = _target;
        attackDamage = _attackDamage;
    }
    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = bulletSpeed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 1f);
        Destroy(gameObject);

        Slime e = target.GetComponent<Slime>();
        e.health -= attackDamage;
        e.healthBar.fillAmount = e.health / e.startHealth;
    }
}