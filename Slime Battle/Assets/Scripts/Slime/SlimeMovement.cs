/* Copyright (c) cc061495 */
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Linq;

public class SlimeMovement : MonoBehaviour {
	[SerializeField]
	private Transform target;
	private NavMeshAgent agent;
	private Transform model;

	private float range;
	private bool move, findNewTarget, findTargetCoolDown;
	private Vector3 prevPos;

	PhotonView photonView;
	SlimeClass slime;
	GameManager gm;

	void Start(){
		gm = GameManager.Instance;
		photonView = GetComponent<PhotonView>();
	}

	public void SetUpNavMeshAgent (Transform _model, SlimeClass _slime) {
		this.slime = _slime;
		this.model = _model;

		agent = model.GetComponent<NavMeshAgent>();
		agent.speed = slime.movemonetSpeed;
		agent.acceleration = slime.movemonetSpeed;
		agent.stoppingDistance = 1.5f;
	}
	
	void Update () {
		if (gm.currentState == GameManager.State.battle_start && photonView.isMine) {  //when the battle starts, start to execute
            if (target != null){
                if((target.position - model.position).sqrMagnitude <= Mathf.Pow(range, 2)){
					LookAtTarget();
					GetComponent<SlimeAction>().Action(slime, model);		//Action to the target
					if(move){
						move = false;
						agent.angularSpeed = 0f;
						agent.destination = model.position;		//stand on the current position
					}
				}
				else if(!move){
					move = true;
					agent.angularSpeed = 120f;
				}
            }
		}
	}

	public void StartUpdatePathLoop(){
		InvokeRepeating("UpdatePath", Random.Range(0, 0.5f), 0.1f);
	}

	void UpdatePath(){
		if(gm.currentState == GameManager.State.battle_start){
			if(target == null){
				findNewTarget = false;
				TargetSearching();		//find new target if target = null
			}

			if(target != null && move){
				agent.destination = target.position;	//finding new target position
				if(agent.pathStatus != NavMeshPathStatus.PathComplete && !findNewTarget){
					findNewTarget = true;
					Debug.Log("findNewTarget");
					TargetSearching();
				}
			}
		}
		else if(gm.currentState == GameManager.State.battle_end)
			CancelInvoke("UpdatePath");
	}

	void LookAtTarget(){
		Vector3 dir = target.position - model.position;
		Quaternion lookRotation = Quaternion.LookRotation (dir);
		Vector3 rotation = Quaternion.Lerp (model.rotation, lookRotation, Time.deltaTime * slime.turnSpeed).eulerAngles;
		model.rotation = Quaternion.Euler (0f, rotation.y, 0f);
	}

	public void TargetSearching(){
		if(slime.isMeleeAttack || slime.isRangedAttack || slime.isAreaEffectDamage || slime.isExplosion){
			List<Transform> enemies = gm.GetEnemies(transform);
			if(enemies.Count > 0){
				if(findNewTarget){
					target = enemies.OrderByDescending(o => o.parent.GetComponent<Slime>().GetSlimeClass().killingPriority).
									 ThenBy(o => (o.position - model.position).sqrMagnitude).FirstOrDefault();
				}
				else{
					target = enemies.OrderBy(o => o.parent.GetComponent<Slime>().GetSlimeClass().killingPriority).
									 ThenBy(o => (o.position - model.position).sqrMagnitude).FirstOrDefault();
				}
			}
		}
		else if(slime.isHealing){
			List<Transform> myTeam = new List<Transform>(gm.GetMyTeam(transform));
			if(myTeam.Count > 1){
				myTeam.Remove(model);
				foreach(Transform slime in myTeam.ToList()){
					if(slime.parent.GetComponent<Slime>().GetSlimeClass().isBuilding)
						myTeam.Remove(slime);
				}
				
				bool findAnyLowHealth = false;
				
				foreach(Transform slime in myTeam){
					if(slime.parent.GetComponent<SlimeHealth>().getCurrentHealth() != slime.parent.GetComponent<SlimeHealth>().getStartHealth()){
						findAnyLowHealth = true;
						break;
					}
				}

				if(findAnyLowHealth){
					target = myTeam.OrderBy(o => (o.parent.GetComponent<SlimeHealth>().getCurrentHealth() / o.parent.GetComponent<SlimeHealth>().getStartHealth()))
								   .ThenBy(o => (o.position - model.position).sqrMagnitude).FirstOrDefault();
				}
				else{
					target = myTeam.OrderBy(o => o.parent.GetComponent<Slime>().GetSlimeClass().healingPriority)
								   .ThenBy(o => (o.position - model.position).sqrMagnitude).FirstOrDefault();
				}
			}
		}
		
		if(target != null){
			range = slime.scaleRadius + slime.actionRange + target.parent.GetComponent<Slime>().GetSlimeClass().scaleRadius;
			photonView.RPC("RPC_ClientSetTarget", PhotonTargets.Others, target.parent.gameObject.GetPhotonView().viewID);
		}
    }

	[PunRPC]
	private void RPC_ClientSetTarget(int targetView){
		List<Transform> enemies = gm.GetEnemies(transform);
		foreach(Transform slime in enemies){
			if(slime.parent.gameObject.GetPhotonView().viewID == targetView)
				target = slime;
		}
	}

	public Transform GetTarget(){
		return target;
	}

	public void FindNewTargetWithFewSecond(){
		Invoke("TargetSearching", 0.5f);
	}
}