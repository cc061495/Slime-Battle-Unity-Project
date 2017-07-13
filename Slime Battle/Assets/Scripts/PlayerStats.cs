using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
	
	public static int Cost;
	public int startCost = 10;

	void Start(){
		Cost = startCost;
	}
}
