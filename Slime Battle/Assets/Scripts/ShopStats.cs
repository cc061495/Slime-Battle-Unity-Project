/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;

public class ShopStats : MonoBehaviour {

	public Image icon, healthBar, damageBar, speedBar, rangeBar, rateBar;
	public Text typeText, costText, sizeText;
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
		showCardStatsBar = true;
	}

	public void CloseCardStats(){
		showCardStatsBar = false;
		ResetAllStatsBar();
	}

	private void ShowCardInfo(Card cardSelected){
		if(card != cardSelected){
			card = cardSelected;
			icon.sprite = card.icon;

			typeText.text = card.type;
			costText.text = "$ " + card.cost;
			sizeText.text = "Size: " + SizeConvert(card.size);
		}
	}

	private void ResetAllStatsBar(){
		healthBar.fillAmount = 0;
		damageBar.fillAmount = 0;
		speedBar.fillAmount = 0;
		rangeBar.fillAmount = 0;
		rateBar.fillAmount = 0;
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
}
