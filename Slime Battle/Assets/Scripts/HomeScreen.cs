/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour {

	void Awake(){
		//fix the fps = 30 in game
		//Application.targetFrameRate = 30;
	}

	public void GoToGameMenuScene(){
		Debug.Log("Game Menu!");
		SceneManager.LoadScene("GameMenu");
	}

	public void QuitGame(){
		Application.Quit();
	}
}
