using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawnCost : MonoBehaviour {

	SlimeClass slime;
	GameManager gameManager;
	PlayerStats playerStats;

	public void StartSpawnCostLoop(){
		gameManager = GameManager.Instance;
		playerStats = PlayerStats.Instance;
		slime = GetComponent<Slime>().GetSlimeClass();
		InvokeRepeating("SpawningCost", 0, slime.actionCoolDown);
	}

	private void SpawningCost(){
		if(gameManager.currentState == GameManager.State.battle_start)
			playerStats.CostToSpawn(slime.moneyCanBeSpawned);
		else if(gameManager.currentState == GameManager.State.battle_end)
			CancelInvoke("SpawningCost");
	}
}
