/* Copyright (c) cc061495 */
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	public GameObject pauseMenuPanel, confirmMenuPanel;

	public void PauseButtonPressed(){
		TogglePausePanel();
	}

	private void TogglePausePanel(){
		pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
	}

	public void Resume(){
		TogglePausePanel();
	}

	public void QuitButtonPressed(){
		TogglePausePanel();
		confirmMenuPanel.SetActive(true);
	}

	public void CancelButtonPressed(){
		confirmMenuPanel.SetActive(false);
	}

	public void Quit(){
		GetComponent<GameManager>().LeaveTheRoom();
	}
}
