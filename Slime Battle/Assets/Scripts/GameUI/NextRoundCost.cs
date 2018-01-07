/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NextRoundCost : MonoBehaviour {

	public Text costText, nextRoundCostText;
	Animator animator;

	public void TextSetting(){
		animator = GetComponent<Animator>();
		StartCoroutine(StartAnimateText());
	}

	IEnumerator StartAnimateText(){
		int cost = PlayerStats.playerCost;
		int roundBonus = (PlayerStats.Instance.GetBounsCost() * GameManager.Instance.currentRound);
		int costSpawned = PlayerStats.Instance.GetSpawnedCost();

		nextRoundCostText.text = "<color=#ffff00ff>Next Round Cost</color>";
		costText.text = "$0";
		yield return new WaitForSeconds(1.5f);

		animator.Play("CostDisplay", 0, 0f);
		nextRoundCostText.text = "<size=45>Remaining Cost +" + cost + "</size>";
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(AnimateText(costText, 0, cost));
		yield return new WaitForSeconds(2f);

		animator.Play("CostDisplay", 0, 0f);
		nextRoundCostText.text = "<size=45>Round Bonus +" + roundBonus + "</size>";
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(AnimateText(costText, cost, cost+roundBonus));
		yield return new WaitForSeconds(2f);

		animator.Play("CostDisplay", 0, 0f);
		nextRoundCostText.text = "<size=45>Cost Spawned +" + costSpawned + "</size>";
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(AnimateText(costText, cost+roundBonus, cost+roundBonus+costSpawned));
	}

	IEnumerator AnimateText(Text textToAnimate, int start, int end){
		int startValue = start, endValue = end;

		while(startValue < endValue){
			if((endValue - startValue) > 100)
				startValue += 100;
			else if((endValue - startValue) > 10)
				startValue += 10;
			else if((endValue - startValue) > 0)
				startValue++;
				
			textToAnimate.text = "$" + startValue.ToString();
			yield return new WaitForSeconds(0.03f);
		}
	}

	public void ResetAllTheText(){
		costText.text = "$" + 0;
		nextRoundCostText.text = "Next Round Cost";
	}
}
