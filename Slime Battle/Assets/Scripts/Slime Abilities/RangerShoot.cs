using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerShoot : MonoBehaviour {

	[SerializeField]
	private Transform firePoint;

	ObjectPooler objectPooler;

	void Start(){
		objectPooler = ObjectPooler.Instance;
	}

	public GameObject ShootingBullet(){
		return(objectPooler.SpawnFromPool("Bullet", firePoint.position, firePoint.rotation));
	}
}
