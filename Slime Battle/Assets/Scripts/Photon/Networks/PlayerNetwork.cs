using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour {

	public static PlayerNetwork Instance;
	//PlayerName can get from other scripts, but cant set from others
	public string PlayerName { get ; private set;}
	private PhotonView photonView;
	private int numOfPlayersInGame = 0;

	// Use this for initialization
	private void Awake () {
		Instance = this;
		photonView = GetComponent<PhotonView> ();
		//fix the fps = 30 in game
		Application.targetFrameRate = 30;	
		//setting the default player name(Player#12)
		PlayerName = "Player#" + Random.Range (1, 1000);
		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}

	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode){
		if (scene.name == "Main") {
			if (PhotonNetwork.isMasterClient)
				MasterLoadedGame ();
			else
				NonMasterLoadedGame ();
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
		if (numOfPlayersInGame == PhotonNetwork.playerList.Length) {
			Debug.Log ("All players in Game now!");
			GameManager.Instance.GameStart ();
		}
	}
}
