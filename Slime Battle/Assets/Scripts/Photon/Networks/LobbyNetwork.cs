﻿using UnityEngine;
using UnityEngine.UI;

public class LobbyNetwork : MonoBehaviour
{
    public Button createRoomButton;
	public Text countPlayersOnlineText;
    public bool isPressedBackButton;
    // Use this for initialization
    private void Start(){
        //Connect to Photon as configured in the editor
        //if player is not connected, connect to the Photon
        if (!PhotonNetwork.connected){
            Debug.Log("Connecting to server...");
            PhotonNetwork.ConnectUsingSettings("0.0.0"); //specify a game version
        }
        else{
            createRoomButton.interactable = true;
            /* Rejoin the default Lobby, otherwise you cannot find the rooms */
            /* needs to call OnReceivedRoomListUpdate() to find the rooms */
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }
    }
    //called by photon when user connected to the Photon Cloud
    private void OnConnectedToMaster(){
        Debug.Log("Connected to master");
        PhotonNetwork.automaticallySyncScene = false;
        //get the Player name in PlayerNetwork script
        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;
        //join the default Lobby
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    //called on entering a lobby on the Master Server
    private void OnJoinedLobby(){
        Debug.Log("Joined Lobby.");
        //player can create a room now if the player is connected to the Photon Cloud
        createRoomButton.interactable = true;

        if (!PhotonNetwork.inRoom)
            MainCanvasManager.Instance.lobbyCanvas.transform.SetAsLastSibling();
    }
    //called after disconnecting from the Photon server
    private void OnDisconnectedFromPhoton(){
        if(!isPressedBackButton){
            Debug.Log("Reconnecting the Server");
            PhotonNetwork.Reconnect();
        }
    }

    public void OnLobbyStatisticsUpdate(){
        string countPlayersOnline;
	 	countPlayersOnline = PhotonNetwork.countOfPlayers.ToString() + " Online Players";
	 	countPlayersOnlineText.text = countPlayersOnline; 
    }
}
