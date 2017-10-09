/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour {

    public static MenuScreen Instance;
    
    void Awake(){
        Instance = this; 
		scrollRectSnap = Home.GetComponent<ScrollRectSnap>();
    }

    public enum Layout{home, inventory, deck, shop};
    public Layout currentLayout;

	public RectTransform Home, Inventory, Deck, Shop;
	public GameObject BackButton;
	public Text PlayerNameText, PlayerBalanceText;
	ScrollRectSnap scrollRectSnap;

	public void SetPlayerStatus(){
		PlayerNameText.text = PlayerData.Instance.playerName;
		PlayerBalanceText.text = PlayerData.Instance.playerBalance.ToString();

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
			scrollRectSnap.EnableAllAnimator(true);
		}
		else{
			BackButton.SetActive(true);
		}

		if(currentLayout == Layout.home && nextLayout != Layout.home)
			scrollRectSnap.EnableAllAnimator(false);

		if(currentLayout == Layout.inventory && nextLayout != Layout.inventory)
			InventoryStats.Instance.CloseCardStats();
			
		if(currentLayout != nextLayout || defaultSetting){
			currentLayout = nextLayout;
			layout.SetAsLastSibling();
		}
	}

	public void BackButtonDisplay(bool display){
		BackButton.SetActive(display);
	}
}
