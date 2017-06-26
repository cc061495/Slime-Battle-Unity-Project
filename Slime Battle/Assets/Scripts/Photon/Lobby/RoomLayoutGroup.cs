using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGroup : MonoBehaviour
{
    [SerializeField]
    private GameObject _roomListingPrefab;
    private GameObject RoomListingPrefab
    {
        get { return _roomListingPrefab; }
    }
    //create a roomListingButtons List
    private List<RoomListing> _roomListingButtons = new List<RoomListing>();
    private List<RoomListing> RoomListingButtons
    {
        get { return _roomListingButtons; }
    }
    //Called for any update of the room-listing while in a lobby
    private void OnReceivedRoomListUpdate()
    {
        //get all the rooms in the PhotonNetwork
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        //get each of the room
        foreach (RoomInfo room in rooms)
        {
            //check the room exists -> updated = true
            RoomReceived(room);
        }
        RemoveOldRooms();
    }

    private void RoomReceived(RoomInfo room)
    {
        //find the room in PhotonNetwork
        //find the room index which the room.name(PhotonNetwork) matchs RoomListingButtons.RoomName
        int index = RoomListingButtons.FindIndex(x => x.RoomName == room.Name);
        //if room could not be found(new room)
        if (index == -1)
        {
            if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomListingObj = Instantiate(RoomListingPrefab);
                roomListingObj.transform.SetParent(transform, false);

                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
                //add the roomListing to the RoomListingButtons
                RoomListingButtons.Add(roomListing);

                index = (RoomListingButtons.Count - 1);
            }
        }
        //if room is created, set up the Room name and updated
        if (index != -1)
        {
            RoomListing roomListing = RoomListingButtons[index];
            roomListing.SetRoomNameText(room.Name, room.PlayerCount, room.MaxPlayers);
            roomListing.Updated = true;
        }
    }

    private void RemoveOldRooms()
    {
        List<RoomListing> removeRooms = new List<RoomListing>();

        foreach (RoomListing roomListing in RoomListingButtons)
        {
            if (!roomListing.Updated)
                removeRooms.Add(roomListing);
            else
                roomListing.Updated = false;    //become no longer exist
        }

        foreach (RoomListing roomListing in removeRooms)
        {
            RoomListingButtons.Remove(roomListing);
            GameObject roomListingObj = roomListing.gameObject;
            Destroy(roomListingObj);
        }
    }
}
