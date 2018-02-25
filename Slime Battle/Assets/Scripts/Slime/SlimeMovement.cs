/* Copyright (c) cc061495 */
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Linq;

public class SlimeMovement : MonoBehaviour {
	private Transform _transform;
	[SerializeField]
	private Transform target, prevTarget;
	private NavMeshAgent navMeshAgent;
	private Transform agent;

	private float range;
	private bool move, findNewTarget;

	TeamController.SearchMode mode;
	PhotonView photonView;
	SlimeClass slime;
	GameManager gm;
	TeamController tm;
	SlimeAction slimeAction;
	List<Transform> enemies, myTeam;
	
	void Awake(){
		_transform = transform;
		agent = GetComponent<Slime>().GetAgent();
		gm = GameManager.Instance;
		tm = TeamController.Instance;
		photonView = GetComponent<PhotonView>();
		slimeAction = GetComponent<SlimeAction>();
	}

	public void SetUpNavMeshAgent (SlimeClass _slime) {
		this.slime = _slime;
		navMeshAgent = agent.GetComponent<NavMeshAgent>();
		//agent.baseOffset = 0.9f;	//fix slime float on the ground problem
		navMeshAgent.speed = slime.movemonetSpeed;
		navMeshAgent.acceleration = slime.movemonetSpeed;
		navMeshAgent.stoppingDistance = 1.5f; //previous (1.5f)

		enemies = gm.GetEnemies(_transform);
	}
	
	void Update () {
		/* when the battle starts, start to execute */
		if (gm.currentState == GameManager.State.battle_start && photonView.isMine && target) {
			if(DistanceCalculate(target.position, agent.position) <= range*range){
				if(move && agent)
					SetupMovementSetting(false, 0f, 1);

				slimeAction.Action();		//Action to the target
				LookAtTarget();				//Look at the target
			}
			else if(!move)
				SetupMovementSetting(true, 120f, 50);
		}
	}
	/* Start this function in the Slime.cs */
	public void StartUpdatePathLoop(){
		InvokeRepeating("UpdatePath", Random.Range(0.1f, 0.5f), 0.5f);
	}

	private void UpdatePath(){
		if(gm.currentState == GameManager.State.battle_start){
			if(target == null){
				findNewTarget = false;
				SetupMovementSetting(true, 120f, 50);
			}

			if(move){
				TargetSearching();		//find new target every 0.2s
			
				if(target){
					if(mode != TeamController.SearchMode.defense)
						navMeshAgent.destination = target.position;	//set target position if target is found
					else	
						navMeshAgent.destination = agent.position;
					//Debug.Log(transform.name + "->" + target.parent.name);
					//Debug.Log(agent.pathStatus);
					/* if the target is not reachable, find the new target again */
					if(navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete && !findNewTarget){
						findNewTarget = true;
					}
				}
			}
			else
				navMeshAgent.destination = agent.position;		//stand on the current position
		}
		else if(gm.currentState == GameManager.State.battle_end)
			CancelInvoke("UpdatePath");
	}

