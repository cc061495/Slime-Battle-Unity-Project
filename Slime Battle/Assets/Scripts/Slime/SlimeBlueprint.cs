using System.Collections;
using UnityEngine;

[System.Serializable]
public class SlimeBlueprint{

	public GameObject slimePrefab{get;private set;}
    public int cost{get; private set;}
	public int size{get; private set;}
	public string team{get; private set;}
	public Vector3 spawnPosOffset{get; private set;}
	public string name{get; private set;}

	public SlimeBlueprint(string _name, GameObject _prefab){
		slimePrefab = _prefab;
		name = _name;

		switch (_name){
			case "Slime":
				spawnPosOffset = new Vector3(0,0.5f,0);
				cost = 10;
				size = 1;
				break;

			case "Giant":
				spawnPosOffset = new Vector3(0,2.5f,0);
				cost = 50;
				size = 4;
				break;

			case "Ranger":
				spawnPosOffset = new Vector3(0,1.5f,0);
				cost = 15;
				size = 1;
				break;

			case "Healer":
				spawnPosOffset = new Vector3(0,1.5f,0);
				cost = 25;
				size = 1;
				break;

			case "Bomber":
				spawnPosOffset = new Vector3(0,1.5f,0);
				cost = 8;
				size = 1;
				break;

			case "Wall":
				spawnPosOffset = new Vector3(0,1.5f,0);
				cost = 3;
				size = 1;
				break;
				
			default:
				spawnPosOffset = new Vector3(0,1.5f,0);
				cost = 1;
				size = 1;
				break;
		}
	}

	// private void setCost(int _cost){
	// 	cost = _cost;
	// }

	// private void setSize(int _size){
	// 	size = _size;
	// }

	// public int getCost(){
	// 	return cost;
	// }

	// public string getTeam(){
	// 	return team;
	// }

	// public int getSize(){
	// 	return size;
	// }

	// public Vector3 getOffset(){
	// 	return spawnPosOffset;
	// }

	// public string getPrefabName(){
	// 	return slimePrefab.name;
	// }
}
