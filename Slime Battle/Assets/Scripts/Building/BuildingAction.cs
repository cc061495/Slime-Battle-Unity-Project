using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingAction : MonoBehaviour {

	[SerializeField]
	private Transform firePoint;
	[SerializeField]
	private GameObject rangedWeaponPrefab;

	List<Transform> enemyList = new List<Transform>();
	PhotonView photonView;
	private float coolDown = 0f;
	private float range;
	private SlimeClass slime;
	private bool action;
	GameManager gameManager;
	Transform target, prevTarget, model, agent;

	void Start(){
		photonView = transform.GetComponent<PhotonView>();
		gameManager = GameManager.Instance;
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
		InvokeRepeating("FindTheTarget", Random.Range(0.1f, 0.5f), 1f);
	}

	private void FindTheTarget(){
		if(target == null)
			action = false;

		if(!action){
			Debug.Log("FIND TARGET");
			FindTheNearestEnemy();
		}

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
		SlimeHealth tarHealth = target.root.GetComponent<SlimeHealth>();
		GameObject b = (GameObject)Instantiate (rangedWeaponPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = b.GetComponent<Bullet>();
		bullet.Seek (target, attackDamage, tarHealth);
	}

	[PunRPC]
	private void RPC_RangedAttack(float attackDamage, float effectAreaRadius, float slowDownPrecentage){
		GameObject b = (GameObject)Instantiate (rangedWeaponPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = b.GetComponent<Bullet>();
		bullet.Seek (target, attackDamage, effectAreaRadius, slowDownPrecentage);
	}

	private void FindTheNearestEnemy(){
		/* Kill the shortest distance enemy */
		target = enemyList.OrderBy(o => DistanceCalculate(o.position, agent.position)).FirstOrDefault();

		if(target != null && target != prevTarget){
			prevTarget = target;
			range = slime.scaleRadius + slime.actionRange + target.root.GetComponent<Slime>().GetSlimeClass().scaleRadius;
			//Client set the target
			photonView.RPC("RPC_ClientSetTarget", PhotonTargets.Others, target.root.gameObject.GetPhotonView().viewID);
		}
	}

	[PunRPC]
	private void RPC_ClientSetTarget(int targetView){
		int index = enemyList.FindIndex(x => x.root.gameObject.GetPhotonView().viewID == targetView);
		/* if index >= 0, target is found and not equal to null */
		if(index != -1)
			target = enemyList[index];
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
			model.SetPositionAndRotation(model.position, Quaternion.Euler (0f, rotation.y, 0f));
		}
	}
}