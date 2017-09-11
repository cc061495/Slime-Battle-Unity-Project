using UnityEngine; 
using UnityEngine.UI;

public class PlayerShop:MonoBehaviour {

	public static PlayerShop Instance;
	public GameObject _slime, _giant, _ranger, _healer, _bomber, _wall;
	private SlimeBlueprint[] blueprint = new SlimeBlueprint[6];
	public Button[] shopButton = new Button[6];
	public Text[] shopText = new Text[6];
	private int prevNum;

	SpawnManager spawnManager; 
	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		spawnManager = SpawnManager.Instance;
		blueprint[0] = new SlimeBlueprint("Slime", _slime);
		blueprint[1] = new SlimeBlueprint("Giant", _giant);
		blueprint[2] = new SlimeBlueprint("Ranger", _ranger);
		blueprint[3] = new SlimeBlueprint("Healer", _healer);
		blueprint[4] = new SlimeBlueprint("Bomber", _bomber);
		blueprint[5] = new SlimeBlueprint("Wall", _wall);

    	ButtonsUpdate();
	}
	/*
	public void SelectSlime() {
		ShopSelect(0);
	}

	public void SelectGiant() {
		ShopSelect(1);
	}

	public void SelectRanger() {
		ShopSelect(2);
	}

	public void SelectHealer() {
		ShopSelect(3);
	}

	public void SelectBomber() { 
		ShopSelect(4);
	}

	public void SelectWall() {
		ShopSelect(5);
	}
	*/
	public void ButtonsUpdate(){
		for(int i = 0;i < 6;i++){
			if(PlayerStats.playerCost >= blueprint[i].cost){
				shopButton[i].interactable = true;
			}
			else{
				shopButton[i].interactable = false;
				shopText[i].text = blueprint[i].name;
			}
		}
	}

	public void ShopSelect(int selectedNum){
		spawnManager.SelectSlimeToSpawn (blueprint[selectedNum]);
		TextSetting(blueprint[selectedNum], selectedNum);
	}

	private void TextSetting(SlimeBlueprint slime, int num){
		if(prevNum != num && shopText[prevNum].text != blueprint[prevNum].name){
			shopText[prevNum].text = blueprint[prevNum].name;
		}

		prevNum = num;
		shopText[num].text = "$" + slime.cost;
	}

	public void ResetShopText(){
		if(shopText[prevNum].text != blueprint[prevNum].name){
			shopText[prevNum].text = blueprint[prevNum].name;
		}
	}
}