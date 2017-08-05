/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;

public class SlimeHealth : MonoBehaviour {

	public float currentHealth{get;private set;}
	public float startHealth{get;private set;}
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

		if(!photonView.isMine){
		 	DisplaySlime(false);
		}
		else{
			if(!PhotonNetwork.isMasterClient && !slime.isBuilding)
				Invoke("NetworkTransfer", 0.5f);
		}
	}

	private void NetworkTransfer(){
		photonView.TransferOwnership(GameManager.Instance.masterPlayer);
	}

	private void GeneratePlayerHealthBar(Transform playerModel){
        healthBar = Instantiate(healthBarPrefab, new Vector3(0f,1000f,0f), Quaternion.identity) as GameObject;
        healthBar.transform.SetParent(healthBarGroup, false);
		playerHealth = healthBar.GetComponent<HealthBar>();
        playerHealth.SetHealthBarData(playerModel);
    }

	public void DisplayHealthBar(bool display){
		healthBar.SetActive(display);
	}

	public void DisplaySlime(bool display){
		model.gameObject.SetActive(display);
		if(GameManager.Instance.currentState == GameManager.State.build_end){
			NetworkEnable();
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

	public void TakeDamage(float attackDamage){
		//Only Master client deal with attack damage
		//currentHealth must be larger than 0 HP(important) !!!
		if(currentHealth > 0){
			currentHealth -= attackDamage;
			float amount = currentHealth / startHealth;
			playerHealth.OnHealthChanged(amount);
			//Update the others client health and health bar
			photonView.RPC("RPC_UpdateHealth", PhotonTargets.Others, currentHealth, amount);
		}
	}

	public void TakeHealing(float heal){
		//Only Master client deal with attack damage
		//currentHealth must be larger than 0 HP(important) !!!
		if(currentHealth > 0 && currentHealth < startHealth){
			currentHealth += heal;
			if(currentHealth > startHealth)
				currentHealth = startHealth;

			float amount = currentHealth / startHealth;
			playerHealth.OnHealthChanged(amount);
			//Update the others client health and health bar
			photonView.RPC("RPC_UpdateHealth", PhotonTargets.Others, currentHealth, amount);
		}
	}

	[PunRPC]
	private void RPC_UpdateHealth(float master_CurrentHealth, float master_fillAmount){
		currentHealth = master_CurrentHealth;
		playerHealth.OnHealthChanged(master_fillAmount);
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

	// private void ChangeHealthBarPos(){
	// 	healthBarPos.SetPositionAndRotation(healthBarPos.position, Quaternion.Euler(90f, 0f, 0f));
	// 	//healthBarPos.rotation = Quaternion.Euler(90f, 0f, 0f);
	// 	healthBar.fillOrigin = (int)Image.OriginHorizontal.Right;
	// }
}