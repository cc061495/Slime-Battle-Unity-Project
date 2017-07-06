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
	[Space]
	public Transform proxy;
	public Transform model;

	private Transform target;
	private float range, dist;
	private float turnSpeed = 3f;
	private NavMeshAgent agent;
	private NavMeshObstacle obstacle;
    private GameManager gm;
	private bool pathUpdate, move;
	List<Transform> enemies;

    void Start(){
		//PathFinding config
        agent = proxy.GetComponent<NavMeshAgent>();
		obstacle = proxy.GetComponent<NavMeshObstacle>();
		
		agent.speed = movemonetSpeed;
		agent.acceleration = 9999f;
		agent.angularSpeed = 0f;
		
        gm = GameManager.Instance;
		//Define enemy team
		enemies = (transform.tag == "Team_RED") ? (gm.team_blue) : (gm.team_red);
        JoinTeamList();
    }

	// Update is called once per frame
	void Update(){
        if (gm.currentState == GameManager.State.battle_start) {  //when the battle starts, start to execute

			if (target == null)		//find the target first, if target = null
                target = FindNearestTarget();

            if (target != null){	//found target
                if((target.position - proxy.position).sqrMagnitude < Mathf.Pow(actionRange, 2)){
					agent.enabled = false;
					obstacle.enabled = true;
					GetComponent<SlimeAction>().Action (target, attackSpeed, attackDamage);	//Action to the target
				}
				else{
					obstacle.enabled = false;
					agent.enabled = true;
					if(!pathUpdate){
						pathUpdate = true;
						InvokeRepeating("UpdatePath", 0f, 0.5f);
					}
				}
            }
		}
	}

	void FixedUpdate(){
		if(target != null){
			Vector3 dir = target.position - proxy.position;
			Quaternion lookRotation = Quaternion.LookRotation (dir);
			Vector3 rotation = Quaternion.Lerp (model.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
			model.rotation = Quaternion.Euler (0f, rotation.y, 0f);
			//how fast that the model follow the proxy
			model.position = Vector3.Lerp(model.position, proxy.position, Time.deltaTime * 5);
		}
	}

	void UpdatePath(){
		if(target != null && agent.enabled){
			Debug.Log("finding the path");
			agent.destination = target.position;	//finding new target position
		}
	}

	Transform FindNearestTarget(){
		target = enemies.OrderBy(o => (o.transform.position - proxy.position).sqrMagnitude).FirstOrDefault();
		return target;
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

	void JoinTeamList(){
        if(transform.tag == "Team_RED")
            gm.team_red.Add(proxy);
		else
            gm.team_blue.Add(proxy);
	}

	void RemoveFromTeamList(){
		if(transform.tag == "Team_RED")
            gm.team_red.Remove(proxy);
		else
            gm.team_blue.Remove(proxy);

        gm.CheckAnyEmptyTeam();
    }

	public void SlimeDead(){
		StopMoving();
		RemoveFromTeamList();
	}

	public void StopMoving(){
		CancelInvoke("UpdatePath");
		agent.enabled = false;
	}
}