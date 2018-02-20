/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;

public class SlimeHealth : MonoBehaviour {

	public float currentHealth{get;private set;}
	public float startHealth{get;private set;}
	private float damageReduced = 1f;
	public int buff = 0;
	private Transform model;
	private PhotonView photonView;

	[Header("Slime Health Bar")]
	[SerializeField]
	private GameObject healthBarPrefab;
	public GameObject healthBar;
	private Transform healthBarGroup;

	SlimeClass slime;
	HealthBar playerHealth;

	void Awake(){
		photonView = GetComponent<PhotonView>();
		model = GetComponent<Slime>().GetModel();
	}

	public void SetUpSlimeHealth(SlimeClass _slime){
		slime = _slime;
		healthBarGroup = GameManager.Instance.healthBarParent;
		GeneratePlayerHealthBar(model);

		startHealth = slime.startHealth;
		currentHealth = startHealth;
	}

	private void GeneratePlayerHealthBar(Transform playerModel){
        healthBar = Instantiate(healthBarPrefab, new Vector3(0f,1000f,0f), Quaternion.identity) as GameObject;
        healthBar.transform.SetParent(healthBarGroup, false);
		playerHealth = healthBar.GetComponent<HealthBar>();
        playerHealth.SetHealthBarData(playerModel);
    }

	public void TakeDamage(float attackDamage){
		//Only Master client deal with attack damage
		//currentHealth must be larger than 0 HP(important) !!!
		if(currentHealth > 0){
			currentHealth -= (attackDamage * damageReduced);
			float amount = currentHealth / startHealth;
			playerHealth.OnHealthChanged(amount);
			//Update the others client health and health bar
			photonView.RPC("RPC_UpdateHealth", PhotonTargets.Others, currentHealth, amount);
		}
	}

	public void TakeHealing(float healPoint){
		//Only Master client deal with healing
		//currentHealth must be larger than 0 HP(important) !!!
		if(currentHealth > 0 && currentHealth < startHealth){
			//Health += healing percent * slime's start health
			currentHealth += healPoint;
			if(currentHealth > startHealth)
				currentHealth = startHealth;

			float amount = currentHealth / startHealth;
			playerHealth.OnHealthChanged(amount);
			//Update the others client health and health bar
			photonView.RPC("RPC_UpdateHealth", PhotonTargets.Others, currentHealth, amount);
		}
	}

	/* Update Client slime health and health bar */
	[PunRPC]
	private void RPC_UpdateHealth(float master_CurrentHealth, float master_fillAmount){
		currentHealth = master_CurrentHealth;
		playerHealth.OnHealthChanged(master_fillAmount);
		//Sync the dead, after update all clients' slime health
		if(currentHealth <= 0)
			photonView.RPC("RPC_SlimeDie", PhotonTargets.All);
	}

	/* Method for slime to die immediately */
	public void SuddenDeath(){
		TakeDamage(currentHealth);
	}

	[PunRPC]
	private void RPC_SlimeDie(){
		if(slime.isCleavable && PhotonNetwork.isMasterClient)
			GetComponent<SlimeSplite>().DoTheSlimeSplite(model.position);

		GetComponent<Slime>().RemoveFromTeamList();

		if(slime.isBuilding)
			BuildingUI.Instance.HideTheBuildingPanel(this);
	
		if(photonView != null && photonView.isMine)
			PhotonNetwork.Destroy(gameObject);
	}

	public void SetDamageReduced(float amount){
		if(damageReduced != amount){
			damageReduced = amount;
			Debug.Log(transform.name + ": " +damageReduced);
		}
	}

	public void SetupDefaultDamageReduced(){
		damageReduced = 1f;
		Debug.Log(transform.name + ": " +damageReduced);
	}
}