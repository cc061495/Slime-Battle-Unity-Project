/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour {

	public GameObject Inventory, Team, Shop;
	public Text PlayerNameText, PlayerBalanceText;

	public void SetPlayerStatus(){
		PlayerNameText.text = PlayerData.Instance.playerName;
		PlayerBalanceText.text = PlayerData.Instance.playerBalance.ToString();
	}

	public void InventoryButtonPressed(){
		
	}

	public void TeamButtonPressed(){

	}

	public void ShopButtonPressed(){

	}

	public void GoToGameLobby(){
		Debug.Log("Game Lobby!");
		SceneManager.LoadScene("GameLobby");
	}

	public void QuitGame(){
		Application.Quit();
	}
}
