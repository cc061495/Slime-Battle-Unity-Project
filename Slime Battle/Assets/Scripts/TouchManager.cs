/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {
	/* Useless staff */
	GameManager gameManager;
	GameObject gObj = null;
	Plane objPlane;
	Vector3 m0;

	void Start(){
		gameManager = GameManager.Instance;
	}

	Ray GenerateMouseRay(Vector3 touchPos){
		Vector3 mousePosFar = new Vector3(touchPos.x, touchPos.y, Camera.main.farClipPlane);
		Vector3 mousePosNear = new Vector3(touchPos.x, touchPos.y, Camera.main.nearClipPlane);
		Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
		Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

		Ray mr = new Ray(mousePosN, mousePosF - mousePosN);
		return mr;
	}

	void Update(){
		if(Input.touchCount > 0 && gameManager.currentState == GameManager.State.build_start){
			if(Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved){
				Ray mouseRay = GenerateMouseRay(Input.GetTouch(0).position);
				RaycastHit hit;

				if(Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit)){
					if(hit.transform.gameObject.tag == "node"){
						gObj = hit.transform.gameObject;
						gObj.GetComponent<Node>().TouchEnter();
					}
					// objPlane = new Plane(Camera.main.transform.forward*-1, gObj.transform.position);

					// Ray mRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
					// float rayDistance;
					// objPlane.Raycast(mRay, out rayDistance);
					// m0 = gObj.transform.position - mRay.GetPoint(rayDistance);
				}
			}
			/*
			else if(Input.GetTouch(0).phase == TouchPhase.Moved){
				Ray mRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				float rayDistance;
				if(objPlane.Raycast(mRay, out rayDistance))
					gObj.transform.position = mRay.GetPoint(rayDistance) + m0;
			}
			*/
			else if(Input.GetTouch(0).phase == TouchPhase.Ended && gObj){
				gObj = null;
			}
		}
	}
}
