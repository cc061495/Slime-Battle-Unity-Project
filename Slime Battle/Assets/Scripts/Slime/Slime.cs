using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Slime : Photon.MonoBehaviour{
	
	[Header("Slime Propeties")]
	public float startHealth;		//Slime's health
	public float attackDamage;		//Slime's attack damage
	public float attackSpeed;		//Attack count per second
	public float movemonetSpeed;	//Slime's movement speed
	public float actionRange;		//Slime's action range
	public float scaleRadius;
	[Space]
	public Transform model;
	private Transform target;
	private float turnSpeed = 3f, range;
	private NavMeshAgent agent;
    private GameManager gm;
	private bool pathUpdate, move;
	List<Transform> enemies;

    void Start(){
		//PathFinding config
        agent = model.GetComponent<NavMeshAgent>();
		agent.speed = movemonetSpeed;
		agent.acceleration = movemonetSpeed;
		agent.stoppingDistance = actionRange;
		//agent.angularSpeed = 0f;

        gm = GameManager.Instance;
		//Define enemy
		enemies = (transform.tag == "Team_RED") ? (gm.team_blue) : (gm.team_red);
        JoinTeamList();
    }

	// Update is called once per frame
	void Update(){
        if (gm.currentState == GameManager.State.battle_start) {  //when the battle starts, start to execute

			if (target == null){
                UpdateTarget();		//find new target if target = null
				if(!pathUpdate){
					pathUpdate = true;
					InvokeRepeating("UpdatePath", 0f, 0.5f);
				}
			}

            if (target != null){
                if((target.position - model.position).sqrMagnitude <= Mathf.Pow(range, 2)){
					LookAtTarget();
					GetComponent<SlimeAction>().Action (target, attackSpeed, attackDamage);	//Action to the target
					if(move){
						move = false;
						agent.destination = model.position;		//stand on the current position
					}
				}
            }
		}
	}

	void UpdatePath(){
		if(target != null && (target.position - model.position).sqrMagnitude > Mathf.Pow(range, 2)){
			move = true;
			agent.destination = target.position;	//finding new target position
		}
	}

	void UpdateTarget(){
		target = enemies.OrderBy(o => (o.transform.position - model.position).sqrMagnitude).FirstOrDefault();
		range = scaleRadius + actionRange + target.parent.GetComponent<Slime>().scaleRadius;
		// Transform nearestEnemy = null;
		// if (enemies.Count > 0) {
		// 	float shortestDistance = Mathf.Infinity;
		// 	foreach (Transform enemy in enemies) {
		// 		if(enemy != null){
		// 			float distanceToEnemy = (transform.position - enemy.transform.position).sqrMagnitude;
		// 			if (distanceToEnemy < shortestDistance) {
		// 				shortestDistance = distanceToEnemy;
		// 				nearestEnemy = enemy;
		// 			}
		// 		}
		// 	}
		// }
		//return nearestEnemy;
    }	

	void LookAtTarget(){
		Vector3 dir = target.position - model.position;
		Quaternion lookRotation = Quaternion.LookRotation (dir);
		Vector3 rotation = Quaternion.Lerp (model.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		model.rotation = Quaternion.Euler (0f, rotation.y, 0f);
	}

	void JoinTeamList(){
        if(transform.tag == "Team_RED")
            gm.team_red.Add(model);
		else
            gm.team_blue.Add(model);
	}

	void RemoveFromTeamList(){
		if(transform.tag == "Team_RED")
            gm.team_red.Remove(model);
		else
            gm.team_blue.Remove(model);

        gm.CheckAnyEmptyTeam();
    }

	public void SlimeDead(){
		CancelInvoke("UpdatePath");
		RemoveFromTeamList();
	}
}