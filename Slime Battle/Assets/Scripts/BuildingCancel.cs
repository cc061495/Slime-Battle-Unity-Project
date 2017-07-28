using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCancel : MonoBehaviour {

	PhotonView photonView;
	Transform building;
	SlimeHealth buildHealth;

	void Start(){
		building = transform.parent;
		photonView = building.GetComponent<PhotonView>();
		buildHealth = building.GetComponent<SlimeHealth>();
	}

    void OnMouseDown(){
        if(GameManager.Instance.currentState == GameManager.State.battle_start && photonView.isMine){
			buildHealth.TakeDamage(buildHealth.currentHealth);
		}
    }
}
