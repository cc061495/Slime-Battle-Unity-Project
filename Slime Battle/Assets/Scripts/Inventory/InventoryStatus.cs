using UnityEngine;
using UnityEngine.UI;

public class InventoryStatus : MonoBehaviour {

	public static InventoryStatus Instance;
    
    void Awake(){
        Instance = this; 
    }

	public Image icon, healthBar, damageBar, speedBar, rangeBar, rateBar;
	public GameObject statsPanel;
	public Text nameText, typeText, costText, sizeText;

	private float lerpSpeed = 2f;
	bool showCardStatusBar = false;
	int slotNumSelected;
	Card card;

	void Update(){
		if(showCardStatusBar){
			healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, card.health / 100, Time.deltaTime * lerpSpeed);
			damageBar.fillAmount = Mathf.Lerp(damageBar.fillAmount, card.attackDamage / 10, Time.deltaTime * lerpSpeed);
			speedBar.fillAmount = Mathf.Lerp(speedBar.fillAmount, card.movemonetSpeed / 10, Time.deltaTime * lerpSpeed);
			rangeBar.fillAmount = Mathf.Lerp(rangeBar.fillAmount, card.actionRange / 10, Time.deltaTime * lerpSpeed);
			if(card.actionCoolDown > 0)
				rateBar.fillAmount = Mathf.Lerp(rateBar.fillAmount, (1f / card.actionCoolDown) / 5, Time.deltaTime * lerpSpeed);
		}
	}

	public void ShowCardStats(Card cardSelected){
		if(card != cardSelected){
			card = cardSelected;
			icon.sprite = card.icon;

			nameText.text = card.name;
			typeText.text = card.type;
			costText.text = "$ " + card.cost;
			sizeText.text = "Size: " + card.size;
		}
		statsPanel.SetActive(true);
		MenuScreen.Instance.BackButtonDisplay(false);
		showCardStatusBar = true;
	}

	public void CloseButtonPressed(){
		CloseCardStatus();
		MenuScreen.Instance.BackButtonDisplay(true);
	}

	public void CloseCardStatus(){
		statsPanel.SetActive(false);
		showCardStatusBar = false;

		healthBar.fillAmount = 0;
		damageBar.fillAmount = 0;
		speedBar.fillAmount = 0;
		rangeBar.fillAmount = 0;
		rateBar.fillAmount = 0;
	}
}
