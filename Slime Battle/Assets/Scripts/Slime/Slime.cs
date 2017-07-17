using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour{

	[Header("Name")]
	public string slimeName;
	[Space]
	[Header("Slime model")]
	[SerializeField]
	private Transform model;

	private SlimeClass slimeClass;
    private GameManager gm;

	void Start(){
		gm = GameManager.Instance;
		slimeClass = new SlimeClass(slimeName);
		//PathFinding config
		GetComponent<SlimeMovement>().SetUpNavMeshAgent();
		//Slime Health Display config
		GetComponent<SlimeHealth>().SetUpSlimeHealth();
		//Join team list
		JoinTeamList();
	}

	void JoinTeamList(){
        if(transform.tag == "Team_RED"){
            gm.team_red.Add(model);
			if(slimeName != "Healer")
				gm.team_red_attacker.Add(model);
		}
		else{
            gm.team_blue.Add(model);
			if(slimeName != "Healer")
				gm.team_blue_attacker.Add(model);
		}
	}

	public void RemoveFromTeamList(){
		if(transform.tag == "Team_RED"){
            gm.team_red.Remove(model);
			if(slimeName != "Healer")
				gm.team_red_attacker.Remove(model);
		}
		else{
            gm.team_blue.Remove(model);
			if(slimeName != "Healer")
				gm.team_blue_attacker.Remove(model);
		}

        gm.CheckAnyEmptyTeam();
    }

	public Transform GetModel(){
		return model;
	}

	public SlimeClass GetSlimeClass(){
		return slimeClass;
	}
}