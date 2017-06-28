using UnityEngine; 

public class PlayerShop:MonoBehaviour {

	public SlimeBlueprint slimeA; 
	public SlimeBlueprint slimeB; 
	public SlimeBlueprint bigSlimeA; 
	public SlimeBlueprint rangerA; 

	SpawnManager spawnManager; 

	// Use this for initialization
	void Start () {
		spawnManager = SpawnManager.Instance; 
	}
	
	public void SelectSlimeA() {
		spawnManager.SelectSlimeToSpawn (slimeA); 
	}

	public void SelectSlimeB() {
		spawnManager.SelectSlimeToSpawn (slimeB); 
	}

	public void SelectBigSlimeA() {
		spawnManager.SelectSlimeToSpawn (bigSlimeA); 
	}

	public void SelectRangerA() {
		spawnManager.SelectSlimeToSpawn (rangerA); 
	}
}
