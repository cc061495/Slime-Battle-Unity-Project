using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateRoom : Photon.MonoBehaviour {

	public RoomSetting roomSetting;

	public void OnClick_CreateRoom(){
		RoomOptions roomOptions = new RoomOptions (){ IsVisible = true, IsOpen = true, MaxPlayers = 2 };
		roomOptions.CustomRoomProperties = new Hashtable();
		/* The room's custom properties to set */
		roomOptions.CustomRoomProperties.Add("Name", roomSetting.GetRoomName());
		roomOptions.CustomRoomProperties.Add("Round", roomSetting.GetRoundOfGame());
		/* Defines the custom room properties that get listed in the lobby */
		roomOptions.CustomRoomPropertiesForLobby = new string[] {"Name", "Round"};

		string roomID = PlayerSetting.Instance.playerName + "," + Random.Range (1, 10000);
		//Creates a room but fails if this room is existing already
		if (PhotonNetwork.CreateRoom (roomID, roomOptions, TypedLobby.Default)) {
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
