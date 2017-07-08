using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Slime : Photon.MonoBehaviour{

	[Header("Name")]
	public string slimeName;
	[Space]
	[Header("Slime model")]
	public Transform model;

	public SlimeClass s;
	private Transform target;
	private float range;
	private NavMeshAgent agent;
    private GameManager gm;
	private bool pathUpdate, move;
	List<Transform> enemies;

	void Start(){
		s = new SlimeClass(slimeName);
		//PathFinding config
        agent = model.GetComponent<NavMeshAgent>();
		agent.speed = s.getMovementSpeed();
		agent.acceleration = s.getMovementSpeed();
		agent.stoppingDistance = s.getActionRange();
		//agent.angularSpeed = 0f;

        gm = GameManager.Instance;
		//Define enemy
		JoinTeamList();
		enemies = (transform.tag == "Team_RED") ? (gm.team_blue) : (gm.team_red);
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
					GetComponent<SlimeAction>().Action (target, s.getAttackSpeed(), s.getAttackDamage());	//Action to the target
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
		range = s.getScaleRadius() + s.getActionRange()+ target.parent.GetComponent<Slime>().s.getScaleRadius();
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
		Vector3 rotation = Quaternion.Lerp (model.rotation, lookRotation, Time.deltaTime * s.getTurnSpeed()).eulerAngles;
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