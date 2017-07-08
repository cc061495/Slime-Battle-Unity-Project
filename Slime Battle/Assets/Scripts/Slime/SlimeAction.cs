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

	public void Action(Transform target, float attackSpeed, float attackDamage){
		slime = GetComponent<Slime>().s;
		if (coolDown <= 0f) {
			if (slime.isMeleeAttack()){
				MeleeAttack (target, attackDamage);
			}
			else if (slime.isRangedAttack())
				RangedAttack (target, attackDamage);

			coolDown = 1f / attackSpeed;
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
