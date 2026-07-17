using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualsHider : MonoBehaviour {

	// Use this for initialization
	public void Hide() {
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	public void Unhide () {
		gameObject.SetActive(true);
	}
}
