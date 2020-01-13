using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlliTest_Option : MonoBehaviour {

	//declare instance of alliteration test script 
	public AlliterationTest_UIManagerScript allit; 
	public GameObject UIManager;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown() {
		UIManager = GameObject.Find ("UIManager"); 
		allit = UIManager.GetComponent<AlliterationTest_UIManagerScript>();

		allit.SelectOption (this.name);
		Debug.Log("MOUSEISDOWN");
	}
}
