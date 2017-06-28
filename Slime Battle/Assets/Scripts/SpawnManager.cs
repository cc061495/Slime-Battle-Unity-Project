using UnityEngine; 
using UnityEngine.UI; 

public class SpawnManager:MonoBehaviour {

	public static SpawnManager Instance; 

	void Awake() {
		Instance = this; 
	}

	private SlimeBlueprint slimeToSpawn; 


	public bool CanSpawn {get {return slimeToSpawn != null; }}

	public void SelectSlimeToSpawn(SlimeBlueprint slime) {
		slimeToSpawn = slime; 
	}

	public SlimeBlueprint GetSlimeToSpawn() {
		return slimeToSpawn; 
	}
}
