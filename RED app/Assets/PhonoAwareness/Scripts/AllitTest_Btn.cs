using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllitTest_Btn : MonoBehaviour {

	//declare instance of alliteration test script 
	public AlliterationTest_UIManagerScript allit; 
	public GameObject UIManager; 


	void OnMouseDown()
	{
		UIManager = GameObject.Find ("UIManager"); 
		allit = UIManager.GetComponent<AlliterationTest_UIManagerScript>();
		//start running the experiment
		allit.StartAlliterationTest();
	}

}
