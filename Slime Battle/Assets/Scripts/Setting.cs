/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour {

	public GameObject confirmPanel, renamePanel, resetPanel, creditsPanel;
	public InputField renameText;
	public SceneFader sceneFader;

	public void DisplayRenamePanel(bool display){
		OpenPanel(display);
		renamePanel.SetActive(display);

		if(!display)
			renameText.text = "";
	}

	public void DisplayResetSavePanel(bool display){
		OpenPanel(display);
		resetPanel.SetActive(display);
	}

	public void DisplayCredits(bool display){
		OpenPanel(display);
		creditsPanel.SetActive(display);
	}

	private void OpenPanel(bool display){
		confirmPanel.SetActive(display);
		MenuScreen.Instance.BackButtonDisplay(!display);

		if(display)
			AudioManager.instance.Play("Tap");
		else
			AudioManager.instance.Play("TapBack");
	}

	public void PlayerRename(){
		string playerName = renameText.text;
		if(!string.IsNullOrEmpty(playerName)){
			PlayerData.Instance.SavePlayerName(playerName);
			MenuScreen.Instance.PlayerNameText.text = playerName;

			renamePanel.SetActive(false);
			confirmPanel.SetActive(false);
			MenuScreen.Instance.BackButtonDisplay(true);
			AudioManager.instance.Play("Tap");
		}
		else
			AudioManager.instance.Play("Error");
	}

	public void ResetPlayerData(){
		AudioManager.instance.Play("Tap");
		PlayerData.Instance.ResetPlayerData();
		Destroy(GameObject.Find("PlayerCardDeck"));
		AudioManager.instance.ChangeTheme("Home");
		sceneFader.FadeTo("HomeScreen");
	}
}
