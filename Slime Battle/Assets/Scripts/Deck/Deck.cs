using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour {

	/* Singleton */
	public static Deck Instance;

	public delegate void OnCardChanged();
	public OnCardChanged onCardChangedCallback;

	public Card[] cardDeck = new Card[6];
	public List<int> usedSpace = new List<int>();
	private int currentEmptyCardSlot;
	private int spaceOfCardDeck = 6;

	void Awake(){
		Instance = this;
	}

	public void Add(Card card){
		if(usedSpace.Count >= spaceOfCardDeck){
			Debug.Log("Not enough room.");
			return;
		}
		//find the current empty card slot in the deck
		for (int i = 0; i < spaceOfCardDeck; i++){
			if(cardDeck[i] == null){
				Debug.Log(currentEmptyCardSlot);
				currentEmptyCardSlot = i;
				break;
			}
		}
		usedSpace.Add(currentEmptyCardSlot);
		cardDeck[currentEmptyCardSlot] = card;

		if(onCardChangedCallback != null)
			onCardChangedCallback.Invoke();
	}

	public void Remove(Card card, int slot){
		usedSpace.Remove(slot);
		cardDeck[slot] = null;

		if(onCardChangedCallback != null)
			onCardChangedCallback.Invoke();
	}
}
