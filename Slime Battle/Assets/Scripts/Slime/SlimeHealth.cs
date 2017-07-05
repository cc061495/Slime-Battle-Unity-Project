/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeHealth : Photon.MonoBehaviour {

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
		//Only Master client deal with attack damage
		//currentHealth must be larger than 0 HP(important) !!!
		if(PhotonNetwork.isMasterClient && currentHealth > 0){
			currentHealth -= attackDamage;
			healthBar.fillAmount = currentHealth / startHealth;
			//Update the others client health and health bar
			photonView.RPC("RPC_UpdateHealth", PhotonTargets.Others, currentHealth, healthBar.fillAmount);
		}
	}

	[PunRPC]
	private void RPC_UpdateHealth(float master_CurrentHealth, float master_fillAmount){
		currentHealth = master_CurrentHealth;
		healthBar.fillAmount = master_fillAmount;
		//Sync the dead, after update all clients'slime health
		if(currentHealth <= 0)
			photonView.RPC("RPC_SlimeDie", PhotonTargets.All);
	}

	[PunRPC]
	private void RPC_SlimeDie(){
		GetComponent<Slime>().SlimeDead();
		if(photonView.isMine)
			PhotonNetwork.Destroy(transform.parent.gameObject);
	}
}
