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
	public Image[] img;
	public Card[] cards;
	public RectTransform center;	//Center to compare the distance for each button
	public Text selectedName;
	public Animator Title;
	public Button purchaseButton;

	// Private Variables
	private bool dragging = false; //Will be true, while we drag the panel
	private int bttnDistance = 500;	//Will hold the distance between the buttons
	private int minButtonNum;	//To hold the number of the button, with smallest distance to center
	private int prevButtonNum = -1;

	void Update(){
		if(MenuScreen.Instance.currentLayout == MenuScreen.Layout.shop){
			if(!dragging){
				LerpToBttn(minButtonNum * -bttnDistance);
			}
		}
	}

	void Start(){
		EnableAllAnimator(false);

		for(int i=0;i<img.Length;i++)
			img[i].sprite = cards[i].icon;
	}

	public void Shop_DefaultSetting(){
		SelectWhichButton(minButtonNum, true);
		EnableAllAnimator(true);
	}

	private void LerpToBttn(int position){
		float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * 15f);
		Vector2 newPosition = new Vector2 (newX, panel.anchoredPosition.y);

		panel.anchoredPosition = newPosition;
	}

	private void BtnScale(Vector2 scale){
		RectTransform selectedButton = img[minButtonNum].GetComponent<RectTransform>();
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
		float[] distance = new float[img.Length];
		float minDistance = float.MaxValue;
		// Find the distance with all buttons between the center
		for (int i = 0; i < img.Length; i++){
			distance[i] = Mathf.Abs(center.transform.position.x - img[i].transform.position.x);
		}
		// if the button's distance is larger than 10 with the center => user is scrolling(left or right)
		if(distance[minButtonNum] > 10){
			// if the user is not scrolling the first button to the left and last button to the right
			if(!(minButtonNum == 0 && center.transform.position.x - img[minButtonNum].transform.position.x < 0) &&
			   !(minButtonNum == img.Length - 1 && center.transform.position.x - img[minButtonNum].transform.position.x > 0))
			   // user can scroll to the another button => 2nd nearest button
			   distance[minButtonNum] = 1000;
		}
		// Find the min distance in distance[]
		minDistance = Mathf.Min(distance);
		// Find the min distance index => minButtonNum
		for (int i = 0; i < img.Length; i++){
			if(minDistance == distance[i])
				minButtonNum = i;
		}

		SelectWhichButton(minButtonNum, true);
	}

	private void SelectWhichButton(int num, bool toggle){
		purchaseButton.interactable = PlayerData.Instance.CheckPlayerCard(minButtonNum);
		ButtonAndTitleAnimator(num, toggle);
		//text = Button name
		selectedName.text = img[num].name + "\n$ " +cards[num].coins;		
		// Setting the button interactable, selected button -> true
		if(num != prevButtonNum){
			Color tmpColor = img[num].color;
			tmpColor.a = 1f;
			img[num].color = tmpColor;

			if(prevButtonNum > -1){
				tmpColor = img[prevButtonNum].color;
				tmpColor.a = 0.4f;
				img[prevButtonNum].color = tmpColor;
			}
			prevButtonNum = num;
		}
	}

	private void ButtonAndTitleAnimator(int num, bool toggle){
		img[num].GetComponent<Animator>().SetBool("Scale", toggle);
		Title.SetBool("Fade", toggle);
	}

	public void EnableAllAnimator(bool display){
		img[minButtonNum].GetComponent<Animator>().enabled = display;
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