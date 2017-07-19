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
				AreaAttack(slime.getAttackDamage(), slime.getAreaEffectRadius());
			}
			else if(slime.isExplosion()){
				Explosion(slime.getAttackDamage(), slime.getAreaEffectRadius());
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

	private void AreaAttack(float attackDamage, float effectAreaRadius){
		Collider[] slimes = Physics.OverlapSphere(target.position, effectAreaRadius);
		foreach (Collider slime in slimes){
			if(slime.transform.parent.tag == target.parent.tag){
				SlimeHealth h = slime.transform.parent.GetComponent<SlimeHealth>();

				float distanceFromSphereCentre = (slime.transform.position - target.position).sqrMagnitude;
				h.TakeDamage(DamageWithRadius(attackDamage, distanceFromSphereCentre, effectAreaRadius));
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

	private void Explosion(float attackDamage, float effectAreaRadius){
		Collider[] slimes = Physics.OverlapSphere(model.position, effectAreaRadius);
		foreach (Collider slime in slimes){
			if(slime.transform.parent.tag == target.parent.tag){
				SlimeHealth h = slime.transform.parent.GetComponent<SlimeHealth>();

				float distanceFromSphereCentre = (slime.transform.position - model.position).sqrMagnitude;
				h.TakeDamage(DamageWithRadius(attackDamage, distanceFromSphereCentre, effectAreaRadius));
			}
		}
		GetComponent<SlimeHealth>().TakeDamage(GetComponent<SlimeHealth>().getCurrentHealth());
	}

	private float DamageWithRadius(float attackDamage,float distanceFromCentre, float sphereRadius){
		float radius = Mathf.Pow(sphereRadius, 2);
		float areaDamage = ((radius - distanceFromCentre) / radius) * attackDamage;
		if(areaDamage < 0)
			areaDamage = 0;
			
		return Mathf.Ceil(areaDamage);
	}
}
