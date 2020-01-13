using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VocabProgressBarScript : MonoBehaviour {

	public GameObject SceneManagerGO;
	public Vocab_SceneControllerMainTask SceneManagerScript;



	// Use this for initialization
	void Start () {
		SceneManagerScript = SceneManagerGO.GetComponent<Vocab_SceneControllerMainTask> ();


	}

	// Update is called once per frame
	void Update () {

		gameObject.GetComponent<Slider> ().value = ((float)SceneManagerScript.sidepress.Count)/((float)SceneManagerScript.WordToMatch.Length);


	}
}
