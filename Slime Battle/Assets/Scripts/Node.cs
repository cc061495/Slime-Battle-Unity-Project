using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Node : Photon.MonoBehaviour
{
    // public Color selectColor;
    // public Color availableColor;
    // public Color teamA_Color;
    // public Color teamB_Color;

    public GameObject slime;
    private GameObject _slime;
    public SlimeBlueprint slimeblueprint;
    public string team_node;
    [SerializeField]
    private GameObject tile;
    private GameObject _tile;
    private Renderer rend;
    //private Color startColor;
    GameManager gameManager;
    SpawnManager spawnManager;

    // Use this for initialization
    void Start(){
        //rend = GetComponent<Renderer>();
        //startColor = rend.material.color;
        gameManager = GameManager.Instance;
        spawnManager = SpawnManager.Instance;
    }

    void OnMouseEnter(){
        if(gameManager.currentState == GameManager.State.build_start)
            TouchEnter();
    }

    public void TouchEnter(){
        if (slime != null || !spawnManager.CanSpawn)
            return;

        SpawnSlime(spawnManager.GetSlimeToSpawn());
    }

    void SpawnSlime(SlimeBlueprint blueprint){
        //Check Player Cost > slime money -> can build
        if (blueprint.getTeam() != team_node)
            return;

        int size = blueprint.getSize();
        Vector3 offset = blueprint.getOffset();

        switch (size){
            case 1:
                _slime = PhotonNetwork.Instantiate(blueprint.getPrefabName(), GetBuildPos(offset), Quaternion.identity, 0);
                _slime.GetComponent<Slime>().model.rotation = transform.rotation;
                _tile = Instantiate(tile, transform.position + new Vector3(0, 0.51f, 0), Quaternion.identity);
                slime = _slime;
                slimeblueprint = blueprint;
                //rend.material.color = GetTeamColor();
                break;

            case 4:
                Spawn_Size_2x2_Slime(blueprint, offset);
                break;

            default:
                break;
        }
        //Building effect
    }

    void Spawn_Size_2x2_Slime(SlimeBlueprint blueprint, Vector3 offset){
        List<Node> nodes = new List<Node>();
        Vector3 buildOffset = CanBuild2x2(nodes);

        if(buildOffset != Vector3.zero){
            _slime = PhotonNetwork.Instantiate(blueprint.getPrefabName(), GetBuildPos(buildOffset + offset), Quaternion.identity, 0);
            _slime.GetComponent<Slime>().model.rotation = transform.rotation;
            _tile = Instantiate(tile, transform.position + new Vector3(0, 0.51f, 0) + buildOffset, Quaternion.identity);
            _tile.transform.localScale = new Vector3(0.6f,1,0.6f);

            foreach (Node node in nodes){
                node.slime = _slime;
                node.slimeblueprint = blueprint;
            }
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
            
            Collider[] colliders = Physics.OverlapSphere(transform.position + sphereOffset, 1f);
            foreach (Collider col in colliders){
                if (col.gameObject.tag == "node"){
                    Node e = col.gameObject.GetComponent<Node>();
                    
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

    public Vector3 GetBuildPos(Vector3 offset){
        return transform.position + offset;
    }

    public void ResetNode(){
        // rend.material.color = startColor;
        if(_tile != null)
            Destroy(_tile);
            
        slime = null;   //reset all the slime
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
