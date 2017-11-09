/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public GameObject CameraRed, CameraBlue;
    public Animator cameraAnimator;

    void Awake(){
        if (PhotonNetwork.isMasterClient){
            CameraBlue.SetActive(false);
            cameraAnimator = CameraRed.GetComponent<Animator>();
        }
        else{
            CameraRed.SetActive(false);
            cameraAnimator = CameraBlue.GetComponent<Animator>();
        }
    }

    public void SetCameraMovement(bool move){
        cameraAnimator.SetBool("Build", move);
    }
}
