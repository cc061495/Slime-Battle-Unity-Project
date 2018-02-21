using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour{

	[Header("Slime name")]
	public string slimeName;
	[Space]
	[Header("Slime model")]
	[SerializeField]
	private Transform model;
	private Transform _transform;
	private SlimeClass slimeClass;
	private SlimeMovement move;
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
		move = GetComponent<SlimeMovement>();
		if(move != null)
			move.SetUpNavMeshAgent(slimeClass);
		//Slime Health Display config
		SlimeHealth health = GetComponent<SlimeHealth>();
		if(health != null)
			health.SetUpSlimeHealth(slimeClass);

		if(!photonView.isMine && !slimeClass.canSpawnInBattle)
		 	DisplaySlime(false, true);
		else if(slimeClass.canSpawnInBattle)
			SlimeComponentEnable();
	}

	private void JoinTeamList(){
        if(_transform.tag == "Team_RED")
            AddTeam(gm.team_red, gm.team_red2, gm.team_invisible);
	  	else
            AddTeam(gm.team_blue, gm.team_blue2, gm.team_invisible);
	}

	private void AddTeam(List<Transform> team, List<Transform> team2, List<Transform> team3){
		if(!slimeClass.isInvisible)
			team.Add(model);	//team with building, not including invisible gameobject
		else
			team3.Add(model);

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
		if(team.Contains(model))
			team.Remove(model);
		if(team2.Contains(model))
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

	public void DisplaySlime(bool display, bool runSetActive){
		if(runSetActive)
			model.gameObject.SetActive(display);

		if(GameManager.Instance.currentState == GameManager.State.build_end){
			if(!PhotonNetwork.isMasterClient && photonView.isMine && slimeClass.isNetworkTransfer){
				Invoke("NetworkTransfer", Random.Range(0f,2f));
			}

			Invoke("SlimeComponentEnable", 4f);
		}
	}

	private void NetworkTransfer(){
		photonView.TransferOwnership(GameManager.Instance.masterPlayer);
	}

	private void SlimeComponentEnable(){
		//Debug.Log("Network is enabled");
		SlimeNetwork network = GetComponent<SlimeNetwork>();
		if(network != null)
			network.enabled = true;

		if(move != null && photonView.isMine){
			move.StartUpdatePathLoop();		//slime start finding the target
		}

		BuildingSpawnCost money = GetComponent<BuildingSpawnCost>();
		if(money != null && photonView.isMine){
			money.StartSpawnCostLoop();
		}

		SlimeSummon summon = GetComponent<SlimeSummon>();
		if(summon != null && photonView.isMine){
			summon.StartSummonLoop();
		}

		Guardian guardian = GetComponent<Guardian>();
		if(guardian != null && photonView.isMine){
			guardian.SpellingGuardianBuff(slimeClass);
		}
	}

	public void EnableObstacleCarve(){
		if(slimeClass.isBuilding)
			model.GetComponent<NavMeshObstacle>().carving = true;
	}
}