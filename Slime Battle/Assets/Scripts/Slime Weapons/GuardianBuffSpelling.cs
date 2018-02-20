/* Copyright (c) cc061495 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardianBuffSpelling : MonoBehaviour {
	public SphereCollider sphereCollider;
	private Slime s;
	private SlimeClass slime;

	GameManager gameManager;
	Transform rootObject;
    List<SlimeHealth> buffList = new List<SlimeHealth>();

	void Start(){
		gameManager = GameManager.Instance;
		rootObject = transform.root;
		s = rootObject.GetComponent<Slime>();
		slime = s.GetSlimeClass();
		sphereCollider.radius = slime.areaEffectRadius;
	}

	void OnTriggerEnter(Collider other){
		Transform targetSlime = other.transform.root;
		if(targetSlime.tag == rootObject.tag){
			SlimeHealth h = targetSlime.GetComponent<SlimeHealth>();
			buffList.Add(h);
			h.buff++;
			h.SetDamageReduced(slime.damageReducedPercentage);
		}
	}

	void OnTriggerExit(Collider other){
		Transform targetSlime = other.transform.root;
		if(targetSlime.tag == rootObject.tag){
			SlimeHealth h = targetSlime.GetComponent<SlimeHealth>();
			h.buff--;
			if(h.buff == 0){
				buffList.Remove(h);
				h.SetupDefaultDamageReduced();
			}
		}
	}

	void OnDestroy(){
		if(gameManager.currentState == GameManager.State.battle_start){
			for(int i=0; i < buffList.Count; i++){
				if(buffList[i] != null){
					buffList[i].buff--;
					if(buffList[i].buff == 0)
						buffList[i].SetupDefaultDamageReduced();
				}
			}
		}
	}
}
