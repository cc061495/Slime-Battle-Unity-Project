/* Copyright (c) cc061495 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour {

	public Button shopButton;
	public Image shopButtonImage;

	public Text costText;
	public Image modelImage;
	public Text modelSizeText;
	private Color modelImageColor;

	public void SetupShopButton(int cost, Sprite image, int size){
		costText.text = "$" + cost;
		modelImage.sprite = image;
		modelSizeText.text = SizeConvert(size);

		modelImageColor = modelImage.color;
		ResetShopButton();
	}

	public void ShopButtonPressed(){
		modelImage.enabled = false;
		modelSizeText.enabled = false;
		costText.enabled = true;
	}

	public void ResetShopButton(){
		modelImage.enabled = true;
		modelSizeText.enabled = true;
		costText.enabled = false;
	}

	public void SetShopButtonInteractable(bool display){
		shopButton.interactable = display;
	}

	public void ChangeModelImageAlpha(float alpha){
		modelImageColor.a = alpha;
	}

	public void SetModelImageColor(){
		modelImage.color = modelImageColor;
	}

	public void SetShopButtonColor(Color c){
		shopButtonImage.color = c;
	}

	public Color GetShopButtonColor(){
		return shopButtonImage.color;
	}

	private string SizeConvert(int size){
		if(size == 1)
			return "1x1";
		else if(size == 2)
			return "1x2";
		else if(size == 4)
			return "2x2";
		else
			return "ERROR";
	}
}
