/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeNetwork : MonoBehaviour {

	private Transform model;
	private Vector3 realPosition = Vector3.zero;
	private Quaternion realRotation = Quaternion.identity;
	private bool gotFirstUpdate = false;
	PhotonView photonView;

	void Awake () {
		model = GetComponent<Slime>().GetModel();
		PhotonNetwork.sendRate = 20;	//default(20) //(60)
		PhotonNetwork.sendRateOnSerialize = 15;		//default(10) //(30)

		photonView = GetComponent<PhotonView>();
	}

	// Do all MOVEMENT and other physics stuff here
	void Update () {
		if(photonView.isMine){
			// Do nothing - slime.cs is moving us
		}else{
			model.position = Vector3.Lerp(model.position, realPosition, 5f * Time.deltaTime);
			model.rotation = Quaternion.Lerp(model.rotation, realRotation, 5f * Time.deltaTime);
			// model.SetPositionAndRotation(
			// 	Vector3.Lerp(model.position, realPosition, 5f * GameManager.globalDeltaTime),
			// 	Quaternion.Lerp(model.rotation, realRotation, 5f * GameManager.globalDeltaTime)
			// );
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if(stream.isWriting){
			// This is OUR player, we need to send our actual position to the network.
			stream.SendNext(model.position);
			stream.SendNext(model.rotation);
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
				model.SetPositionAndRotation(realPosition, realRotation);
				gotFirstUpdate = true;
			}
		}
	}
}
