using UnityEngine; 

public class PlayerShop:MonoBehaviour {

	public SlimeBlueprint slime;
	public SlimeBlueprint giant;
	public SlimeBlueprint ranger;

	SpawnManager spawnManager; 

	// Use this for initialization
	void Start () {
		spawnManager = SpawnManager.Instance; 
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
