/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour {

	public Image image;
	public AnimationCurve curve;
	private const float animationSpeed = 2f;

	void Start(){
		StartCoroutine(FadeIn());
	}

	public void FadeTo(string scene){
		StartCoroutine(FadeOut(scene));
	}

	public void FadeToWithPhotonNetwork(string scene){
		StartCoroutine(FadeOutWithPhotonNetwork(scene));
	}

	IEnumerator FadeIn(){
		float t = 1f;
		
		while(t > 0){
			t -= Time.deltaTime * animationSpeed;
			//setting t value in the curve X axis
			float a = curve.Evaluate(t);
			image.color = new Color(0f, 0f, 0f, a);
			yield return 0;
		}
	}

	IEnumerator FadeOut(string scene){
		float t = 0f;
		
		while(t < 1f){
			t += Time.deltaTime * animationSpeed;
			//setting t value in the curve X axis
			float a = curve.Evaluate(t);
			image.color = new Color(0f, 0f, 0f, a);
			yield return 0;
		}
		SceneManager.LoadScene(scene);
	}

	IEnumerator FadeOutWithPhotonNetwork(string scene){
		float t = 0f;
		
		while(t < 1f){
			t += Time.deltaTime * animationSpeed;
			//setting t value in the curve X axis
			float a = curve.Evaluate(t);
			image.color = new Color(0f, 0f, 0f, a);
			yield return 0;
		}
        PhotonNetwork.LoadLevel(scene);
	}
}
