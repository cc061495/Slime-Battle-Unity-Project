using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCanvas : Photon.PunBehaviour {

	[SerializeField]
	private RoomLayoutGroup _roomlayoutGroup;
	private RoomLayoutGroup RoomLayoutGroup{
		get{ return _roomlayoutGroup; }
	}

	public Text countPlayersOnlineText;

	public void OnClickJoinRoom(string roomName){
		if (PhotonNetwork.JoinRoom (roomName)) {
			Debug.Log ("Join room: " + roomName);
			//MainCanvasManager.Instance.CurrentRoomCanvas.setPlayerRoomNameText (roomName);
		} 
		else {
			Debug.Log ("Join room failed");
		}
	}

	public override void OnLobbyStatisticsUpdate(){
		string countPlayersOnline;
		countPlayersOnline = PhotonNetwork.countOfPlayers.ToString() + " Players Online";
		countPlayersOnlineText.text = countPlayersOnline;
	}
}
