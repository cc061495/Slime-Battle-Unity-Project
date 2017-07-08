using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : Photon.MonoBehaviour {

	public PhotonPlayer PhotonPlayer{ get; private set;}

	[SerializeField]
	private Text _playerName;
	private Text PlayerName{
		get{ return _playerName; }
	}

	public void ApplyPhotonPlayer(PhotonPlayer photonPlayer){
		PhotonPlayer = photonPlayer;
		PlayerName.text = photonPlayer.NickName;
	}
	/*
	public void SetPlayerHost(){
		PlayerName.text = PlayerName.text + " (Host)";
	}
	*/
}
