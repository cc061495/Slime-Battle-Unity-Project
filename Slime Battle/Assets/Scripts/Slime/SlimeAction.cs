/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlimeAction : MonoBehaviour {

	private float coolDown = 0f;
	public float castTime = 0f;
	private SlimeClass slime;
	private Transform target, agent;
	private SlimeMovement movement;
	private SlimeHealth health;
	private SlimeHealth tarHealth;
	PhotonView photonView;

	void Start(){
		photonView = GetComponent<PhotonView>();
		slime = GetComponent<Slime>().GetSlimeClass();
		agent = GetComponent<Slime>().GetAgent();
		movement = GetComponent<SlimeMovement>();
		health = GetComponent<SlimeHealth>();
	}

	public void Action(){
		if (coolDown <= 0f && health.currentHealth > 0) {
			if (slime.isMeleeAttack){
				MeleeAttack (slime.attackDamage);
				if(slime.isInvisible)
					GetComponent<ShadowAppear>().Appear();
			}
			else if (slime.isRangedAttack){
				photonView.RPC("RangedAttack",PhotonTargets.All, slime.attackDamage);
			}
			else if(slime.isHealing){
				Healing (slime.healPercentage * tarHealth.startHealth);
				
				if(tarHealth.currentHealth/tarHealth.startHealth > 0.9)
					movement.FindTheTargetAgain();
			}
			else if(slime.isAreaEffectHealing){
				AreaEffectHealing(slime.healingPoint, slime.areaEffectRadius, target.position);

				if(tarHealth.currentHealth/tarHealth.startHealth > 0.9)
					movement.FindTheTargetAgain();
			}
			else if(slime.isAreaEffectDamage){
				AreaEffectDamage(slime.attackDamage, slime.areaEffectRadius, target.position, true);
			}
			else if(slime.isExplosion){
				AreaEffectDamage(slime.attackDamage, slime.areaEffectRadius, agent.position, true);
				/* Explosion Effect */
				health.SuddenDeath();
			}
			else if(slime.isMagicalAreaEffectDamage){
				/* Cast time delay */
				if(castTime <= 0f){
					AreaEffectDamage(slime.attackDamage, slime.areaEffectRadius, target.position, false);
					castTime = slime.castTime;
				}
				else{
					castTime -= Time.deltaTime;
					return;
				}
			}
			coolDown = slime.actionCoolDown;
		}
		else
			coolDown -= Time.deltaTime;
	}

	public void SetTarget(Transform _target){
		target = _target;
		tarHealth = target.root.GetComponent<SlimeHealth>();
	}
		
	private void MeleeAttack(float attackDamage){
		tarHealth.TakeDamage(attackDamage);
	}

	private void AreaEffectDamage(float attackDamage, float effectAreaRadius, Vector3 center, bool damageReduceWithDistance){
		List<Transform> enemyTeam = GameManager.Instance.GetEnemies(transform)
										.Where(x => DistanceCalculate(center, x.position) <= effectAreaRadius*effectAreaRadius).ToList();

		for(int i=0;i<enemyTeam.Count;i++){
			float dmg = attackDamage;
			if(damageReduceWithDistance){
				float distanceFromCentre = DistanceCalculate(enemyTeam[i].position, center);
				//explosion constant(higher = lower damage, lower = higher damage received)
				dmg = attackDamage - distanceFromCentre * 0.15f;
				//Debug.Log("Distance: " + distanceFromCentre + " Damage: " + areaDamage);
				if(dmg < 0)
					continue;	//if the damage is lower than 0, just skip it
			}
			enemyTeam[i].root.GetComponent<SlimeHealth>().TakeDamage(dmg);
		}	
	}

	private void AreaEffectHealing(float healingPoint, float effectAreaRadius, Vector3 center){
		List<Transform> myTeam = GameManager.Instance.GetMyTeamWithoutBuilding(transform)
									.Where(x => x.root != transform)
									.Where(x => DistanceCalculate(x.position, center) <= effectAreaRadius*effectAreaRadius).ToList();

		for(int i=0;i<myTeam.Count;i++)
			myTeam[i].root.GetComponent<SlimeHealth>().TakeHealing(healingPoint);
	}

	[PunRPC]
	private void RangedAttack(float attackDamage){
		Bullet bullet = GetComponent<RangerShoot>().ShootingBullet().GetComponent<Bullet>();
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

		float magnitude = distance.x * distance.x +
						  distance.y * distance.y +
						  distance.z * distance.z;
		return magnitude;
	}
}