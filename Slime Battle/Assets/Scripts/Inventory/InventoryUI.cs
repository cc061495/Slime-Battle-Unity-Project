using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

	public Transform cardsParent;
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
