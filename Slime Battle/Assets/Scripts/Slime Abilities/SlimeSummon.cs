using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSummon : MonoBehaviour {

	public string prefabToSummon;
	GameManager gameManager;
	SlimeClass slime;
	Vector3 agentPos;

	public void StartSummonLoop(){
		gameManager = GameManager.Instance;
		slime = GetComponent<Slime>().GetSlimeClass();
		agentPos = GetComponent<Slime>().GetAgent().position;
		
		InvokeRepeating("Summon", 1f, slime.actionCoolDown);
	}

	public void Summon(){
		if(gameManager.currentState == GameManager.State.battle_start)
			PhotonNetwork.Instantiate(prefabToSummon, agentPos + GetSpawnPos(), Quaternion.identity, 0);
		else if(gameManager.currentState == GameManager.State.battle_end)
			CancelInvoke("Summon");
	}

	private Vector3 GetSpawnPos(){
		Vector3 pos = Vector3.zero;

		if(transform.tag == "Team_RED")
			pos = new Vector3(0,0,1.5f);
		else if(transform.tag == "Team_BLUE")
			pos = new Vector3(0,0,-1.5f);
			
		return pos;
	}
}
