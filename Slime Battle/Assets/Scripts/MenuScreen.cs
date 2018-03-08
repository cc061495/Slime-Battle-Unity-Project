/* Copyright (c) cc061495 */
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

    public enum Layout{home, inventory, deck, shop};
    public Layout currentLayout;

	public RectTransform Home, Inventory, Deck, Shop;
	public GameObject BackButton;
	public Text PlayerNameText, PlayerCoinsText;
	ScrollRectSnap scrollRectSnap_HomeScreen;
	Shop shop;

	public void SetPlayerStatus(){
		PlayerNameText.text = PlayerData.Instance.playerName;
		setCoinsText(PlayerData.Instance.playerCoins);
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

	public void BackToHomeLayout(){
		LayoutSetting(Layout.home, Home, false);
	}

	private void LayoutSetting(Layout nextLayout, RectTransform layout, bool defaultSetting){
		if(nextLayout == Layout.home){
			BackButton.SetActive(false);
			scrollRectSnap_HomeScreen.EnableAllAnimator(true);
		}
		else{
			BackButton.SetActive(true);
		}

		if(nextLayout == Layout.shop)
			Shop.GetComponent<Shop>().Shop_DefaultSetting();

		if(currentLayout == Layout.home && nextLayout != Layout.home)
			scrollRectSnap_HomeScreen.EnableAllAnimator(false);

		if(currentLayout == Layout.inventory && nextLayout != Layout.inventory)
			InventoryStats.Instance.CloseCardStats();

		if(currentLayout == Layout.shop && nextLayout != Layout.shop){
			shop.SlimeModelDisplay(false);
			shop.EnableAllAnimator(false);
		}
			
		if(currentLayout != nextLayout || defaultSetting){
			currentLayout = nextLayout;
			layout.SetAsLastSibling();
		}
	}

	public void BackButtonDisplay(bool display){
		BackButton.SetActive(display);
	}

	public void setCoinsText(int coins){
		PlayerCoinsText.text = coins.ToString();
	}
}
