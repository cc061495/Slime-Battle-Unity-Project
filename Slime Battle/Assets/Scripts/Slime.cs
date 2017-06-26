using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Slime : MonoBehaviour{
	public Transform target;
	public Transform firePoint;
	public RectTransform healthBarPos;
	 
	[Header("Slime Status")]
	public float startHealth;
	public float attackDamage;
	[Tooltip("How fast the slime attack in one second")]
	public float attackSpeed;
	public float movemonetSpeed;
	public float attackRange;
	[Space]
	[Header("Attack Mode")]
	public bool meleeAttack;
	[Space]
	public bool rangedAttack;
	public GameObject rangedWeaponPrefab;
	[Space]
	[Header("Unity Stuff")]
	public Image healthBar;

	public float health;
	private float turnSpeed = 3f;
	private float countDown = 0f;
	private NavMeshAgent agent;
	private Vector3 nextPos;
	private float myColRadius;
	private float tarColRadius;

	void Awake(){
		myColRadius = GetComponent<CapsuleCollider> ().radius;
		agent = GetComponent<NavMeshAgent> ();
		agent.speed = movemonetSpeed;
		agent.acceleration = movemonetSpeed;
		agent.stoppingDistance = attackRange;
		health = startHealth;
	}

	// Update is called once per frame
	void Update(){
		UpdateHealthBarPos();	//health bar position

		if (GameManager.Instance.currentState == GameManager.State.battle_start) {	//Pressed the "Start" Button, start to run
			if (health <= 0) {
				Destroy (transform.parent.gameObject);
				return;
			}

			if (target == null){
				UpdateTarget ();
			}
			else{
				LookToTarget ();
				tarColRadius = target.GetComponent<CapsuleCollider> ().radius;
				float range = myColRadius + tarColRadius + attackRange;
				float dist = Vector3.Distance (transform.position, target.position);

				if (dist > range) {
					nextPos = target.position;
				} else {
					nextPos = transform.position;
					Attack ();
				}
				agent.SetDestination (nextPos);
			}
		}
	}

	void UpdateTarget(){
		GameObject[] enemies = (gameObject.tag == "Team_RED") ? (GameManager.Instance.team_blue) : (GameManager.Instance.team_red);

		if (enemies.Length > 0) {
			float shortestDistance = Mathf.Infinity;
			GameObject nearestEnemy = null;

			foreach (GameObject enemy in enemies) {
				if (enemy != null) {
					float distanceToEnemy = Vector3.Distance (transform.position, enemy.transform.position);
					if (distanceToEnemy < shortestDistance) {
						shortestDistance = distanceToEnemy;
						nearestEnemy = enemy;
					}
				}
			}
			target = (nearestEnemy != null) ? (nearestEnemy.transform) : (null);
		}
	}
		
	void LookToTarget(){
		Vector3 dir = target.position - transform.position;
		if (dir != Vector3.zero) {
			Quaternion lookRotation = Quaternion.LookRotation (dir);
			Vector3 rotation = Quaternion.Lerp (transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
			transform.rotation = Quaternion.Euler (0f, rotation.y, 0f);
		}
	}

	void UpdateHealthBarPos(){
		if(transform.hasChanged)
			healthBarPos.position = new Vector3 (transform.position.x, transform.position.y + 2f, transform.position.z + 1f);
	}

	void Attack(){
		if (countDown <= 0f) {
			if (meleeAttack)
				MeleeAttack ();
			else if (rangedAttack)
				RangedAttack ();

			countDown = 1f / attackSpeed;
		}
		countDown -= Time.deltaTime;

	}
		
	void MeleeAttack(){
		Slime e = target.GetComponent<Slime> ();
		e.health -= attackDamage;
		e.healthBar.fillAmount = e.health / e.startHealth;
	}

	void RangedAttack(){
		GameObject bulletGO = (GameObject)Instantiate (rangedWeaponPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet> ();
		if (bullet != null)
			bullet.Seek (target, attackDamage);
	}

	public void stopMoving(){
		agent.Stop();
	}
}