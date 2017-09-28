using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardDeck : MonoBehaviour {

	/* Singleton */
	public static PlayerCardDeck Instance;

	void Awake(){
		Instance = this;
		DontDestroyOnLoad(this);
	}

	public Card[] deck = new Card[6];
}
