using UnityEngine; 
using UnityEngine.UI;

public class PlayerShop:MonoBehaviour {

	public static PlayerShop Instance;
	private SlimeBlueprint[] blueprint = new SlimeBlueprint[6];
	public RectTransform[] shopButton = new RectTransform[6];
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
				shopButton[i].GetChild(0).GetComponent<Text>().text = blueprint[i].name;
			}
			else{
				blueprint[i] = null;
				shopButton[i].GetComponent<Button>().interactable = false;
			}
		}
		/* Update Shop buttons by cheching player's money */
    	ButtonsUpdate();
	}

	public void ButtonsUpdate(){
		/* Check six monster card price */
		for(int i=0;i<blueprint.Length && blueprint[i] != null;i++){
			if(PlayerStats.playerCost >= blueprint[i].cost){
				shopButton[i].GetComponent<Button>().interactable = true;
			}
			else{
				shopButton[i].GetComponent<Button>().interactable = false;
				ShopButtonReset(i);
			}
		}
	}

	public void ShopSelect(int selectedNum){
		spawnManager.SelectSlimeToSpawn (blueprint[selectedNum]);
		TextSetting(blueprint[selectedNum], selectedNum);
	}

	private void TextSetting(SlimeBlueprint slime, int num){
		if(prevNum != num){
			ShopButtonReset(prevNum);
		}

		prevNum = num;
		shopButton[num].GetComponent<Image>().color = yellowColor;
		shopButton[num].GetChild(0).GetComponent<Text>().text = "$" + slime.cost;
	}

	public void ResetShopText(){
		ShopButtonReset(prevNum);
	}

	private void ShopButtonReset(int index){
		if(shopButton[index].GetComponent<Image>().color != defaultColor){
			shopButton[index].GetComponent<Image>().color = defaultColor;
			if(blueprint[index] != null)
				shopButton[index].GetChild(0).GetComponent<Text>().text = blueprint[index].name;
		}
	}

	private GameObject GetShopPrefab(int num){
		if(PhotonNetwork.isMasterClient)
			return pDeck.deck[num].teamRedPrefab;
		else
			return pDeck.deck[num].teamBluePrefab;
	}
}