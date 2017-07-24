using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Slime : MonoBehaviour{

	[Header("Slime name")]
	public string slimeName;
	[Space]
	[Header("Slime model")]
	[SerializeField]
	private Transform model, _transform;
	private SlimeClass slimeClass;
    private GameManager gm;

	void Start(){
		_transform = transform;
		gm = GameManager.Instance;
		//Join team list
		JoinTeamList();

		slimeClass = new SlimeClass(slimeName);
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
            gm.team_red.Add(model);
	  	else
            gm.team_blue.Add(model);
	}

	public void RemoveFromTeamList(){
		if(_transform.tag == "Team_RED")
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