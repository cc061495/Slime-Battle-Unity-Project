using UnityEngine; 
using UnityEngine.UI;

public class PlayerShop:MonoBehaviour {

	public static PlayerShop Instance;
	public GameObject _slime, _giant, _ranger, _healer, _bomber, _wall;
	private SlimeBlueprint[] blueprint = new SlimeBlueprint[6];
	public RectTransform[] shopButton = new RectTransform[6];
	public Color defaultColor, yellowColor;
	private int prevNum;

	SpawnManager spawnManager; 
	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		spawnManager = SpawnManager.Instance;
		/* Create Player Shop's monster builds */
		blueprint[0] = new SlimeBlueprint("Slime", _slime);
		blueprint[1] = new SlimeBlueprint("Giant", _giant);
		blueprint[2] = new SlimeBlueprint("Ranger", _ranger);
		blueprint[3] = new SlimeBlueprint("Healer", _healer);
		blueprint[4] = new SlimeBlueprint("Bomber", _bomber);
		blueprint[5] = new SlimeBlueprint("Wall", _wall);
		/* Update Shop buttons by cheching player's money */
    	ButtonsUpdate();
	}

	public void ButtonsUpdate(){
		/* Check six monster card price */
		for(int i=0;i<blueprint.Length;i++){
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
			shopButton[index].GetChild(0).GetComponent<Text>().text = blueprint[index].name;
		}
	}
}