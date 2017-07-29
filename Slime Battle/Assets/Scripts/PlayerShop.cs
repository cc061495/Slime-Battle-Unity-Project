using UnityEngine; 
using UnityEngine.UI;

public class PlayerShop:MonoBehaviour {

	public GameObject _slime, _giant, _ranger, _healer, _bomber, _wall;
	private SlimeBlueprint[] blueprint = new SlimeBlueprint[6];
	public Text[] shopText = new Text[6];
	private int prevNum;

	SpawnManager spawnManager; 

	// Use this for initialization
	void Start () {
		spawnManager = SpawnManager.Instance;
		blueprint[0] = new SlimeBlueprint("Slime", _slime);
		blueprint[1] = new SlimeBlueprint("Giant", _giant);
		blueprint[2] = new SlimeBlueprint("Ranger", _ranger);
		blueprint[3] = new SlimeBlueprint("Healer", _healer);
		blueprint[4] = new SlimeBlueprint("Bomber", _bomber);
		blueprint[5] = new SlimeBlueprint("Wall", _wall);
	}
	
	public void SelectSlime() {
		shopSelect(0);
	}

	public void SelectGiant() {
		shopSelect(1);
	}

	public void SelectRanger() {
		shopSelect(2);
	}

	public void SelectHealer() {
		shopSelect(3);
	}

	public void SelectBomber() { 
		shopSelect(4);
	}

	public void SelectWall() {
		shopSelect(5);
	}

	private void shopSelect(int selectedNum){
		spawnManager.SelectSlimeToSpawn (blueprint[selectedNum]);
		textSetting(blueprint[selectedNum], selectedNum);
	}

	private void textSetting(SlimeBlueprint slime, int num){
		if(prevNum != num && shopText[prevNum].text != blueprint[prevNum].name){
			Debug.Log("Change name");
			shopText[prevNum].text = blueprint[prevNum].name;
		}

		prevNum = num;
		shopText[num].text = "$" + slime.cost;
	}
}