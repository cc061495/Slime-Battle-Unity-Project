using UnityEngine;
using UnityEngine.UI;

public class InventoryStatus : MonoBehaviour {

	public static InventoryStatus Instance;
    
    void Awake(){
        Instance = this; 
    }

	public Image icon, healthBar, damageBar, rateBar, speedBar, rangeBar;
	public GameObject statsPanel;
	public Text nameText, typeText, costText, sizeText;

	int slotNumSelected;
	Card card;

	public void ShowCardStats(Card cardSelected){
		if(card != cardSelected){
			card = cardSelected;
			icon.sprite = card.icon;

			nameText.text = card.name;
			typeText.text = card.type;
			costText.text = "$ " + card.cost;
			sizeText.text = "Size: " + card.size;

			healthBar.fillAmount = card.health / 100;
			damageBar.fillAmount = card.attackDamage / 10;
			rateBar.fillAmount = (card.actionCoolDown > 0) ? (1f / card.actionCoolDown) / 5 : 0;
			speedBar.fillAmount = card.movemonetSpeed / 10;
			rangeBar.fillAmount = card.actionRange / 10;
		}
		statsPanel.SetActive(true);
	}

	public void CloseCardStats(){
		statsPanel.SetActive(false);
	}
}
