/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAction : MonoBehaviour {

	[SerializeField]
	private Transform firePoint;
	[SerializeField]
	private GameObject rangedWeaponPrefab;

	private float coolDown = 0f;
	private Transform target;
	PhotonView photonView;

	void Start(){
		photonView = GetComponent<PhotonView>();
	}

	public void Action(SlimeClass slime){
		photonView.RPC("RPC_SetTarget", PhotonTargets.All);
		float attackDamage = slime.getAttackDamage();

		if (coolDown <= 0f) {
			if (slime.isMeleeAttack()){
				MeleeAttack (attackDamage);
			}
			else if (slime.isRangedAttack()){
				photonView.RPC("RangedAttack",PhotonTargets.All, attackDamage);
			}
			else if(slime.isHealing()){
				SlimeHealth tarParentHealth = target.parent.GetComponent<SlimeHealth>();
				if(tarParentHealth.getCurrentHealth() >= tarParentHealth.getStartHealth())
					GetComponent<SlimeMovement>().UpdateTarget();
				else
					Healing (attackDamage);
			}

			coolDown = 1f / slime.getAttackSpeed();
		}
		coolDown -= Time.deltaTime;
	}

	[PunRPC]
	private void RPC_SetTarget(){
		target = GetComponent<SlimeMovement>().GetTarget();
	}
		
	void MeleeAttack(float attackDamage){
		SlimeHealth h = target.parent.GetComponent<SlimeHealth>();
		h.TakeDamage(attackDamage);
		/* 
		PhotonView pv = target.GetComponent<PhotonView>();

		if(pv != null && target != null)
			target.GetComponent<PhotonView>().RPC("RPC_TakeDamage", PhotonTargets.All, attackDamage);
		*/
	}

	[PunRPC]
	private void RangedAttack(float attackDamage){
		GameObject bulletGO = (GameObject)Instantiate (rangedWeaponPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet>();
		if (bullet != null)
			bullet.Seek (target, attackDamage);
	}

	void Healing(float attackDamage){
		SlimeHealth h = target.parent.GetComponent<SlimeHealth>();
		h.TakeHealing(attackDamage);
	}
}
