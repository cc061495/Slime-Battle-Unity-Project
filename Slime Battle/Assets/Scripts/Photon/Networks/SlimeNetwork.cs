/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeNetwork : MonoBehaviour {

	private Vector3 realPosition = Vector3.zero;
	private Quaternion realRotation = Quaternion.identity;
	private bool gotFirstUpdate = false;
	PhotonView photonView;

	void Awake () {
		PhotonNetwork.sendRate = 60;	//default(20)
		PhotonNetwork.sendRateOnSerialize = 20;		//default(10)

		photonView = GetComponent<PhotonView>();
	}
	// FixedUpdate is called once per physics loop
	// Do all MOVEMENT and other physics stuff here
	void FixedUpdate () {
		if(photonView.isMine){
			// Do nothing - slime.cs is moving us
		}else{
			transform.position = Vector3.Lerp(transform.position, realPosition, 5f * Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 5f * Time.deltaTime);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if(stream.isWriting){
			// This is OUR player, we need to send our actual position to the network.
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else{
			// This is someone else's player, we need to receive their position
			// as of a few millisecond, and update our version of that player.
			realPosition = (Vector3) stream.ReceiveNext();
			realRotation = (Quaternion) stream.ReceiveNext();

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
