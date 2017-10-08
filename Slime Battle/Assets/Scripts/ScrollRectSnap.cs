/* Copyright (c) cc061495 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour {

	// Public Variables
	public RectTransform panel;	//To hold the ScrollPanel
	public Button[] bttn;
	public RectTransform center;	//Center to compare the distance for each button
	public Image arrow;
	public Text selectedName;

	// Private Variables
	private float[] distance;	//All buttons' distance to the center
	private bool dragging = false; //Will be true, while we drag the panel
	private int bttnDistance;	//Will hold the distance between the buttons
	private int minButtonNum;	//To hold the number of the button, with smallest distance to center

	void Start(){
		int bttnLength = bttn.Length;
		distance = new float[bttnLength];

		//Get distance between buttons
		bttnDistance = (int)Mathf.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.x - bttn[0].GetComponent<RectTransform>().anchoredPosition.x);
		Debug.Log(bttnDistance);
	}

	void Update(){
		for (int i = 0; i < bttn.Length; i++){
			distance[i] = Mathf.Abs(center.transform.position.x - bttn[i].transform.position.x);
		}

		float minDistance = Mathf.Min(distance);	//Get the min distnace

		for (int j = 0; j < bttn.Length; j++){
			if(minDistance == distance[j])
				minButtonNum = j;
		}

		if(!dragging){
			LerpToBttn(minButtonNum * -bttnDistance);
		}
	}

	void LerpToBttn(int position){
		float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * 5f);
		Vector2 newPosition = new Vector2 (newX, panel.anchoredPosition.y);

		panel.anchoredPosition = newPosition;
	}

	public void StartDrag(){
		dragging = true;
		arrow.enabled = false;
		selectedName.enabled = false;
	}

	public void EndDrag(){
		dragging = false;
		arrow.enabled = true;
		selectedName.text = bttn[minButtonNum].name;
		selectedName.enabled = true;
	}
}
