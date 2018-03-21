using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingAction : MonoBehaviour {

	[SerializeField]
	private Transform firePoint;

	List<Transform> enemyList = new List<Transform>();
	PhotonView photonView;
	private float coolDown = 0f;
	private float range;
	private SlimeClass slime;
	private bool action;
	GameManager gameManager;
	ObjectPooler objectPooler;
	TeamController teamController;
	Transform target, prevTarget, model, agent;
	SlimeHealth tarHealth;

	void Start(){
		photonView = transform.GetComponent<PhotonView>();
		gameManager = GameManager.Instance;
		objectPooler = ObjectPooler.Instance;
		teamController = TeamController.Instance;
		model = GetComponent<Slime>().GetModel();
		agent = GetComponent<Slime>().GetAgent();

		enemyList = gameManager.GetEnemies(transform);
	}

	void Update(){
		if(gameManager.currentState == GameManager.State.battle_start && photonView.isMine && target){
			if(DistanceCalculate(target.position, agent.position) <= range*range){	
				LookAtTarget();
				Action();

				if(!action)
					action = true;
			}
			else if(action)
				action = false;
		}
	}

	public void SetUpBuilding(SlimeClass _slime){
		slime = _slime;
		InvokeRepeating("FindTheTarget", Random.Range(0.1f, 0.5f), 0.5f);
	}

	private void FindTheTarget(){
		if(target == null)
			action = false;

		if(!action)
			FindTheNearestEnemy();

		if(gameManager.currentState == GameManager.State.battle_end)
			CancelInvoke("FindTheTarget");
	}

	private void Action(){
		if (coolDown <= 0f) {
			if(!slime.canSlowDown)
				photonView.RPC("RPC_RangedAttack", PhotonTargets.All, slime.attackDamage);
			else
				photonView.RPC("RPC_RangedAttack", PhotonTargets.All, slime.attackDamage, slime.areaEffectRadius, slime.slowDownPercentage);
			
			coolDown = slime.actionCoolDown;
		}
		else
			coolDown -= Time.deltaTime;
	}

	[PunRPC]
	private void RPC_RangedAttack(float attackDamage){
		GameObject b = objectPooler.SpawnFromPool("Bullet", firePoint.position, firePoint.rotation);
		Bullet bullet = b.GetComponent<Bullet>();
		bullet.Seek (target, attackDamage, tarHealth, "Bullet");
	}

	[PunRPC]
	private void RPC_RangedAttack(float attackDamage, float effectAreaRadius, float slowDownPrecentage){
		GameObject b = objectPooler.SpawnFromPool("IceCube", firePoint.position, firePoint.rotation);
		Bullet bullet = b.GetComponent<Bullet>();
		bullet.Seek (target, attackDamage, effectAreaRadius, slowDownPrecentage, "IceCube");
	}

	private TeamController.SearchMode mode; 

	private void FindTheNearestEnemy(){
		mode = teamController.GetTeamSearchMode(transform);
		/* Kill the shortest distance enemy */
		if(mode == TeamController.SearchMode.distance || mode == TeamController.SearchMode.defense){
			target = enemyList.OrderBy(o => DistanceCalculate(o.position, agent.position)).FirstOrDefault();
		}
		/* Kill the shortest distance enemy with lowest health precentage */
		else if(mode == TeamController.SearchMode.health){
			target = enemyList.OrderBy(o => (o.root.GetComponent<SlimeHealth>().currentHealth / o.root.GetComponent<SlimeHealth>().startHealth)).
							ThenBy(o => DistanceCalculate(o.position, agent.position)).FirstOrDefault();
		}
		/* Kill the shortest distance with class priority() */
		else if(mode == TeamController.SearchMode.priority){
			target = enemyList.OrderBy(o => o.root.GetComponent<Slime>().GetSlimeClass().classPriority).
							ThenBy(o => DistanceCalculate(o.position, agent.position)).FirstOrDefault();
		}

		if(target != null && target != prevTarget){
			prevTarget = target;
			tarHealth = target.root.GetComponent<SlimeHealth>();
			range = slime.scaleRadius + slime.actionRange + target.root.GetComponent<Slime>().GetSlimeClass().scaleRadius;
			//Client set the target
			photonView.RPC("RPC_ClientSetTarget", PhotonTargets.Others, target.root.gameObject.GetPhotonView().viewID);
		}
	}

	[PunRPC]
	private void RPC_ClientSetTarget(int targetView){
		int index = enemyList.FindIndex(x => x.root.gameObject.GetPhotonView().viewID == targetView);
		/* if index >= 0, target is found and not equal to null */
		if(index != -1){
			target = enemyList[index];
			tarHealth = target.root.GetComponent<SlimeHealth>();
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

	private void LookAtTarget(){
		Vector3 dir;
		dir.x = target.position.x - model.position.x;
		dir.y = target.position.y - model.position.y;
		dir.z = target.position.z - model.position.z;

		if(dir != Vector3.zero){
			Quaternion lookRotation = Quaternion.LookRotation (dir);
			Vector3 rotation = Quaternion.Lerp (model.rotation, lookRotation, Time.deltaTime * slime.turnSpeed).eulerAngles;
			model.SetPositionAndRotation(model.position, Quaternion.Euler (-90f, rotation.y, 0f));
		}
	}
}