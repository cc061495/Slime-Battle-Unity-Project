﻿using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Inventory/Card")]
public class Card : ScriptableObject {

	new public string name = "New Card";
	public GameObject teamRedPrefab, teamBluePrefab;
	public Sprite icon = null;
	public bool isDefaultCard = false;

	public virtual void Select(Button selectedBtn){
		//Select the card
		//Something might happen
		Debug.Log("Selected card: " + name);

		//if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.home)

		if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.deck){
			Deck.Instance.Add(this, selectedBtn);
		}
	}

	public virtual void Remove(int slot){
		if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.deck)
			Deck.Instance.Remove(slot);
	}
}
