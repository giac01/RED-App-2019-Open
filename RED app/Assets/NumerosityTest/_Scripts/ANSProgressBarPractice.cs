using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ANSProgressBarPractice : MonoBehaviour {

	public GameObject SceneManagerGO;
	public ANS_SceneControllerPracticeRounds SceneManagerScript;



	// Use this for initialization
	void Start () {
		SceneManagerScript = SceneManagerGO.GetComponent<ANS_SceneControllerPracticeRounds> ();


	}

	// Update is called once per frame
	void Update () {

		gameObject.GetComponent<Slider> ().value = ((float)SceneManagerScript.sidepress.Count)/(3);


	}
}
