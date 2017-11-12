/* Copyright (c) cc061495 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerEmotion : MonoBehaviour {

	public Sprite[] emotionSprite = new Sprite[3];
	public GameObject[] emotion = new GameObject[2];
	private PhotonView photonView;
	private bool onClick;

	// Use this for initialization
	void Start () {
		photonView = GetComponent<PhotonView>();
	}
	
	public void OnClickEmotionButton(int emotionIndex){
		if(onClick)
			return;

		int randomNum = Random.Range(1,7);

		if(PhotonNetwork.isMasterClient)
			photonView.RPC("RPC_EmotionCreate", PhotonTargets.All, emotionIndex, 0, randomNum);
		else
			photonView.RPC("RPC_EmotionCreate", PhotonTargets.All, emotionIndex, 1, randomNum);
		
		onClick = true;
		Invoke("ResetOnClick", 0.5f);
	}

	private void ResetOnClick(){
		onClick = false;
	}

	[PunRPC]
	private void RPC_EmotionCreate(int emotionIndex, int index, int randomNumber){

		Image emotionImage = emotion[index].GetComponent<Image>();
		emotionImage.sprite = emotionSprite[emotionIndex];
		if(!emotionImage.enabled)
			emotionImage.enabled = true;

		Animator emotionAnimator = emotion[index].GetComponent<Animator>();
		if(emotionAnimator.GetInteger("Emotion") > 0)
			emotionAnimator.Rebind();
			
		emotionAnimator.SetInteger("Emotion", randomNumber);
	}
}