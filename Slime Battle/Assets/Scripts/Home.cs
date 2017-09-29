/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour {

	public void GoToGameLobby(){
		if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.home){
			Card[] deck = PlayerCardDeck.Instance.deck;
			for (int i = 0; i < deck.Length; i++){
				if(PlayerCardDeck.Instance.deck[i] != null){
					Debug.Log("Game Lobby!");
					SceneManager.LoadScene("GameLobby");
					break;
				}
			}
		}
	}

	public void QuitGame(){
		Application.Quit();
	}
}
