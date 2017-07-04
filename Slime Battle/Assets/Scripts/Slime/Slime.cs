using System.Collections.Generic;
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

	private Transform target;
	private float myColRadius, tarColRadius, range;
	private float turnSpeed = 3f;
	private NavMeshAgent agent;
    private GameManager gm;
	List<Transform> enemies;

    void Start(){
		//Slime confing
        myColRadius = GetComponent<CapsuleCollider> ().radius;
		//PathFinding config
        agent = GetComponent<NavMeshAgent> ();
		agent.speed = movemonetSpeed;
		agent.acceleration = movemonetSpeed;
        agent.stoppingDistance = actionRange + myColRadius;

        gm = GameManager.Instance;
		//Define enemy
		enemies = (transform.tag == "Team_RED") ? (gm.team_blue) : (gm.team_red);
        JoinTeamList();
    }

	// Update is called once per frame
	void Update(){
        if (gm.currentState == GameManager.State.battle_start) {  //when the battle starts, start to execute
			
			if (target == null){
                target = UpdateTarget();
				agent.SetDestination(target.position);
			}

            if (target != null){
				LookAtTarget();
				float dist = Vector3.Distance(transform.position, target.position);	//must use Vector3.Distance()
                if (dist > range && agent.destination != target.position) {
					agent.destination = target.position;	//set new target pos
				} 
				else {
					if(agent.destination != transform.position)
						agent.destination = transform.position;
					GetComponent<SlimeAction>().Action (target, attackSpeed, attackDamage);	//Action to the target
				}
            }
		}
	}

	Transform UpdateTarget(){
		target = enemies.OrderBy(o => (o.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
		tarColRadius = target.GetComponent<CapsuleCollider> ().radius;
		range = myColRadius + tarColRadius + actionRange;
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

	void LookAtTarget(){
		Vector3 dir = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation (dir);
		Vector3 rotation = Quaternion.Lerp (transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		transform.rotation = Quaternion.Euler (0f, rotation.y, 0f);
	}

	void JoinTeamList(){
        if(transform.tag == "Team_RED")
            gm.team_red.Add(transform);
		else
            gm.team_blue.Add(transform);
	}

	void RemoveFromTeamList(){
		if(transform.tag == "Team_RED")
            gm.team_red.Remove(transform);
		else
            gm.team_blue.Remove(transform);

        gm.CheckAnyEmptyTeam();
    }

	public void SlimeDead(){
		StopMoving();
		RemoveFromTeamList();
	}

	public void StopMoving(){
		agent.destination = transform.position;
	}
}