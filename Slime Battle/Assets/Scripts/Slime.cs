using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Slime : MonoBehaviour{
	public Transform target;
	public Transform firePoint;
	public RectTransform healthBarPos;
	
	[Header("Slime Propeties")]
	public float startHealth;		//Slime's health
	public float attackDamage;		//Slime's attack damage
	public float attackSpeed;		//Attack count per second
	public float movemonetSpeed;	//Slime's movement speed
	public float actionRange;		//Slime's action range
	[Space]
	[Header("Slime Attack Mode")]
	public bool meleeAttack;
	[Space]
	public bool rangedAttack;
	public GameObject rangedWeaponPrefab;
	[Space]
	[Header("Slime Health Bar")]
	public Image healthBar;

	public float currentHealth;
	private float turnSpeed = 3f;
	private float countDown = 0f;
	private NavMeshAgent agent;
	private Vector3 nextPos;
	private float myColRadius;
	private float tarColRadius;
    private bool dead = false;
    private GameManager gm;

    void Start(){
		//Slime confing
        myColRadius = GetComponent<CapsuleCollider> ().radius;
		currentHealth = startHealth;
		//PathFinding config
        agent = GetComponent<NavMeshAgent> ();
		agent.speed = movemonetSpeed;
		agent.acceleration = movemonetSpeed;
        agent.stoppingDistance = actionRange + myColRadius;

        gm = GameManager.Instance;

        JoinTeamList();
    }

	// Update is called once per frame
	void Update(){
		UpdateHealthBarPos();   //health bar position

        if (gm.currentState == GameManager.State.battle_start) {  //when the battle starts, start to execute
            if (currentHealth <= 0){
                Destroy(transform.parent.gameObject);
                RemoveFromTeamList();
                dead = true;
            }

            if (target == null)
                UpdateTarget();

            if (target != null){
				LookToTarget ();
				tarColRadius = target.GetComponent<CapsuleCollider> ().radius;
				float range = myColRadius + tarColRadius + actionRange;
				float dist = Vector3.Distance (transform.position, target.position);
                if (dist > range && target.transform.hasChanged) {
					nextPos = target.position;		//set new target pos
				} else {
					nextPos = transform.position;	//Stay on the ground
					Action ();						//Action to the target
				}
				if(!dead)
                	agent.SetDestination(nextPos);
            }
		}
	}

	void UpdateTarget(){
		List<GameObject> enemies = (gameObject.tag == "Team_RED") ? (gm.team_blue) : (gm.team_red);

		if (enemies.Count > 0) {
			float shortestDistance = Mathf.Infinity;
			GameObject nearestEnemy = null;

			foreach (GameObject enemy in enemies) {
				float distanceToEnemy = Vector3.Distance (transform.position, enemy.transform.position);
				if (distanceToEnemy < shortestDistance) {
					shortestDistance = distanceToEnemy;
					nearestEnemy = enemy;
				}
			}
            target = nearestEnemy.transform;
        }
		else
            target = null;
    }
		
	void LookToTarget(){
		Vector3 dir = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation (dir);
		Vector3 rotation = Quaternion.Lerp (transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		transform.rotation = Quaternion.Euler (0f, rotation.y, 0f);
	}

	void Action(){
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
		e.currentHealth -= attackDamage;
		e.healthBar.fillAmount = e.currentHealth / e.startHealth;
	}

	void RangedAttack(){
		GameObject bulletGO = (GameObject)Instantiate (rangedWeaponPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet> ();
		if (bullet != null)
			bullet.Seek (target, attackDamage);
	}

	void UpdateHealthBarPos(){
		if(healthBarPos.hasChanged){
			if(PhotonNetwork.isMasterClient){
				healthBarPos.position = new Vector3 (transform.position.x, transform.position.y + 2f, transform.position.z + 1f);
			}
			else{
				healthBarPos.position = new Vector3 (transform.position.x, transform.position.y + 2f, transform.position.z - 1f);
				healthBarPos.rotation = Quaternion.Euler(90f, 0f, 0f);
			}
		}
	}

	void JoinTeamList(){
        if(gameObject.tag == "Team_RED")
            gm.team_red.Add(gameObject);
		else
            gm.team_blue.Add(gameObject);
	}

	void RemoveFromTeamList(){
		if(gameObject.tag == "Team_RED")
            gm.team_red.Remove(gameObject);
		else
            gm.team_blue.Remove(gameObject);

        gm.CheckAnyEmptyTeam();
    }

	public void stopMoving(){
        agent.Stop();
	}
}