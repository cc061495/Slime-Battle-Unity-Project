/* Copyright (c) cc061495 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour {

	public SceneFader sceneFader;
	public Animator errorTextAnimator;
	public Text errorText;

	public void GoToGameLobby(){
		if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.home){
			if(IsDeckValid()){
				Debug.Log("Game Lobby!");
				AudioManager.instance.Play("Tap");
				sceneFader.FadeTo("GameLobby");
			}
			else
				AudioManager.instance.Play("Error");
		}
	}

	public void QuitGame(){
		Application.Quit();
	}

	private bool IsDeckValid(){
		Card[] deck = PlayerCardDeck.Instance.deck;
		for (int i = 0; i < deck.Length; i++){
			// Check the deck is exist AND combat card or not.
			if(deck[i] != null && deck[i].combatCard)
				return true;
		}
		if(!errorText.enabled)
			StartCoroutine(ErrorTextActive());
		return false;
	}

	private IEnumerator ErrorTextActive(){
		errorText.text = "ERROR: Your card deck is invalid.";
		errorText.enabled = true;
		errorTextAnimator.Play(0);
		yield return new WaitForSeconds(5f);
		errorText.enabled = false;
	}
}
