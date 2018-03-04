/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RewardsPanel : MonoBehaviour {

	public GameObject pauseMenu, confirmPanel;
	public Text playerMoneyText, roundBonusText, winBonusText, totalMoneyText;
	private int money, roundBonus, winBonus;

	public void TextSetting(){
		StartCoroutine(StartAnimateText());
		ClosePauseMenuIfOpened();
	}

	IEnumerator StartAnimateText(){
		money = PlayerData.Instance.playerMoney;
		roundBonus = (10 * GameManager.Instance.currentRound);

		playerMoneyText.text = "$" + money;
		yield return new WaitForSeconds(2f);
		int total = money + roundBonus + winBonus;
		yield return StartCoroutine(AnimateText(roundBonusText, roundBonus, "+ "));
		yield return StartCoroutine(AnimateText(winBonusText, winBonus, "+ "));
		yield return StartCoroutine(AnimateText(totalMoneyText, total, "$"));
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
			yield return new WaitForSeconds(0.04f);
		}
	}

	public void SetUpWinBouns(string winner){
		if(winner.Equals("red"))
			SetUpTwoPlayersWinBouns(50, 10);
		else if(winner.Equals("blue"))
			SetUpTwoPlayersWinBouns(10, 50);
		else if(winner.Equals("draw"))
			SetUpTwoPlayersWinBouns(30, 30);
	}

	private void SetUpTwoPlayersWinBouns(int redWinBouns, int blueWinBouns){
		if(PhotonNetwork.isMasterClient)
			winBonus = redWinBouns;
		else
			winBonus = blueWinBouns;
	}

	private void ClosePauseMenuIfOpened(){
		if(pauseMenu.activeSelf)
			pauseMenu.SetActive(false);
		if(confirmPanel.activeSelf)
			confirmPanel.SetActive(false);
	}
}