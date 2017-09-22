/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour {

	public void GoToGameLobby(){
		if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.home){
			Debug.Log("Game Lobby!");
			SceneManager.LoadScene("GameLobby");
		}
	}

	public void QuitGame(){
		Application.Quit();
	}
}
