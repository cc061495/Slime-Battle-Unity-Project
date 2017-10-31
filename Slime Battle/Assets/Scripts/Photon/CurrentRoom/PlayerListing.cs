using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerListing : Photon.MonoBehaviour {

	public PhotonPlayer PhotonPlayer{ get; private set;}

	[SerializeField]
	private Text _playerName;
	private Text PlayerName{
		get{ return _playerName; }
	}

	[SerializeField]
	private Text _playerPing;
	private Text m_playerPing{
		get{ return _playerPing;}
	}

	[SerializeField]
	private Image _roomHost;
	private Image RoomHost{
		get{ return _roomHost;}
	}

	public void ApplyPhotonPlayer(PhotonPlayer photonPlayer){
		PhotonPlayer = photonPlayer;
		PlayerName.text = photonPlayer.NickName;
		HostSetting();
		StartCoroutine(C_ShowPing());
	}

	public void HostSetting(){
		if(PhotonPlayer.IsMasterClient)
			RoomHost.gameObject.SetActive(true);
		else
			RoomHost.gameObject.SetActive(false);
	}

	private IEnumerator C_ShowPing(){
		while(PhotonNetwork.connected){
			int ping = (int) PhotonPlayer.CustomProperties["Ping"];

			if(ping < 100)
				m_playerPing.color = Color.green;
			else if(ping > 100 && ping < 200)
				m_playerPing.color = Color.yellow;
			else if(ping > 200)
				m_playerPing.color = Color.red;

			m_playerPing.text = "Ping: " + ping.ToString();
			yield return new WaitForSeconds(1f);
		}
		yield break;	
	}
	/*
	public void SetPlayerHost(){
		PlayerName.text = PlayerName.text + " (Host)";
	}
	*/
}
