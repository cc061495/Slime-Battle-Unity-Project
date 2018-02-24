using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSplite : MonoBehaviour {

	public string nextPrefabToSplite;
	const int minRan = 1, maxRan = 4;
	private int instantiateNum;

	public void DoTheSlimeSplite(Vector3 pos){
		instantiateNum = Random.Range(minRan, maxRan);
		
		for (int i = 0; i < instantiateNum; i++){
			PhotonNetwork.Instantiate(nextPrefabToSplite,pos,Quaternion.identity,0);
		}
	}
}
