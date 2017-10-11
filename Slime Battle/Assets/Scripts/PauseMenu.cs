/* Copyright (c) cc061495 */
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	public GameObject pauseMenuPanel;

	public void PauseButtonPressed(){
		TogglePausePanel();
	}

	private void TogglePausePanel(){
		pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
	}

	public void Resume(){
		TogglePausePanel();
	}

	public void Quit(){
		GetComponent<GameManager>().LeaveTheRoom();
	}
}
