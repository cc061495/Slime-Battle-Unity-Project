/* Copyright (c) cc061495 */
using System.Collections;
using UnityEngine;

public class SlimeClass{

	private float startHealth;		//Slime's health
	private float attackDamage;		//Slime's attack damage
	private float actionSpeed;		//Attack count per second
	private float movemonetSpeed;	//Slime's movement speed
	private float actionRange;		//Slime's action range
	private float healingPoint;		//Slime's healing point
	private float scaleRadius;		//Slime's scale radius
	private float areaEffectRadius;
	private int healingPriority;
	private float turnSpeed = 3f;
	private bool meleeAttack = false, rangedAttack = false, healing = false, areaEffectDamage = false, explosion = false;

	public SlimeClass(string slimeName){
		switch (slimeName){
			case "Slime":
				//Health, Damage, Attack Speed, Movement Speed, Action Range, Scale Radius
				setStartHealth	( 32 );
				setMovementSpeed( 5 );
				setActionRange	( 1 );
				setScaleRadius	( 0.5f );
				setHealingPriority ( 1 );

				SetMeleeAttack	( true );
				setAttackDamage	( 2 );
				setActionSpeed	( 2 );
				break;

			case "Giant":
				setStartHealth	( 100 );
				setMovementSpeed( 3.5f );
				setActionRange	( 2 );
				setScaleRadius	( 1 );
				setHealingPriority ( 1 );

				SetAreaEffectDamage ( true );
				setAttackDamage	( 3 );
				setAreaEffectRadius( 2f );
				setActionSpeed	( 1.5f );
				break;

			case "Ranger":
				setStartHealth	( 12 );
				setMovementSpeed( 5 );
				setActionRange	( 9 );
				setScaleRadius	( 0.5f );
				setHealingPriority ( 2 );

				SetRangedAttack	( true );
				setAttackDamage	( 2 );
				setActionSpeed	( 1 );
				break;

			case "Healer":
				setStartHealth	( 10 );
				setMovementSpeed( 5 );
				setActionRange	( 6.5f );
				setScaleRadius	( 0.5f );
				setHealingPriority ( 3 );

				SetHealing		( true );
				setHealingPoint	( 0.5f );
				setActionSpeed	( 5 );
				break;

			case "Bomber":
				setStartHealth	( 10 );
				setMovementSpeed( 8 );
				setActionRange	( 0.5f );
				setScaleRadius	( 0.5f );
				setHealingPriority ( 2 );

				SetExplosion ( true );
				setAttackDamage	( 8 );
				setAreaEffectRadius( 5 );
				setActionSpeed	( 0.01f );
				break;

			default:
				setStartHealth	( 10 );
				setMovementSpeed( 5 );
				setActionRange	( 1 );
				setScaleRadius	( 0.5f );
				setHealingPriority ( 1 );

				SetMeleeAttack	( true );
				setAttackDamage	( 2 );
				setActionSpeed	( 1 );
				break;
		}
	}

	private void setStartHealth(float _health){
		startHealth = _health;
	}

	public float getStartHealth(){
		return startHealth;
	}

	private void setAttackDamage(float _damage){
		attackDamage = _damage;
	}

	public float getAttackDamage(){
		return attackDamage;
	}

	private void setActionSpeed(float _attackSpeed){
		actionSpeed = _attackSpeed;
	}

	public float getActionSpeed(){
		return actionSpeed;
	}

	private void setMovementSpeed(float _movementSpeed){
		movemonetSpeed = _movementSpeed;
	}

	public float getMovementSpeed(){
		return movemonetSpeed;
	}

	private void setActionRange(float _actionRange){
		actionRange = _actionRange;
	}

	public float getActionRange(){
		return actionRange;
	}

	private void setHealingPoint(float _healingPoint){
		healingPoint = _healingPoint;
	}

	public float getHealingPoint(){
		return healingPoint;
	}

	private void setScaleRadius(float _scaleRadius){
		scaleRadius = _scaleRadius;
	}

	public float getScaleRadius(){
		return scaleRadius;
	}

	private void setAreaEffectRadius(float _areaEffectRadius){
		areaEffectRadius = _areaEffectRadius;
	}

	public float getAreaEffectRadius(){
		return areaEffectRadius;
	}

	private void setHealingPriority(int priority){
		healingPriority = priority;
	}

	public int getHealingPriority(){
		return healingPriority;
	}

	public float getTurnSpeed(){
		return turnSpeed;
	}

	private void SetMeleeAttack(bool melee){
		meleeAttack = melee;
	}

	private void SetRangedAttack(bool ranged){
		rangedAttack = ranged;
	}

	private void SetHealing(bool heal){
		healing = heal;
	}

	private void SetAreaEffectDamage(bool area){
		areaEffectDamage = area;
	}

	private void SetExplosion(bool explode){
		explosion = explode;
	}

	public bool isMeleeAttack(){
		return meleeAttack;
	}

	public bool isRangedAttack(){
		return rangedAttack;
	}

	public bool isHealing(){
		return healing;
	}

	public bool isAreaEffectDamage(){
		return areaEffectDamage;
	}

	public bool isExplosion(){
		return explosion;
	}
}
