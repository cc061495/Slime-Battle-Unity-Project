using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour{

	[Header("Slime name")]
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
		SlimeMovement move = GetComponent<SlimeMovement>();
		if(move != null)
			GetComponent<SlimeMovement>().SetUpNavMeshAgent(model, slimeClass);
		//Slime Health Display config
		SlimeHealth health = GetComponent<SlimeHealth>();
		if(health != null)
			GetComponent<SlimeHealth>().SetUpSlimeHealth(model, slimeClass);
		//Join team list
		JoinTeamList();
	}

	private void JoinTeamList(){
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

	public Transform GetModel(){
		return model;
	}

	public SlimeClass GetSlimeClass(){
		return slimeClass;
	}
}