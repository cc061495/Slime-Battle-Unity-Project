using UnityEngine; 

public class PlayerShop:MonoBehaviour {

	public GameObject _slime, _giant, _ranger, _healer, _bomber, _wall;
	private SlimeBlueprint slime, giant, ranger, healer, bomber, wall;

	SpawnManager spawnManager; 

	// Use this for initialization
	void Start () {
		spawnManager = SpawnManager.Instance;
		slime = new SlimeBlueprint("Slime", _slime);
		giant = new SlimeBlueprint("Giant", _giant);
		ranger = new SlimeBlueprint("Ranger", _ranger);
		healer = new SlimeBlueprint("Healer", _healer);
		bomber = new SlimeBlueprint("Bomber", _bomber);
		wall = new SlimeBlueprint("Wall", _wall);
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

	public void SelectWall() {
		spawnManager.SelectSlimeToSpawn (wall); 
	}
}