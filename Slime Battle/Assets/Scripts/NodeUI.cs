using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour {

	public static NodeUI Instance;
	public Text sellAmount;
	public GameObject nodeUI, arrowUI;
	private Node target;
	private GameManager gameManager;

	// void Start(){
	// 	if(!PhotonNetwork.isMasterClient){
	// 		nodeUI.transform.position += new Vector3(0,0,-6);
	// 		nodeUI.transform.rotation = Quaternion.Euler(90,0,180);
	// 	}
	// }
	void Awake(){
		Instance = this;
	}
	void Start(){
		gameManager = GameManager.Instance;
	}

	public void SetTarget(Node _target){
		target = _target;
		//transform.position = target.GetSlimePostion();
		
		sellAmount.text = "$" + target.slimeblueprint.cost;
		arrowUI.transform.position = target.slime.transform.position + new Vector3(0,3,0);

		gameManager.ShopDisplay(false);
		SellingPanelDisplay(true);
	}

	public void Hide(){
		gameManager.ShopDisplay(true);
		SellingPanelDisplay(false);
	}

	public void Sell(){
		target.SellSlime();
		SpawnManager.Instance.DeselectNode();
	}

	public void SellingPanelDisplay(bool display){
		nodeUI.SetActive(display);
		arrowUI.SetActive(display);
	}
}
