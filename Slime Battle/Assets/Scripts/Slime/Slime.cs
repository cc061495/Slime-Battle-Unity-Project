using System.Collections.Generic;
using UnityEngine;

public class Slime : Photon.MonoBehaviour{

	[Header("Name")]
	public string slimeName;
	[Space]
	[Header("Slime model")]
	[SerializeField]
	private Transform model;

	private SlimeClass slimeClass;
    private GameManager gm;
	List<Transform> enemies;

	void Start(){
		gm = GameManager.Instance;
		slimeClass = new SlimeClass(slimeName);
		//PathFinding config
		GetComponent<SlimeMovement>().SetUpNavMeshAgent();
		//Slime Health Display config
		GetComponent<SlimeHealth>().SetUpSlimeHealth();
		//Join team list
		JoinTeamList();
		//Define enemies
		enemies = (transform.tag == "Team_RED") ? (gm.team_blue) : (gm.team_red);
	}

	void JoinTeamList(){
        if(transform.tag == "Team_RED")
            gm.team_red.Add(model);
		else
            gm.team_blue.Add(model);
	}

	public void RemoveFromTeamList(){
		if(transform.tag == "Team_RED")
            gm.team_red.Remove(model);
		else
            gm.team_blue.Remove(model);

        gm.CheckAnyEmptyTeam();
    }

	public List<Transform> GetEmenies(){
		return enemies;
	}

	public Transform GetModel(){
		return model;
	}

	public SlimeClass GetSlimeClass(){
		return slimeClass;
	}
}