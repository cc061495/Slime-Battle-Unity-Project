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
	private Transform target, model;
	private SlimeMovement movement;
	private SlimeHealth health;
	private SlimeHealth tarHealth;
	PhotonView photonView;

	void Start(){
		photonView = GetComponent<PhotonView>();
		slime = GetComponent<Slime>().GetSlimeClass();
		model = GetComponent<Slime>().GetModel();
		movement = GetComponent<SlimeMovement>();
		health = GetComponent<SlimeHealth>();
	}

	public void Action(){
		if (coolDown <= 0f) {
			// if(target == null)
			// 	SetTarget();

			if (slime.isMeleeAttack){
				MeleeAttack (slime.attackDamage);
			}
			else if (slime.isRangedAttack){
				photonView.RPC("RangedAttack",PhotonTargets.All, slime.attackDamage);
			}
			else if(slime.isHealing){
				if(tarHealth.currentHealth >= tarHealth.startHealth){
					movement.TargetSearching();
					SetTarget();
				}
				else{
					Healing (slime.healPercentage);
				}
			}
			else if(slime.isAreaEffectDamage){
				AreaEffectDamage(slime.attackDamage, slime.areaEffectRadius, target.position);
			}
			else if(slime.isExplosion){
				AreaEffectDamage(slime.attackDamage, slime.areaEffectRadius, model.position);
				health.TakeDamage(health.currentHealth);
			}
			
			coolDown = slime.actionCoolDown;
		}
		else
			coolDown -= Time.deltaTime;
	}

	public void SetTarget(){
		target = movement.GetTarget();
		if(target != null)
			tarHealth = target.parent.GetComponent<SlimeHealth>();
	}
		
	private void MeleeAttack(float attackDamage){
		tarHealth.TakeDamage(attackDamage);
	}

	private void AreaEffectDamage(float attackDamage, float effectAreaRadius, Vector3 centre){
		Collider[] slimes = Physics.OverlapSphere(centre, effectAreaRadius);
		for(int i=0;i<slimes.Length;i++){
			if(slimes[i].transform.parent.tag == target.parent.tag){
				SlimeHealth h = slimes[i].transform.parent.GetComponent<SlimeHealth>();

				float distanceFromCentre = DistanceCalculate(slimes[i].transform.position, centre);
				//explosion constant(higher = lower damage, lower = higher damage received)
				float areaDamage = attackDamage - distanceFromCentre * 0.16f;
				//Debug.Log(distanceFromCentre + " Damage: " + areaDamage);
				if(areaDamage < 0)
					areaDamage = 0;
				h.TakeDamage(areaDamage);
			}
		}
	}

	[PunRPC]
	private void RangedAttack(float attackDamage){
		GameObject bulletGO = (GameObject)Instantiate (rangedWeaponPrefab, firePoint.position, firePoint.rotation);
		SetTarget();

		Bullet bullet = bulletGO.GetComponent<Bullet>();
		if (bullet != null)
			bullet.Seek (target, attackDamage, tarHealth);
	}

	private void Healing(float healingPoint){
		tarHealth.TakeHealing(healingPoint);
	}

	private float DistanceCalculate(Vector3 pos1, Vector3 pos2){
		Vector3 distance = Vector3.zero;
		distance.x = pos1.x - pos2.x;
		distance.y = pos1.y - pos2.y;
		distance.z = pos1.z - pos2.z;

		float magnitude = distance.x * distance.x+
						  distance.y * distance.y+
						  distance.z * distance.z;
		return magnitude;
	}
}