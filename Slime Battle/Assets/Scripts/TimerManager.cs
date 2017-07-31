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

	private const float BuildingTime = 45f;
	private float startTime;
    private float gameTimer;
	private bool isRedReady, isBlueReady, allReady;
    public RectTransform worldSpaceCanvasPos;
	public Button readyButton;
    public Image timerBar, redState, blueState;
	GameManager gameManager;
	PhotonView photonView;

	void Start () {
		photonView = GetComponent<PhotonView>();
		gameManager = GameManager.Instance;
        if(PhotonNetwork.isMasterClient){
            worldSpaceCanvasPos.rotation = Quaternion.Euler(90f, 0f, 0f);
		}
        else{
			worldSpaceCanvasPos.position += new Vector3(0,0,-10);
            worldSpaceCanvasPos.rotation = Quaternion.Euler(90f, 180f, 0f);
		}
	}
	
	void Update () {
		if(gameTimer > 0 && gameManager.currentState == GameManager.State.build_start && !allReady){
			gameTimer -= Time.deltaTime;
			
        	timerBar.fillAmount = gameTimer / startTime;

			if(Input.GetKeyDown("space"))
				setReady();

			if((gameTimer <= 0 || (isRedReady && isBlueReady))){
				allReady = true;
				GameManager.Instance.currentState = GameManager.State.build_end;
				Invoke("ResetAllReady", 1f);
			}
		}
	}

	void ResetAllReady(){
		isRedReady = false;
		isBlueReady = false;

		redState.enabled = false;
		blueState.enabled = false;
		worldSpaceCanvasPos.gameObject.SetActive(false);
		GameManager.Instance.BuildEnd();
	}

	public void setBuildingTime(){
		worldSpaceCanvasPos.gameObject.SetActive(true);
		timerBar.fillAmount = 1;
		Invoke("StartTimer", 0.5f);
	}

	void StartTimer(){
		gameTimer = BuildingTime;
		startTime = gameTimer;
		allReady = false;
	}

	public void setReady(){
		if(PhotonNetwork.isMasterClient)
			photonView.RPC("RPC_setRedReady", PhotonTargets.All);
		else
			photonView.RPC("RPC_setBlueReady", PhotonTargets.All);
	}

	[PunRPC]
	private void RPC_setRedReady(){
		isRedReady = true;
		redState.enabled = true;
		redState.color = Color.red;
	}

	[PunRPC]
	private void RPC_setBlueReady(){
		isBlueReady = true;
		blueState.enabled = true;
		blueState.color = Color.blue;
	}
}
