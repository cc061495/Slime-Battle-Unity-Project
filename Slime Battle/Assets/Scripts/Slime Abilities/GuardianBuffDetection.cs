/* Copyright (c) cc061495 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardianBuffDetection : MonoBehaviour {
	public Guardian guardian;

	GameManager gameManager;
	Transform rootObject;
    List<SlimeHealth> buffList = new List<SlimeHealth>();

	void Awake(){
		gameManager = GameManager.Instance;
		rootObject = transform.root;
	}

	void OnTriggerEnter(Collider other){
		Slime s = other.transform.root.GetComponent<Slime>();
		if(s == null)
			return;

		Transform targetSlime = other.transform.root;
		SlimeClass slimeClass = s.GetSlimeClass();
		
		if(targetSlime.tag == rootObject.tag && !slimeClass.isBuilding){
			SlimeHealth h = targetSlime.GetComponent<SlimeHealth>();
			buffList.Add(h);
			h.buffIndex++;
			h.SetDamageReduced(guardian.GetDamageReducedPercentage());
		}
	}

	void OnTriggerExit(Collider other){
		Slime s = other.transform.root.GetComponent<Slime>();
		if(s == null)
			return;		
		
		Transform targetSlime = other.transform.root;
		SlimeClass slimeClass = s.GetSlimeClass();

		if(targetSlime.tag == rootObject.tag && !slimeClass.isBuilding){
			SlimeHealth h = targetSlime.GetComponent<SlimeHealth>();
			h.buffIndex--;
			if(h.buffIndex == 0){
				buffList.Remove(h);
				h.SetupDefaultDamageReduced();
			}
		}
	}

	void OnDestroy(){
		if(gameManager.currentState == GameManager.State.battle_start){
			for(int i=0; i < buffList.Count; i++){
				if(buffList[i] != null){
					buffList[i].buffIndex--;
					if(buffList[i].buffIndex == 0)
						buffList[i].SetupDefaultDamageReduced();
				}
			}
		}
	}
}
