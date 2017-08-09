using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour {

	public Text sellAmount;
	public GameObject nodeUI;
	private Node target;

	void Start(){
		if(!PhotonNetwork.isMasterClient){
			nodeUI.transform.position += new Vector3(0,0,-6);
			nodeUI.transform.rotation = Quaternion.Euler(90,0,180);
		}
	}

	public void SetTarget(Node _target){
		target = _target;
		transform.position = target.GetSlimePostion();
		
		sellAmount.text = "$" + target.slimeblueprint.cost;

		nodeUI.SetActive(true);
	}

	public void Hide(){
		nodeUI.SetActive(false);
	}

	public void Sell(){
		target.SellSlime();
		SpawnManager.Instance.DeselectNode();
	}
}
