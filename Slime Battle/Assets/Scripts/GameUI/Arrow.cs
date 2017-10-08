using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour {

	private Vector2 positionCorrection = new Vector2(0, 50);

	private Animator arrowRotation;
	private RectTransform arrow;
	private Image arrowImage;
	float sizeDeltaX, sizeDeltaY;
	GameManager gameManager;

	void Start(){
		arrow = GetComponent<RectTransform>();
		arrowImage = GetComponent<Image>();
		arrowRotation = GetComponent<Animator>();
		gameManager = GameManager.Instance;
		sizeDeltaX = gameManager.canvasForHealthBar.sizeDelta.x;
        sizeDeltaY = gameManager.canvasForHealthBar.sizeDelta.y;
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

	public void ArrowDisplay(bool display){
		arrowImage.enabled = display;
		arrowRotation.enabled = display;
	}
}
