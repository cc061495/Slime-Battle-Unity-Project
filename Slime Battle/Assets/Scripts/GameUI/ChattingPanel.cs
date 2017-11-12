/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChattingPanel : MonoBehaviour {

	public Sprite[] chatEmoji = new Sprite[15];
	public Sprite[] chatEmoji2 = new Sprite[15];
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
		RPC_EmojiCreate(index, 0);
		photonView.RPC("RPC_EmojiCreate", PhotonTargets.Others, index, 1);
		/* Display the emotion */
		StartCoroutine(FadeOutChatPanel());
	}

	[PunRPC]
	private void RPC_EmojiCreate(int index, int spawnPoint){
		Image emojiImage = emoji[spawnPoint].GetComponent<Image>();

		emojiImage.sprite = spawnPoint == 0 ? chatEmoji[index] : chatEmoji2[index];
		if(!emojiImage.enabled)
			emojiImage.enabled = true;
		
		emoji[spawnPoint].GetComponent<Animator>().Rebind();
	}
	
	IEnumerator FadeOutChatPanel(){
		chatPanelAnimator.Play("Back", 0, 0f);
		yield return new WaitForSeconds(0.15f);
		SetChatPanelDisplay(false);
		if(gameManager.currentState == GameManager.State.battle_start){
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
