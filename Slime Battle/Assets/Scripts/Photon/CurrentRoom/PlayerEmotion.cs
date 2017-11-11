/* Copyright (c) cc061495 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerEmotion : MonoBehaviour {

	public GameObject emotionPrefab;
	public Transform[] spawnPoint = new Transform[2];
	public Sprite helloSprite, readySprite, battleSprite; 

	private GameObject[] emotion = new GameObject[2];
	private PhotonView photonView;
	private bool onClick, isDestroyed;

	// Use this for initialization
	void Start () {
		photonView = GetComponent<PhotonView>();
	}
	
	public void OnClickEmotionButton(string emotionString){
		if(onClick)
			return;

		int randomNum = Random.Range(1,7);

		if(PhotonNetwork.isMasterClient)
			photonView.RPC("RPC_EmotionCreate", PhotonTargets.All, emotionString, 0, randomNum);
		else
			photonView.RPC("RPC_EmotionCreate", PhotonTargets.All, emotionString, 1, randomNum);
		
		onClick = true;
		Invoke("ResetOnClick", 0.5f);
	}

	private void ResetOnClick(){
		onClick = false;
	}

	[PunRPC]
	private void RPC_EmotionCreate(string emotionString, int index, int randomNumber){

		if(emotion[index] == null)
			emotion[index] = Instantiate(emotionPrefab, spawnPoint[index]);
		
		if(emotion[index] != null){
			Image emotionImage = emotion[index].GetComponent<Image>();
			Animator emotionAnimator = emotion[index].GetComponent<Animator>();
			//emotion[index].transform.position = emotionSpawnPoint.position;

			if(emotionAnimator.GetInteger("Emotion") > 0)
				emotionAnimator.Rebind();

			emotionAnimator.SetInteger("Emotion", randomNumber);

			if(emotionString == "Hello")
				emotionImage.sprite = helloSprite;
			else if(emotionString == "Ready")
				emotionImage.sprite = readySprite;
			else if(emotionString == "Battle")
				emotionImage.sprite = battleSprite;
		}
	}
}