/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeHealth : MonoBehaviour {

	public float currentHealth;
	private float startHealth;

	[Header("Slime Health Bar")]
	[SerializeField]
	private Image healthBar;
	[SerializeField]
	private RectTransform healthBarPos;

	void Start(){
		currentHealth = GetComponent<Slime> ().startHealth;
		startHealth = currentHealth;

		if(!PhotonNetwork.isMasterClient){
			healthBarPos.rotation = Quaternion.Euler(90f, 0f, 0f);
			healthBar.fillOrigin = (int)Image.OriginHorizontal.Right;
		}
	}

	void Update(){
		UpdateHealthBarPos();   //health bar position
	}

	public void UpdateHealthBarPos(){
		if(healthBarPos.hasChanged){
			if(PhotonNetwork.isMasterClient)
				healthBarPos.position = new Vector3 (transform.position.x+0f, transform.position.y+2f, transform.position.z+1f);
			else
				healthBarPos.position = new Vector3 (transform.position.x+0f, transform.position.y+2f, transform.position.z-1f);
		}
	}
	
	public void TakeDamage(float attackDamage){
		//Only master client take damage when hp > 0
		//currentHealth must be larger than 0 HP(important) !!!
		if(PhotonNetwork.isMasterClient && currentHealth > 0){
			currentHealth -= attackDamage;
			healthBar.fillAmount = currentHealth / startHealth;

			PhotonView pv = GetComponent<PhotonView>();
			if(pv != null)
				pv.RPC("RPC_UpdateHealth", PhotonTargets.Others, currentHealth, healthBar.fillAmount);
		}
	}

	[PunRPC]
	private void RPC_UpdateHealth(float master_CurrentHealth, float master_fillAmout){
		PhotonView pv = GetComponent<PhotonView>();
		if(pv != null){
			currentHealth = master_CurrentHealth;
			healthBar.fillAmount = master_fillAmout;

			if(currentHealth <= 0)
				pv.RPC("RPC_Die", PhotonTargets.All);
		}
	}

	[PunRPC]
	public void RPC_Die(){
		GetComponent<Slime>().SlimeDead();
		
		PhotonView pv = transform.parent.GetComponent<PhotonView>();
		if(pv != null){
			if(pv.instantiationId == 0)
				Destroy(transform.parent.gameObject);	//Destroy the gameobject which is not in the network
			else{
				if(pv.isMine)
					PhotonNetwork.Destroy(pv);	//Destroy your owner PhotonNetwork gameobject
			}
		}
	}
}
