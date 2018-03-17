using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Linq;

public class Node : MonoBehaviour
{
    public GameObject slime;
    private GameObject _slime;
    public SlimeBlueprint slimeblueprint;
    public string teamNode;
    private MeshRenderer tile;
    public List<Node> nodeList;
    Transform _transform;
    GameManager gameManager;
    SpawnManager spawnManager;
    PlayerStats playerStats;
    Vector3 buildPosition;

    // Use this for initialization
    void Start(){
        tile = GetComponent<MeshRenderer>();
        _transform = transform;

        if(PhotonNetwork.isMasterClient && teamNode == "BLUE")
            GetComponent<BoxCollider>().enabled = false;
        else if(!PhotonNetwork.isMasterClient && teamNode == "RED")
            GetComponent<BoxCollider>().enabled = false;

        gameManager = GameManager.Instance;
        spawnManager = SpawnManager.Instance;
        playerStats = PlayerStats.Instance;
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
        if (slime != null || !spawnManager.CanSpawn || spawnManager.AnyNodeSelected){
            return;
        }

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

            case 2:
                Spawn_Size_1x2_Slime(blueprint, offset);
                break;

            case 4:
                Spawn_Size_2x2_Slime(blueprint, offset);
                break;

            default:
                break;
        }

        if(slime){
            playerStats.PurchaseSlime(blueprint.cost);
            PlayerShop.Instance.ButtonsUpdate();
        }

        //Building effect
    }

    public void BuildSlime(GameObject _slime, SlimeBlueprint _blueprint){
        slime = _slime;
        slimeblueprint = _blueprint;
        tile.enabled = true;
        gameManager.nodeList.Add(this);
    }

    void Spawn_Size_2x2_Slime(SlimeBlueprint blueprint, Vector3 offset){
        nodeList = new List<Node>();
        Vector3 buildOffset = CanBuild2x2(nodeList);
        
        if(buildOffset != Vector3.zero){
            _slime = PhotonNetwork.Instantiate(blueprint.slimePrefab.name, GetBuildPos(buildOffset + offset), Quaternion.identity, 0);
            
            for(int i=0;i<nodeList.Count;i++){
                nodeList[i].BuildSlime(_slime, blueprint);
                nodeList[i].nodeList = nodeList.ToList();
            }   
        }
    }

    void Spawn_Size_1x2_Slime(SlimeBlueprint blueprint, Vector3 offset){
        nodeList = new List<Node>();
        Vector3 buildOffset = CanBuild1x2(nodeList);
        
        if(buildOffset != Vector3.zero){
            _slime = PhotonNetwork.Instantiate(blueprint.slimePrefab.name, GetBuildPos(buildOffset + offset), Quaternion.identity, 0);
            
            for(int i=0;i<nodeList.Count;i++){
                nodeList[i].BuildSlime(_slime, blueprint);
                nodeList[i].nodeList = nodeList.ToList();
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

    Vector3 CanBuild1x2(List<Node> nodes){
        Vector3 sphereOffset = Vector3.zero;

        for (int i = 0; i < 2; i++){
            nodes.Clear();

            if(PhotonNetwork.isMasterClient){
                if(i == 0)
                    sphereOffset = new Vector3(-1.5f,0,0);  // left
                else if(i == 1)
                    sphereOffset = new Vector3(1.5f,0,0);   // right
            }
            else{
                if(i == 0)
                    sphereOffset = new Vector3(1.5f,0,0);  // left
                else if(i == 1)
                    sphereOffset = new Vector3(-1.5f,0,0);   // right
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

            if(nodes.Count == 2)
                return sphereOffset;
        }
        return Vector3.zero;
    }

    public void SellSlime(){
        /* Selling the slime and get back the money */
        playerStats.SellSlime(slimeblueprint.cost);
        /* Remove the slime from team list in two players */
        slime.GetComponent<Slime>().SyncRemoveTeamList();
        /* Destroy the slime in the Photon Network */
        PhotonNetwork.Destroy(slime);
        /* Selling Effect */
        if(nodeList.Count > 0){
            List<Node> tempNodeList = nodeList.ToList();
            for(int i=0;i<tempNodeList.Count;i++)
                NodeResetting(tempNodeList[i]);
        }
        else
            NodeResetting(this);
        /* Update the player shop buttons after selling */
        PlayerShop.Instance.ButtonsUpdate();
    }

    public void NodeResetting(Node n){
        //Debug.Log(n);
        n.slimeblueprint = null;
        n.tile.enabled = false;
        n.slime = null;
        if(n.nodeList.Count > 0)
            n.nodeList.Clear();
    }

    public Vector3 GetBuildPos(Vector3 offset){
        Vector3 pos;
        pos.x = _transform.position.x + offset.x;
        pos.y = offset.y;
        pos.z = _transform.position.z + offset.z;
        return pos;
    }
}