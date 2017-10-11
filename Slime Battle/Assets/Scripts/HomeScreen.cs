/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour {

	public SceneFader sceneFader;

	void Awake(){
		//fix the fps = 30 in game
		//Application.targetFrameRate = 30;
	}

	public void GoToGameMenuScene(){
		Debug.Log("Game Menu!");
		sceneFader.FadeTo("GameMenu");
	}

	public void QuitGame(){
		Application.Quit();
	}
}
