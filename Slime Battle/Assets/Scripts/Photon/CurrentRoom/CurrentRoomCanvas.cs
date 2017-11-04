using UnityEngine;
using UnityEngine.UI;

public class CurrentRoomCanvas : MonoBehaviour {
	
	[SerializeField]
	private Text _roomNameText;
	private Text roomNameText{
		get{ return _roomNameText; }
	}

	[SerializeField]
	private Text _roundText;
	private Text roundText{
		get{ return _roundText; }
	}

	//called by photon whenever you join a room.
	private void OnJoinedRoom(){
		roomNameText.text = (string) PhotonNetwork.room.CustomProperties["Name"];
		roundText.text = "BO" + (int) PhotonNetwork.room.CustomProperties["Round"];
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
