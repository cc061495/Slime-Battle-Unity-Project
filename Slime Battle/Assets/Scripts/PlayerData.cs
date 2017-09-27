/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

	public static PlayerData Instance;

	public string playerName {get ; private set;}
	public int playerBalance {get ; private set;}
	public string[] playerDeckSlot = new string[6];

	private void Awake () {
		Instance = this;
		LoadPlayerSetting();
	}

	// Use this for initialization
	void Start () {

	}

	public void ClearPlayerData(){
		PlayerPrefs.DeleteAll();
		LoadPlayerSetting();
	}

	private void LoadPlayerSetting(){
		LoadPlayerNameSetting();
		LoadPlayerBalanceSetting();

		MenuScreen.Instance.SetPlayerStatus();
	}

	private void LoadPlayerNameSetting(){
		if(PlayerPrefs.HasKey("PlayerName"))
			playerName = PlayerPrefs.GetString("PlayerName");
		else{
			playerName = "Player#" + Random.Range (100, 1000);
			PlayerPrefs.SetString("PlayerName", playerName);
		}
	}

	private void LoadPlayerBalanceSetting(){
		if(PlayerPrefs.HasKey("PlayerBalance"))
			playerBalance = PlayerPrefs.GetInt("PlayerBalance");
		else{
			playerBalance = 100;
			PlayerPrefs.SetInt("PlayerBalance", playerBalance);
		}
	}

	public void SavePlayerCardDeck(int slot, string cardName){
		playerDeckSlot[slot] = cardName;
		PlayerPrefs.SetString(getSlotString(slot), cardName);
	}

	public void ClearPlayerCardDeck(int slot){
		playerDeckSlot[slot] = null;
		PlayerPrefs.DeleteKey(getSlotString(slot));
	}

	private string getSlotString(int num){
		if(num == 0)
			return "Slot1";
		else if(num == 1)
			return "Slot2";
		else if(num == 2)
			return "Slot3";
		else if(num == 3)
			return "Slot4";
		else if(num == 4)
			return "Slot5";
		else if(num == 5)
			return "Slot6";
	
		return null;
	}
}
