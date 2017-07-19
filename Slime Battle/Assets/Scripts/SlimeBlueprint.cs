using System.Collections;
using UnityEngine;

[System.Serializable]
public class SlimeBlueprint{

	public GameObject slimePrefab{get;private set;}
    public int cost{get; private set;}
	public int size{get; private set;}
	public string team{get; private set;}
	public Vector3 spawnPosOffset{get; private set;}

	public SlimeBlueprint(string _name, string _team, GameObject _prefab){
		team = _team;
		slimePrefab = _prefab;

		switch (_name){
			case "Slime":
				cost = 1;
				size = 1;
				break;

			case "Giant":
				cost = 1;
				size = 4;
				break;

			case "Ranger":
				cost = 1;
				size = 1;
				break;

			case "Healer":
				cost = 1;
				size = 1;
				break;

			case "Bomber":
				cost = 1;
				size = 1;
				break;
				
			default:
				cost = 1;
				size = 1;
				break;
		}
		if(size == 1)
			spawnPosOffset = new Vector3(0,1.5f,0);
		else if(size == 4)
			spawnPosOffset = new Vector3(0,2.5f,0);
		
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
