using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour {

	/* Singleton */
	public static Deck Instance;

	void Awake(){
		Instance = this;
		slots = deckParent.GetComponentsInChildren<DeckSlot>();
	}

	//public Card[] cardDeck = new Card[6];
	private const int spaceOfCardDeck = 6;
	public Transform deckParent;

	DeckSlot[] slots;
	int numOfCardDeck;

	public void Add(Card card, int inventorySlotNum){
		if(numOfCardDeck >= spaceOfCardDeck){
			Debug.Log("Not enough room.");
			return;
		}
		//find the current empty card slot in the deck
		int currentEmptyCardSlot = findEmptyCardSlot();
		Load(card, currentEmptyCardSlot, inventorySlotNum);

		PlayerData.Instance.SavePlayerCardDeck(currentEmptyCardSlot, inventorySlotNum);
	}

	public void Load(Card card, int deckSlotNum, int inventorySlotNum){
		numOfCardDeck++;
		PlayerCardDeck.Instance.deck[deckSlotNum] = card;
		slots[deckSlotNum].AddCard(card, inventorySlotNum);
	}

	public void Remove(int deckSlot){
		numOfCardDeck--;
		PlayerCardDeck.Instance.deck[deckSlot] = null;
		slots[deckSlot].ClearSlot();

		PlayerData.Instance.ClearPlayerCardDeck(deckSlot);
	}

	private int findEmptyCardSlot(){
		for (int i = 0; i < spaceOfCardDeck; i++){
			if(PlayerCardDeck.Instance.deck[i] == null)
				return i;
		}
		return -1;	
	}
}
