using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour {

	public Text sellAmount;
	public GameObject nodeUI;
	private Node target;
	private GameManager gameManager;

	// void Start(){
	// 	if(!PhotonNetwork.isMasterClient){
	// 		nodeUI.transform.position += new Vector3(0,0,-6);
	// 		nodeUI.transform.rotation = Quaternion.Euler(90,0,180);
	// 	}
	// }
	void Start(){
		gameManager = GameManager.Instance;
	}

	public void SetTarget(Node _target){
		target = _target;
		//transform.position = target.GetSlimePostion();
		
		sellAmount.text = "$" + target.slimeblueprint.cost;

		gameManager.ShopDisplay(false);
		nodeUI.SetActive(true);
	}

	public void Hide(){
		gameManager.ShopDisplay(true);
		nodeUI.SetActive(false);
	}

	public void Sell(){
		target.SellSlime();
		SpawnManager.Instance.DeselectNode();
	}
}
