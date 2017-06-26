using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour {

	public static SpawnManager instance;

	void Awake(){
		instance = this;
	}

	private SlimeBlueprint slimeToSpawn;


	public bool CanSpawn{ get { return slimeToSpawn != null; } }

	public void SelectSlimeToSpawn(SlimeBlueprint slime){
		slimeToSpawn = slime;
		PlayerStats.currentState = PlayerStats.State.building;
	}

	public SlimeBlueprint GetSlimeToSpawn(){
		return slimeToSpawn;
	}
}
