/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RewardsPanel : MonoBehaviour {

	public GameObject pauseMenu, confirmPanel;
	public Text playerCoinsText, roundBonusText, winBonusText, totalCoinsText, resultText;
	private int coins, roundBonus, winBonus, total;

	public void TextSetting(){
		StartCoroutine(StartAnimateText());
		ClosePauseMenuIfOpened();
	}

	IEnumerator StartAnimateText(){
		playerCoinsText.text = "$" + GetPlayerCoins();
		yield return new WaitForSeconds(4.5f);
		yield return StartCoroutine(AnimateText(roundBonusText, GetRoundBouns(), "+ "));
		yield return StartCoroutine(AnimateText(winBonusText, winBonus, "+ "));
		yield return StartCoroutine(AnimateText(totalCoinsText, total, "$"));
	}

	IEnumerator AnimateText(Text textToAnimate, int end, string symbol){
		int startValue = 0, endValue = end;

		while(startValue < endValue){
			if((endValue - startValue) > 10000)
				startValue += 10000;
			else if((endValue - startValue) > 1000)
				startValue += 1000;
			else if((endValue - startValue) > 100)
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
			SetUpTwoPlayersWinBouns(50, 10, "Victory", "Defeat");
		else if(winner.Equals("blue"))
			SetUpTwoPlayersWinBouns(10, 50, "Defeat", "Victory");
		else if(winner.Equals("draw"))
			SetUpTwoPlayersWinBouns(50, 50, "Draw", "Draw");
	}

	private void SetUpTwoPlayersWinBouns(int redWinBouns, int blueWinBouns, string redResult, string blueResult){
		if(PhotonNetwork.isMasterClient){
			winBonus = redWinBouns;
			resultText.color = GetResultTextColor(redResult);
			resultText.text = redResult;
		}
		else{
			winBonus = blueWinBouns;
			resultText.color = GetResultTextColor(blueResult);
			resultText.text = blueResult;
		}

		SetTotalCoins(winBonus);
	}

	private Color GetResultTextColor(string result){
		if(result.Equals("Victory"))
			return Color.green;
		else if(result.Equals("Defeat"))
			return Color.red;
		else
			return Color.white;
	}

	private void ClosePauseMenuIfOpened(){
		if(pauseMenu.activeSelf)
			pauseMenu.SetActive(false);
		if(confirmPanel.activeSelf)
			confirmPanel.SetActive(false);
	}

	private void SetTotalCoins(int _winBouns){
		total = GetPlayerCoins() + GetRoundBouns() + _winBouns;
		PlayerData.Instance.SavePlayerCoins(total);
	}

	private int GetPlayerCoins(){
		return PlayerData.Instance.playerCoins;
	}

	private int GetRoundBouns(){
		return (10 * GameManager.Instance.currentRound);
	}
}