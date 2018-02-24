using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerShoot : MonoBehaviour {

	[SerializeField]
	private Transform firePoint;
	[SerializeField]
	private GameObject rangedWeaponPrefab;

	public GameObject ShootingBullet(){
		return((GameObject)Instantiate (rangedWeaponPrefab, firePoint.position, firePoint.rotation));
	}
}
