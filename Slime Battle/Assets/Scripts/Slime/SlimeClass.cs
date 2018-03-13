/* Copyright (c) cc061495 */
using System.Collections;
using UnityEngine;

public class SlimeClass{
	
	public float startHealth{get;private set;}			//Slime's health
	public float attackDamage{get;private set;}			//Slime's attack damage
	public float actionCoolDown{get; private set;}		//Slime's action cool down time
	public float castTime{get; private set;}			//Slime's casting time
	public float movemonetSpeed{get; private set;}		//Slime's movement speed
	public float actionRange{get; private set;}			//Slime's action range
	public float healPercentage{get; private set;}		//Slime's healing effect in percentage
	public float healingPoint{get; private set;}		//Slime's healing point
	public float scaleRadius{get; private set;}			//Slime's scale radius
	public float areaEffectRadius{get; private set;}
	public float detectRadius{get; private set;}
	public float damageReducedPercentage{get; private set;}
	public float slowDownPercentage{get; private set;}
	/* slime > building */
	public int killingPriority{get; private set;}
	/* melee attack > ranged attack > healer > invisible */
	public int healingPriority{get; private set;}
	/* healer + invisible > ranged attack > melee attack > building */
	public int classPriority{get; private set;}
	public int moneyCanBeSpawned{get; private set;}
	public float turnSpeed = 3f;

	public bool isMeleeAttack{get; private set;}
	public bool isRangedAttack{get; private set;}
	public bool isHealing{get; private set;}
	public bool isAreaEffectHealing{get; private set;}
	public bool isAreaEffectDamage{get; private set;}
	public bool isExplosion{get; private set;}
	public bool isBuilding{get; private set;}
	public bool isMagicalAreaEffectDamage{get; private set;}
	public bool isGuardianBuff{get; private set;}
	public bool isInvisible{get; set;}
	public bool isCleavable{get; private set;}
	public bool isSummoner{get; private set;}
	public bool canSpawnInBattle{get; private set;}
	public bool canSpawnMoney{get; private set;}
	public bool isNetworkTransfer{get; private set;}
	public bool canCarve{get; private set;}
	public bool canSlowDown{get; private set;}

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
				/* Network */
				isNetworkTransfer = true;
				break;

			case "Giant":
				/* Slime Properties */
				startHealth 	= 100 ;
				movemonetSpeed 	= 3f ;
				actionRange 	= 1 ;
				scaleRadius 	= 1.5f ;
				/* Slime Priority */
				healingPriority = 1 ;
				killingPriority = 1 ;
				classPriority 	= 3 ;
				/* Ability */
				isAreaEffectDamage	= true ;
				attackDamage 		= 4 ;
				areaEffectRadius 	= 3.5f ;
				actionCoolDown 		= 1f ;
				/* Network */
				isNetworkTransfer = true;
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
				/* Network */
				isNetworkTransfer = true;
				break;

			case "Healer":
				/* Slime Properties */
				startHealth 	= 10 ;
				movemonetSpeed 	= 5 ;
				actionRange 	= 7f ;
				scaleRadius 	= 0.5f ;
				/* Slime Priority */
				healingPriority = 3 ;
				killingPriority = 1 ;
				classPriority 	= 1 ;
				/* Ability */
				isHealing 		= true ;
				healPercentage 	= 0.08f ;
				actionCoolDown  = 0.5f ;
				/* Network */
				isNetworkTransfer = true;
				break;

			case "Bomber":
				/* Slime Properties */
				startHealth 	= 5 ;
				movemonetSpeed 	= 7 ;
				actionRange 	= 0.7f ;
				scaleRadius 	= 0.5f ;
				/* Slime Priority */
				healingPriority = 2 ;
				killingPriority = 1 ;
				classPriority 	= 3 ; 
				/* Ability */
				isExplosion	 		= true ;
				attackDamage 		= 6.5f ;
				areaEffectRadius 	= 8 ;
				actionCoolDown 		= -1 ;
				/* Network */
				isNetworkTransfer = true;
				break;

