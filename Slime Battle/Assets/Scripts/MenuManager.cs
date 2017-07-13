/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	void Awake(){
		//fix the fps = 30 in game
		//Application.targetFrameRate = 30;
	}

	public void SingleplayerGame(){
		Debug.Log("Start singleplayer game!");
		SceneManager.LoadScene("Main");
	}

	public void MultiplayerGame(){
		Debug.Log("Start multiplayer game!");
		SceneManager.LoadScene("GameLobby");
	}

	public void QuitGame(){
		Application.Quit();
	}
}
