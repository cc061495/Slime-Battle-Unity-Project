using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardDeck : MonoBehaviour {

	/* Singleton */
	public static PlayerCardDeck Instance;
	public Card[] deck = new Card[6];

	void Awake(){
		Instance = this;
		DontDestroyOnLoad(this);
	}
}
