/* Copyright (c) cc061495 */
using System.Collections;
using UnityEngine;

public class SlimeClass{
	
	public float startHealth{get;private set;}			//Slime's health
	public float attackDamage{get;private set;}			//Slime's attack damage
	public float actionSpeed{get; private set;}			//Attack count per second
	public float movemonetSpeed{get; private set;}		//Slime's movement speed
	public float actionRange{get; private set;}			//Slime's action range
	public float healingPoint{get; private set;}		//Slime's healing point
	public float scaleRadius{get; private set;}			//Slime's scale radius
	public float areaEffectRadius{get; private set;}
	public int healingPriority{get; private set;}
	public float turnSpeed = 3f;

	public bool isMeleeAttack{get; private set;}
	public bool isRangedAttack{get; private set;}
	public bool isHealing{get; private set;}
	public bool isAreaEffectDamage{get; private set;}
	public bool isExplosion{get; private set;}

	public SlimeClass(string slimeName){
		switch (slimeName){
			case "Slime":
				/* Slime Properties */
				startHealth 	= 32 ;
				movemonetSpeed 	= 5 ;
				actionRange 	= 1 ;
				scaleRadius 	= 0.5f ;
				healingPriority = 1 ;
				/* Ability */
				isMeleeAttack 	= true;
				attackDamage 	= 2;
				actionSpeed 	= 2;
				break;

			case "Giant":
				/* Slime Properties */
				startHealth 	= 100 ;
				movemonetSpeed 	= 3.5f ;
				actionRange 	= 2 ;
				scaleRadius 	= 1 ;
				healingPriority = 1 ;
				/* Ability */
				isAreaEffectDamage	= true ;
				attackDamage 		= 3 ;
				areaEffectRadius 	= 2 ;
				actionSpeed 		= 1.5f ;
				break;

			case "Ranger":
				/* Slime Properties */
				startHealth 	= 12 ;
				movemonetSpeed	= 5 ;
				actionRange 	= 9 ;
				scaleRadius 	= 0.5f ;
				healingPriority = 2 ;
				/* Ability */
				isRangedAttack 	= true ;
				attackDamage 	= 2 ;
				actionSpeed 	= 1 ;
				break;

			case "Healer":
				/* Slime Properties */
				startHealth 	= 10 ;
				movemonetSpeed 	= 5 ;
				actionRange 	= 6.5f ;
				scaleRadius 	= 0.5f ;
				healingPriority = 3 ;
				/* Ability */
				isHealing 		= true ;
				healingPoint 	= 0.5f ;
				actionSpeed 	= 5 ;
				break;

			case "Bomber":
				/* Slime Properties */
				startHealth 	= 10 ;
				movemonetSpeed 	= 8 ;
				actionRange 	= 0.5f ;
				scaleRadius 	= 0.5f ;
				healingPriority = 2 ;
				/* Ability */
				isExplosion	 		= true ;
				attackDamage 		= 8 ;
				areaEffectRadius 	= 5 ;
				actionSpeed 		= 0.01f ;
				break;

			default:
				/* Slime Properties */
				startHealth 	= 10 ;
				movemonetSpeed 	= 5 ;
				actionRange 	= 1 ;
				scaleRadius		= 0.5f ;
				healingPriority = 1 ;
				/* Ability */
				isMeleeAttack 	= true ;
				attackDamage 	= 2 ;
				actionSpeed 	= 1 ;
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