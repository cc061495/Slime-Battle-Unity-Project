using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

	public static PlayerStats Instance;
	[SerializeField]
	private Text playerCostText;
	[SerializeField]
	private GameObject playerInfoPanel;

	public static int playerCost;
	private const int startCost = 100;
	private const int roundBounsCost = 200;
	public int spawnedCost;

	GameManager gm;

	void Awake(){
		Instance = this;	
		gm = GameManager.Instance;
	}

	void Start(){
		playerCost = startCost;
		UpdatePlayerCostText();
	}

	public void UpdatePlayerCostText(){
		playerCostText.text = "$ " + playerCost;
	}

	public void NewRoundCostUpdate(){
		/* PlayerCost = Cost + RoundBouns*/
		playerCost += roundBounsCost * gm.currentRound + spawnedCost;
		spawnedCost = 0;
		UpdatePlayerCostText();
	}

	public void PlayerInfoPanelDisplay(bool display){
		playerInfoPanel.SetActive(display);
	}

	public void PurchaseSlime(int cost){
		playerCost -= cost;
		UpdatePlayerCostText();
	}

	public void SellSlime(int cost){
		playerCost += cost;
		UpdatePlayerCostText();
	}

	public void CostToSpawn(int cost){
		spawnedCost += cost;
	}
	
	public int GetBounsCost(){
		return roundBounsCost;
	}

	public int GetSpawnedCost(){
		return spawnedCost;
	}
}