			case "Wall":
				/* Building Properties */
				startHealth 	= 30 ;
				scaleRadius 	= 1.5f ;
				/* Slime Priority */
				healingPriority = 0 ;
				killingPriority = 2 ;
				classPriority 	= 4 ;
				/* Ability */
				isBuilding 		= true ;
				canCarve		= true ;
				/* Network */
				isNetworkTransfer = false;
				break;

			case "Ore":
				/* Building Properties */
				startHealth 	= 10 ;
				scaleRadius 	= 1.3f ;
				/* Slime Priority */
				healingPriority = 0 ;
				killingPriority = 2 ;
				classPriority 	= 4 ;
				/* Ability */
				isBuilding 		  = true ;
				canCarve		  = true ;
				canSpawnMoney	  = true ;
				moneyCanBeSpawned = 10 ;
				actionCoolDown 	  = 5 ;
				/* Network */
				isNetworkTransfer = false;
				break;

			case "Mine":
				/* Building Properties */
				startHealth 	= 20 ;
				scaleRadius 	= 1f ;
				/* Slime Priority */
				healingPriority = 0 ;
				killingPriority = 2 ;
				classPriority 	= 4 ;
				/* Ability */
				isBuilding 		  = true ;
				isInvisible		  = true ;
				detectRadius	  = 2f ;
				attackDamage 	  = 8 ;
				areaEffectRadius  = 6 ;
				actionCoolDown 	  = -1 ;
				/* Network */
				isNetworkTransfer = true;
				break;

			case "Splitter":
				/* Slime Properties */
				startHealth 	= 35 ;
				movemonetSpeed 	= 3f ;
				actionRange 	= 1 ;
				scaleRadius 	= 1f ;
				/* Slime Priority */
				healingPriority = 1 ;
				killingPriority = 1 ;
				classPriority 	= 3 ;
				/* Ability */
				isCleavable			= true ;
				isMeleeAttack 		= true ;
				attackDamage 		= 5 ;
				actionCoolDown 		= 0.5f ;
				/* Network */
				isNetworkTransfer = true;
				break;

			case "Splitter_Medium":
				/* Slime Properties */
				startHealth 	= 18 ;
				movemonetSpeed 	= 5f ;
				actionRange 	= 1 ;
				scaleRadius 	= 0.75f ;
				/* Slime Priority */
				healingPriority = 1 ;
				killingPriority = 1 ;
				classPriority 	= 3 ;
				/* Ability */
				isCleavable			= true ;
				isMeleeAttack 		= true ;
				canSpawnInBattle	= true ;
				attackDamage 		= 3 ;
				actionCoolDown 		= 0.4f ;
				/* Network */
				isNetworkTransfer = true;
				break;

			case "Splitter_Small":
				/* Slime Properties */
				startHealth 	= 12 ;
				movemonetSpeed 	= 7f ;
				actionRange 	= 1 ;
				scaleRadius 	= 0.5f ;
				/* Slime Priority */
				healingPriority = 1 ;
				killingPriority = 1 ;
				classPriority 	= 3 ;
				/* Ability */
				isMeleeAttack 		= true ;
				canSpawnInBattle	= true ;
				attackDamage 		= 1 ;
				actionCoolDown 		= 0.3f ;
				/* Network */
				isNetworkTransfer = true;
				break;

			case "Wizard":
				/* Slime Properties */
				startHealth 	= 13 ;
				movemonetSpeed 	= 4 ;
				actionRange 	= 8.5f ;
				scaleRadius 	= 0.55f ;
				/* Slime Priority */
				healingPriority = 2 ;
				killingPriority = 1 ;
				classPriority 	= 2 ;
				/* Ability */
				isMagicalAreaEffectDamage = true ;
				attackDamage 			  = 4f ;
				areaEffectRadius 		  = 3 ;
				//actionCoolDown 			  = 0 ;
				castTime				  = 2f ;
				/* Network */
				isNetworkTransfer = true;
				break;

