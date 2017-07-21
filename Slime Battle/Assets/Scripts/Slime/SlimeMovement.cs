﻿/* Copyright (c) cc061495 */
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Linq;

public class SlimeMovement : MonoBehaviour {
	[SerializeField]
	private Transform target;
	private NavMeshAgent agent;
	private Transform model;

	private float range, findTargetCoolDown;
	private bool pathUpdate, move, findNewTarget;
	private Vector3 prevPos;

	PhotonView photonView;
	SlimeClass slime;
	GameManager gm;
	[SerializeField]
	private List<Transform> myTeam;

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
		agent.stoppingDistance = slime.actionRange;
	}
	
	void Update () {
		if (gm.currentState == GameManager.State.battle_start && photonView.isMine) {  //when the battle starts, start to execute
			if(findTargetCoolDown > 0)
				findTargetCoolDown -= Time.deltaTime;

			if (target == null){
                UpdateTarget();		//find new target if target = null
				if(!pathUpdate){
					pathUpdate = true;
					InvokeRepeating("UpdatePath", Random.Range(0, 0.5f), 0.5f);
				}
			}

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

	void UpdatePath(){
		if(gm.currentState == GameManager.State.battle_start){
			if(target != null && move){
				Debug.Log(agent.pathStatus);
				if(agent.pathStatus != NavMeshPathStatus.PathComplete && !findNewTarget){
					findNewTarget = true;
					TargetSearching();
				}
				agent.destination = target.position;	//finding new target position
			}
		}
		else
			CancelInvoke("UpdatePath");
	}

	void LookAtTarget(){
		Vector3 dir = target.position - model.position;
		Quaternion lookRotation = Quaternion.LookRotation (dir);
		Vector3 rotation = Quaternion.Lerp (model.rotation, lookRotation, Time.deltaTime * slime.turnSpeed).eulerAngles;
		model.rotation = Quaternion.Euler (0f, rotation.y, 0f);
	}

	public void UpdateTarget(){
		if(findTargetCoolDown <= 0){
			findTargetCoolDown = 0.5f;
			TargetSearching();
		}
	}

	private void TargetSearching(){
		if(slime.isMeleeAttack || slime.isRangedAttack || slime.isAreaEffectDamage || slime.isExplosion){
			List<Transform> enemies = GameManager.Instance.GetEnemies(transform);
			if(enemies.Count > 0){
				if(findNewTarget){
					target = enemies.OrderByDescending(o => o.parent.GetComponent<Slime>().GetSlimeClass().killingPriority).
									 ThenBy(o => (o.position - model.position).sqrMagnitude).FirstOrDefault();
					findNewTarget = false;
				}
				else{
					target = enemies.OrderBy(o => o.parent.GetComponent<Slime>().GetSlimeClass().killingPriority).
									 ThenBy(o => (o.position - model.position).sqrMagnitude).FirstOrDefault();
				}
			}
		}
		else if(slime.isHealing){
			myTeam = new List<Transform>(GameManager.Instance.GetMyTeam(transform));
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
					Invoke("UpdateTarget", 0.6f);
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
		target = PhotonView.Find(targetView).GetComponent<Slime>().GetModel();
	}

	public Transform GetTarget(){
		return target;
	}
}