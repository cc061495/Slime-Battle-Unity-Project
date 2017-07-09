using System.Collections;
using UnityEngine;

[System.Serializable]
public class SlimeBlueprint{

	private GameObject slimePrefab;
    private int cost;
	private int size;
	private string team;
	private Vector3 spawnPosOffset;

	public SlimeBlueprint(string _name, string _team, GameObject _prefab){
		team = _team;
		slimePrefab = _prefab;

		switch (_name)
		{
			case "Slime":
				setCost	( 1 );
				setSize	( 1 );
				break;

			case "Giant":
				setCost	( 1 );
				setSize	( 4 );
				break;

			case "Ranger":
				setCost	( 1 );
				setSize	( 1 );
				break;
				
			default:
				setCost	( 1 );
				setSize	( 1 );
				break;
		}

		if(size == 1)
			spawnPosOffset = new Vector3(0,1.5f,0);
		else if(size == 4)
			spawnPosOffset = new Vector3(0,2.5f,0);
	}

	private void setCost(int _cost){
		cost = _cost;
	}

	private void setSize(int _size){
		size = _size;
	}

	public int getCost(){
		return cost;
	}

	public string getTeam(){
		return team;
	}

	public int getSize(){
		return size;
	}

	public Vector3 getOffset(){
		return spawnPosOffset;
	}

	public string getPrefabName(){
		return slimePrefab.name;
	}
}
