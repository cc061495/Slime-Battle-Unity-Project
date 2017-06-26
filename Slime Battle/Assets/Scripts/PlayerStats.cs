using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

	public enum State{idle, building, battle};
	public static State currentState;
	public static int Cost;
	public int startCost = 10;


	// Use this for initialization
	void Start () {
		currentState = State.idle;	
	}
}
