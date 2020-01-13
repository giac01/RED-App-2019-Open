//DEPRECIATED CODE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScene_ContButton : MonoBehaviour {
	public GameObject MainCamera;
	public Button PlayButton;




	// Use this for initialization
	void Start () {
		PlayButton = gameObject.GetComponent<Button>();
		PlayButton.onClick.AddListener(TaskOnClick);
	}

	// Update is called once per frame
	void TaskOnClick () {
		print ("click");
	
	}

}

