using UnityEngine;

public class LobbyCanvas : MonoBehaviour {
	
    public SceneFader sceneFader;
	public LobbyNetwork lobbyNetwork;

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

    public void OnClick_BackButton(){
        Destroy(GameObject.Find("DDOL"));
		Destroy(GameObject.Find("PlayerCardDeck"));
		lobbyNetwork.isPressedBackButton = true;
        PhotonNetwork.Disconnect();
        sceneFader.FadeTo("GameMenu");
    }
}
