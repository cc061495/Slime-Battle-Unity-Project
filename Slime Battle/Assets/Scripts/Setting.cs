﻿/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour {

	public GameObject confirmPanel, renamePanel, resetPanel, creditsPanel;
	public InputField renameText;

	public void DisplayRenamePanel(bool display){
		OpenPanel(display);
		renamePanel.SetActive(display);
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
	}

	public void PlayerRename(){
		string playerName = renameText.text;
		if(!string.IsNullOrEmpty(playerName)){
			PlayerData.Instance.SavePlayerName(playerName);
			MenuScreen.Instance.PlayerNameText.text = playerName;
		}
	}

	public void ResetPlayerData(){
		PlayerData.Instance.ResetPlayerData();
		DisplayResetSavePanel(false);
	}
}
