using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : Photon.MonoBehaviour {
	[SerializeField]
	private Text _roomName;
	private Text RoomName{
		get{ return _roomName; }
	}

	private string gameRoomName;

	public void OnClick_CreateRoom(){
		RoomOptions roomOptions = new RoomOptions (){ IsVisible = true, IsOpen = true, MaxPlayers = 2 };
		gameRoomName = RoomName.text;
		//check the room name is empty, set default room name
		if (RoomName.text == "")
			gameRoomName = PlayerNetwork.Instance.PlayerName + "'s room";
		
		//Creates a room but fails if this room is existing already
		if (PhotonNetwork.CreateRoom (gameRoomName, roomOptions, TypedLobby.Default)) {
			Debug.Log ("create room successful sent.");
		} else {
			Debug.Log ("create room failed to send");
		}
	}
	//called when a CreateRoom() call failed
	private void OnPhotonCreateRoomFailed(object[] codeAndMessage){
		Debug.Log ("create room failed: " + codeAndMessage [1]);
	}
	//called when a CreateRoom() call successed
	private void OnCreatedRoom(){
		Debug.Log ("Room created successfully."); 
	}
}
