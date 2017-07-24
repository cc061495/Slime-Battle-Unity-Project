using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayForNodes{

	private Transform[] nodes = new Transform[112];
	private int nodeCount;
	
	public void Add(Transform node){
		nodes[nodeCount] = node;
		nodeCount++;
	}

	public int GetNodeCount(){
		return nodeCount;
	}

	public Transform GetNode(int count){
		return nodes[count];
	}
}
