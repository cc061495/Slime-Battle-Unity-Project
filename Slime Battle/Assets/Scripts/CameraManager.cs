/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public GameObject CameraRed, CameraBlue;
    public Animator moveRed, moveBlue;

    void Awake(){
        if (PhotonNetwork.isMasterClient)
            CameraBlue.SetActive(false);
        else
            CameraRed.SetActive(false);
    }

    public void CamMove_Build(){
        if(PhotonNetwork.isMasterClient)
            moveRed.SetBool("Build", true);
        else
            moveBlue.SetBool("Build", true);
    }

    public void CamMove_Battle(){
        if(PhotonNetwork.isMasterClient)
            moveRed.SetBool("Build", false);
        else
            moveBlue.SetBool("Build", false);
    }

    // void Start () {
    //     if (PhotonNetwork.isMasterClient){
    //         transform.position = new Vector3(1.5f, 40f, -20f);
	// 		transform.rotation = Quaternion.Euler(70f, 0f, 0f);
    //     }
    //     else{
	// 		transform.position = new Vector3(1.5f, 40f, 20f);
	// 		transform.rotation = Quaternion.Euler(70f, 180f, 0f);
	// 	}
    // }
}
