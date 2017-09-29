using UnityEngine;
using UnityEngine.UI;

public class InventoryStats : MonoBehaviour {

	public static InventoryStats Instance;
    
    void Awake(){
        Instance = this; 
    }

	public Image icon;
	public GameObject statsPanel;
	public Text nameText, costText, sizeText;

	int slotNumSelected;
	Card card;

	public void ShowCardStats(Card cardSelected){
		card = cardSelected;
		nameText.text = card.name;
		costText.text = "Cost: $" + card.cost;
		sizeText.text = "Size: " + card.size;

		icon.sprite = card.icon;
		icon.enabled = true;

		statsPanel.SetActive(true);
	}

	public void CloseCardStats(){
		statsPanel.SetActive(false);
	}
}
