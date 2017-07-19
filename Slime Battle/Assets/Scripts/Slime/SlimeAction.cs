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
	private Transform target, model;
	PhotonView photonView;

	void Start(){
		photonView = GetComponent<PhotonView>();
	}

	public void Action(SlimeClass slime){
		model = GetComponent<Slime>().GetModel();
		photonView.RPC("RPC_SetTarget", PhotonTargets.All);

		if (coolDown <= 0f) {
			if (slime.isMeleeAttack()){
				MeleeAttack (slime.getAttackDamage());
			}
			else if (slime.isRangedAttack()){
				photonView.RPC("RangedAttack",PhotonTargets.All, slime.getAttackDamage());
			}
			else if(slime.isHealing()){
				SlimeHealth tarParentHealth = target.parent.GetComponent<SlimeHealth>();
				if(tarParentHealth.getCurrentHealth() >= tarParentHealth.getStartHealth())
					GetComponent<SlimeMovement>().UpdateTarget();
				else
					Healing (slime.getHealingPoint());
			}
			else if(slime.isAreaEffectDamage()){
				AreaEffectDamage(slime.getAttackDamage(), slime.getAreaEffectRadius(), target.position);
			}
			else if(slime.isExplosion()){
				AreaEffectDamage(slime.getAttackDamage(), slime.getAreaEffectRadius(), model.position);
				GetComponent<SlimeHealth>().TakeDamage(GetComponent<SlimeHealth>().getCurrentHealth());
			}

			coolDown = 1f / slime.getActionSpeed();
		}
		coolDown -= Time.deltaTime;
	}

	[PunRPC]
	private void RPC_SetTarget(){
		target = GetComponent<SlimeMovement>().GetTarget();
	}
		
	private void MeleeAttack(float attackDamage){
		SlimeHealth h = target.parent.GetComponent<SlimeHealth>();
		h.TakeDamage(attackDamage);
	}

	private void AreaEffectDamage(float attackDamage, float effectAreaRadius, Vector3 centre){
		Collider[] slimes = Physics.OverlapSphere(centre, effectAreaRadius);
		foreach (Collider slime in slimes){
			if(slime.transform.parent.tag == target.parent.tag){
				SlimeHealth h = slime.transform.parent.GetComponent<SlimeHealth>();

				float distanceFromCentre = (slime.transform.position - centre).sqrMagnitude;
				float radius = Mathf.Pow(effectAreaRadius, 2);
				float areaDamage = ((radius - distanceFromCentre) / radius) * attackDamage;
				if(areaDamage < 0)
					areaDamage = 0;
				h.TakeDamage(areaDamage);
			}
		}
	}

	[PunRPC]
	private void RangedAttack(float attackDamage){
		GameObject bulletGO = (GameObject)Instantiate (rangedWeaponPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet>();
		if (bullet != null)
			bullet.Seek (target, attackDamage);
	}

	private void Healing(float healingPoint){
		SlimeHealth h = target.parent.GetComponent<SlimeHealth>();
		h.TakeHealing(healingPoint);
	}
}
