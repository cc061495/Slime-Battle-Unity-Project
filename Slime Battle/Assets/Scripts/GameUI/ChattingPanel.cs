/* Copyright (c) cc061495 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChattingPanel : MonoBehaviour {

	public static ChattingPanel Instance;
	public GameObject chatPanel, chatButton;

	Animator chatPanelAnimator;
    
    void Awake(){
        Instance = this;
    }

	GameManager gameManager;
	TeamController teamController;

	void Start(){
		gameManager = GameManager.Instance;
		teamController = TeamController.Instance;
		chatPanelAnimator = chatPanel.GetComponent<Animator>();
	}

	public void OnClick_ChatButton(){
		SetChatPanelDisplay(true);
		SetChatButtonDisplay(false);
		teamController.SetControlButtonDisplay(false);
	}

	/* Action for Attack Mode Button */
	public void OnClick_Emote(int index) {
		/* Display the emotion */
		StartCoroutine(DefineControlWhichTeam());
	}
	
	IEnumerator DefineControlWhichTeam(){

		chatPanelAnimator.Play("Back", 0, 0f);
		yield return new WaitForSeconds(0.2f);
		if(gameManager.currentState == GameManager.State.battle_start){
			SetChatPanelDisplay(false);
			SetChatButtonDisplay(true);
			teamController.SetControlButtonDisplay(true);
		}
	}

	public void SetChatButtonDisplay(bool display){
		if(chatButton.activeSelf != display)
			chatButton.SetActive(display);
	}

	public void SetChatPanelDisplay(bool display){
		if(chatPanel.activeSelf != display)
			chatPanel.SetActive(display);
	}
}
