using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour {

	public static PlayerNetwork Instance;
	//PlayerName can get from other scripts, but cant set from others
	public string PlayerName { get ; private set;}
	private int numOfPlayersInGame = 0;
	PhotonView photonView;

	// Use this for initialization
	private void Awake () {
		Instance = this;
		photonView = GetComponent<PhotonView>();

		Application.targetFrameRate = 30;
		//setting the default player name(Player#12)
		PlayerName = "Player#" + Random.Range (100, 1000);
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
		PhotonNetwork.LoadLevel ("Main");
	}

	[PunRPC]
	private void RPC_LoadedGameScene(){
		numOfPlayersInGame++;
		Debug.Log(numOfPlayersInGame);
		if (numOfPlayersInGame == PhotonNetwork.room.PlayerCount) {
			Debug.Log ("All players in Game now!");
			//numOfPlayersInGame = 2;		//reset to 0, when all the players in game.
			photonView.RPC ("RPC_GameStart", PhotonTargets.All);
		}
	}

	[PunRPC]
	private void RPC_GameStart(){
		GameManager.Instance.GameStart();
	}
}