			case "Summoner":
				/* Slime Properties */
				startHealth 	= 15 ;
				scaleRadius 	= 0.7f ;
				/* Slime Priority */
				healingPriority = 3 ;
				killingPriority = 1 ;
				classPriority 	= 1 ;
				/* Ability */
				isSummoner		 = true ;
				actionCoolDown   = 7 ;
				/* Network */
				isNetworkTransfer = true;
				break;	

			case "Summoner_s":
				/* Slime Properties */
				startHealth 	= 10 ;
				movemonetSpeed 	= 5f ;
				actionRange 	= 0.8f ;
				scaleRadius 	= 0.45f ;
				/* Slime Priority */
				healingPriority = 1 ;
				killingPriority = 1 ;
				classPriority 	= 3 ;
				/* Ability */
				isMeleeAttack 		= true ;
				canSpawnInBattle	= true ;
				attackDamage 		= 1f ;
				actionCoolDown 		= 0.8f ;
				/* Network */
				isNetworkTransfer = true;
				break;

			case "Priest":
				/* Slime Properties */
				startHealth 	= 10 ;
				movemonetSpeed 	= 5 ;
				actionRange 	= 7f ;
				scaleRadius 	= 0.5f ;
				/* Slime Priority */
				healingPriority = 3 ;
				killingPriority = 1 ;
				classPriority 	= 1 ;
				/* Ability */
				isAreaEffectHealing	= true ;
				areaEffectRadius 	= 5 ;
				healingPoint 		= 3 ;
				actionCoolDown  	= 1.5f ;
				/* Network */
				isNetworkTransfer = true;
				break;

			case "Guardian":
				/* Slime Properties */
				startHealth 	= 30 ;
				movemonetSpeed 	= 3 ;
				actionRange 	= 1f ;
				scaleRadius 	= 0.7f ;
				/* Slime Priority */
				healingPriority = 1 ;
				killingPriority = 1 ;
				classPriority 	= 1 ;
				/* Ability */
				isMeleeAttack			= true ;
				attackDamage			= 1f ;
				actionCoolDown  		= 1f ;
				isGuardianBuff			= true ;
				areaEffectRadius 		= 8 ;
				damageReducedPercentage = 0.6f ;
				/* Network */
				isNetworkTransfer = true;
				break;

			case "Shadow":
				/* Building Properties */
				startHealth 	= 25 ;
				movemonetSpeed 	= 8f ;
				actionRange 	= 1f ;
				scaleRadius 	= 0.7f ;
				/* Slime Priority */
				healingPriority = 4 ;
				killingPriority = 1 ;
				classPriority 	= 1 ;
				/* Ability */
				isInvisible		  = true ;
				isMeleeAttack	  = true ;
				attackDamage 	  = 5f ;
				actionCoolDown 	  = 0.2f ;
				/* Network */
				isNetworkTransfer = true;
				break;

			case "Turret":
				/* Building Properties */
				startHealth 	= 15 ;
				scaleRadius 	= 1f ;
				actionRange 	= 9f ;
				/* Slime Priority */
				healingPriority = 0 ;
				killingPriority = 2 ;
				classPriority 	= 4 ;
				/* Ability */
				isBuilding 		  = true ;
				canCarve	      = true ;
				isRangedAttack	  = true ;
				attackDamage 	  = 1.5f ;
				actionCoolDown 	  = 1f ;
				/* Network */
				isNetworkTransfer = true;
				break;

			case "Snowman":
				/* Building Properties */
				startHealth 	= 15 ;
				scaleRadius 	= 2.5f ;
				actionRange 	= 5.5f ;
				/* Slime Priority */
				healingPriority = 0 ;
				killingPriority = 2 ;
				classPriority 	= 4 ;
				/* Ability */
				isBuilding 		  	= true ;
				canCarve	      	= true ;
				isRangedAttack	  	= true ;
				canSlowDown		  	= true ;
				attackDamage 	  	= 1f ;
				areaEffectRadius  	= 5 ;
				slowDownPercentage 	= 0.5f ;
				actionCoolDown 	  	= 2f ;
				/* Network */
				isNetworkTransfer = true;
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