using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCancel : MonoBehaviour {

    void OnMouseDown(){
        if(GameManager.Instance.currentState == GameManager.State.battle_start){
			BuildingUI.Instance.Show(transform, transform.root.GetComponent<SlimeHealth>());
            AudioManager.instance.Play("Tap");
		}
    }
}
