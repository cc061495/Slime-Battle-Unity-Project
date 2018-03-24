using UnityEngine;

public class LobbyCanvas : MonoBehaviour {
	
    public SceneFader sceneFader;
	public LobbyNetwork lobbyNetwork;
	public GameObject roomSettingPanel;

	[SerializeField]
	private RoomLayoutGroup _roomlayoutGroup;
	private RoomLayoutGroup RoomLayoutGroup{
		get{ return _roomlayoutGroup; }
	}

	public void OnClickJoinRoom(string roomName){
		if (PhotonNetwork.JoinRoom (roomName)) {
			Debug.Log ("Join room: " + roomName);
			AudioManager.instance.Play("Tap");
		} 
		else {
			Debug.Log ("Join room failed");
			AudioManager.instance.Play("Error");
		}
	}

    public void OnClick_BackButton(){
        Destroy(GameObject.Find("DDOL"));
		Destroy(GameObject.Find("PlayerCardDeck"));
		lobbyNetwork.isPressedBackButton = true;
        PhotonNetwork.Disconnect();
		AudioManager.instance.Play("TapBack");
        sceneFader.FadeTo("GameMenu");
    }

	public void OnClick_RoomSettingButton(){
		roomSettingPanel.SetActive(true);
		AudioManager.instance.Play("Tap");
	}
}
