﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Node : Photon.MonoBehaviour
{
    public Color selectColor;
    public Color availableColor;
    public Color teamA_Color;
    public Color teamB_Color;

    public GameObject slime;
    public SlimeBlueprint slimeblueprint;
    public int team_node;

    private Renderer rend;
    private Color startColor;
    SpawnManager spawnManager;
    GameManager gameManager;

    // Use this for initialization
    void Start(){
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        spawnManager = SpawnManager.Instance;
        gameManager = GameManager.Instance;
    }
    //when player touch on the moblie screen
    void OnMouseEnter(){
        if(slime == null)
            rend.material.color = selectColor;

        if (gameManager.currentState == GameManager.State.building){

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

    void SpawnSlime(SlimeBlueprint blueprint){
        //Check Player Cost > slime money -> can build
        if (blueprint.team != team_node)
            return;

        int size = blueprint.size;
        Vector3 offset = blueprint.spawnPosOffset;

        switch (size){
            case 1:
                GameObject _slime = PhotonNetwork.Instantiate(blueprint.slimePrefab.name, GetBuildPos(offset), transform.rotation, 0);
                _slime.GetComponent<SlimeNetwork>().enabled = true;
                slime = _slime;
                slimeblueprint = blueprint;
                rend.material.color = GetTeamColor();
                break;

            case 4:
                Spawn_Size_2x2_Slime(blueprint, size, offset);
                break;

            default:
                break;
        }
        //Building effect
    }

    void Spawn_Size_2x2_Slime(SlimeBlueprint blueprint, int size, Vector3 offset){
        float radius = 1f;
        int numOfNode = 0;

        Collider[] colliders = Physics.OverlapSphere(transform.position + offset, radius);
        foreach (Collider col in colliders){
            if (col.gameObject.tag == "node"){
                Node e = col.gameObject.GetComponent<Node>();
                if (e.slime != null)
                    return;

                numOfNode++;
            }
        }

        if (numOfNode == size){
            GameObject _slime = PhotonNetwork.Instantiate(blueprint.slimePrefab.name, GetBuildPos(offset), transform.rotation, 0);
            _slime.GetComponent<SlimeNetwork>().enabled = true;
            foreach (Collider col in colliders){
                if (col.gameObject.tag == "node"){
                    Node e = col.gameObject.GetComponent<Node>();
                    e.slime = _slime;
                    e.slimeblueprint = blueprint;
                    e.rend.material.color = GetTeamColor();
                }
            }
        }
    }

    Color GetTeamColor(){
        if (team_node == 1)
            return teamA_Color;
        else if (team_node == 2)
            return teamB_Color;

        return Color.clear;
    }

    public Vector3 GetBuildPos(Vector3 offset){
        return transform.position + offset;
    }

    public void ResetNode(){
        rend.material.color = startColor;
        slime = null; //reset all the slime
    }

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
}
