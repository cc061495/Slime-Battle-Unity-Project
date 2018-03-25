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
	public Animator Title;

	// Private Variables
	private bool dragging = false; //Will be true, while we drag the panel
	private int bttnDistance = 500;	//Will hold the distance between the buttons
	private int minButtonNum;	//To hold the number of the button, with smallest distance to center
	private int prevButtonNum = -1;

	void Update(){
		if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.home){
			if(!dragging){
				LerpToBttn(minButtonNum * -bttnDistance);
			}
		}
	}

	void Start(){
		SelectWhichButton(0, true);
	}

	private void LerpToBttn(int position){
		float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * 15f);
		Vector2 newPosition = new Vector2 (newX, panel.anchoredPosition.y);

		panel.anchoredPosition = newPosition;
	}

	private void BtnScale(Vector2 scale){
		RectTransform selectedButton = bttn[minButtonNum].GetComponent<RectTransform>();
		selectedButton.sizeDelta = Vector2.Lerp(selectedButton.sizeDelta, scale, Time.deltaTime * 10f);
	}

	public void StartDrag(){
		dragging = true;
		ButtonAndTitleAnimator(minButtonNum, false);
	}

	public void EndDrag(){
		dragging = false;
		FindNearestButton();
	}

	private void FindNearestButton(){
		float[] distance = new float[bttn.Length];
		float minDistance = float.MaxValue;
		// Find the distance with all buttons between the center
		for (int i = 0; i < bttn.Length; i++){
			distance[i] = Mathf.Abs(center.transform.position.x - bttn[i].transform.position.x);
		}
		// if the button's distance is larger than 10 with the center => user is scrolling(left or right)
		if(distance[minButtonNum] > 5){
			// if the user is not scrolling the first button to the left and last button to the right
			if(!(minButtonNum == 0 && center.transform.position.x - bttn[minButtonNum].transform.position.x < 0) &&
			   !(minButtonNum == bttn.Length - 1 && center.transform.position.x - bttn[minButtonNum].transform.position.x > 0))
			   // user can scroll to the another button => 2nd nearest button
			   distance[minButtonNum] = 1000;
		}
		// Find the min distance in distance[]
		minDistance = Mathf.Min(distance);
		// Find the min distance index => minButtonNum
		for (int i = 0; i < bttn.Length; i++){
			if(minDistance == distance[i])
				minButtonNum = i;
		}

		SelectWhichButton(minButtonNum, true);
	}

	private void SelectWhichButton(int num, bool toggle){
		ButtonAndTitleAnimator(num, toggle);
		selectedName.text = bttn[num].name;		//text = Button name
		// Setting the button interactable, selected button -> true
		if(num != prevButtonNum){
			bttn[num].interactable = true;

			if(prevButtonNum > -1)
				bttn[prevButtonNum].interactable = false;

			prevButtonNum = num;
		}
	}

	private void ButtonAndTitleAnimator(int num, bool toggle){
		bttn[num].animator.SetBool("Scale", toggle);
		Title.SetBool("Fade", toggle);
	}

	public void EnableAllAnimator(bool display){
		bttn[minButtonNum].animator.enabled = display;
		Title.enabled = display;
	}
}