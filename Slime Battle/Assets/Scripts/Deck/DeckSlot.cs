using UnityEngine;
using UnityEngine.UI;

public class DeckSlot : MonoBehaviour {

	public Image icon;
	public Button selectButton;
	int selectedSlotNum;
	Card card;

	public void AddCard(Card newCard, int slotNum){
		selectedSlotNum = slotNum;
		card = newCard;

		icon.sprite = card.icon_red;
		icon.enabled = true;

		selectButton.interactable = true;
		Inventory.Instance.inventoryUI[0].slots[selectedSlotNum].selectButton.interactable = false;
	}

	public void ClearSlot(){
		card = null;

		icon.sprite = null;
		icon.enabled = false;

		selectButton.interactable = false;
		Inventory.Instance.inventoryUI[0].slots[selectedSlotNum].selectButton.interactable = true;
	}

	public void CardRemove(int slot){
		if(card != null)
			card.Remove(slot);
	}

	public void OpenCardStatus(){
		if(card != null)
			InventoryStats.Instance.ShowCardStats(card);
	}
}
