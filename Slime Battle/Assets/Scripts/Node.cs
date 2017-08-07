using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Node : MonoBehaviour
{
    public GameObject slime;
    private GameObject _slime;
    public SlimeBlueprint slimeblueprint;
    public string teamNode;
    private MeshRenderer tile;
    Transform _transform;
    GameManager gameManager;
    SpawnManager spawnManager;
    Vector3 buildPosition;

    // Use this for initialization
    void Start(){
        tile = GetComponent<MeshRenderer>();
        tile.enabled = false;

        _transform = transform;

        if(PhotonNetwork.isMasterClient && teamNode == "BLUE")
            GetComponent<BoxCollider>().enabled = false;
        else if(!PhotonNetwork.isMasterClient && teamNode == "RED")
            GetComponent<BoxCollider>().enabled = false;

        gameManager = GameManager.Instance;
        spawnManager = SpawnManager.Instance;
    }

    void OnMouseEnter(){
        if(gameManager.currentState == GameManager.State.build_start)
            TouchEnter();
    }

    void OnMouseDown(){
        if(gameManager.currentState != GameManager.State.build_start)
            return;

        if (slime != null)
            spawnManager.SelectNode(this);
    }

    public void TouchEnter(){
        if (slime != null || !spawnManager.CanSpawn || spawnManager.AnyNodeSelected)
            return;

        SpawnSlime(spawnManager.GetSlimeToSpawn());
    }

    void SpawnSlime(SlimeBlueprint blueprint){
        //Check Player Cost > slime money -> can build
        if(PlayerStats.playerCost < blueprint.cost)
            return;

        int size = blueprint.size;
        Vector3 offset = blueprint.spawnPosOffset;

        switch (size){
            case 1:
                buildPosition = GetBuildPos(offset);
                _slime = PhotonNetwork.Instantiate(blueprint.slimePrefab.name, buildPosition, Quaternion.identity, 0);
                BuildSlime(_slime, blueprint);
                break;

            case 4:
                Spawn_Size_2x2_Slime(blueprint, offset);
                break;

            default:
                break;
        }

        if(slime)
            PlayerStats.Instance.PurchaseSlime(blueprint.cost);        
        //Building effect
    }

    public void BuildSlime(GameObject _slime, SlimeBlueprint _blueprint){
        slime = _slime;
        slimeblueprint = _blueprint;
        tile.enabled = true;
        gameManager.nodeList.Add(this);
    }

    void Spawn_Size_2x2_Slime(SlimeBlueprint blueprint, Vector3 offset){
        List<Node> nodes = new List<Node>();
        Vector3 buildOffset = CanBuild2x2(nodes);
        
        if(buildOffset != Vector3.zero){
            _slime = PhotonNetwork.Instantiate(blueprint.slimePrefab.name, GetBuildPos(buildOffset + offset), Quaternion.identity, 0);
            
            for(int i=0;i<nodes.Count;i++)
                nodes[i].BuildSlime(_slime, blueprint);
        }
    }

    Vector3 CanBuild2x2(List<Node> nodes){
        Vector3 sphereOffset = Vector3.zero;

        for (int i = 0; i < 4; i++){
            nodes.Clear();

            if(PhotonNetwork.isMasterClient){
                if(i == 0)
                    sphereOffset = new Vector3(-1.5f,0,-1.5f);  //bottom left
                else if(i == 1)
                    sphereOffset = new Vector3(1.5f,0,-1.5f);   //bottm right
                else if(i == 2)
                    sphereOffset = new Vector3(-1.5f,0,1.5f);   //upper left
                else if(i == 3)
                    sphereOffset = new Vector3(1.5f,0,1.5f);    //upper right
            }
            else{
                if(i == 0)
                    sphereOffset = new Vector3(1.5f,0,1.5f);    //bottom left
                else if(i == 1)
                    sphereOffset = new Vector3(-1.5f,0,1.5f);   //bottom right
                else if(i == 2)
                    sphereOffset = new Vector3(1.5f,0,-1.5f);   //upper left
                else if(i == 3)
                    sphereOffset = new Vector3(-1.5f,0,-1.5f);  //upper right
            }
            
            Collider[] colliders = Physics.OverlapSphere(_transform.position + sphereOffset, 1f);
            for(int j=0;j<colliders.Length;j++){
                if(colliders[j].gameObject.tag == "node"){
                    Node e = colliders[j].gameObject.GetComponent<Node>();
                    
                    if (e.slime != null)
                        break;

                    nodes.Add(e);
                }
            }

            if(nodes.Count == 4)
                return sphereOffset;
        }
        return Vector3.zero;
    }

    public void SellSlime(){
        PlayerStats.Instance.SellSlime(slimeblueprint.cost);
        /* Selling Effect */

        tile.enabled = false;
        slime.GetComponent<Slime>().SyncRemoveTeamList();
        PhotonNetwork.Destroy(slime);
        slimeblueprint = null;
    }

    public Vector3 GetBuildPos(Vector3 offset){
        return _transform.position + offset;
    }

    public Vector3 GetSlimePostion(){
        return slime.transform.position;
    }

    public void ResetNode(){
        // rend.material.color = startColor;
        tile.enabled = false;
        slime = null;   //reset all the slime
        buildPosition = Vector3.zero;
    }

    // Color GetTeamColor(){
    //     if (team_node == "RED")
    //         return teamA_Color;
    //     else if (team_node == "BLUE")
    //         return teamB_Color;

    //     return Color.clear;
    // }

    /*Testing Touch on mobile (still not working)
	void Update(){
		if (Input.touchCount > 0) {
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
				RaycastHit hit;

				if (Physics.Raycast (ray, out hit) && hit.transform.gameObject.tag == "node")
					MouseEnter ();
			}
			if (Input.GetTouch (0).phase == TouchPhase.Ended)
				MouseExit();
		}
	}
	*/

    /*
    public void TouchEnter(){
        if(slime == null)
            rend.material.color = selectColor;

        if (gameManager.currentState == GameManager.State.build_start){

            if (slime != null || !spawnManager.CanSpawn)
                return;

            SpawnSlime(spawnManager.GetSlimeToSpawn());
        }
    }
    //when player exit the mobile screen
    void OnMouseExit(){
        if (slime == null)
            rend.material.color = startColor;
    }
    */

    /*
    //when player touch on the moblie screen
    public void Node_Select(){
        //Check Empty node
        if(slime == null){
            _tile = Instantiate(tile, transform.position + new Vector3(0, 0.51f, 0), Quaternion.identity);
            SlimeBlueprint blueprint = spawnManager.GetSlimeToSpawn();
            //Any slime selected in the shop?
            if(spawnManager.CanSpawn && blueprint.getTeam() == team_node){
                _tile.GetComponent<Renderer>().material.color = Color.green;

                if(blueprint.getSize() == 4){
                    Vector3 offset = new Vector3(1.5f, 0f, -1.5f);
                    if(!PhotonNetwork.isMasterClient){
                        offset = new Vector3(-1.5f, 0f, 1.5f);
                    }

                    if(CanBuild(blueprint.getSize(), offset)){
                        _tile.transform.position += offset;
                        _tile.transform.localScale = new Vector3(0.6f,1,0.6f);
                    }
                }
            }
        }
    }

    public void Node_Build(){
        if(slime == null){
            if(spawnManager.CanSpawn)
                SpawnSlime(spawnManager.GetSlimeToSpawn());
            else
                Destroy(_tile);
        }
    }
    */
}