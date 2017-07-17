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
	private bool pathUpdate, move;

	PhotonView photonView;
	SlimeClass slimeClass;
	GameManager gm;

	void Start(){
		gm = GameManager.Instance;
		photonView = GetComponent<PhotonView>();
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
                	UpdateTarget();		//find new target if target = null
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
		if(target != null && move && agent != null){
			agent.destination = target.position;	//finding new target position
		}
	}	

	public void StopUpdatePath(){
		CancelInvoke("UpdatePath");
	}

	void LookAtTarget(){
		Vector3 dir = target.position - model.position;
		Quaternion lookRotation = Quaternion.LookRotation (dir);
		Vector3 rotation = Quaternion.Lerp (model.rotation, lookRotation, Time.deltaTime * slimeClass.getTurnSpeed()).eulerAngles;
		model.rotation = Quaternion.Euler (0f, rotation.y, 0f);
	}

	public void UpdateTarget(){
		photonView.RPC("RPC_UpdateTarget", PhotonTargets.All);
	}

	[PunRPC]
	private void RPC_UpdateTarget(){
		if(slimeClass.isMeleeAttack() || slimeClass.isRangedAttack() || slimeClass.isAreaEffectDamage()){
			List<Transform> enemies = GameManager.Instance.GetEnemies(transform);
			target = enemies.OrderBy(o => (o.position - model.position).sqrMagnitude).FirstOrDefault();
		}
		else if(slimeClass.isHealing()){
			List<Transform> myTeam = GameManager.Instance.GetMyTeam(transform);
			List<Transform> myAttackerTeam = GameManager.Instance.GetMyAttackerTeam(transform);
			List<Transform> myHealerTeam = new List<Transform>(GameManager.Instance.GetMyHealerTeam(transform));
			myHealerTeam.Remove(model);

			bool findLowestHealth = false;

			foreach(Transform slime in myTeam){
				if(slime.parent != transform){
					if(slime.parent.GetComponent<SlimeHealth>().getCurrentHealth() != slime.parent.GetComponent<SlimeHealth>().getStartHealth()){
						findLowestHealth = true;
						break;
					}
				}
			}

			if(myAttackerTeam.Count > 0)
				findHealTarget(findLowestHealth, myAttackerTeam);
			else if(myHealerTeam.Count > 0)
				findHealTarget(findLowestHealth, myHealerTeam);
		}

		if(target != null)
			range = slimeClass.getScaleRadius() + slimeClass.getActionRange() + target.parent.GetComponent<Slime>().GetSlimeClass().getScaleRadius();
    }

	public Transform GetTarget(){
		return target;
	}

	private void findHealTarget(bool lowHealth, List<Transform> healTargets){
		if(lowHealth)
			target = healTargets.OrderBy(o => (o.parent.GetComponent<SlimeHealth>().getCurrentHealth() / o.parent.GetComponent<SlimeHealth>().getStartHealth())).FirstOrDefault();
		else
			target = healTargets.OrderBy(o => (o.position - model.position).sqrMagnitude).FirstOrDefault();
	}
}
