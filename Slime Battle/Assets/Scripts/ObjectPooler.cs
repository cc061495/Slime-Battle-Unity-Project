/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPooler : MonoBehaviour {

	[System.Serializable]
	public class Pool{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	#region Singleton
	public static ObjectPooler Instance;
	private void Awake(){
		Instance = this;
	}
	#endregion
	public List<Pool> pools;
	public List<GameObject> objToDestory;
	public Dictionary<string, Queue<GameObject>> poolDictionary;

	public void PoolAnalysis(List<Transform> team_red, List<Transform> team_blue){

		List<Transform> teamList = team_red.Concat(team_blue).ToList();

		for(int i=0;i<teamList.Count;i++){
			string s_name = teamList[i].root.GetComponent<Slime>().slimeName;

			if(s_name == "Ranger"){
				pools[0].size++;
			}
			else if(s_name == "Turret"){
				pools[0].size++;
			}
			else if(s_name== "Snowman"){
				pools[1].size++;
			}
		}
		StartUsingObjectPooler();
	}

	public void StartUsingObjectPooler () {
		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		for (int i = 0; i < pools.Count; i++){
			Queue<GameObject> objectPool = new Queue<GameObject>();

			for (int j = 0; j < pools[i].size; j++){
				GameObject obj = Instantiate(pools[i].prefab);
				objToDestory.Add(obj);
				objectPool.Enqueue(obj);
				
				obj.SetActive(false);
			}
			poolDictionary.Add(pools[i].tag, objectPool);
		}
	}

	public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation){
		if(!poolDictionary.ContainsKey(tag)){
			Debug.LogWarning("Pool with tag " + tag + "doesn't exist.");
			return null;
		}
		GameObject objectToSpawn = poolDictionary[tag].Dequeue();

		objectToSpawn.transform.position = position;
		objectToSpawn.transform.rotation = rotation;

		return objectToSpawn;
	}

	public void BackToPool(string tag, GameObject obj){
		obj.SetActive(false);
		poolDictionary[tag].Enqueue(obj);
	}

	public void DestoryObjectPool(){
		for (int i = 0; i < pools.Count; i++)
			pools[i].size = 0;

		for (int i = 0; i < objToDestory.Count; i++)
			Destroy(objToDestory[i]);

		objToDestory.Clear();
	}
}
