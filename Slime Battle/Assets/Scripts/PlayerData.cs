/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

	public static PlayerData Instance;
	
	public string playerName {get; private set;}
	public int playerCoins {get; private set;}
	//public int[] playerDeckSlot = new int[6];
	private string[] slotKeys = new string[6]{"Slot1", "Slot2", "Slot3", "Slot4", "Slot5", "Slot6"};
	private string[] shopItems = new string[10]{"Item1","Item2","Item3","Item4","Item5","Item6","Item7","Item8","Item9","Item10"};

	public InventoryUI inventoryUI;

	private void Awake () {
		Instance = this;
		LoadPlayerSetting();
	}

	private void LoadPlayerSetting(){
		LoadPlayerNameSetting();
		LoadPlayerCoins();

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

	public void SavePlayerCoins(int coins){
		playerCoins = coins;
		PlayerPrefs.SetInt("PlayerCoins", coins);
	}

	public void LoadPlayerCoins(){
		if(PlayerPrefs.HasKey("PlayerCoins"))
			playerCoins = PlayerPrefs.GetInt("PlayerCoins");
		else{
			playerCoins = 200;
			PlayerPrefs.SetInt("PlayerCoins", playerCoins);
		}
	}

	public void SavePlayerCard(int itemIndex, int count){
		PlayerPrefs.SetInt(shopItems[itemIndex], count);
		Inventory.Instance.Add(Shop.Instance.cards[itemIndex]);
	}

	public void LoadPlayerCard(){
		for (int i = 0;i <shopItems.Length;i++){
			if(PlayerPrefs.HasKey(shopItems[i])){
				Inventory.Instance.cards.Add(Shop.Instance.cards[i]);
			}
		}
	}

	public bool CheckPlayerCard(int itemIndex){
		Debug.Log(itemIndex);
		if(PlayerPrefs.HasKey(shopItems[itemIndex]))
			return false;
		return true;
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
