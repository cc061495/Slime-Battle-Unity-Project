using UnityEngine; 
using UnityEngine.UI; 

public class SpawnManager:MonoBehaviour {

	public static SpawnManager Instance;

	void Awake() {
		Instance = this;
	}

	private Node selectedNode;
	private SlimeBlueprint slimeToSpawn; 
	public bool CanSpawn {get {return slimeToSpawn != null; }}
	public bool AnyNodeSelected{get {return selectedNode != null; }}

	public void SelectNode (Node node){
		if(selectedNode == node)
			return;
		
		selectedNode = node;
		//slimeToSpawn = null;

		SellingUI.Instance.SetTarget(node);
	}

	public void DeselectNode(){
		/* Clear the selected node */
		ClearSelectedNode();
        /* Clear the selected slime to spawn */
       	ClearSlimeToSpawn();
        /* Reset the selected shop text from cost text to name text */
        PlayerShop.Instance.ResetShopText();

		SellingUI.Instance.Hide();
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

	public void ClearSelectedNode(){
		selectedNode = null;
	}
}
