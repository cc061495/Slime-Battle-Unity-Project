using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamController : MonoBehaviour {
	public static TeamController Instance;
    
    void Awake(){
        Instance = this;
    }

	public enum SearchMode{distance, health, priority, defense};
    public SearchMode redTeam_searchMode, blueTeam_searchMode;
	public Sprite[] atkModeImg = new Sprite[4];
	public Text modeText;
	public GameObject controlPanel, controlButton;
	private string prevMode;
	PhotonView photonView;
	GameManager gameManager;

	void Start(){
		photonView = GetComponent<PhotonView>();
		gameManager = GameManager.Instance;
		SetToDefaultSearchMode();
	}

	public void SetToDefaultSearchMode(){
		if(PhotonNetwork.isMasterClient){
			redTeam_searchMode = SearchMode.distance;
			blueTeam_searchMode = SearchMode.distance;
		}
		ModeTextSetting("Distance", 0);
	}
	/* Action for Distance Button */
	public void OnClick_FindTargetBy_Distance() {
		ModeTextSetting("Distance", 0);
		StartCoroutine(DefineControlWhichTeam(SearchMode.distance));
	}
	/* Action for Health Button */
	public void OnClick_FindTargetBy_Health(){
		ModeTextSetting("Health", 1);
		StartCoroutine(DefineControlWhichTeam(SearchMode.health));
	}
	/* Action for Priority Button */
	public void OnClick_FindTargetBy_Priority(){
		ModeTextSetting("Priority", 2);
		StartCoroutine(DefineControlWhichTeam(SearchMode.priority));
	}
	/* Action for Priority Button */
	public void OnClick_FindTargetBy_Defense(){
		ModeTextSetting("Defense", 3);
		StartCoroutine(DefineControlWhichTeam(SearchMode.defense));
	}

	private void ModeTextSetting(string mode, int index){
		if(prevMode != mode){
			modeText.text = "Attack Mode:\n" + "<color=#FFFFFFFF>" + mode + "</color>";
			controlButton.GetComponent<Image>().sprite = atkModeImg[index];
			prevMode = mode;
		}
	}

	IEnumerator DefineControlWhichTeam(SearchMode mode){
		controlPanel.GetComponent<Animator>().Play("Back", 0, 0f);
		yield return new WaitForSeconds(0.2f);
		if(gameManager.currentState == GameManager.State.battle_start){
			controlPanel.SetActive(false);
			controlButton.SetActive(true);
			gameManager.chatButton.SetActive(true);
		}

		if(PhotonNetwork.isMasterClient){
			redTeam_searchMode = mode;
			CallTargetSearching(gameManager.team_red2);
		}
		else
			photonView.RPC("RPC_CallTargetSearching", PhotonTargets.MasterClient, mode);
	}

	[PunRPC]
	private void RPC_CallTargetSearching(SearchMode mode){
		blueTeam_searchMode = mode;
		CallTargetSearching(gameManager.team_blue2);
	}

	private void CallTargetSearching(List<Transform> team){
		for(int i=0;i<team.Count;i++){
			if(!team[i].parent.GetComponent<Slime>().GetSlimeClass().isHealing){
				team[i].parent.GetComponent<SlimeMovement>().FindTheTargetAgain();
			}
		}
	}

	public void OnClick_ControlButton(){
		//controlPanel.GetComponent<Animator>().Rebind();
		controlPanel.SetActive(true);
		controlButton.SetActive(false);
		gameManager.chatButton.SetActive(false);
	}

	public SearchMode GetTeamSearchMode(Transform slime){
		if(slime.tag == "Team_RED")
			return redTeam_searchMode;
		else
			return blueTeam_searchMode;
	}
}