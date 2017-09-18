using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour {

	public static BuildingUI Instance;
	public GameObject buildingPanel;

	private Arrow arrow;
	private GameManager gameManager;
	SlimeHealth buildHealth;

	void Awake(){
		Instance = this;
	}

	void Start(){
		gameManager = GameManager.Instance;
		arrow = GetComponent<Arrow>();
	}

	public void Show(Transform s, SlimeHealth h){
		buildHealth = h;
		arrow.PositionArrow(s);

		gameManager.teamControlPanel.SetActive(false);
		BuildingPanelDisplay(true);
	}

	public void Destroy(){
		if(buildHealth != null)
			buildHealth.TakeDamage(buildHealth.currentHealth);
			
		Cancel();
	}

	public void Cancel(){
		gameManager.teamControlPanel.SetActive(true);
		BuildingPanelDisplay(false);
	}

	public void HideTheBuildingPanel(SlimeHealth h){
		if(buildHealth == h)
			Cancel();
	}

	public void BuildingPanelDisplay(bool display){
		buildingPanel.SetActive(display);
		arrow.arrowUI.SetActive(display);
	}
}
