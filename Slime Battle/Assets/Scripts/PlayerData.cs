/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

	public static PlayerData Instance;

	public string playerName {get ; private set;}
	public int playerBalance {get ; private set;}

	private void Awake () {
		Instance = this;
		LoadPlayerSetting();
	}

	// Use this for initialization
	void Start () {

	}

	public void ClearPlayerData(){
		PlayerPrefs.DeleteAll();
		LoadPlayerSetting();
	}

	private void LoadPlayerSetting(){
		LoadPlayerNameSetting();
		LoadPlayerBalanceSetting();

		MenuScreen.Instance.SetPlayerStatus();
	}

	private void LoadPlayerNameSetting(){
		if(PlayerPrefs.HasKey("PlayerName"))
			playerName = PlayerPrefs.GetString("PlayerName");
		else{
			playerName = "Player#" + Random.Range (100, 1000);
			PlayerPrefs.SetString("PlayerName", playerName);
		}
	}

	private void LoadPlayerBalanceSetting(){
		if(PlayerPrefs.HasKey("PlayerBalance"))
			playerBalance = PlayerPrefs.GetInt("PlayerBalance");
		else{
			playerBalance = 100;
			PlayerPrefs.SetInt("PlayerBalance", playerBalance);
		}
	}
}
