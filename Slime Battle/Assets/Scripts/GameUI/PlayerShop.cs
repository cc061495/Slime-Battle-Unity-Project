/* Copyright (c) cc061495 */
using UnityEngine; 
using UnityEngine.UI;

public class PlayerShop:MonoBehaviour {

	public static PlayerShop Instance;
	private SlimeBlueprint[] blueprint = new SlimeBlueprint[6];
	public ShopButton[] shopButtons = new ShopButton[6];
	public Color defaultColor, yellowColor;
	private int prevNum;

	PlayerCardDeck pDeck;
	SpawnManager spawnManager; 

	void Awake(){
		Instance = this;
		pDeck = PlayerCardDeck.Instance;
		spawnManager = SpawnManager.Instance;
	}

	// Use this for initialization
	void Start () {
		/* Create Player Shop's monster builds */
		for (int i = 0; i < blueprint.Length; i++){
			if(pDeck.deck[i] != null){
				blueprint[i] = new SlimeBlueprint(pDeck.deck[i], GetShopPrefab(i));
				int cost = blueprint[i].cost;
				Sprite sprite = PhotonNetwork.isMasterClient ? blueprint[i].icon_red : blueprint[i].icon_blue;
				int size = blueprint[i].size;
				
				shopButtons[i].SetupShopButton(cost, sprite, size);
			}
			else{
				blueprint[i] = null;
				shopButtons[i].SetShopButtonInteractable(false);
			}
		}
		/* Update Shop buttons by cheching player's money */
    	ButtonsUpdate();
	}

	public void ButtonsUpdate(){
		/* Check six monster card price */
		for(int i=0;i<blueprint.Length;i++){
			if(blueprint[i] != null){
				if(PlayerStats.playerCost >= blueprint[i].cost){
					shopButtons[i].SetShopButtonInteractable(true);
					shopButtons[i].ChangeModelImageAlpha(1f);
				}
				else{
					shopButtons[i].SetShopButtonInteractable(false);
					shopButtons[i].ChangeModelImageAlpha(0.5f);
					ShopButtonReset(i);
				}
				shopButtons[i].SetModelImageColor();
			}
		}
	}

	public void ShopSelect(int selectedNum){
		spawnManager.SelectSlimeToSpawn (blueprint[selectedNum]);
		TextSetting(blueprint[selectedNum], selectedNum);

		AudioManager.instance.Play("Tap");
	}

	private void TextSetting(SlimeBlueprint slime, int num){
		if(prevNum != num){
			ShopButtonReset(prevNum);
		}

		prevNum = num;
		shopButtons[num].SetShopButtonColor(yellowColor);
		shopButtons[num].ShopButtonPressed();
	}

	public void ResetShopText(){
		ShopButtonReset(prevNum);
	}

	private void ShopButtonReset(int index){
		if(shopButtons[index].GetShopButtonColor() != defaultColor){
			shopButtons[index].SetShopButtonColor(defaultColor);
			if(blueprint[index] != null){
				shopButtons[index].ResetShopButton();
			}
		}
	}

	private GameObject GetShopPrefab(int num){
		if(PhotonNetwork.isMasterClient)
			return pDeck.deck[num].teamRedPrefab;
		else
			return pDeck.deck[num].teamBluePrefab;
	}
}