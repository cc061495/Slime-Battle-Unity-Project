using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

	public static PlayerStats Instance;
	[SerializeField]
	private Text playerCostText;
	[SerializeField]
	private GameObject playerInfoPanel;
	public static int playerCost;
	private int startCost = 1000;
	private int bounsCost = 50;

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
		playerCost += startCost + bounsCost * gm.currentRound;
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
}
