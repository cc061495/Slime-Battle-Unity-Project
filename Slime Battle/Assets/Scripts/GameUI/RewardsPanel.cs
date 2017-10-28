/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RewardsPanel : MonoBehaviour {

	public Text costText, roundBounsText, extraBounsText, totalText;

	public void TextSetting(){
		int cost = PlayerStats.playerCost;
		int roundBouns = (PlayerStats.Instance.GetBounsCost() * GameManager.Instance.currentRound);
		int extraBouns = 0;
		
		StartCoroutine(AnimateText(costText, PlayerStats.playerCost, "$ "));
		StartCoroutine(AnimateText(roundBounsText, roundBouns, "+ "));
		StartCoroutine(AnimateText(extraBounsText, extraBouns, "+ "));

		int total = cost + roundBouns;
		StartCoroutine(AnimateText(totalText, total, "$ "));
	}

	IEnumerator AnimateText(Text textToAnimate, int end, string symbol){
		int startValue = 0, endValue = end;

		while(startValue < endValue){
			if((endValue - startValue) > 100)
				startValue += 100;
			else if((endValue - startValue) > 10)
				startValue += 10;
			else if((endValue - startValue) > 0)
				startValue++;
				
			textToAnimate.text = symbol + startValue.ToString();
			yield return new WaitForSeconds(0.05f);
		}
	}

	public void ResetAllTheText(){
		costText.text = "$ " + 0;
		roundBounsText.text = "+ " + 0;
		extraBounsText.text = "+ " + 0;
		totalText.text = "$ " + 0;
	}
}
