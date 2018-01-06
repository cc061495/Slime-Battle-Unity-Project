/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour {

	public static TimerManager Instance;

    void Awake(){
        Instance = this;   
    }

	private const float BuildingTime = 60f; //default 60s
	private float startTime, gameTimer;
	private bool isRedReady, isBlueReady, timerReady, readyCoolDown;
	private Color originalColor;
    public RectTransform worldSpaceCanvasPos;
	public Button readyButton;
    public Image timerBar, redState, blueState;
	GameManager gameManager;
	PhotonView photonView;

	void Start () {
		photonView = GetComponent<PhotonView>();
		gameManager = GameManager.Instance;
		originalColor = redState.color;

        if(PhotonNetwork.isMasterClient){
            worldSpaceCanvasPos.rotation = Quaternion.Euler(90f, 0f, 0f);
		}
        else{
			worldSpaceCanvasPos.position += new Vector3(0,0,-10);
            worldSpaceCanvasPos.rotation = Quaternion.Euler(90f, 180f, 0f);
		}
	}
	
	void Update () {
		if(gameManager.currentState == GameManager.State.build_start && timerReady){
			if(gameTimer > 0){
				gameTimer -= Time.deltaTime;
        		timerBar.fillAmount = gameTimer / startTime;
			}
			/* Delete it later */
			if(Input.GetKeyDown("space"))
				setReady();

			if(gameTimer <= 0 || (isRedReady && isBlueReady)){
				if(PhotonNetwork.isMasterClient)
					photonView.RPC("SyncToReady", PhotonTargets.All);
			}
		}
	}

	[PunRPC]
	private void SyncToReady(){
		gameManager.currentState = GameManager.State.build_end;
		Invoke("ResetAllReady", 1f);
	}

	void ResetAllReady(){
		timerReady = false;
		isRedReady = false;
		isBlueReady = false;

		redState.color = originalColor;
		blueState.color = originalColor;

		worldSpaceCanvasPos.gameObject.SetActive(false);
		gameManager.BuildEnd();
	}

	public void setBuildingTime(){
		timerBar.fillAmount = 1;
		worldSpaceCanvasPos.gameObject.SetActive(true);
		Invoke("StartTimer", 0.5f);
	}

	void StartTimer(){
		gameTimer = BuildingTime;
		startTime = gameTimer;
		readyCoolDown = true;
		timerReady = true;
	}

	public void setReady(){
		if(gameManager.currentState == GameManager.State.build_start && readyCoolDown){
			readyCoolDown = false;
			if(PhotonNetwork.isMasterClient)
				photonView.RPC("RPC_setRedReady", PhotonTargets.All);
			else
				photonView.RPC("RPC_setBlueReady", PhotonTargets.All);

			Invoke("ResetReadyCoolDown", 0.5f);
		}
	}

	private void ResetReadyCoolDown(){
		readyCoolDown = true;
	}

	[PunRPC]
	private void RPC_setRedReady(){
		isRedReady = !isRedReady;
		redState.color = (isRedReady) ? Color.red : originalColor;
	}

	[PunRPC]
	private void RPC_setBlueReady(){
		isBlueReady = !isBlueReady;
		blueState.color = (isBlueReady) ? Color.blue : originalColor;
	}
}
