/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;

public class RoomSetting : MonoBehaviour {

	public Toggle[] matchFormat = new Toggle[3];
	public InputField roomNameText;
	private int roundOfGames = 5;
	private string roomName;

	public void OnClick_SaveButton(){
		/* Saving Room Setting */
		roomName = roomNameText.text;

		for (int i = 0; i < matchFormat.Length; i++){
			if(matchFormat[i].isOn)
				roundOfGames = ConvertToMatchRounds(i);
		}

		this.gameObject.SetActive(false);
		AudioManager.instance.Play("Tap");
	}

	public void OnClick_CancelButton(){
		roomNameText.text = roomName;
		matchFormat[ConvertToToggleIndex(roundOfGames)].isOn = true;
		this.gameObject.SetActive(false);
		AudioManager.instance.Play("TapBack");
	}

	private int ConvertToMatchRounds(int input){
		if(input == 0)
			return 5;
		else if(input == 1)
			return 7;
		else
			return 9;
	}

	private int ConvertToToggleIndex(int input){
		if(input == 5)
			return 0;
		else if(input == 7)
			return 1;
		else
			return 2;
	}

	public string GetRoomName(){
		//check the room name is empty, set default room name
		if(string.IsNullOrEmpty(roomName))
			roomName = PlayerSetting.Instance.playerName + "'s room";

		return roomName;
	}

	public int GetRoundOfGame(){
		return roundOfGames;
	}
}
