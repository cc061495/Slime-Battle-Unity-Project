/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeNetwork : MonoBehaviour {

	private Vector3 realPosition = Vector3.zero;
	private Quaternion realRotation = Quaternion.identity;
	private bool gotFirstUpdate = false;
	PhotonView photonView;
	//SlimeHealth slimeHealth;
	//private float health;

	void Awake () {
		PhotonNetwork.sendRate = 20;
		PhotonNetwork.sendRateOnSerialize = 10;	

		photonView = GetComponent<PhotonView>();
		//slimeHealth = GetComponent<SlimeHealth>();
	}
	
	void Update () {
		if(photonView.isMine){
			// Do nothing - slime.cs is moving us
			//slimeHealth.currentHealth = health;
		}else{
			transform.position = Vector3.Lerp(transform.position, realPosition, 0.25f);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, realRotation, 500 * Time.deltaTime);
			//health = slimeHealth.currentHealth;
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if(stream.isWriting){
			// This is OUR player, we need to send our actual position to the network.
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			//stream.SendNext(health);
		}
		else{
			// This is someone else's player, we need to receive their position
			// as of a few millisecond, and update our version of that player.
			realPosition = (Vector3) stream.ReceiveNext();
			realRotation = (Quaternion) stream.ReceiveNext();
			//health = (float)stream.ReceiveNext();

			// Right now, "realPosition" holds the other person's position at the LAST frame.
			// Instead of simply updating "realPosition" and continuning to lerp,
			// We MAY want to set our transform.position to immediately to this old "realPosition"
			// and then update realPosition
			if(!gotFirstUpdate){
				transform.position = realPosition;
				transform.rotation = realRotation;
				gotFirstUpdate = true;
			}
		}
	}
}
