using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	/* Singleton */
	public static Inventory Instance;

	public delegate void OnCardChanged();
	public OnCardChanged onCardChangedCallback;

	public Card[] defaultCards = new Card[7];
	public List<Card> cards = new List<Card>();
	public InventoryUI[] inventoryUI = new InventoryUI[2];
	//public int space = 20;

	void Awake(){
		Instance = this;
		for (int i = 0; i < inventoryUI.Length; i++)
			inventoryUI[i].SetupInventoryUI();
	}

	void Start(){
		/* Set up the default cards ONLY */
		for (int i = 0; i < defaultCards.Length; i++){
			cards.Add(defaultCards[i]);
			cards[i].SetupSlimeProperties();
		}
		
		if(onCardChangedCallback != null){
			onCardChangedCallback.Invoke();
		}

		//Load player saved card deck
		PlayerData.Instance.LoadPlayerCardDeck();
	}


	public void Add(Card card){
		if(!card.isDefaultCard){
			// if(cards.Count >= space){
			// 	Debug.Log("Not enough room.");
			// 	return;
			// }
			cards.Add(card);

			if(onCardChangedCallback != null)
				onCardChangedCallback.Invoke();
		}
	}

	public void Remove(Card card){
		cards.Remove(card);

		if(onCardChangedCallback != null)
			onCardChangedCallback.Invoke();
	}
}
