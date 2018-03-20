using UnityEngine;
using UnityEngine.UI;

public class PlayerSetting : MonoBehaviour {

	public static PlayerSetting Instance;

	//PlayerName can get from other scripts, but cant set from others
	public string playerName { get ; private set;}
	public Text playerCoinsText;

	// Use this for initialization
	void Awake(){
		Instance = this;
		playerCoinsText.text = PlayerData.Instance.playerCoins.ToString();

		//setting the default player name(Player#12)
		//it will set the player name in LobbyNetwork.cs, CreateRoom.cs, RoomSetting.cs
		playerName = PlayerData.Instance.playerName;
	}
}
