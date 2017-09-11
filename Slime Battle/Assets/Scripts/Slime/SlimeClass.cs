/* Copyright (c) cc061495 */
using System.Collections;
using UnityEngine;

public class SlimeClass{
	
	public float startHealth{get;private set;}			//Slime's health
	public float attackDamage{get;private set;}			//Slime's attack damage
	public float actionCoolDown{get; private set;}		//Slime's action cool down time
	public float movemonetSpeed{get; private set;}		//Slime's movement speed
	public float actionRange{get; private set;}			//Slime's action range
	public float healPercentage{get; private set;}		//Slime's healing effect in percentage
	public float scaleRadius{get; private set;}			//Slime's scale radius
	public float areaEffectRadius{get; private set;}
	/* slime > building */
	public int killingPriority{get; private set;}
	/* melee attack > ranged attack > healer */
	public int healingPriority{get; private set;}
	/* healer > ranged attack > melee attack > building */
	public int classPriority{get; private set;}
	public float turnSpeed = 3f;

	public bool isMeleeAttack{get; private set;}
	public bool isRangedAttack{get; private set;}
	public bool isHealing{get; private set;}
	public bool isAreaEffectDamage{get; private set;}
	public bool isExplosion{get; private set;}
	public bool isBuilding{get; private set;}

	public SlimeClass(string slimeName){
		switch (slimeName){
			case "Slime":
				/* Slime Properties */
				startHealth 	= 25 ;
				movemonetSpeed 	= 5 ;
				actionRange 	= 1 ;
				scaleRadius 	= 0.5f ;
				/* Slime Priority */
				healingPriority = 1 ;
				killingPriority = 1 ;
				classPriority 	= 3 ;
				/* Ability */
				isMeleeAttack 	= true ;
				attackDamage 	= 2f ;
				actionCoolDown 	= 0.6f ;
				break;

			case "Giant":
				/* Slime Properties */
				startHealth 	= 100 ;
				movemonetSpeed 	= 3.5f ;
				actionRange 	= 1 ;
				scaleRadius 	= 1f ;
				/* Slime Priority */
				healingPriority = 1 ;
				killingPriority = 1 ;
				classPriority 	= 3 ;
				/* Ability */
				isAreaEffectDamage	= true ;
				attackDamage 		= 4 ;
				areaEffectRadius 	= 1.5f ;
				actionCoolDown 		= 1.5f ;
				break;

			case "Ranger":
				/* Slime Properties */
				startHealth 	= 12 ;
				movemonetSpeed	= 5 ;
				actionRange 	= 8 ;
				scaleRadius 	= 0.55f ;
				/* Slime Priority */
				healingPriority = 2 ;
				killingPriority = 1 ;
				classPriority 	= 2 ;
				/* Ability */
				isRangedAttack 	= true ;
				attackDamage 	= 2.5f ;
				actionCoolDown 	= 1f ;
				break;

			case "Healer":
				/* Slime Properties */
				startHealth 	= 10 ;
				movemonetSpeed 	= 5 ;
				actionRange 	= 6.5f ;
				scaleRadius 	= 0.5f ;
				/* Slime Priority */
				healingPriority = 3 ;
				killingPriority = 1 ;
				classPriority 	= 1 ;
				/* Ability */
				isHealing 		= true ;
				healPercentage 	= 0.1f ;
				actionCoolDown  = 0.5f ;
				break;

			case "Bomber":
				/* Slime Properties */
				startHealth 	= 10 ;
				movemonetSpeed 	= 8 ;
				actionRange 	= 0.5f ;
				scaleRadius 	= 0.5f ;
				/* Slime Priority */
				healingPriority = 2 ;
				killingPriority = 1 ;
				classPriority 	= 3 ; 
				/* Ability */
				isExplosion	 		= true ;
				attackDamage 		= 8 ;
				areaEffectRadius 	= 5 ;
				actionCoolDown 		= 100 ;
				break;

			case "Wall":
				/* Wall Properties */
				startHealth 	= 25 ;
				scaleRadius 	= 1.3f ;
				/* Slime Priority */
				healingPriority = 0 ;
				killingPriority = 2 ;
				classPriority 	= 4 ;
				/* Ability */
				isBuilding 		= true ;
				break;

			default:
				/* Slime Properties */
				startHealth 	= 10 ;
				movemonetSpeed 	= 5 ;
				actionRange 	= 1 ;
				scaleRadius		= 0.5f ;
				/* Slime Priority */
				healingPriority = 1 ;
				killingPriority = 1 ;
				classPriority 	= 3 ;
				/* Ability */
				isMeleeAttack 	= true ;
				attackDamage 	= 2 ;
				actionCoolDown 	= 1 ;
				break;
		}
	}
}
	// private void setStartHealth(float _health){
	// 	startHealth = _health;
	// }

	// public float getStartHealth(){
	// 	return startHealth;
	// }

	// private void setAttackDamage(float _damage){
	// 	attackDamage = _damage;
	// }

	// public float getAttackDamage(){
	// 	return attackDamage;
	// }

	// private void setActionSpeed(float _attackSpeed){
	// 	actionSpeed = _attackSpeed;
	// }

	// public float getActionSpeed(){
	// 	return actionSpeed;
	// }

	// private void setMovementSpeed(float _movementSpeed){
	// 	movemonetSpeed = _movementSpeed;
	// }

	// public float getMovementSpeed(){
	// 	return movemonetSpeed;
	// }

	// private void setActionRange(float _actionRange){
	// 	actionRange = _actionRange;
	// }

	// public float getActionRange(){
	// 	return actionRange;
	// }

	// private void setHealingPoint(float _healingPoint){
	// 	healingPoint = _healingPoint;
	// }

	// public float getHealingPoint(){
	// 	return healingPoint;
	// }

	// private void setScaleRadius(float _scaleRadius){
	// 	scaleRadius = _scaleRadius;
	// }

	// public float getScaleRadius(){
	// 	return scaleRadius;
	// }

	// private void setAreaEffectRadius(float _areaEffectRadius){
	// 	areaEffectRadius = _areaEffectRadius;
	// }

	// public float getAreaEffectRadius(){
	// 	return areaEffectRadius;
	// }

	// private void setHealingPriority(int priority){
	// 	healingPriority = priority;
	// }

	// public int getHealingPriority(){
	// 	return healingPriority;
	// }

	// public float getTurnSpeed(){
	// 	return turnSpeed;
	// }

	// private void SetMeleeAttack(bool melee){
	// 	meleeAttack = melee;
	// }

	// private void SetRangedAttack(bool ranged){
	// 	rangedAttack = ranged;
	// }

	// private void SetHealing(bool heal){
	// 	healing = heal;
	// }

	// private void SetAreaEffectDamage(bool area){
	// 	areaEffectDamage = area;
	// }

	// private void SetExplosion(bool explode){
	// 	explosion = explode;
	// }

	// public bool isMeleeAttack(){
	// 	return meleeAttack;
	// }

	// public bool isRangedAttack(){
	// 	return rangedAttack;
	// }

	// public bool isHealing(){
	// 	return healing;
	// }

	// public bool isAreaEffectDamage(){
	// 	return areaEffectDamage;
	// }

	// public bool isExplosion(){
	// 	return explosion;
	// }