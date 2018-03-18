using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

	public Image icon;
	public Button selectButton;
	private int inventorySlotNum;
	Card card;

	public void AddCard(Card newCard, int num){
		inventorySlotNum = num;
		card = newCard;

		icon.sprite = card.icon_red;
		icon.enabled = true;

		selectButton.interactable = true;
	}

	public void ClearSlot(){
		card = null;

		icon.sprite = null;
		icon.enabled = false;

		selectButton.interactable = false;
	}

	public void OnRemoveButton(){
		Inventory.Instance.Remove(card);
	}

	public void CardSelect(){
		if(card != null && selectButton.interactable){
			card.Select(inventorySlotNum);			
		}
	}

	public void SavedCardSelect(int deckSlotNum){
		if(card != null)
			card.Load(deckSlotNum, inventorySlotNum);
	}

	public void OpenCardStatus(){
		if(card != null)
			InventoryStats.Instance.ShowCardStats(card);
	}
}
