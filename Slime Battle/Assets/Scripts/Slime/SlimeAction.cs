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
	private SlimeClass slime;

	public void Action(Transform target, SlimeClass slime){
		if (coolDown <= 0f) {
			if (slime.isMeleeAttack()){
				MeleeAttack (target, slime.getAttackDamage());
			}
			else if (slime.isRangedAttack())
				RangedAttack (target, slime.getAttackDamage());

			coolDown = 1f / slime.getAttackSpeed();
		}
		coolDown -= Time.deltaTime;
	}
		
	void MeleeAttack(Transform target, float attackDamage){
		SlimeHealth h = target.parent.GetComponent<SlimeHealth>();
		h.TakeDamage(attackDamage);
		/* 
		PhotonView pv = target.GetComponent<PhotonView>();

		if(pv != null && target != null)
			target.GetComponent<PhotonView>().RPC("RPC_TakeDamage", PhotonTargets.All, attackDamage);
		*/
	}

	void RangedAttack(Transform target, float attackDamage){
		GameObject bulletGO = (GameObject)Instantiate (rangedWeaponPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet> ();
		if (bullet != null)
			bullet.Seek (target, attackDamage);
	}
}
