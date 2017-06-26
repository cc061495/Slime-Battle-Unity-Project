using UnityEngine;

public class PlayerShop : MonoBehaviour {

	public SlimeBlueprint slimeA;
	public SlimeBlueprint slimeB;
	public SlimeBlueprint bigSlimeA;
	public SlimeBlueprint rangerA;

	SpawnManager manager;

	// Use this for initialization
	void Start () {
		manager = SpawnManager.instance;
	}
	
	public void SelectSlimeA(){
		manager.SelectSlimeToSpawn (slimeA);
	}

	public void SelectSlimeB(){
		manager.SelectSlimeToSpawn (slimeB);
	}

	public void SelectBigSlimeA(){
		manager.SelectSlimeToSpawn (bigSlimeA);
	}

	public void SelectRangerA(){
		manager.SelectSlimeToSpawn (rangerA);
	}
}
