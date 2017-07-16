﻿/* Copyright (c) cc061495 */
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Linq;

public class SlimeMovement : Photon.MonoBehaviour {

	private Transform target;
	private NavMeshAgent agent;
	private Transform model;

	private float range;
	private bool pathUpdate, move;

	SlimeClass slimeClass;
	GameManager gm;

	void Start(){
		gm = GameManager.Instance;
	}

	public void SetUpNavMeshAgent () {
		slimeClass = GetComponent<Slime>().GetSlimeClass();
		model = GetComponent<Slime>().GetModel();

		agent = model.GetComponent<NavMeshAgent>();
		agent.speed = slimeClass.getMovementSpeed();
		agent.acceleration = slimeClass.getMovementSpeed();
		agent.stoppingDistance = slimeClass.getActionRange();
	}
	
	void Update () {
		if (gm.currentState == GameManager.State.battle_start && photonView.isMine) {  //when the battle starts, start to execute
			if (target == null){
                photonView.RPC("RPC_UpdateTarget", PhotonTargets.All);		//find new target if target = null
				if(!pathUpdate){
					pathUpdate = true;
					InvokeRepeating("UpdatePath", 0f, 0.5f);
				}
			}

            if (target != null){
                if((target.position - model.position).sqrMagnitude <= Mathf.Pow(range, 2)){
					LookAtTarget();
					GetComponent<SlimeAction>().Action(slimeClass);		//Action to the target
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
		if(target != null && move){
			agent.destination = target.position;	//finding new target position
		}
	}	

	void LookAtTarget(){
		Vector3 dir = target.position - model.position;
		Quaternion lookRotation = Quaternion.LookRotation (dir);
		Vector3 rotation = Quaternion.Lerp (model.rotation, lookRotation, Time.deltaTime * slimeClass.getTurnSpeed()).eulerAngles;
		model.rotation = Quaternion.Euler (0f, rotation.y, 0f);
	}

	[PunRPC]
	private void RPC_UpdateTarget(){
		List<Transform> enemies = GetComponent<Slime>().GetEmenies();
		target = enemies.OrderBy(o => (o.transform.position - model.position).sqrMagnitude).FirstOrDefault();
		range = slimeClass.getScaleRadius() + slimeClass.getActionRange() + target.parent.GetComponent<Slime>().GetSlimeClass().getScaleRadius();
    }

	public Transform GetTarget(){
		return target;
	}
}
