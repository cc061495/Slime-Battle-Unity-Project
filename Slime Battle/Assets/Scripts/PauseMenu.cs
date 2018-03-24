/* Copyright (c) cc061495 */
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	public GameObject pauseMenuPanel, confirmMenuPanel;

	public void PauseButtonPressed(){
		TogglePausePanel();
		AudioManager.instance.Play("Tap");
	}

	private void TogglePausePanel(){
		pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);
	}

	public void Resume(){
		TogglePausePanel();
		AudioManager.instance.Play("TapBack");
	}

	public void QuitButtonPressed(){
		TogglePausePanel();
		confirmMenuPanel.SetActive(true);
		AudioManager.instance.Play("Tap");
	}

	public void CancelButtonPressed(){
		confirmMenuPanel.SetActive(false);
		AudioManager.instance.Play("TapBack");
	}

	public void Quit(){
		AudioManager.instance.Play("Tap");
		GetComponent<GameManager>().LeaveTheRoom();
	}
}
