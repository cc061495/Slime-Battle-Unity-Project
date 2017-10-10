using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private Vector2 positionCorrection = new Vector2(0, 25);
	public Image healthBarImage, barHandlerImage;
    public Sprite team_red, team_blue, damageBar, healBar;
    public RectTransform targetCanvas, healthBar;
    public Transform objectToFollow;
    bool healthBarChanged;
    float sizeDeltaX, sizeDeltaY;
 
    public void SetHealthBarData(Transform targetTransform){
        this.targetCanvas = GameManager.Instance.canvasForHealthBar;
        objectToFollow = targetTransform;
        healthBar = GetComponent<RectTransform>();
        sizeDeltaX = targetCanvas.sizeDelta.x;
        sizeDeltaY = targetCanvas.sizeDelta.y;

        healthBarImage.sprite = (targetTransform.parent.tag == "Team_RED") ? team_red : team_blue;
    }

    public void OnHealthChanged(float healthFill){
        float currentFillAmount = healthBarImage.fillAmount;

        if(currentFillAmount == 1)
            HealthBarTakeDamage();

        if(healthFill < currentFillAmount){
            barHandlerImage.sprite = damageBar;
            healthBarImage.fillAmount = healthFill;
            StartCoroutine(SettingBarHandlerFillAmount(true));
        }
        else{
            barHandlerImage.sprite = healBar;
            barHandlerImage.fillAmount = healthFill;
            StartCoroutine(SettingBarHandlerFillAmount(false));
        }

        barHandlerImage.GetComponent<Animation>().Play();
    }

    private void HealthBarTakeDamage(){
        healthBarChanged = true;

        GetComponent<Image>().enabled = true;
        healthBarImage.enabled = true;
        barHandlerImage.enabled = true;
    }

    IEnumerator SettingBarHandlerFillAmount(bool damage){
        yield return new WaitForSeconds(0.2f);

        if(damage)
            barHandlerImage.fillAmount = healthBarImage.fillAmount;
        else
            healthBarImage.fillAmount = barHandlerImage.fillAmount;
    }

	void Update(){
		if(objectToFollow){
            if(healthBarChanged){
                if(GameManager.Instance.currentState == GameManager.State.battle_start || GameManager.Instance.currentState == GameManager.State.battle_end)
                    RepositionHealthBar();
            }
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