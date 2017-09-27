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
		
		blueprint[0] = new SlimeBlueprint(PlayerPrefs.GetString("Slot1"), GetShopGameObject(PlayerPrefs.GetString("Slot1")));
		blueprint[1] = new SlimeBlueprint(PlayerPrefs.GetString("Slot2"), GetShopGameObject(PlayerPrefs.GetString("Slot2")));
		blueprint[2] = new SlimeBlueprint(PlayerPrefs.GetString("Slot3"), GetShopGameObject(PlayerPrefs.GetString("Slot3")));
		blueprint[3] = new SlimeBlueprint(PlayerPrefs.GetString("Slot4"), GetShopGameObject(PlayerPrefs.GetString("Slot4")));
		blueprint[4] = new SlimeBlueprint(PlayerPrefs.GetString("Slot5"), GetShopGameObject(PlayerPrefs.GetString("Slot5")));
		blueprint[5] = new SlimeBlueprint(PlayerPrefs.GetString("Slot6"), GetShopGameObject(PlayerPrefs.GetString("Slot6")));

		for (int i = 0; i < 6; i++)
			shopButton[i].GetChild(0).GetComponent<Text>().text = blueprint[i].name;
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

	private GameObject GetShopGameObject(string s){
		if(s == "Slime")
			return _slime;
		else if(s == "Giant")
			return _giant;
		else if(s == "Ranger")
			return _ranger;
		else if(s == "Healer")
			return _healer;
		else if(s == "Bomber")
			return _bomber;
		else if(s == "Wall")
			return _wall;

		return null;
	}
}