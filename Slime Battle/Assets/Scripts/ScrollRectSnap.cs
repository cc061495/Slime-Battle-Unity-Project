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
		float distance, minDistance = float.MaxValue;

		for (int i = 0; i < bttn.Length; i++){
			distance = Mathf.Abs(center.transform.position.x - bttn[i].transform.position.x);
			if(distance < minDistance){
				minDistance = distance;
				minButtonNum = i;
			}
		}
		SelectWhichButton(minButtonNum, true);
	}

	private void SelectWhichButton(int num, bool toggle){
		ButtonAndTitleAnimator(num, toggle);
		selectedName.text = bttn[num].name;		//text = Button name
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
