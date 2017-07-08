using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour {

	[SerializeField]
	private RoomLayoutGroup _roomlayoutGroup;
	private RoomLayoutGroup RoomLayoutGroup{
		get{ return _roomlayoutGroup; }
	}

	public void OnClickJoinRoom(string roomName){
		if (PhotonNetwork.JoinRoom (roomName)) {
			Debug.Log ("Join room: " + roomName);
			//MainCanvasManager.Instance.CurrentRoomCanvas.setPlayerRoomNameText (roomName);
		} 
		else {
			Debug.Log ("Join room failed");
		}
	}
}
