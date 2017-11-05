using UnityEngine;
using UnityEngine.UI;

public class LobbyNetwork : MonoBehaviour
{
    public Button createRoomButton;
    public bool isPressedBackButton;

    public Text countPlayersOnlineText, networkInfoText;
    // Use this for initialization
    private void Start(){
        /* Show the Network Info */
        DisplayNetworkInfo();
        //Connect to Photon as configured in the editor
        //if player is not connected, connect to the Photon
        if (!PhotonNetwork.connected){
            Debug.Log("Connecting to server...");
            PhotonNetwork.ConnectUsingSettings("0.0.0"); //specify a game version

            DisplayNetworkInfo();
        }
        else{
            // createRoomButton.interactable = true;
            /* Rejoin the default Lobby, otherwise you cannot find the rooms */
            /* needs to call OnReceivedRoomListUpdate() to find the rooms */
            PhotonNetwork.Reconnect();
        }
    }
    //called by photon when user connected to the Photon Cloud
    private void OnConnectedToMaster(){
        DisplayNetworkInfo();
        Debug.Log("Connected to master");
        PhotonNetwork.automaticallySyncScene = false;
        //get the Player name in PlayerNetwork script
        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;
        //join the default Lobby
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    //called on entering a lobby on the Master Server
    private void OnJoinedLobby(){
        DisplayNetworkInfo();
        Debug.Log("Joined Lobby.");
        //player can create a room now if the player is connected to the Photon Cloud
        createRoomButton.interactable = true;

        if (!PhotonNetwork.inRoom)
            MainCanvasManager.Instance.lobbyCanvas.transform.SetAsLastSibling();
    }
    //called after disconnecting from the Photon server
    private void OnDisconnectedFromPhoton(){
        DisplayNetworkInfo();
        createRoomButton.interactable = false;

        if(!isPressedBackButton){
            Debug.Log("Reconnecting the Server");
            PhotonNetwork.Reconnect();
        }
    }

	private void DisplayNetworkInfo(){
		Debug.Log(PhotonNetwork.connectionState);
        string colorTag = "color=#ffffff96"; //white

        if(PhotonNetwork.connectionState.ToString() == "Disconnected")
            colorTag = "<color=#ff000096>"; //red
        else if(PhotonNetwork.connectionState.ToString() == "Connecting")
            colorTag = "<color=#ffff0096>"; //yellow
        else if(PhotonNetwork.connectionState.ToString() == "Connected")
            colorTag = "<color=#00ff0096>"; //lime

		networkInfoText.text = "Server Region: " + "<b>" + PhotonNetwork.CloudRegion.ToString().ToUpper() +"</b>"+ 
							   "\nState: " + "<b>" + colorTag + PhotonNetwork.connectionState.ToString()+"</color></b>";
	}

    public void OnLobbyStatisticsUpdate(){
        string countPlayersOnline;
	 	countPlayersOnline = PhotonNetwork.countOfPlayers.ToString() + " Online Players";
	 	countPlayersOnlineText.text = countPlayersOnline; 
    }
}
