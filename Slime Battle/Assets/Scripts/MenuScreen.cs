/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour {

    public static MenuScreen Instance;
    
    void Awake(){
        Instance = this; 
    }

    public enum Layout{home, inventory, deck, shop};
    public Layout currentLayout;

	public RectTransform Home, Inventory, Deck, Shop;
	public GameObject BackButton;
	public Text PlayerNameText, PlayerBalanceText;

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

	private void LayoutSetting(Layout l, RectTransform r, bool defaultSetting){
		if(l == Layout.home)
			BackButton.SetActive(false);
		else
			BackButton.SetActive(true);
			
		if(currentLayout != l || defaultSetting){
			currentLayout = l;
			r.SetAsLastSibling();
		}
	}
}
