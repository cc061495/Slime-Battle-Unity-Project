using UnityEngine;
using UnityEngine.UI;

public class DeckSlot : MonoBehaviour {

	public Image icon;
	public Button selectButton;
	private Button selectedCard;
	Card card;

	public void AddCard(Card newCard){
		card = newCard;

		icon.sprite = card.icon;
		icon.enabled = true;

		selectButton.interactable = true;
		//selectedCard.interactable = false;
	}

	public void ClearSlot(){
		card = null;

		icon.sprite = null;
		icon.enabled = false;

		selectButton.interactable = false;
		//selectedCard.interactable = true;
	}

	public void OnRemoveButton(){
		Inventory.instance.Remove(card);
	}

	public void CardRemove(int slot){
		if(card != null){
			card.Remove(slot);
		}
	}
}
