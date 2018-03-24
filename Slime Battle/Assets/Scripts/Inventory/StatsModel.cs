/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StatsModel : MonoBehaviour {

	public MeshFilter slimeModel;

	private bool isTurnLeftButtonPressed = false, isRightLeftButtonPressed = false;
	private float rotationSpeed = 100f;
	private Vector3 turnLeft = new Vector3(0f, 0f, 1f);
	private Vector3 turnRight = new Vector3(0f, 0f, -1f);


	public void SetupTheModelMesh(Mesh m){
		slimeModel.mesh = m;
	}

	public void ResetModelRotation(){
		slimeModel.transform.rotation = Quaternion.Euler (-90f, -180f, 0f);
	}

	// Update is called once per frame
	void Update () {
		if(isTurnLeftButtonPressed)
			slimeModel.transform.Rotate(turnLeft * Time.deltaTime * rotationSpeed);
		else if(isRightLeftButtonPressed)
			slimeModel.transform.Rotate(turnRight * Time.deltaTime * rotationSpeed);
	}

	public void onPointerDownButton(string dir){
		if(dir == "Left")
			isTurnLeftButtonPressed = true;
		else if(dir == "Right")
			isRightLeftButtonPressed = true;

		AudioManager.instance.Play("Tap");
	}

	public void onPointerUpButton(string dir){
		if(dir == "Left")
			isTurnLeftButtonPressed = false;
		else if(dir == "Right")
			isRightLeftButtonPressed = false;
	}
}
