/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour {

	public SceneFader sceneFader;

	void Awake(){
		//fix the fps = 60 in the home screen
		Application.targetFrameRate = 60;
	}

	public void GoToGameMenuScene(){
		Debug.Log("Game Menu!");
		sceneFader.FadeTo("GameMenu");
	}

	public void QuitGame(){
		Application.Quit();
	}
}
