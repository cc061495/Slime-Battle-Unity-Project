/* Copyright (c) cc061495 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	/* Singleton */
	public static Shop Instance;
	void Awake(){
		Instance = this;
	}
	// Public Variables
	public RectTransform panel;	//To hold the ScrollPanel
	public GameObject[] slime;
	public Card[] cards;
	public RectTransform center;	//Center to compare the distance for each button
	public Text selectedName;
	public Animator Title;
	public Button purchaseButton;

	// Private Variables
	private bool dragging = false; //Will be true, while we drag the panel
	private int bttnDistance = 450;	//Will hold the distance between the buttons
	private int minButtonNum;	//To hold the number of the button, with smallest distance to center
	private int prevButtonNum = -1;

	void Update(){
		if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.shop){
			if(!dragging){
				LerpToBttn(minButtonNum * -bttnDistance);
			}
		}
	}

	public void Shop_DefaultSetting(){
		SlimeModelDisplay(true);
		SelectWhichButton(minButtonNum, true);
		EnableAllAnimator(true);
	}

	public void SlimeModelDisplay(bool display){
		for(int i=0;i<slime.Length;i++)
			slime[i].SetActive(display);
	}

	private void LerpToBttn(int position){
		float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * 15f);
		Vector2 newPosition = new Vector2 (newX, panel.anchoredPosition.y);

		panel.anchoredPosition = newPosition;
	}

	private void BtnScale(Vector2 scale){
		RectTransform selectedButton = slime[minButtonNum].GetComponent<RectTransform>();
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
		float[] distance = new float[slime.Length];
		float minDistance = float.MaxValue;
		// Find the distance with all buttons between the center
		for (int i = 0; i < slime.Length; i++){
			distance[i] = Mathf.Abs(center.transform.position.x - slime[i].transform.position.x);
		}
		// if the button's distance is larger than 10 with the center => user is scrolling(left or right)
		if(distance[minButtonNum] > 10){
			// if the user is not scrolling the first button to the left and last button to the right
			if(!(minButtonNum == 0 && center.transform.position.x - slime[minButtonNum].transform.position.x < 0) &&
			   !(minButtonNum == slime.Length - 1 && center.transform.position.x - slime[minButtonNum].transform.position.x > 0))
			   // user can scroll to the another button => 2nd nearest button
			   distance[minButtonNum] = 1000;
		}
		// Find the min distance in distance[]
		minDistance = Mathf.Min(distance);
		// Find the min distance index => minButtonNum
		for (int i = 0; i < slime.Length; i++){
			if(minDistance == distance[i])
				minButtonNum = i;
		}

		SelectWhichButton(minButtonNum, true);
	}

	private void SelectWhichButton(int num, bool toggle){
		GameObject childObject = slime[num].transform.GetChild(0).gameObject;
		purchaseButton.interactable = PlayerData.Instance.CheckPlayerCard(minButtonNum);
		//text = Button name
		selectedName.text = childObject.name + "\n$ " + cards[num].coins;		
		// Setting the button interactable, selected button -> true
		if(num != prevButtonNum){
			childObject.SetActive(true);
			childObject.GetComponent<MeshFilter>().mesh = cards[num].mesh;

			if(prevButtonNum > -1){
				slime[prevButtonNum].transform.GetChild(0).gameObject.SetActive(false);
			}
			prevButtonNum = num;
		}
		ButtonAndTitleAnimator(num, toggle);
	}

	private void ButtonAndTitleAnimator(int num, bool toggle){
		slime[num].transform.GetChild(0).GetComponent<Animator>().SetBool("Rotate", toggle);
		Title.SetBool("Fade", toggle);
	}

	public void EnableAllAnimator(bool display){
		slime[prevButtonNum].transform.GetChild(0).GetComponent<Animator>().enabled = display;
		Title.enabled = display;
	}

	public void Buy(){
		int curr_coins = PlayerData.Instance.playerCoins - cards[minButtonNum].coins;
		if(curr_coins >= 0 && PlayerData.Instance.CheckPlayerCard(minButtonNum)){
			PlayerData.Instance.SavePlayerCard(minButtonNum, 1);
			PlayerData.Instance.SavePlayerCoins(curr_coins);
			MenuScreen.Instance.setCoinsText(curr_coins);
			purchaseButton.interactable = PlayerData.Instance.CheckPlayerCard(minButtonNum);
		}
		else{
			// Not enough coins :(
		}
	}
}