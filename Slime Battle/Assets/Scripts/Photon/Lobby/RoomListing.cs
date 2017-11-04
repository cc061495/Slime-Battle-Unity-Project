using UnityEngine;
using UnityEngine.UI;

public class RoomListing : MonoBehaviour {
	//display the room name
	[SerializeField]
	private Text _roomNameText;
	private Text roomNameText{
		get{ return _roomNameText; }
	}

	[SerializeField]
	private Text _roomInfoText;
	private Text roomInfoText{
		get{ return _roomInfoText; }
	}

	[SerializeField]
	private Text _roundText;
	private Text roundText{
		get{ return _roundText; }
	}

	public string RoomName{ get; private set; }
	public int RoomPlayerCount{ get ; private set; }
	public bool Updated{ get; set; }

	// Use this for initialization
	private void Start () {
		//get the LobbyCanvas GameObject in Main Canvas
		GameObject lobbyCanvasObj = MainCanvasManager.Instance.lobbyCanvas.gameObject;
		if (lobbyCanvasObj == null)
			return;
		//get the lobbyCanvas srcipt in lobbyCanvas Object
		LobbyCanvas lobbyCanvas = lobbyCanvasObj.GetComponent<LobbyCanvas> ();
		//Add button listener and call the LobbyCanvas.OnClickJoinRoom function by pressed the button
		Button button = GetComponent<Button> ();
		button.onClick.AddListener (() => lobbyCanvas.OnClickJoinRoom (RoomName));
	}
	//called by Unity, if the gameObject is Destroyed
	private void OnDestroy(){
		Button button = GetComponent<Button> ();
		button.onClick.RemoveAllListeners ();
	}

	public void SetRoomNameText(string name, int playerCount, int maxPlayerCount, string roomID, int round){
		RoomName = roomID;
		RoomPlayerCount = playerCount;
		roomNameText.text = name;
		roomInfoText.text = "Player Count: " + RoomPlayerCount + "/" + maxPlayerCount;
		roundText.text = "Format: "+ "BO" + round;
	}
}
