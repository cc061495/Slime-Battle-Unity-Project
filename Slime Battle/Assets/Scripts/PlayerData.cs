/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

	public static PlayerData Instance;
	
	public string playerName {get; private set;}
	public int playerMoney {get; private set;}
	//public int[] playerDeckSlot = new int[6];
	private string[] slotKeys = new string[6]{"Slot1", "Slot2", "Slot3", "Slot4", "Slot5", "Slot6"};

	public InventoryUI inventoryUI;

	private void Awake () {
		Instance = this;
		LoadPlayerSetting();
	}

	private void LoadPlayerSetting(){
		LoadPlayerNameSetting();
		LoadPlayerMoneySetting();

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

	private void LoadPlayerMoneySetting(){
		if(PlayerPrefs.HasKey("PlayerBalance"))
			playerMoney = PlayerPrefs.GetInt("PlayerBalance");
		else{
			playerMoney = 100;
			PlayerPrefs.SetInt("PlayerBalance", playerMoney);
		}
	}

	public void SavePlayerCardDeck(int deckSlotNum, int inventorySlotNum){
		//playerDeckSlot[deckSlotNum] = inventorySlotNum;
		PlayerPrefs.SetInt(slotKeys[deckSlotNum], inventorySlotNum);
	}

	public void ClearPlayerCardDeck(int deckSlotNum){
		//playerDeckSlot[deckSlotNum] = -1;
		PlayerPrefs.DeleteKey(slotKeys[deckSlotNum]);
	}

	public void LoadPlayerCardDeck(){
		for (int i = 0; i < slotKeys.Length; i++){
			if(PlayerPrefs.HasKey(slotKeys[i])){
				//playerDeckSlot[i] = PlayerPrefs.GetInt(slotKeys[i]);
				inventoryUI.UpdatePlayerCardDeck(i, PlayerPrefs.GetInt(slotKeys[i]));
			}
		}
	}
}