	private void TargetSearching(){
		if(slime.isMeleeAttack || slime.isRangedAttack || slime.isAreaEffectDamage || slime.isExplosion || slime.isMagicalAreaEffectDamage){
			if(enemies.Count > 0){
				if(findNewTarget){
					/* Kill the shortest distance enemy */
					target = enemies.OrderBy(o => DistanceCalculate(o.position, agent.position)).FirstOrDefault();
				}
				else
					DefaultSearching();
			}
		}
		else if(slime.isHealing || slime.isAreaEffectHealing){
			/* Create a healer team list that not include itself and building */
			myTeam = gm.GetMyTeam(_transform).Where(x => x.root != transform)
											 .Where(x => !x.root.GetComponent<Slime>().GetSlimeClass().isBuilding).ToList();
			/* Check if the team list count > 0 */
			if(myTeam.Count > 0){
				bool findAnyLowHealth = false;
				/* find any not full health in the team list */
				for(int i=0;i<myTeam.Count;i++){
					if(myTeam[i].root.GetComponent<SlimeHealth>().currentHealth != myTeam[i].root.GetComponent<SlimeHealth>().startHealth){
						findAnyLowHealth = true;
						break;
					}
				}

				if(findAnyLowHealth){
					/* Find the nearest and lowest health target */
					target = myTeam.OrderBy(o => (o.root.GetComponent<SlimeHealth>().currentHealth / o.root.GetComponent<SlimeHealth>().startHealth))
								   .ThenBy(o => DistanceCalculate(o.position, agent.position)).FirstOrDefault();					   
				}
				else{
					/* Target with priority: melee attack > ranged attack > healer */
					target = myTeam.OrderBy(o => o.root.GetComponent<Slime>().GetSlimeClass().healingPriority)
								   .ThenBy(o => DistanceCalculate(o.position, agent.position)).FirstOrDefault();
				}
			}
		}
		/* if the target is found */
		if(target != null && target != prevTarget){
			prevTarget = target;
			slimeAction.SetTarget(target);
			range = slime.scaleRadius + slime.actionRange + target.root.GetComponent<Slime>().GetSlimeClass().scaleRadius;

			//Client set the target
			if(slime.isRangedAttack){
				//Debug.Log("RPC CALLS!!!");
				photonView.RPC("RPC_ClientSetTarget", PhotonTargets.Others, target.root.gameObject.GetPhotonView().viewID);
			}
		}
    }

	[PunRPC]
	private void RPC_ClientSetTarget(int targetView){
		int index = enemies.FindIndex(x => x.root.gameObject.GetPhotonView().viewID == targetView);
		/* if index >= 0, target is found and not equal to null */
		if(index != -1){
			target = enemies[index];
			slimeAction.SetTarget(target);	//setting target in client side
		}
	}

	public void FindTheTargetAgain(){
		findNewTarget = false;
		TargetSearching();
	}

	private void DefaultSearching(){
		mode = tm.GetTeamSearchMode(_transform);
		/* Kill the shortest distance enemy with killing priority(slime -> building) */
		if(mode == TeamController.SearchMode.distance || mode == TeamController.SearchMode.defense){
			target = enemies.OrderBy(o => o.root.GetComponent<Slime>().GetSlimeClass().killingPriority).
							ThenBy(o => DistanceCalculate(o.position, agent.position)).FirstOrDefault();
		}
		/* Kill the shortest distance enemy with lowest health precentage */
		else if(mode == TeamController.SearchMode.health){
			target = enemies.OrderBy(o => (o.root.GetComponent<SlimeHealth>().currentHealth / o.root.GetComponent<SlimeHealth>().startHealth)).
							ThenBy(o => DistanceCalculate(o.position, agent.position)).FirstOrDefault();
		}
		/* Kill the shortest distance with class priority() */
		else if(mode == TeamController.SearchMode.priority){
			target = enemies.OrderBy(o => o.root.GetComponent<Slime>().GetSlimeClass().classPriority).
							ThenBy(o => DistanceCalculate(o.position, agent.position)).FirstOrDefault();
		}
	}

	private void SetupMovementSetting(bool _move, float _angularSpeed, int _priority){
		move = _move;

		navMeshAgent.angularSpeed = _angularSpeed;
		if(slime.isMeleeAttack)
			navMeshAgent.avoidancePriority = _priority;
	}
	
	private void LookAtTarget(){
		Vector3 dir;
		dir.x = target.position.x - agent.position.x;
		dir.y = target.position.y - agent.position.y;
		dir.z = target.position.z - agent.position.z;

		if(dir != Vector3.zero){
			Quaternion lookRotation = Quaternion.LookRotation (dir);
			Vector3 rotation = Quaternion.Lerp (agent.rotation, lookRotation, Time.deltaTime * slime.turnSpeed).eulerAngles;
			agent.rotation = Quaternion.Euler (0f, rotation.y, 0f);
		}
	}

	private float DistanceCalculate(Vector3 pos1, Vector3 pos2){
		Vector3 distance;
		distance.x = pos1.x - pos2.x;
		distance.y = pos1.y - pos2.y;
		distance.z = pos1.z - pos2.z;

		float magnitude = distance.x * distance.x +
						  distance.y * distance.y +
						  distance.z * distance.z;
		return magnitude;
	}
}