using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerNetwork : MonoBehaviour {

	public static PlayerNetwork Instance;
	//PlayerName can get from other scripts, but cant set from others
	public string PlayerName { get ; private set;}
	public SceneFader sceneFader;
	private int numOfPlayersInGame = 0;
	private ExitGames.Client.Photon.Hashtable m_playerCustomProperties = new ExitGames.Client.Photon.Hashtable();
	private Coroutine m_pingCoroutine;
	PhotonView photonView;

	// Use this for initialization
	private void Awake () {
		Instance = this;
		photonView = GetComponent<PhotonView>();
		//fix the fps = 30 in game
		Application.targetFrameRate = 30;
		PhotonNetwork.sendRate = 20;	//default(20) //(60)
		PhotonNetwork.sendRateOnSerialize = 15;		//default(10) //(30)
		//setting the default player name(Player#12)
		//it will set the player name in LobbyNetwork.cs, CreateRoom.cs
		PlayerName = PlayerData.Instance.playerName;
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDestroy(){
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
		if (scene.name == "Main") {
			if (PhotonNetwork.isMasterClient){
				Debug.Log("Master loaded the scene");
				MasterLoadedGame ();
			}
			else{
				Debug.Log("Client loaded the scene");
				NonMasterLoadedGame ();
			}
		}
	}

	private void MasterLoadedGame(){
		photonView.RPC ("RPC_LoadedGameScene", PhotonTargets.MasterClient);
		photonView.RPC ("RPC_LoadGameOthers", PhotonTargets.Others);
	}

	private void NonMasterLoadedGame(){
		photonView.RPC ("RPC_LoadedGameScene", PhotonTargets.MasterClient);
	}

	[PunRPC]
	private void RPC_LoadGameOthers(){
		sceneFader.FadeToWithPhotonNetwork("Main");
		//PhotonNetwork.LoadLevel ("Main");
	}

	[PunRPC]
	private void RPC_LoadedGameScene(){
		numOfPlayersInGame++;
		if (numOfPlayersInGame == PhotonNetwork.playerList.Length) {
			Debug.Log ("All players are in the game scene!");
			photonView.RPC ("RPC_GameStart", PhotonTargets.All);
		}
	}

	[PunRPC]
	private void RPC_GameStart(){
		GameManager.Instance.GameStart();
	}

	private IEnumerator C_SetPing(){
		while(PhotonNetwork.connected){
			m_playerCustomProperties["Ping"] = PhotonNetwork.GetPing();
			PhotonNetwork.player.SetCustomProperties(m_playerCustomProperties);

			yield return new WaitForSeconds(5f);
		}
		yield break;
	}

	//When connected to the master server (photon)
	private void OnConnectedToMaster(){
		if(m_pingCoroutine != null)
			StopCoroutine(m_pingCoroutine);
		
		m_pingCoroutine = StartCoroutine(C_SetPing());
	}
}
