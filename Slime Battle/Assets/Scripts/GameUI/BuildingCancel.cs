﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCancel : MonoBehaviour {

    void OnMouseDown(){
        if(GameManager.Instance.currentState == GameManager.State.battle_start){
			BuildingUI.Instance.Show(transform, transform.parent.GetComponent<SlimeHealth>());
		}
    }
}
