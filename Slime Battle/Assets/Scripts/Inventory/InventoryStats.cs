using UnityEngine;
using UnityEngine.UI;

public class InventoryStats : MonoBehaviour {

	public static InventoryStats Instance;
    
    void Awake(){
        Instance = this; 
    }

	public Image icon;
	private Button buttonSelected;
	//public Text nameText, costText, sizeText;
	Card card;
	
	public void ShowCardStats(Card cardSelected, Button button){
		card = cardSelected;
		icon.sprite = card.icon;
		icon.enabled = true;

		if(buttonSelected != null)
			buttonSelected.interactable = true;

		buttonSelected = button;
		buttonSelected.interactable = false;
	}
}
