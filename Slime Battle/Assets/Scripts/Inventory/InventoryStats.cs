/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;

public class InventoryStats : MonoBehaviour {

	public static InventoryStats Instance;
    
    void Awake(){
        Instance = this; 
    }

	public Image icon, healthBar, damageBar, speedBar, rangeBar, rateBar;
	public GameObject statsPanel, leftArrowButton, rightArrowButton, statusPage, modelPage;
	public Text nameText, typeText, costText, sizeText;
	public MeshFilter slimeModel;

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
			sizeText.text = "Size: " + SizeConvert(card.size);

			slimeModel.mesh = card.mesh;
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
		if(statusPage.activeSelf)
			SettingPageActive(false);
		else{
			SettingPageActive(true);
		}
	}

	private void SettingPageActive(bool display){
		statusPage.SetActive(display);
		modelPage.SetActive(!display);
	}

	public void ModelRotateLeft(){
		Debug.Log("HI");
		//Quaternion r = slimeModel.transform.rotation;
		//slimeModel.transform.rotation = Quaternion.Euler (0f, 0f, 0f);
	}

	public void ModelRotateRight(){
		slimeModel.transform.Rotate(0, -Time.deltaTime, 0, Space.Self);
	}
}
