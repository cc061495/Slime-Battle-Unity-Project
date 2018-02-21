using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSummon : MonoBehaviour {

	public string prefabToSummon;
	GameManager gameManager;
	SlimeClass slime;
	Vector3 modelPos;

	public void StartSummonLoop(){
		gameManager = GameManager.Instance;
		slime = GetComponent<Slime>().GetSlimeClass();
		modelPos = GetComponent<Slime>().GetModel().position;
		
		InvokeRepeating("Summon", 0, slime.actionCoolDown);
	}

	public void Summon(){
		if(gameManager.currentState == GameManager.State.battle_start)
			PhotonNetwork.Instantiate(prefabToSummon, modelPos, Quaternion.identity, 0);
		else if(gameManager.currentState == GameManager.State.battle_end)
			CancelInvoke("Summon");
	}
}
