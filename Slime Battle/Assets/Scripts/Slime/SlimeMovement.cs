/* Copyright (c) cc061495 */
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Linq;

public class SlimeMovement : MonoBehaviour {
	private Transform _transform;

	[SerializeField]
	private Transform target;
	private NavMeshAgent agent;
	private Transform model;

	private float range;
	private bool move, findNewTarget, findTargetCoolDown;

	PhotonView photonView;
	SlimeClass slime;
	GameManager gm;
	TeamController tm;
	SlimeAction slimeAction;
	List<Transform> enemies, myTeam;
	

	void Awake(){
		_transform = transform;
		model = GetComponent<Slime>().GetModel();
		gm = GameManager.Instance;
		tm = TeamController.Instance;
		photonView = GetComponent<PhotonView>();
		slimeAction = GetComponent<SlimeAction>();
	}

	public void SetUpNavMeshAgent (SlimeClass _slime) {
		this.slime = _slime;

		agent = model.GetComponent<NavMeshAgent>();
		agent.speed = slime.movemonetSpeed;
		agent.acceleration = slime.movemonetSpeed;
		agent.stoppingDistance = 1.5f;

		enemies = gm.GetEnemies(_transform);
	}
	
	void Update () {
		if (gm.currentState == GameManager.State.battle_start && photonView.isMine) {  //when the battle starts, start to execute
            if (target){
                if(DistanceCalculate(target.position, model.position) <= range*range){
					//model.LookAt(new Vector3(target.position.x, model.position.y, target.position.z));
					slimeAction.Action();		//Action to the target
					LookAtTarget();

					if(move){
						move = false;
						agent.angularSpeed = 0f;
						agent.destination = model.position;		//stand on the current position
						if(slime.isMeleeAttack)
							agent.avoidancePriority = 1;
					}
				}
				else if(!move){
					move = true;
					agent.angularSpeed = 120f;
					if(slime.isMeleeAttack)
						agent.avoidancePriority = 50;
				}
            }
		}
	}

	public void StartUpdatePathLoop(){
		InvokeRepeating("UpdatePath", Random.Range(0, 0.5f), 0.2f);
	}

	void UpdatePath(){
		if(gm.currentState == GameManager.State.battle_start){
			if(target == null){
				findNewTarget = false;
				TargetSearching();		//find new target if target = null
			}

			if(target){
				if(move){
					agent.destination = target.position;	//finding new target position
					if(agent.pathStatus != NavMeshPathStatus.PathComplete && !findNewTarget){
						findNewTarget = true;
						//Debug.Log("findNewTarget");
						TargetSearching();
					}
				}
			}
		}
		else if(gm.currentState == GameManager.State.battle_end)
			CancelInvoke("UpdatePath");
	}
	
	void LookAtTarget(){
		Vector3 dir;
		dir.x = target.position.x - model.position.x;
		dir.y = target.position.y - model.position.y;
		dir.z = target.position.z - model.position.z;

		if(dir != Vector3.zero){
			Quaternion lookRotation = Quaternion.LookRotation (dir);
			Vector3 rotation = Quaternion.Lerp (model.rotation, lookRotation, Time.deltaTime * slime.turnSpeed).eulerAngles;
			model.SetPositionAndRotation(model.position, Quaternion.Euler (0f, rotation.y, 0f));
		}
	}

	public void TargetSearching(){
		if(slime.isMeleeAttack || slime.isRangedAttack || slime.isAreaEffectDamage || slime.isExplosion){
			if(enemies.Count > 0){
				if(findNewTarget){
					target = enemies.OrderByDescending(o => o.parent.GetComponent<Slime>().GetSlimeClass().killingPriority).
									 ThenBy(o => DistanceCalculate(o.position, model.position)).FirstOrDefault();
				}
				else{
					TeamController.SearchMode mode = tm.GetTeamSearchMode(_transform);
					if(mode == TeamController.SearchMode.distance){
						target = enemies.OrderBy(o => o.parent.GetComponent<Slime>().GetSlimeClass().killingPriority).
										ThenBy(o => DistanceCalculate(o.position, model.position)).FirstOrDefault();
					}
					else if(mode == TeamController.SearchMode.health){
						target = enemies.OrderBy(o => (o.parent.GetComponent<SlimeHealth>().currentHealth / o.parent.GetComponent<SlimeHealth>().startHealth)).
										ThenBy(o => DistanceCalculate(o.position, model.position)).FirstOrDefault();
					}
					else if(mode == TeamController.SearchMode.priority){
						target = enemies.OrderByDescending(o => o.parent.GetComponent<Slime>().GetSlimeClass().healingPriority).
										ThenBy(o => DistanceCalculate(o.position, model.position)).FirstOrDefault();
					}
				}
			}
		}
		else if(slime.isHealing){
			myTeam = gm.GetMyTeam(_transform).Where(x => x.parent != transform)
											 .Where(x => !x.parent.GetComponent<Slime>().GetSlimeClass().isBuilding).ToList();

			if(myTeam.Count > 0){
				
				bool findAnyLowHealth = false;
				
				for(int i=0;i<myTeam.Count;i++){
					if(myTeam[i].parent.GetComponent<SlimeHealth>().currentHealth != myTeam[i].parent.GetComponent<SlimeHealth>().startHealth){
						findAnyLowHealth = true;
						break;
					}
				}

				if(findAnyLowHealth){
					target = myTeam.OrderBy(o => (o.parent.GetComponent<SlimeHealth>().currentHealth / o.parent.GetComponent<SlimeHealth>().startHealth))
								   .ThenBy(o => DistanceCalculate(o.position, model.position)).FirstOrDefault();
								   
				}
				else{
					target = myTeam.OrderBy(o => o.parent.GetComponent<Slime>().GetSlimeClass().healingPriority)
								   .ThenBy(o => DistanceCalculate(o.position, model.position)).FirstOrDefault();
				}
			}
		}
		
		if(target != null){
			photonView.RPC("RPC_ClientSetTarget", PhotonTargets.Others, target.parent.gameObject.GetPhotonView().viewID);
			range = slime.scaleRadius + slime.actionRange + target.parent.GetComponent<Slime>().GetSlimeClass().scaleRadius;
		}
    }

	[PunRPC]
	private void RPC_ClientSetTarget(int targetView){
		List<Transform> enemies = gm.GetEnemies(_transform);
		for(int i=0;i<enemies.Count;i++){
			if(enemies[i].parent.gameObject.GetPhotonView().viewID == targetView)
				target = enemies[i];
		}
	}

	public Transform GetTarget(){
		return target;
	}

	private float DistanceCalculate(Vector3 pos1, Vector3 pos2){
		Vector3 distance;
		distance.x = pos1.x - pos2.x;
		distance.y = pos1.y - pos2.y;
		distance.z = pos1.z - pos2.z;

		float magnitude = distance.x * distance.x+
						  distance.y * distance.y+
						  distance.z * distance.z;
		return magnitude;
	}
}