/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerData : MonoBehaviour {

	public static PlayerData Instance;
	
	public string playerName {get; private set;}
	public int playerCoins {get; private set;}
	private string[] slotKeys = new string[6]{"Slot1", "Slot2", "Slot3", "Slot4", "Slot5", "Slot6"};
	private string[] inventoryItems = new string[10]{"Item1","Item2","Item3","Item4","Item5","Item6","Item7","Item8","Item9","Item10"};
	public InventoryUI inventoryUI;

	private void Awake () {
		Instance = this;
		LoadPlayerSetting();
		//PlayerPrefs.DeleteAll();
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
			playerName = "Player#" + UnityEngine.Random.Range (100, 1000);
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
			playerCoins = 500;
			PlayerPrefs.SetInt("PlayerCoins", playerCoins);
		}
	}

	public void SavePlayerCard(int itemIndex){
		PlayerPrefs.SetInt(inventoryItems[itemIndex], Inventory.Instance.cards.Count+1);
		Inventory.Instance.Add(Shop.Instance.cards[itemIndex]);
	}

	public void LoadPlayerCard(){
		List<Card> cards = new List<Card>();
		for (int i = 0;i <inventoryItems.Length;i++){
			if(PlayerPrefs.HasKey(inventoryItems[i])){
				Card _card = new Card(i, PlayerPrefs.GetInt(inventoryItems[i]));
				cards.Add(_card);
			}
		}
		cards = cards.OrderBy(c => c.itemPos).ToList();
		for(int i = 0;i<cards.Count;i++)
			Inventory.Instance.cards.Add(Shop.Instance.cards[cards[i].itemId]);
	}

	public bool CheckPlayerCard(int itemIndex){
		if(PlayerPrefs.HasKey(inventoryItems[itemIndex]))
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

	private class Card{
		public int itemId;
		public int itemPos;

		public Card(int id, int pos){
			this.itemId = id;
			this.itemPos = pos;
		}
	}
}
