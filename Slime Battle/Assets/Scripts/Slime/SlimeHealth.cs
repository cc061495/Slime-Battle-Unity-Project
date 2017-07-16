/* Copyright (c) cc061495 */
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
	private bool updateDisplay;

	public void SetUpSlimeHealth(){
		model = GetComponent<Slime>().GetModel();
		currentHealth = GetComponent<Slime>().GetSlimeClass().getStartHealth();
		startHealth = currentHealth;
		UpdateHealthBarPos();
		if(!photonView.isMine)
		 	DisplaySlime(false);

		if(!PhotonNetwork.isMasterClient){
			ChangeHealthBarPos();

			if(photonView.isMine)
				Invoke("TransferOwner", 0.5f);
		}
	}

	void TransferOwner(){
		GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.masterClient);
	}

	void FixedUpdate(){
		//health bar position
		UpdateHealthBarPos();
		//show the model and health when state = end_build
		if(GameManager.Instance.currentState == GameManager.State.build_end && !updateDisplay){
			updateDisplay = true;
			DisplaySlime(true);
			GetComponent<SlimeNetwork>().enabled = true;
		}
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
		if(healthBarPos.hasChanged && model != null){
			if(PhotonNetwork.isMasterClient)
				healthBarPos.position = new Vector3 (model.position.x, model.position.y+2f, model.position.z+1f);
			else
				healthBarPos.position = new Vector3 (model.position.x, model.position.y+2f, model.position.z-1f);
		}
	}

	public void TakeDamage(float attackDamage){
		//Only Master client deal with attack damage
		//currentHealth must be larger than 0 HP(important) !!!
		if(photonView.isMine && currentHealth > 0){
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
		GetComponent<Slime>().RemoveFromTeamList();
		if(photonView.isMine)
			PhotonNetwork.Destroy(gameObject);
	}
}
