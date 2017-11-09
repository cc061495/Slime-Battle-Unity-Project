using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour {

	public static BuildingUI Instance;
	public GameObject buildingPanel;
	public Arrow arrow;

	SlimeHealth buildHealth;
	TeamController teamController;
	ChattingPanel chattingPanel;

	void Awake(){
		Instance = this;
	}

	void Start(){
		teamController = TeamController.Instance;
		chattingPanel = ChattingPanel.Instance;
	}

	public void Show(Transform s, SlimeHealth h){
		buildHealth = h;
		arrow.PositionArrow(s);
		
		teamController.SetControlPanelDisplay(false);
		teamController.SetControlButtonDisplay(false);
		chattingPanel.SetChatButtonDisplay(false);
		chattingPanel.SetChatPanelDisplay(false);
		BuildingPanelDisplay(true);
	}

	public void Destroy(){
		if(buildHealth != null)
			buildHealth.SuddenDeath();
			
		Cancel();
	}

	public void Cancel(){

		teamController.SetControlButtonDisplay(true);
		chattingPanel.SetChatButtonDisplay(true);
		BuildingPanelDisplay(false);
	}

	public void HideTheBuildingPanel(SlimeHealth h){
		if(buildHealth == h)
			Cancel();
	}

	public void BuildingPanelDisplay(bool display){
		buildingPanel.SetActive(display);
		arrow.ArrowDisplay(display);
	}
}
