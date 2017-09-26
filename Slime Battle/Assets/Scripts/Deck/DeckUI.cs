using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckUI : MonoBehaviour {

	public Transform deckParent;
	Deck deck;

	DeckSlot[] slots;

	// Use this for initialization
	void Awake(){
		deck = Deck.Instance;
		deck.onCardChangedCallback += UpdateUI;

		slots = deckParent.GetComponentsInChildren<DeckSlot>();
	}

	void UpdateUI(){
		Debug.Log("Updating UI");
		for (int i = 0; i < slots.Length; i++){
			if(i < deck.usedSpace.Count){
				slots[deck.usedSpace[i]].AddCard(deck.cardDeck[i]);
			}
			else{
				slots[i].ClearSlot();
			}
		}
	}
}
