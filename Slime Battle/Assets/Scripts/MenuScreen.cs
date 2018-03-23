/* Copyright (c) cc061495 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour {

    public static MenuScreen Instance;
    
    void Awake(){
        Instance = this; 
		scrollRectSnap_HomeScreen = Home.GetComponent<ScrollRectSnap>();
		shop = Shop.GetComponent<Shop>();
		//fix the fps = 60 in the menu screen
		Application.targetFrameRate = 60;
    }

	void Update(){
		if(Input.GetKey(KeyCode.Escape))
			LayoutSetting(Layout.home, Home, false);
	}

    public enum Layout{home, inventory, deck, shop, setting};
    public Layout currentLayout;

	public RectTransform Home, Inventory, Deck, Shop, Setting;
	public GameObject BackButton;
	public Text PlayerNameText, PlayerCoinsText;
	ScrollRectSnap scrollRectSnap_HomeScreen;
	Shop shop;

	public void SetPlayerStatus(string name, int coins){
		PlayerNameText.text = name;
		PlayerCoinsText.text = coins.ToString();
		LayoutSetting(Layout.home, Home, true);
	}

	public void OpenInventoryLayout(){
		LayoutSetting(Layout.inventory, Inventory, false);
	}

	public void OpenDeckLayout(){
		LayoutSetting(Layout.deck, Deck, false);
	}

	public void OpenShopLayout(){
		LayoutSetting(Layout.shop, Shop, false);
	}

	public void OpenSettingLayout(){
		LayoutSetting(Layout.setting, Setting, false);
	}

	public void BackToHomeLayout(){
		LayoutSetting(Layout.home, Home, false);
	}

	private void LayoutSetting(Layout nextLayout, RectTransform layout, bool defaultSetting){
		if(currentLayout == Layout.home && nextLayout != Layout.home){
			scrollRectSnap_HomeScreen.EnableAllAnimator(false);
			BackButton.SetActive(true);
			layout.gameObject.SetActive(true);

			if(nextLayout == Layout.shop)
				Shop.GetComponent<Shop>().Shop_DefaultSetting();
		}
		else if(currentLayout != Layout.home && nextLayout == Layout.home){
			Home.gameObject.SetActive(true);
			BackButton.SetActive(false);
			scrollRectSnap_HomeScreen.EnableAllAnimator(true);
		}

		if(currentLayout == Layout.inventory && nextLayout != Layout.inventory){
			Inventory.gameObject.SetActive(false);
			InventoryStats.Instance.CloseCardStats();
		}

		if(currentLayout == Layout.shop && nextLayout != Layout.shop){
			Shop.gameObject.SetActive(false);
			shop.SlimeModelDisplay(false);
			shop.EnableAllAnimator(false);
		}

		if(currentLayout == Layout.deck && nextLayout != Layout.deck){
			Deck.gameObject.SetActive(false);
		}

		if(currentLayout == Layout.setting && nextLayout != Layout.setting){
			Setting.gameObject.SetActive(false);
		}
			
		if(currentLayout != nextLayout || defaultSetting){
			currentLayout = nextLayout;
			layout.SetAsLastSibling();
		}
	}

	public void BackButtonDisplay(bool display){
		BackButton.SetActive(display);
	}

	public void DisableAllPanel(){
		if(Inventory.gameObject.activeSelf){
			Inventory.gameObject.SetActive(false);
			Deck.gameObject.SetActive(false);
			Shop.gameObject.SetActive(false);
			Setting.gameObject.SetActive(false);
		}
	}
}
