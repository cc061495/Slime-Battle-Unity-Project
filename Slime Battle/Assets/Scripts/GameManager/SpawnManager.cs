using UnityEngine; 
using UnityEngine.UI; 

public class SpawnManager:MonoBehaviour {

	public static SpawnManager Instance;

	void Awake() {
		Instance = this;
	}

	private Node selectedNode;
	private SlimeBlueprint slimeToSpawn; 

	public NodeUI nodeUI;
	public bool CanSpawn {get {return slimeToSpawn != null; }}
	public bool AnyNodeSelected{get {return selectedNode != null; }}

	public void SelectNode (Node node){
		if(selectedNode == node)
			return;
		
		selectedNode = node;
		//slimeToSpawn = null;

		nodeUI.SetTarget(node);
	}

	public void DeselectNode(){
		selectedNode = null;
		nodeUI.Hide();
	}

	public void SelectSlimeToSpawn(SlimeBlueprint slime) {
		slimeToSpawn = slime; 
	}

	public SlimeBlueprint GetSlimeToSpawn() {
		return slimeToSpawn; 
	}

	public void ClearSlimeToSpawn(){
		slimeToSpawn = null;
	}
}
