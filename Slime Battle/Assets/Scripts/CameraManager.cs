/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    void Start () {
        if (PhotonNetwork.isMasterClient){
            transform.position = new Vector3(1.5f, 40f, -20f);
			transform.rotation = Quaternion.Euler(70f, 0f, 0f);
        }
        else{
			transform.position = new Vector3(1.5f, 40f, 20f);
			transform.rotation = Quaternion.Euler(110f, 0f, 180f);
		}
    }
}
