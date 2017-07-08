/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeHealth : Photon.MonoBehaviour {

	private float currentHealth;
	private float startHealth;
	private Transform model;

	[Header("Slime Health Bar")]
	[SerializeField]
	private Image healthBar;
	[SerializeField]
	private RectTransform healthBarPos;

	void Start(){
		model = GetComponent<Slime>().model;
		currentHealth = GetComponent<Slime> ().s.getStartHealth();
		startHealth = currentHealth;
		UpdateHealthBarPos();

		if(!PhotonNetwork.isMasterClient)
			ChangeHealthBarPos();

		if(!photonView.isMine)
			DisplaySlime(false);
	}

	void FixedUpdate(){
		//health bar position
		UpdateHealthBarPos();
		//show the model and health when state = end_build
		if(GameManager.Instance.currentState == GameManager.State.build_end && !model.gameObject.activeSelf)
			DisplaySlime(true);
	}

	private void DisplaySlime(bool display){
		healthBarPos.gameObject.SetActive(display);
		model.gameObject.SetActive(display);
	}

	private void ChangeHealthBarPos(){
		healthBarPos.rotation = Quaternion.Euler(90f, 0f, 0f);
		healthBar.fillOrigin = (int)Image.OriginHorizontal.Right;
	}

	private void UpdateHealthBarPos(){
		if(healthBarPos.hasChanged){
			if(PhotonNetwork.isMasterClient)
				healthBarPos.position = new Vector3 (model.position.x, model.position.y+2f, model.position.z+1f);
			else
				healthBarPos.position = new Vector3 (model.position.x, model.position.y+2f, model.position.z-1f);
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
			PhotonNetwork.Destroy(gameObject);
	}
}
