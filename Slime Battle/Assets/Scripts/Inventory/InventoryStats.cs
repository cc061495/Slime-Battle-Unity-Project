/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;

public class InventoryStats : MonoBehaviour {

	public static InventoryStats Instance;
    
    void Awake(){
        Instance = this; 
    }

	public Image icon, healthBar, damageBar, speedBar, rangeBar, rateBar;
	public GameObject statsPanel, leftArrowButton, rightArrowButton;
	public Text nameText, typeText, costText, sizeText;

	private float lerpSpeed = 2f;
	bool showCardStatsBar = false;
	int slotNumSelected;
	Card card;

	void Update(){
		if(showCardStatsBar){
			healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, card.health / 100, Time.deltaTime * lerpSpeed);
			damageBar.fillAmount = Mathf.Lerp(damageBar.fillAmount, card.attackDamage / 10, Time.deltaTime * lerpSpeed);
			speedBar.fillAmount = Mathf.Lerp(speedBar.fillAmount, card.movemonetSpeed / 10, Time.deltaTime * lerpSpeed);
			rangeBar.fillAmount = Mathf.Lerp(rangeBar.fillAmount, card.actionRange / 10, Time.deltaTime * lerpSpeed);
			if(card.actionCoolDown > 0)
				rateBar.fillAmount = Mathf.Lerp(rateBar.fillAmount, (1f / card.actionCoolDown) / 5, Time.deltaTime * lerpSpeed);
		}
	}

	public void ShowCardStats(Card cardSelected){
		ShowCardInfo(cardSelected);

		statsPanel.SetActive(true);
		MenuScreen.Instance.BackButtonDisplay(false);
		showCardStatsBar = true;
	}

	public void CloseButtonPressed(){
		CloseCardStats();
		MenuScreen.Instance.BackButtonDisplay(true);
	}

	public void CloseCardStats(){
		statsPanel.SetActive(false);
		showCardStatsBar = false;

		ResetAllStatsBar();
	}

	public void ArrowButtonPressed(int nextNum){
		int currentCardNum = (Inventory.Instance.cards.IndexOf(card));
		Card nextCard = Inventory.Instance.cards[currentCardNum + nextNum];
		ShowAnotherCardStats(nextCard);
	}

	private void ShowAnotherCardStats(Card cardSelected){
		ShowCardInfo(cardSelected);
		ResetAllStatsBar();
	}

	private void ShowCardInfo(Card cardSelected){
		if(card != cardSelected){
			card = cardSelected;
			icon.sprite = card.icon;

			nameText.text = card.name;
			typeText.text = card.type;
			costText.text = "$ " + card.cost;
			sizeText.text = "Size: " + card.size;
		}
		ShowArrowButton();
	}

	private void ResetAllStatsBar(){
		healthBar.fillAmount = 0;
		damageBar.fillAmount = 0;
		speedBar.fillAmount = 0;
		rangeBar.fillAmount = 0;
		rateBar.fillAmount = 0;
	}

	private void ShowArrowButton(){
		if(Inventory.Instance.cards.IndexOf(card) == 0)
			leftArrowButton.SetActive(false);
		else
			leftArrowButton.SetActive(true);

		if(Inventory.Instance.cards.IndexOf(card) == Inventory.Instance.cards.Count - 1)
			rightArrowButton.SetActive(false);
		else
			rightArrowButton.SetActive(true);
	}
}
