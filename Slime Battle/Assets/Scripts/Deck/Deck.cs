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

	public void Add(Card card, Button selectedButton){
		if(numOfCardDeck >= spaceOfCardDeck){
			Debug.Log("Not enough room.");
			return;
		}
		//find the current empty card slot in the deck
		int currentEmptyCardSlot = findEmptyCardSlot();

		numOfCardDeck++;
		PlayerCardDeck.Instance.deck[currentEmptyCardSlot] = card;
		slots[currentEmptyCardSlot].AddCard(card, selectedButton);

		PlayerData.Instance.SavePlayerCardDeck(currentEmptyCardSlot, card.name);
	}

	public void Remove(int slot){
		numOfCardDeck--;
		PlayerCardDeck.Instance.deck[slot] = null;
		slots[slot].ClearSlot();

		PlayerData.Instance.ClearPlayerCardDeck(slot);
	}

	private int findEmptyCardSlot(){
		for (int i = 0; i < spaceOfCardDeck; i++){
			if(PlayerCardDeck.Instance.deck[i] == null)
				return i;
		}
		return -1;	
	}
}
