using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellingUI : MonoBehaviour {

	public static SellingUI Instance;
	public Text sellAmount;
	public GameObject sellingPanel;

	private Arrow arrow;
	private Node target;
	private GameManager gameManager;
	float sizeDeltaX, sizeDeltaY;

	void Awake(){
		Instance = this;
	}

	void Start(){
		gameManager = GameManager.Instance;
		arrow = GetComponent<Arrow>();
	}

	public void SetTarget(Node _target){
		target = _target;
		//transform.position = target.GetSlimePostion();
		
		sellAmount.text = "$" + target.slimeblueprint.cost;
		
		arrow.PositionArrow(target.slime.transform);

		gameManager.ShopDisplay(false);
		SellingPanelDisplay(true);
	}

	public void Hide(){
		gameManager.ShopDisplay(true);
		SellingPanelDisplay(false);
	}

	public void Sell(){
		target.SellSlime();
		SpawnManager.Instance.DeselectNode();
	}

	public void SellingPanelDisplay(bool display){
		sellingPanel.SetActive(display);
		arrow.arrowUI.SetActive(display);
	}
}
