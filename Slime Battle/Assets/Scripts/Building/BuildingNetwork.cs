/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNetwork : MonoBehaviour {

	private Transform model;
	private Quaternion realRotation = Quaternion.identity;
	private bool gotFirstUpdate = false;
	PhotonView photonView;

	void Awake () {
		model = GetComponent<Slime>().GetModel();
		photonView = GetComponent<PhotonView>();
	}

	// Do all MOVEMENT and other physics stuff here
	void Update () {
		if(photonView.isMine){
			// Do nothing - SlimeMovement.cs is moving us
		}else{
			model.rotation = Quaternion.Lerp(model.rotation, realRotation, 5f * Time.deltaTime);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if(stream.isWriting){
			// This is OUR player, we need to send our actual position to the network.
			stream.SendNext(model.rotation);
		}
		else{
			// This is someone else's player, we need to receive their position
			// as of a few millisecond, and update our version of that player.
			//realPosition = (Vector3) stream.ReceiveNext();
			realRotation = (Quaternion) stream.ReceiveNext();

			// Right now, "realPosition" holds the other person's position at the LAST frame.
			// Instead of simply updating "realPosition" and continuning to lerp,
			// We MAY want to set our transform.position to immediately to this old "realPosition"
			// and then update realPosition
			if(!gotFirstUpdate){
				model.rotation = realRotation;
				gotFirstUpdate = true;
			}
		}
	}
}
