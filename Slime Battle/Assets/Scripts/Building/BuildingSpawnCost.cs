using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawnCost : MonoBehaviour {

	SlimeClass slime;
	GameManager gameManager;

	public void StartSpawnCostLoop(){
		gameManager = GameManager.Instance;
		slime = GetComponent<Slime>().GetSlimeClass();
		InvokeRepeating("SpawningCost", 0, slime.actionCoolDown);
	}

	private void SpawningCost(){
		if(gameManager.currentState == GameManager.State.battle_start)
			PlayerStats.Instance.CostToSpawn(slime.moneyCanBeSpawned);
		else if(gameManager.currentState == GameManager.State.battle_end)
			CancelInvoke("SpawningCost");
	}
}
