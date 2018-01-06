/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RewardsPanel : MonoBehaviour {

	public Text costText, roundBonusText, extraBonusText, totalText;

	public void TextSetting(){
		StartCoroutine(StartAnimateText());
	}

	IEnumerator StartAnimateText(){
		int cost = PlayerStats.playerCost;
		int roundBonus = (PlayerStats.Instance.GetBounsCost() * GameManager.Instance.currentRound);
		int extraBonus = 0;
		int total = cost + roundBonus;

		costText.text = "$ " + PlayerStats.playerCost;
		yield return new WaitForSeconds(0.5f);
		yield return StartCoroutine(AnimateText(roundBonusText, roundBonus, "+ "));
		yield return StartCoroutine(AnimateText(extraBonusText, extraBonus, "+ "));
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
		roundBonusText.text = "+ " + 0;
		extraBonusText.text = "+ " + 0;
		totalText.text = "$" + 0;
	}
}