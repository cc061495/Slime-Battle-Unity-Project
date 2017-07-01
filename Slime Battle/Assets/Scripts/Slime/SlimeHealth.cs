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
	}

	public void UpdateHealthBarPos(){
		if(healthBarPos.hasChanged){
			if(PhotonNetwork.isMasterClient){
				healthBarPos.position = new Vector3 (transform.position.x, transform.position.y + 2f, transform.position.z + 1f);
			}
			else{
				healthBarPos.position = new Vector3 (transform.position.x, transform.position.y + 2f, transform.position.z - 1f);
				healthBarPos.rotation = Quaternion.Euler(90f, 0f, 0f);
			}
		}
	}
	
	public void TakeDamage(float attackDamage){
		if(PhotonNetwork.isMasterClient){
			currentHealth -= attackDamage;
			healthBar.fillAmount = currentHealth / startHealth;
			/*
			if(currentHealth <= 0)
				GetComponent<Slime>().Die();
			*/
			PhotonView pv = GetComponent<PhotonView>();
			if(pv != null)
				pv.RPC("RPC_UpdateHealth", PhotonTargets.Others, currentHealth, healthBar.fillAmount);
		}
	}

	[PunRPC]
	private void RPC_UpdateHealth(float m_CurrentHealth, float m_fillAmout){
		PhotonView pv = GetComponent<PhotonView>();
		if(pv != null){
			currentHealth = m_CurrentHealth;
			healthBar.fillAmount = m_fillAmout;

			if(currentHealth <= 0)
				pv.RPC("RPC_Die", PhotonTargets.All);
		}
	}
}
