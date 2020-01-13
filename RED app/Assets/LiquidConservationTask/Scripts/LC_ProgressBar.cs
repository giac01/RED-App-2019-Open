using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LC_ProgressBar : MonoBehaviour {

	public GameObject SceneManagerGO; //asigned in gui
	public LC_SceneManagerScript SceneManagerScript;
	public float NTrials;


	// Use this for initialization
	void Start () {
		SceneManagerScript = SceneManagerGO.GetComponent<LC_SceneManagerScript> ();
		NTrials = 38;
	}

	// Update is called once per frame
	void Update () {
		//NTrials = SceneManagerScript.NTrials;

		gameObject.GetComponent<Slider> ().value = ((SceneManagerScript.SceneOrder-1)*10+SceneManagerScript.TrialI)/(NTrials-1);


	}
}
