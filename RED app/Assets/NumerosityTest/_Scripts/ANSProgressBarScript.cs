using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ANSProgressBarScript : MonoBehaviour {

	public GameObject SceneManagerGO;
	public ANS_SceneControllerMainTask SceneManagerScript;
	//public NumberOfTrials = 116;



	// Use this for initialization
	void Start () {
		SceneManagerScript = SceneManagerGO.GetComponent<ANS_SceneControllerMainTask> ();


	}

	// Update is called once per frame
	void Update () {

		gameObject.GetComponent<Slider> ().value = ((float)SceneManagerScript.sidepress.Count)/((float)SceneManagerScript.correctSide.Length);


	}
}
