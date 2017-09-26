using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	/* Singleton */
	public static Inventory instance;

	public delegate void OnCardChanged();
	public OnCardChanged onCardChangedCallback;

	public Card[] defaultCards = new Card[6];
	public List<Card> cards = new List<Card>();
	//public int space = 20;

	void Awake(){
		instance = this;
	}

	void Start(){
		for (int i = 0; i < defaultCards.Length; i++)
		{
			cards.Add(defaultCards[i]);
		}
		
		if(onCardChangedCallback != null){
			onCardChangedCallback.Invoke();
		}
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
