/* Copyright (c) cc061495 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Guardian : MonoBehaviour {
	public SphereCollider sphereCollider;
	private SlimeClass slime;

	public void SpellingGuardianBuff(SlimeClass _slime){
		slime = _slime;
		sphereCollider.enabled = true;
		sphereCollider.radius = slime.areaEffectRadius;
	}

	public float GetDamageReducedPercentage(){
		return slime.damageReducedPercentage;
	}
}
