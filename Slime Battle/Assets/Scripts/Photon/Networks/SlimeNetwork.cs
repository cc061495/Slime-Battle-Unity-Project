/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeNetwork : Photon.MonoBehaviour {

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;

	void Start () {
		
	}
	
	void Update () {
		if(photonView.isMine){
			// Do nothing - slime.cs is moving us
		}else{
			transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
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
		}
	}
}
