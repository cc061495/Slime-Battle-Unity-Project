using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCancel : MonoBehaviour {

    void OnMouseDown(){
        if(GameManager.Instance.currentState == GameManager.State.battle_start){
			SlimeHealth buildHealth = transform.parent.GetComponent<SlimeHealth>();
			BuildingUI.Instance.Show(transform, buildHealth);
		}
    }
}
