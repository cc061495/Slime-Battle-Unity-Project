using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Slime : MonoBehaviour{
	public Transform target;
	
	[Header("Slime Propeties")]
	public float startHealth;		//Slime's health
	public float attackDamage;		//Slime's attack damage
	public float attackSpeed;		//Attack count per second
	public float movemonetSpeed;	//Slime's movement speed
	public float actionRange;		//Slime's action range

	private float turnSpeed = 3f;
	private NavMeshAgent agent;
	private Vector3 nextPos;
	private float myColRadius;
	private float tarColRadius;
    public bool dead = false;
    private GameManager gm;

    void Start(){
		//Slime confing
        myColRadius = GetComponent<CapsuleCollider> ().radius;
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
		GetComponent<SlimeHealth>().UpdateHealthBarPos();   //health bar position

        if (gm.currentState == GameManager.State.battle_start) {  //when the battle starts, start to execute

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
					GetComponent<SlimeAction>().Action (target, attackSpeed, attackDamage);	//Action to the target
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

	[PunRPC]
	public void RPC_Die(){
		dead = true;
		RemoveFromTeamList();
		
		PhotonView pv = transform.parent.GetComponent<PhotonView>();
		if(pv == null)
			return;

		if(pv.instantiationId == 0){
			Destroy(transform.parent.gameObject);
		}
		else{
			if(pv.isMine){
				PhotonNetwork.Destroy(pv);
			}
		}
		
	}

	public void stopMoving(){
        agent.Stop();
	}
}