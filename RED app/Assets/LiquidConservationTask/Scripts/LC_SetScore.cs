using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LC_SetScore : MonoBehaviour {

	public GameObject SceneManagerGO;
	public LC_SceneManagerScript SceneManagerScript;
	public AudioSource ScoreAudio;
	public float Score;
	//public LC_TapButtonScript TapScript;

	// Use this for initialization
	void Start () {
		ScoreAudio = gameObject.GetComponent<AudioSource> ();
		SceneManagerGO = GameObject.Find ("SceneManagerGO");
		SceneManagerScript = SceneManagerGO.GetComponent<LC_SceneManagerScript> ();
		//TapScript = GameObject.Find ("Tap").GetComponent<LC_TapButtonScript> ();

	}
	
	// Update is called once per frame
	void Update () {

		if (gameObject.activeSelf ) {
			//ScoreAudio.Play ();

			float TargetArea = SceneManagerScript.TargetPourHeight[SceneManagerScript.TrialI] *SceneManagerScript.RightGlass.GetComponent<GlassScript> ().Width;
			float CurrentArea = SceneManagerScript.RightGlass.GetComponent<GlassScript> ().CurrentWaterArea;
			Score = Mathf.Max(10 * (1 - (Mathf.Abs (TargetArea - CurrentArea) / TargetArea)),0f);
			//print (Score);
			//float scoreValue = (10 * (1 - (Mathf.Abs ((TargetPourHeight [TrialI] * RightWaterWidth [TrialI]) - PourAreaTrial) / (TargetPourHeight [TrialI] * RightWaterWidth [TrialI]))));
			//print (string.Concat (new string[] {
		//		"SS TargetAreatoPour:   ",TargetArea.ToString(), "   ",
			//	"SS Pour Area Trial:    ",CurrentArea.ToString(), "   "
//			}));



			//ScoreAudio.pitch = Score/100*5;
			gameObject.GetComponent<Text> ().text = "Score: " + Mathf.Round (Score).ToString ();

		} else {
			ScoreAudio.Stop ();
		}
		
	}
}
