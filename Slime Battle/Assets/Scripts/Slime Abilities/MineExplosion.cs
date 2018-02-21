using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineExplosion : MonoBehaviour {

	private Slime s;
	private SlimeClass slime;
	private SlimeHealth health;
	private Transform model;

	private string enemyTag;
	PhotonView photonView;
	GameManager gameManager;

	void Start(){
		s = transform.parent.GetComponent<Slime>();
		health = transform.parent.GetComponent<SlimeHealth>();
		model = s.GetModel();
		slime = s.GetSlimeClass();
		GetComponent<SphereCollider>().radius = slime.detectRadius;
		enemyTag = (transform.parent.tag == "Team_RED") ? "Team_BLUE" : "Team_RED";
		photonView = transform.parent.GetComponent<PhotonView>();
		gameManager = GameManager.Instance;
	}

	void OnTriggerEnter(Collider other){
		if(gameManager.currentState == GameManager.State.battle_start && other.transform.parent.tag == enemyTag && slime.isInvisible){
			// display the mine to another player
			if(!model.gameObject.activeSelf)
				model.gameObject.SetActive(true);		
			// explosion!!!
			if(photonView.isMine)
				Invoke("Explosion", 0.35f);

			slime.isInvisible = false;
		}
	}

	private void Explosion(){
		Vector3 centre = model.position;
		float effectAreaRadius = slime.areaEffectRadius;
		float attackDamage = slime.attackDamage;

		Collider[] slimes = Physics.OverlapSphere(centre, effectAreaRadius);

		for(int i=0;i<slimes.Length;i++){
			if(slimes[i].transform.parent.tag == enemyTag){
				SlimeHealth h = slimes[i].transform.parent.GetComponent<SlimeHealth>();

				float distanceFromCentre = DistanceCalculate(slimes[i].transform.position, centre);
				//explosion constant(higher = lower damage, lower = higher damage received)
				float areaDamage = attackDamage - distanceFromCentre * 0.15f;
				//Debug.Log("Distance: " + distanceFromCentre + " Damage: " + areaDamage);
				if(areaDamage < 0)
					continue;	//if the damage is lower than 0, just skip it

				h.TakeDamage(areaDamage);
			}
		}
		health.SuddenDeath();
	}

	private float DistanceCalculate(Vector3 pos1, Vector3 pos2){
		Vector3 distance = Vector3.zero;
		distance.x = pos1.x - pos2.x;
		distance.y = pos1.y - pos2.y;
		distance.z = pos1.z - pos2.z;

		float magnitude = distance.x * distance.x +
						  distance.y * distance.y +
						  distance.z * distance.z;
		return magnitude;
	}
}
