using UnityEngine;
using UnityEngine.UI;

public class CurrentRoomCanvas : MonoBehaviour {
	
	[SerializeField]
	private Text _roomNameText;
	private Text roomNameText{
		get{ return _roomNameText; }
	}

	//called by photon whenever you join a room.
	private void OnJoinedRoom(){
		roomNameText.text = PhotonNetwork.room.Name;
	}
	/*
	public void OnClickStartSync(){
		if (PhotonNetwork.isMasterClient)
			PhotonNetwork.LoadLevel ("Main");
		else
			Debug.Log ("You are not the room master in this room");
	}
	*/
}
