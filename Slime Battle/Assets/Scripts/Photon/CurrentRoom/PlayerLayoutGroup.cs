using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerLayoutGroup : Photon.MonoBehaviour{
    public SceneFader sceneFader;

    #region UI_items
        [SerializeField]
        private GameObject _playerListingPrefab;
        private GameObject PlayerListingPrefab{
            get { return _playerListingPrefab; }
        }
        [SerializeField]
        private Text _roomMatchText;
        private Text roomMatchText{
            get { return _roomMatchText; }
        }

        [SerializeField]
        private Button _roomMatchBtn;
        private Button roomMatchBtn{
            get { return _roomMatchBtn; }
        }

        [SerializeField]
        private Button _roomLeaveBtn;
        private Button roomLeaveBtn{
            get { return _roomLeaveBtn; }
        }
    #endregion
    
    private bool isRoomReady = false;
    private List<PlayerListing> _playerListings = new List<PlayerListing>();
    private List<PlayerListing> PlayerListings{
        get { return _playerListings; }
    }

    //called by photon whenever the master client is switched.
    private void OnMasterClientSwitched(PhotonPlayer newMasterClient){
        UpdateButtonsLayout();
        int index = PlayerListings.FindIndex(x => x.PhotonPlayer == newMasterClient);
        PlayerListings[index].HostSetting();
        
    }

    //called by photon whenever you join a room.
    private void OnJoinedRoom(){
        foreach (Transform child in transform){
            Destroy(child.gameObject);
        }

        UpdateButtonsLayout();
        //show the currentRoom canvas
        MainCanvasManager.Instance.CurrentRoomCanvas.transform.SetAsLastSibling();
        //find all the players in the room
        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
        for (int i = 0; i < photonPlayers.Length; i++){
            PlayerJoinedRoom(photonPlayers[i]);
        }
    }

    //called by photon when a player joins the room.
    private void OnPhotonPlayerConnected(PhotonPlayer photonPlayer){
        PlayerJoinedRoom(photonPlayer);
    }

    //called by photon when a player leaves the room.
    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer){
        if (isRoomReady)
            photonView.RPC("toggleIsRoomReady", PhotonTargets.All);

        PlayerLeftRoom(photonPlayer);
    }

    private void PlayerJoinedRoom(PhotonPlayer photonPlayer){
        if (photonPlayer == null)
            return;

        PlayerLeftRoom(photonPlayer);

        GameObject playerListingObj = Instantiate(PlayerListingPrefab);
        playerListingObj.transform.SetParent(transform, false);

        PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing>();
        playerListing.ApplyPhotonPlayer(photonPlayer);
        PlayerListings.Add(playerListing);
    }

    private void PlayerLeftRoom(PhotonPlayer photonPlayer){
        //find the player in PhotonNetwork
        //find the player index which the photonPlayer(PhotonNetwork) matchs PlayerListings.PhotonPlayer
        int index = PlayerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);
        //if player is found
        if (index != -1){
            Destroy(PlayerListings[index].gameObject);
            PlayerListings.RemoveAt(index);
        }
    }
    //Leave the room
    public void OnClickLeaveRoom(){
        if (isRoomReady)
            photonView.RPC("toggleIsRoomReady", PhotonTargets.All);

        PhotonNetwork.LeaveRoom();
    }
    //Start the match
    public void OnClickStartSync(){
        if (PhotonNetwork.isMasterClient){
            //Master Client Start Match Button
            if (PhotonNetwork.room.PlayerCount == 2 && isRoomReady){
                //Lock the room and load the level
                PhotonNetwork.room.IsOpen = false;
                PhotonNetwork.room.IsVisible = false;
                photonView.RPC("RPC_DisableAllButtons", PhotonTargets.All);
		        sceneFader.FadeToWithPhotonNetwork("Main");
            }
            else
                Debug.Log("Not enought players");
        }
        else{
            //Server Client Ready Button
            photonView.RPC("toggleIsRoomReady", PhotonTargets.All); //toggle isRoomReady
                                                                    //change the Ready button color to Green
            roomMatchBtn.GetComponent<Image>().color = (isRoomReady) ? Color.green : Color.white;
        }
    }

    private void UpdateButtonsLayout(){
        if (!PhotonNetwork.isMasterClient){
            //Server Client
            roomMatchText.text = "Ready";       //Ready Text
            roomMatchBtn.interactable = true;   //Active Ready button
            //Set the Ready button color
            roomMatchBtn.GetComponent<Image>().color = (isRoomReady) ? Color.green : Color.white;
        }
        else{
            //Server Master Client
            roomMatchText.text = "Start\nMatch"; //Start Match Text
            roomMatchBtn.interactable = (isRoomReady) ? true : false;
            //Reset Match button Color
            roomMatchBtn.GetComponent<Image>().color = Color.white;
        }
    }

    [PunRPC]
    private void toggleIsRoomReady(){
        isRoomReady = !isRoomReady;
        //Host can start the match if the players are ready
        if (PhotonNetwork.isMasterClient)
            roomMatchBtn.interactable = (isRoomReady) ? true : false;
    }

    [PunRPC]
    private void RPC_DisableAllButtons(){
        roomMatchBtn.interactable = false;
        roomLeaveBtn.interactable = false;
    }
}