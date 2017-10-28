using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Slime : MonoBehaviour{

	[Header("Slime name")]
	public string slimeName;
	[Header("Cost")]
	public int cost;
	[Space]
	[Header("Slime model")]
	[SerializeField]
	private Transform model;
	private Transform _transform;
	private SlimeClass slimeClass;
    private GameManager gm;
	PhotonView photonView;

	void Start(){
		_transform = transform;
		//Build up the slime class with Name
		slimeClass = new SlimeClass(slimeName);
		photonView = GetComponent<PhotonView>();
		gm = GameManager.Instance;
		//Join team list
		JoinTeamList();
		//PathFinding config
		SlimeMovement move = GetComponent<SlimeMovement>();
		if(move != null)
			move.SetUpNavMeshAgent(slimeClass);
		//Slime Health Display config
		SlimeHealth health = GetComponent<SlimeHealth>();
		if(health != null)
			health.SetUpSlimeHealth(slimeClass);
	}

	private void JoinTeamList(){
        if(_transform.tag == "Team_RED")
            AddTeam(gm.team_red, gm.team_red2);
	  	else
            AddTeam(gm.team_blue, gm.team_blue2);
	}

	private void AddTeam(List<Transform> team, List<Transform> team2){
		team.Add(model);	//team with building
		if(!slimeClass.isBuilding)
			team2.Add(model);	//team without building
	}

	public void RemoveFromTeamList(){
		if(_transform.tag == "Team_RED")
            RemoveTeam(gm.team_red, gm.team_red2);
		else
            RemoveTeam(gm.team_blue, gm.team_blue2);

        gm.CheckAnyEmptyTeam();
    }

	private void RemoveTeam(List<Transform> team, List<Transform> team2){
		team.Remove(model);
		if(!slimeClass.isBuilding)
			team2.Remove(model);
	}

	public Transform GetModel(){
		return model;
	}

	public SlimeClass GetSlimeClass(){
		return slimeClass;
	}
	/* Sync with two players */
	public void SyncRemoveTeamList(){
		photonView.RPC("RPC_RemoveFromTeamList", PhotonTargets.All);
	}

	[PunRPC]
	private void RPC_RemoveFromTeamList(){
		if(_transform.tag == "Team_RED")
            RemoveTeam(gm.team_red, gm.team_red2);
		else
            RemoveTeam(gm.team_blue, gm.team_blue2);
	}
}