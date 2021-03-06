﻿using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Inventory/Card")]
public class Card : ScriptableObject {

	[Header("Slime Card")]
	new public string name = "New Card";
	public Sprite icon_red = null, icon_blue = null;
	public bool isDefaultCard = false;
	public bool combatCard = false;
	[Header("Slime Blueprint")]
	public GameObject teamRedPrefab;
	public GameObject teamBluePrefab;
    public int cost;
	public int size = 1;
	public Vector3 spawnPosOffset = new Vector3(0,1.5f,0);
	
	public float health{get;private set;}			//Slime's health
	public float attackDamage{get;private set;}		//Slime's attack damage
	public float actionCoolDown{get;private set;}	//Slime's action cool down time
	public float movemonetSpeed{get;private set;}	//Slime's movement speed
	public float actionRange{get;private set;}		//Slime's action range
	public float castTime{get; private set;}
	[Header("Action Type")]
	public string type;

	[Header("Shop Setting")]
	public Mesh mesh;
	public int coins;

	SlimeClass slime;

	public void SetupSlimeProperties(){
		if(slime == null)
			slime = new SlimeClass(name);

		health = slime.startHealth;
		attackDamage = slime.attackDamage;
		actionCoolDown = slime.actionCoolDown;
		movemonetSpeed = slime.movemonetSpeed;
		actionRange = slime.actionRange;
		castTime = slime.castTime;
	}
	
	public virtual void Select(int inventorySlotNum){
		//Select the card
		//Something might happen
		Debug.Log("Selected card: " + name);

		// if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.inventory){
		// 	InventoryStats.Instance.ShowCardStats(this);
		// }

		if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.deck){
			Deck.Instance.Add(this, inventorySlotNum);
		}
	}

	public virtual void Remove(int slot){
		if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.deck)
			Deck.Instance.Remove(slot);
	}

	public virtual void Load(int deckSlotNum, int inventorySlotNum){
		Deck.Instance.Load(this, deckSlotNum, inventorySlotNum);
	}
}
