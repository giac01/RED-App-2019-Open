using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumsEnterButton : MonoBehaviour {

	[SerializeField] private SumsSceneController sceneController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// OnMouseDown is called when the button is clicked.
	void OnMouseDown() {

		// Update the sentence.
		sceneController.NextSum();
	}
}
