using UnityEngine; 

public class PlayerShop:MonoBehaviour {

	public string team;
	public GameObject _slime, _giant, _ranger;
	private SlimeBlueprint slime, giant, ranger;

	SpawnManager spawnManager; 

	// Use this for initialization
	void Start () {
		spawnManager = SpawnManager.Instance;
		slime = new SlimeBlueprint("Slime", team, _slime);
		giant = new SlimeBlueprint("Giant", team, _giant);
		ranger = new SlimeBlueprint("Ranger", team, _ranger);
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
}
