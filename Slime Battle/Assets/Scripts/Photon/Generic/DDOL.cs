using UnityEngine;

public class DDOL : MonoBehaviour {
	// Use this for initialization
	private void Awake () {
		//GameObject will not be destroyed between the scenes
		DontDestroyOnLoad (this);
	}
}
