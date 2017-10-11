/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour {

	public SceneFader sceneFader;

	public void GoToGameLobby(){
		if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.home){
			Card[] deck = PlayerCardDeck.Instance.deck;
			for (int i = 0; i < deck.Length; i++){
				if(PlayerCardDeck.Instance.deck[i] != null){
					Debug.Log("Game Lobby!");
					sceneFader.FadeTo("GameLobby");
					break;
				}
			}
		}
	}

	public void QuitGame(){
		Application.Quit();
	}
}
