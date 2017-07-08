/* Copyright (c) cc061495 */
using System.Collections;
using UnityEngine;

public class SlimeClass{

	private float startHealth;		//Slime's health
	private float attackDamage;		//Slime's attack damage
	private float attackSpeed;		//Attack count per second
	private float movemonetSpeed;	//Slime's movement speed
	private float actionRange;		//Slime's action range
	private float scaleRadius;		//Slime's scale radius
	private float turnSpeed = 3f;
	private bool meleeAttack = false, rangedAttack = false;

	public SlimeClass(string slimeName){
		switch (slimeName){
			case "Slime":
				//Health, Damage, Attack Speed, Movement Speed, Action Range, Scale Radius
				setStartHealth	( 32 );
				setAttackDamage	( 2 );
				setAttackSpeed	( 2 );
				setMovementSpeed( 5 );
				setActionRange	( 1 );
				setScaleRadius	( 0.5f );

				SetMeleeAttack	( true );
				break;

			case "Giant":
				setStartHealth	( 160 );
				setAttackDamage	( 5 );
				setAttackSpeed	( 2 );
				setMovementSpeed( 3.5f );
				setActionRange	( 1 );
				setScaleRadius	( 1 );

				SetMeleeAttack	( true );
				break;

			case "Ranger":
				setStartHealth	( 12 );
				setAttackDamage	( 2 );
				setAttackSpeed	( 1 );
				setMovementSpeed( 5 );
				setActionRange	( 9 );
				setScaleRadius	( 0.5f );

				SetRangedAttack	( true );
				break;

			default:
				setStartHealth	( 10 );
				setAttackDamage	( 2 );
				setAttackSpeed	( 1 );
				setMovementSpeed( 5 );
				setActionRange	( 1 );
				setScaleRadius	( 0.5f );

				SetMeleeAttack	(true);
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

	private void setAttackSpeed(float _attackSpeed){
		attackSpeed = _attackSpeed;
	}

	public float getAttackSpeed(){
		return attackSpeed;
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

	private void setScaleRadius(float _scaleRadius){
		scaleRadius = _scaleRadius;
	}

	public float getScaleRadius(){
		return scaleRadius;
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

	public bool isMeleeAttack(){
		return meleeAttack;
	}

	public bool isRangedAttack(){
		return rangedAttack;
	}
}
