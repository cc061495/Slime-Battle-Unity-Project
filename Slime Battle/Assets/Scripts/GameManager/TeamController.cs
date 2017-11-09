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
	private string[] modeString = new string[]{"Distance", "Health", "Priority", "Defense"};
	private SearchMode[] mode = new SearchMode[]{SearchMode.distance, SearchMode.health, SearchMode.priority, SearchMode.defense};
	PhotonView photonView;
	GameManager gameManager;
	ChattingPanel chattingPanel;
	Animator controlPanelAnimator;

	void Start(){
		photonView = GetComponent<PhotonView>();
		gameManager = GameManager.Instance;
		chattingPanel = ChattingPanel.Instance;
		controlPanelAnimator = controlPanel.GetComponent<Animator>();
		SetToDefaultSearchMode();
	}

	public void SetToDefaultSearchMode(){
		if(PhotonNetwork.isMasterClient){
			redTeam_searchMode = SearchMode.distance;
			blueTeam_searchMode = SearchMode.distance;
		}
		ModeTextSetting(0);
	}
	/* Action for Attack Mode Button */
	public void OnClick_FindTargetBy_AttackMode(int index) {
		ModeTextSetting(index);
		StartCoroutine(DefineControlWhichTeam(mode[index]));
	}

	private void ModeTextSetting(int index){
		if(prevMode != modeString[index]){
			modeText.text = "Attack Mode:\n" + "<color=#FFFFFFFF>" + modeString[index] + "</color>";
			controlButton.GetComponent<Image>().sprite = atkModeImg[index];
			prevMode = modeString[index];
		}
	}

	IEnumerator DefineControlWhichTeam(SearchMode mode){
		if(PhotonNetwork.isMasterClient){
			redTeam_searchMode = mode;
			CallTargetSearching(gameManager.team_red2);
		}
		else
			photonView.RPC("RPC_CallTargetSearching", PhotonTargets.MasterClient, mode);

		controlPanelAnimator.Play("Back", 0, 0f);
		yield return new WaitForSeconds(0.2f);

		if(gameManager.currentState == GameManager.State.battle_start){
			SetControlPanelDisplay(false);
			SetControlButtonDisplay(true);
			chattingPanel.SetChatButtonDisplay(true);
		}
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
		SetControlPanelDisplay(true);
		SetControlButtonDisplay(false);
		chattingPanel.SetChatButtonDisplay(false);
	}

	public SearchMode GetTeamSearchMode(Transform slime){
		if(slime.tag == "Team_RED")
			return redTeam_searchMode;
		else
			return blueTeam_searchMode;
	}

	public void SetControlPanelDisplay(bool display){
		if(controlPanel.activeSelf != display)
			controlPanel.SetActive(display);
	}

	public void SetControlButtonDisplay(bool display){
		if(controlButton.activeSelf != display)
			controlButton.SetActive(display);
	}
}