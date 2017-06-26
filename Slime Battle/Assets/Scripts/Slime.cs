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
	private bool dead = false;

	void Awake(){
		myColRadius = GetComponent<CapsuleCollider> ().radius;
		agent = GetComponent<NavMeshAgent> ();
		agent.speed = movemonetSpeed;
		agent.acceleration = movemonetSpeed;
		agent.stoppingDistance = attackRange;
		health = startHealth;
	}

	void Start(){
		//health bar position
		UpdateHealthBarPos();
	}

	// Update is called once per frame
	void Update(){
		bool isStart = GameManager.Instance.battleIsStart;
		bool isEnd = GameManager.Instance.battleIsEnd;

		if (isStart) {	//Pressed the "Start" Button, start to run
			UpdateHealthBarPos();	//health bar position

			if (health <= 0) {
				Destroy (transform.parent.gameObject);
				dead = true;
			}

			if (isEnd) {
				agent.Stop ();
				return;
			}

			if (target == null)
				UpdateTarget ();

			if (target != null) {
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
				if(!dead)
					agent.SetDestination (nextPos);
			}
		}
	}

	void UpdateTarget(){
		GameObject[] enemies = (gameObject.tag == "TeamA") ? (GameManager.Instance.teamB) : (GameManager.Instance.teamA);

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
}
