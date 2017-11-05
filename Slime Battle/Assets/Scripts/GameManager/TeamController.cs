using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamController : MonoBehaviour {
	public static TeamController Instance;
    
    void Awake(){
        Instance = this;
    }

	public enum SearchMode{distance, health, priority};
    public SearchMode redTeam_searchMode, blueTeam_searchMode;
	public Text distanceText, healthText, priorityText;
	public Color selectedTextColor, fadedTextColor;
	private Text prevText;
	PhotonView photonView;

	void Start(){
		photonView = GetComponent<PhotonView>();
		SetToDefaultSearchMode();
	}

	public void SetToDefaultSearchMode(){
		if(PhotonNetwork.isMasterClient){
			redTeam_searchMode = SearchMode.distance;
			blueTeam_searchMode = SearchMode.distance;
		}
		TextColorSetting(distanceText, healthText, priorityText);
	}
	/* Action for Distance Button */
	public void FindTargetBy_Distance() {
		TextColorSetting(distanceText, healthText, priorityText);
		DefineControlWhichTeam(SearchMode.distance);
	}
	/* Action for Health Button */
	public void FindTargetBy_Health(){
		TextColorSetting(healthText, distanceText, priorityText);
		DefineControlWhichTeam(SearchMode.health);
	}
	/* Action for Priority Button */
	public void FindTargetBy_Priority(){
		TextColorSetting(priorityText, distanceText, healthText);
		DefineControlWhichTeam(SearchMode.priority);
	}

	private void TextColorSetting(Text textToSelect, Text textToFade, Text textToFade2){
		textToSelect.color = selectedTextColor;
		textToSelect.fontStyle = FontStyle.Bold;
		textToFade.color = fadedTextColor;
		textToFade.fontStyle = FontStyle.Normal;
		textToFade2.color = fadedTextColor;
		textToFade2.fontStyle = FontStyle.Normal;
	}

	private void DefineControlWhichTeam(SearchMode mode){
		if(PhotonNetwork.isMasterClient){
			redTeam_searchMode = mode;
			//CallTargetSearching(gm.team_red2);
		}
		else
			photonView.RPC("RPC_CallTargetSearching", PhotonTargets.MasterClient, mode);
	}

	[PunRPC]
	private void RPC_CallTargetSearching(SearchMode mode){
		blueTeam_searchMode = mode;
		//CallTargetSearching(gm.team_blue2);
	}

	// private void CallTargetSearching(List<Transform> team){
	// 	for(int i=0;i<team.Count;i++){
	// 		if(!team[i].parent.GetComponent<Slime>().GetSlimeClass().isHealing){
	// 			team[i].parent.GetComponent<SlimeMovement>().FindTheTargetAgain();
	// 		}
	// 	}
	// }

	public SearchMode GetTeamSearchMode(Transform slime){
		if(slime.tag == "Team_RED")
			return redTeam_searchMode;
		else
			return blueTeam_searchMode;
	}
}
