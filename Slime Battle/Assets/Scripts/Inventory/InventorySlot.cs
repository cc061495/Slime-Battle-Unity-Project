using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

	public Image icon;
	public Button selectButton;
	Card card;

	public void AddCard(Card newCard){
		card = newCard;

		icon.sprite = card.icon;
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
		Inventory.instance.Remove(card);
	}

	public void CardSelect(){
		if(card != null){
			card.Select(selectButton);			
		}
	}

	public void SavedCardSelect(int slotNum){
		if(card != null){
			card.Load(selectButton, slotNum);
		}
	}
}
