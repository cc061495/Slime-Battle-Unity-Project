using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour {

	private Vector2 positionCorrection = new Vector2(0, 50);
	public static NodeUI Instance;
	public Text sellAmount;
	public GameObject nodeUI, arrowUI;
	private Node target;
	private GameManager gameManager;

	RectTransform arrow;
	float sizeDeltaX, sizeDeltaY;

	void Awake(){
		Instance = this;
	}
	void Start(){
		gameManager = GameManager.Instance;
		sizeDeltaX = gameManager.canvasForHealthBar.sizeDelta.x;
        sizeDeltaY = gameManager.canvasForHealthBar.sizeDelta.y;
		arrow = arrowUI.GetComponent<RectTransform>();
	}

	public void SetTarget(Node _target){
		target = _target;
		//transform.position = target.GetSlimePostion();
		
		sellAmount.text = "$" + target.slimeblueprint.cost;
		
		PositionArrow(target.slime.transform);

		gameManager.ShopDisplay(false);
		SellingPanelDisplay(true);
	}

	public void PositionArrow(Transform objectToFollow){
        float ViewportPositionX = Camera.main.WorldToViewportPoint(objectToFollow.position).x;
        float ViewportPositionY = Camera.main.WorldToViewportPoint(objectToFollow.position).y;

        float WorldObject_ScreenPositionX = (ViewportPositionX * sizeDeltaX) - (sizeDeltaX * 0.5f);
        float WorldObject_ScreenPositionY = (ViewportPositionY * sizeDeltaY) - (sizeDeltaY * 0.5f);
        //now you can set the position of the ui element
        Vector2 WorldObject_ScreenPosition = new Vector2(WorldObject_ScreenPositionX, WorldObject_ScreenPositionY);
		arrow.anchoredPosition = WorldObject_ScreenPosition + positionCorrection;
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
		nodeUI.SetActive(display);
		arrowUI.SetActive(display);
	}
}
