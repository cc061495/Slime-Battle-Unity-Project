using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : Photon.MonoBehaviour {

	public RoomSetting roomSetting;

	public void OnClick_CreateRoom(){
		RoomOptions roomOptions = new RoomOptions (){ IsVisible = true, IsOpen = true, MaxPlayers = 2 };

		//Creates a room but fails if this room is existing already
		if (PhotonNetwork.CreateRoom (roomSetting.GetRoomName(), roomOptions, TypedLobby.Default)) {
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

		ExitGames.Client.Photon.Hashtable customRoom_Properties = new ExitGames.Client.Photon.Hashtable();

		customRoom_Properties.Add("Rounds", roomSetting.GetRoundOfGame());
		PhotonNetwork.room.SetCustomProperties(customRoom_Properties);
		// customRoom_Properties.Add("RoomName", roomSetting.roomName);
		
		// string name = (string) PhotonNetwork.room.CustomProperties["RoomName"];
		// int bo = (int) PhotonNetwork.room.CustomProperties["Rounds"];
		// Debug.Log(name + ", " + bo);
	}
}
