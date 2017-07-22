/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;

public class SlimeHealth : Photon.MonoBehaviour {

	private float currentHealth;
	private float startHealth;
	private Transform model;
	private SlimeClass slimeClass;

	[Header("Slime Health Bar")]
	[SerializeField]
	private Image healthBar;
	[SerializeField]
	private RectTransform healthBarPos;

	public void SetUpSlimeHealth(Transform _model, SlimeClass slime){
		model = _model;
		slimeClass = slime;
		startHealth = slime.startHealth;
		currentHealth = startHealth;
		UpdateHealthBarPos();

		if(!photonView.isMine)
		 	DisplaySlime(false);

		if(!PhotonNetwork.isMasterClient)
			ChangeHealthBarPos();
	}

	void FixedUpdate(){
		//health bar position
		if((GameManager.Instance.currentState == GameManager.State.battle_start ||
		   GameManager.Instance.currentState == GameManager.State.battle_end) && !slimeClass.isBuilding)
			UpdateHealthBarPos();
	}

	public void DisplaySlime(bool display){
		healthBarPos.gameObject.SetActive(display);
		model.gameObject.SetActive(display);
		if(GameManager.Instance.currentState == GameManager.State.build_end){
			if(!PhotonNetwork.isMasterClient && photonView.isMine)
				GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.masterClient);

			Invoke("NetworkEnable", 3);
		}
	}
	
	private void NetworkEnable(){
		SlimeNetwork network = GetComponent<SlimeNetwork>();
		if(network != null)
			network.enabled = true;

		SlimeMovement movement = GetComponent<SlimeMovement>(); 
		if(movement != null && photonView.isMine)
			movement.StartUpdatePathLoop();
	}

	private void ChangeHealthBarPos(){
		healthBarPos.rotation = Quaternion.Euler(90f, 0f, 0f);
		healthBar.fillOrigin = (int)Image.OriginHorizontal.Right;
	}

	private void UpdateHealthBarPos(){
		if(model != null){
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

	public void TakeHealing(float heal){
		//Only Master client deal with attack damage
		//currentHealth must be larger than 0 HP(important) !!!
		if(photonView.isMine && currentHealth > 0 && currentHealth < startHealth){
			currentHealth += heal;
			if(currentHealth >= startHealth){
				currentHealth = startHealth;
			}
				
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

	public float getCurrentHealth(){
		return currentHealth;
	}

	public float getStartHealth(){
		return startHealth;
	}
}