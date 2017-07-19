using UnityEngine; 

public class PlayerShop:MonoBehaviour {

	public string team;
	public GameObject _slime, _giant, _ranger, _healer, _bomber;
	private SlimeBlueprint slime, giant, ranger, healer, bomber;

	SpawnManager spawnManager; 

	// Use this for initialization
	void Start () {
		spawnManager = SpawnManager.Instance;
		slime = new SlimeBlueprint("Slime", team, _slime);
		giant = new SlimeBlueprint("Giant", team, _giant);
		ranger = new SlimeBlueprint("Ranger", team, _ranger);
		healer = new SlimeBlueprint("Healer", team, _healer);
		bomber = new SlimeBlueprint("Bomber", team, _bomber);
	}
	
	public void SelectSlime() {
		spawnManager.SelectSlimeToSpawn (slime);
	}

	public void SelectGiant() {
		spawnManager.SelectSlimeToSpawn (giant); 
	}

	public void SelectRanger() {
		spawnManager.SelectSlimeToSpawn (ranger); 
	}

	public void SelectHealer() {
		spawnManager.SelectSlimeToSpawn (healer); 
	}

	public void SelectBomber() {
		spawnManager.SelectSlimeToSpawn (bomber); 
	}
}
