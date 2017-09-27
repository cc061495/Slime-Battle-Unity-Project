using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

	public Transform cardsParent;
	Inventory inventory;

	InventorySlot[] slots;
	// Use this for initialization
	void Start(){
		inventory = Inventory.instance;
		inventory.onCardChangedCallback += UpdateUI;

		slots = cardsParent.GetComponentsInChildren<InventorySlot>();
	}

	void UpdateUI(){
		Debug.Log("Updating UI");
		for (int i = 0; i < slots.Length; i++){
			if(i < inventory.cards.Count){
				slots[i].AddCard(inventory.cards[i]);
			}
			else{
				slots[i].ClearSlot();
			}
		}
	}
}
