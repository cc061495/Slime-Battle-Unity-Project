/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;

public class InventoryStats : MonoBehaviour {

	public static InventoryStats Instance;
	private StatsModel statsModel;
    
    void Awake(){
        Instance = this; 
		statsModel = GetComponent<StatsModel>();
    }

	public Image icon, healthBar, damageBar, speedBar, rangeBar, rateBar, PageImage;
	public GameObject statsPanel, leftArrowButton, rightArrowButton, statusPage, modelPage;
	public Text nameText, typeText, costText, sizeText;
	public Sprite modelIcon, statusIcon;

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
			else if(card.castTime > 0)
				rateBar.fillAmount = Mathf.Lerp(rateBar.fillAmount, (1f / card.castTime) / 5, Time.deltaTime * lerpSpeed);
		}
	}

	public void ShowCardStats(Card cardSelected){
		statsPanel.SetActive(true);
		ShowCardInfo(cardSelected);

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

		if(!statusPage.activeSelf)
			BackToStatusPage();

		statsModel.ResetModelRotation();
	}

	public void ArrowButtonPressed(int nextNum){
		int currentCardNum = (Inventory.Instance.cards.IndexOf(card));
		Card nextCard = Inventory.Instance.cards[currentCardNum + nextNum];
		ShowAnotherCardStats(nextCard);
	}

	private void ShowAnotherCardStats(Card cardSelected){
		ShowCardInfo(cardSelected);
		ResetAllStatsBar();
		statsModel.ResetModelRotation();
	}

	private void ShowCardInfo(Card cardSelected){
		if(card != cardSelected){
			card = cardSelected;
			icon.sprite = card.icon_red;

			nameText.text = card.name;
			typeText.text = card.type;
			costText.text = "$ " + card.cost;
			sizeText.text = "Size: " + SizeConvert(card.size);

			statsModel.SetupTheModelMesh(card.mesh);
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

	private string SizeConvert(int size){
		if(size == 1)
			return "1x1";
		else if(size == 2)
			return "1x2";
		else if(size == 4)
			return "2x2";
		else
			return "ERROR";
	}

	public void PageChanges(){
		if(statusPage.activeSelf){
			SettingPageActive(false);
			PageImage.sprite = statusIcon;
		}
		else{
			SettingPageActive(true);
			PageImage.sprite = modelIcon;
		}
	}

	private void SettingPageActive(bool display){
		statusPage.SetActive(display);
		modelPage.SetActive(!display);
	}

	private void BackToStatusPage(){
		statusPage.SetActive(true);
		modelPage.SetActive(false);
		PageImage.sprite = modelIcon;
	}
}
