using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Inventory/Card")]
public class Card : ScriptableObject {

	[Header("Slime Card")]
	new public string name = "New Card";
	public Sprite icon = null;
	public bool isDefaultCard = false;
	[Header("Slime Blueprint")]
	public GameObject teamRedPrefab;
	public GameObject teamBluePrefab;
    public int cost;
	public int size = 1;
	public Vector3 spawnPosOffset = new Vector3(0,1.5f,0);
	[Header("Slime Properties")]
	public float health;			//Slime's health
	public float attackDamage;		//Slime's attack damage
	public float actionCoolDown;	//Slime's action cool down time
	public float movemonetSpeed;	//Slime's movement speed
	public float actionRange;		//Slime's action range
	[Header("Action Type")]
	public bool isMeleeAttack;
	public bool isRangedAttack;
	public bool isHealing;
	public bool isAreaEffectDamage;
	public bool isExplosion;
	public bool isBuilding;
	
	public virtual void Select(int inventorySlotNum){
		//Select the card
		//Something might happen
		Debug.Log("Selected card: " + name);

		if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.inventory){
			InventoryStats.Instance.ShowCardStats(this);
		}

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
