/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

	public static PlayerData Instance;
	
	public string playerName {get ; private set;}
	public int playerBalance {get ; private set;}
	public string[] playerDeckSlot = new string[6];
	private string[] slotKeys = new string[6]{"Slot1", "Slot2", "Slot3", "Slot4", "Slot5", "Slot6"};

	public InventoryUI inventoryUI;

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
		PlayerPrefs.SetString(GetSlotString(slot), cardName);
	}

	public void ClearPlayerCardDeck(int slot){
		playerDeckSlot[slot] = null;
		PlayerPrefs.DeleteKey(GetSlotString(slot));
	}

	private string GetSlotString(int num){
		for (int i = 0; i < slotKeys.Length; i++){
			if(num == i){
				return slotKeys[i];
			}
		}
		return null;
	}

	public void LoadPlayerCardDeck(){
		for (int i = 0; i < slotKeys.Length; i++){
			if(PlayerPrefs.HasKey(slotKeys[i])){
				playerDeckSlot[i] = PlayerPrefs.GetString(slotKeys[i]);
				for (int j = 0; j < Inventory.instance.cards.Count; j++){
					if(playerDeckSlot[i] == Inventory.instance.cards[j].name){
						inventoryUI.UpdatePlayerCardDeck(j,i);
						break;
					}	
				}
			}
		}
	}
}
