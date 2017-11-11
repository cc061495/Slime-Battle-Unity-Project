/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChattingPanel : MonoBehaviour {

	public Sprite[] chatEmoji = new Sprite[15];
	public static ChattingPanel Instance;
	public GameObject chatPanel, chatButton;

	public GameObject[] emoji = new GameObject[2];
	private PhotonView photonView;
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
		photonView = GetComponent<PhotonView>();
	}

	public void OnClick_ChatButton(){
		SetChatPanelDisplay(true);
		SetChatButtonDisplay(false);
		teamController.SetControlButtonDisplay(false);
	}

	/* Action for Attack Mode Button */
	public void OnClick_Emote(int index) {
		photonView.RPC("RPC_EmojiCreate", PhotonTargets.All, index);
		/* Display the emotion */
		StartCoroutine(DefineControlWhichTeam());
	}

	[PunRPC]
	private void RPC_EmojiCreate(int index){
		StartCoroutine(Set_EmojiDisplay(index));
	}

	IEnumerator Set_EmojiDisplay(int index){
		int spawnPoint = PhotonNetwork.isMasterClient ? 1 : 0;
		emoji[spawnPoint].GetComponent<Image>().sprite = chatEmoji[index];
		emoji[spawnPoint].SetActive(true);
		yield return new WaitForSeconds(2f);
		emoji[spawnPoint].SetActive(false);
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
