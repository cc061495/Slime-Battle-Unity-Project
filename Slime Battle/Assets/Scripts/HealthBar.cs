using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private Vector2 positionCorrection = new Vector2(0, 25);
	public Image healthBarImage;
    public RectTransform targetCanvas, healthBar;
    public Transform objectToFollow;
    float sizeDeltaX, sizeDeltaY;
 
    public void SetHealthBarData(Transform targetTransform){
        this.targetCanvas = GameManager.Instance.canvasForHealthBar;
        objectToFollow = targetTransform;
        healthBar = GetComponent<RectTransform>();
        sizeDeltaX = targetCanvas.sizeDelta.x;
        sizeDeltaY = targetCanvas.sizeDelta.y;
    }

    public void OnHealthChanged(float healthFill){
        healthBarImage.fillAmount = healthFill;
    }

	void Update(){
		if(objectToFollow){
            if(GameManager.Instance.currentState == GameManager.State.battle_start || GameManager.Instance.currentState == GameManager.State.battle_end)
				RepositionHealthBar();
		}
		else
    	    Destroy(transform.gameObject);
	}

    public void RepositionHealthBar(){
        float ViewportPositionX = Camera.main.WorldToViewportPoint(objectToFollow.position).x;
        float ViewportPositionY = Camera.main.WorldToViewportPoint(objectToFollow.position).y;

        float WorldObject_ScreenPositionX = (ViewportPositionX * sizeDeltaX) - (sizeDeltaX * 0.5f);
        float WorldObject_ScreenPositionY = (ViewportPositionY * sizeDeltaY) - (sizeDeltaY * 0.5f);
        //now you can set the position of the ui element
        Vector2 WorldObject_ScreenPosition = new Vector2(WorldObject_ScreenPositionX, WorldObject_ScreenPositionY);
		healthBar.anchoredPosition = WorldObject_ScreenPosition + positionCorrection;
    }
}