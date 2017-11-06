/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RewardsPanel : MonoBehaviour {

	public Text costText, roundBounsText, extraBounsText, totalText;

	public void TextSetting(){
		StartCoroutine(StartAnimateText());
	}

	IEnumerator StartAnimateText(){
		int cost = PlayerStats.playerCost;
		int roundBouns = (PlayerStats.Instance.GetBounsCost() * GameManager.Instance.currentRound);
		int extraBouns = 0;
		int total = cost + roundBouns;

		costText.text = "$ " + PlayerStats.playerCost;
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine(AnimateText(roundBounsText, roundBouns, "+ "));
		yield return StartCoroutine(AnimateText(extraBounsText, extraBouns, "+ "));
		yield return StartCoroutine(AnimateText(totalText, total, "$ "));
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
			yield return new WaitForSeconds(0.03f);
		}
	}

	public void ResetAllTheText(){
		costText.text = "$ " + 0;
		roundBounsText.text = "+ " + 0;
		extraBounsText.text = "+ " + 0;
		totalText.text = "$" + 0;
	}
}