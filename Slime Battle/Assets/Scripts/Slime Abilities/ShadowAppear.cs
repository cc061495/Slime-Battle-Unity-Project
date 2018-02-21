using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowAppear : MonoBehaviour {

	private Transform model;
	private SlimeClass slime;
	GameManager gameManager;
	PhotonView photonView;

	void Start(){
		model = transform.root.GetComponent<Slime>().GetModel();
		slime = transform.root.GetComponent<Slime>().GetSlimeClass();
		photonView = GetComponent<PhotonView>();
		gameManager = GameManager.Instance;
	}

	public void Appear(){
		// appear and join the team list
		JoinTeamList();
		// display the mine to another player
		photonView.RPC("DisplayShadow", PhotonTargets.Others);
		slime.isInvisible = false;
	}

	[PunRPC]
	private void DisplayShadow(){
		if(!model.gameObject.activeSelf)
			model.gameObject.SetActive(true);
	}

	private void JoinTeamList(){
        if(transform.root.tag == "Team_RED")
			gameManager.team_red.Add(model);
	  	else
            gameManager.team_blue.Add(model);
		
		Debug.Log("Shadow is added to the team list");
	}
}
