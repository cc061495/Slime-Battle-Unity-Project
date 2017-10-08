﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

	public Transform cardsParent;
	public Text inventoryStorageText;
	Inventory inventory;

	public InventorySlot[] slots;
	// Use this for initialization
	public void SetupInventoryUI(){
		inventory = Inventory.Instance;
		inventory.onCardChangedCallback += UpdateUI;
		slots = cardsParent.GetComponentsInChildren<InventorySlot>();
	}

	void UpdateUI(){
		Debug.Log("Updating Inventory UI");
		inventoryStorageText.text = inventory.cards.Count + " / " + slots.Length;
		
		for (int i = 0; i < slots.Length; i++){
			if(i < inventory.cards.Count){
				slots[i].AddCard(inventory.cards[i], i);
			}
			else{
				slots[i].ClearSlot();
			}
		}
	}

	public void UpdatePlayerCardDeck(int deckSlotNum, int inventorySlotNum){
		slots[inventorySlotNum].SavedCardSelect(deckSlotNum);
	}
}
