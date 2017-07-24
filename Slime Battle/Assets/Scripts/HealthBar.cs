using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private Vector2 positionCorrection = new Vector2(0, 25);
	public Image healthBarImage;
    public RectTransform targetCanvas;
    public Transform objectToFollow;
 
    public void SetHealthBarData(Transform targetTransform){
        this.targetCanvas = GameManager.Instance.canvasForHealthBar;
        objectToFollow = targetTransform;
    }

    public void OnHealthChanged(float healthFill){
        healthBarImage.fillAmount = healthFill;
    }

	void Update(){
		if(objectToFollow == null){
			Destroy(transform.gameObject);
			return;
		}
		else{
			if(GameManager.Instance.currentState == GameManager.State.battle_start || GameManager.Instance.currentState == GameManager.State.battle_end)
				RepositionHealthBar();
		}
	}

    public void RepositionHealthBar(){
		RectTransform healthBar = GetComponent<RectTransform>();
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(objectToFollow.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * targetCanvas.sizeDelta.x) - (targetCanvas.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * targetCanvas.sizeDelta.y) - (targetCanvas.sizeDelta.y * 0.5f)));
        //now you can set the position of the ui element
		healthBar.anchoredPosition = WorldObject_ScreenPosition + positionCorrection;
    }
}