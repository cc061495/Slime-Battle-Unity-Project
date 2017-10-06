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
	
	public float health{get;private set;}			//Slime's health
	public float attackDamage{get;private set;}		//Slime's attack damage
	public float actionCoolDown{get;private set;}	//Slime's action cool down time
	public float movemonetSpeed{get;private set;}	//Slime's movement speed
	public float actionRange{get;private set;}		//Slime's action range
	[Header("Action Type")]
	public string type;

	SlimeClass slime;

	private void SetupSlimeProperties(){
		health = slime.startHealth;
		attackDamage = slime.attackDamage;
		actionCoolDown = slime.actionCoolDown;
		movemonetSpeed = slime.movemonetSpeed;
		actionRange = slime.actionRange;
	}
	
	public virtual void Select(int inventorySlotNum){
		//Select the card
		//Something might happen
		Debug.Log("Selected card: " + name);
		if(slime == null){
			slime = new SlimeClass(name);
			SetupSlimeProperties();
		}

		if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.inventory){
			InventoryStatus.Instance.ShowCardStats(this);
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

	// public string GetCardTypeString(){
	// 	if(isMeleeAttack)
	// 		return "Single Melee Attack";
	// 	else if(isRangedAttack)
	// 		return "Single Ranged Attack";
	// 	else if(isAreaEffectDamage)
	// 		return "Area Effect Damage";
	// 	else if(isExplosion)
	// 		return "Explosion";
	// 	else if(isHealing)
	// 		return "Support";
	// 	else if(isBuilding)
	// 		return "Building";
	// 	else
	// 		return "Unknown";
	// }
}
