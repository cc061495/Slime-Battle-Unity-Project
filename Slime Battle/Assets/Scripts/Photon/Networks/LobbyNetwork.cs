using UnityEngine;

public class LobbyNetwork : MonoBehaviour
{

    // Use this for initialization
    private void Start()
    {
        //Connect to Photon as configured in the editor
        if (!PhotonNetwork.connected)
        {
            Debug.Log("Connecting to server...");
            PhotonNetwork.ConnectUsingSettings("0.0.0"); //specify a game version
        }
    }
    //called by photon when user connected to the Photon Cloud
    private void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.automaticallySyncScene = false;
        //get the Player name in PlayerNetwork script
        PhotonNetwork.playerName = PlayerNetwork.Instance.PlayerName;
        //join the default Lobby
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    //called on entering a lobby on the Master Server
    private void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby.");

        if (!PhotonNetwork.inRoom)
            MainCanvasManager.Instance.lobbyCanvas.transform.SetAsLastSibling();
    }
}
