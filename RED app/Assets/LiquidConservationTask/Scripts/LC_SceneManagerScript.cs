using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Linq;

public class LC_SceneManagerScript : MonoBehaviour {

	[Header("GameObjects")]
	public GameObject[] GlassList;
	public GameObject AdaptiveGlass;
	public GlassScript AdapativeGlassScript;
	public GameObject CanvasGO;
	public GameObject LeftGlass;
	public GameObject RightGlass;
	public GameObject Tap; //asigned in gui
	public LC_TapButtonScript TapScript;
	public GameObject SceneManagerGO;
	public GameObject Logger;
	public DataLogger DataLogger;
	public GameObject Score;
	public GameObject IntroText1; //Explanatory text on the first few rounds!
	public GameObject IntroText2; //Explanatory text on the first few rounds!
	public GameObject Feedback;

	[Header("Game Logic")]
	public float SceneOrder; //assigned in gui.
	public float GlassFillRate;
	public float xOffset; 
	public float yOffset;
	//public int[] GlassOrder;
	public int NTrials;
	public int TrialI; 
	//public float[] TargetPourHeight;
	public float GlassWaterHeight = (float)344.3;
	//public float[] GlassWaterWidths;
	//public float[] GlassWaterAreas;
	public float DistanceBetweenCups = 60;


	[Header("Game Data")]
	public AudioClip WinSound;
	public float TimeOnFirstClick; //Set by LC_TapButtonScript;
	public float TimeOnMouseUp; //Set by LC_TapButtonScript;
	public float PourAreaTrial; //amount poured by pps on trial
	public float PourHeightTrial; //amount poured by pps on trial
	public List<float> Scores;//saved seperately for feedback!

	public string[] DHeader;
	public float[] LeftWaterWidth;
	public float[] RightWaterWidth;
	public float[] TargetPourHeight;



	// Use this for initialization
	void Start () {
		WinSound = (AudioClip)Resources.Load ("Audio/win_freesound", typeof(AudioClip));
		Score = GameObject.Find ("Score");
		Score.SetActive (false);
		AdapativeGlassScript = AdaptiveGlass.GetComponent<GlassScript> ();
		IntroText1 = GameObject.Find ("IntroText1");
		IntroText2 = GameObject.Find ("IntroText2");
		IntroText1.SetActive (false);
		IntroText2.SetActive (false);
	
		SceneManagerGO = GameObject.Find ("SceneManagerGO");
		CanvasGO = GameObject.Find ("Canvas");
		Feedback = CanvasGO.transform.Find ("EndGameFeedback").gameObject;
		Feedback.SetActive (false);
		GlassList = Resources.LoadAll<GameObject> ("GlassPrefabs") as GameObject[];
		//GlassOrder = new int[] {0, 0, 1, 1, 0, 0, 0, 1, 1, 0 , 0, 1};

	
		NTrials = LeftWaterWidth.Length;

		TapScript = Tap.GetComponent<LC_TapButtonScript> ();

		Logger = GameObject.Find ("Logger");
		DataLogger = Logger.GetComponent<DataLogger> ();
		DHeader= new string[] {	"TrialN", "TrialStartTime", "PourStartRT", 
								"PourEndRT", "LeftGlassWidth", "LeftGlassHeight", 
								"RightGlassWidth", "RightGlassHeight", "PourTargetHeight",
								"PourTargetArea", "PouredHeight","PouredArea",
								"Score"
							  };
		DataLogger.LogHeader (DHeader);
		//print (GlassList [1]);





		StartCoroutine("TrialLoop" );
		
	}


	void CalculateScore(float[] inputscores){

	}

	// Update is called once per frame
	public IEnumerator TrialLoop() 
	{

		for (TrialI = 0; TrialI < NTrials; TrialI++) {
			Tap.SetActive (true);
			GlassFillRate = 40000;
			float TrialStartTime = Time.time;
			TapScript.AllowFill = true;

			//Instantiate Objects

			LeftGlass = Instantiate (AdaptiveGlass);
			LeftGlass.transform.SetParent (CanvasGO.transform);
			TapScript.LeftGlass = SceneManagerGO.GetComponent<LC_SceneManagerScript> ().LeftGlass;
			LeftGlass.GetComponent<GlassScript>().SetGlassArea(LeftWaterWidth[TrialI]*GlassWaterHeight);
			LeftGlass.GetComponent<Transform> ().localPosition = new Vector3 (-(DistanceBetweenCups/2)-LeftGlass.GetComponent<GlassScript>().GlassWidth/2, -yOffset, 0);

			RightGlass = Instantiate (AdaptiveGlass);
			RightGlass.transform.SetParent (CanvasGO.transform);
			RightGlass.GetComponent<GlassScript>().SetGlassArea(RightWaterWidth[TrialI]*GlassWaterHeight);
			RightGlass.GetComponent<Transform> ().localPosition = new Vector3 (+(DistanceBetweenCups/2)+RightGlass.GetComponent<GlassScript>().GlassWidth/2, -yOffset, 0);

			//Set target market on right glass 
			RightGlass.GetComponent<GlassScript> ().SetMarkerFunction (TargetPourHeight[TrialI]*RightGlass.GetComponent<GlassScript>().Width);



			if (TrialI < 2 && SceneOrder==1) {
				//print ("test run audio");
				IntroText1.SetActive (true);
				IntroText1.GetComponent<AudioSource> ().Play ();
			}

			//Wait for participant to start and finish pouring the left glass
		
			yield return new WaitUntil (() => TapScript.EndPour==true);
			yield return new WaitForSecondsRealtime (.05f);
			Tap.SetActive (false);
			PourAreaTrial = (float)LeftGlass.GetComponent<GlassScript> ().CurrentWaterArea;   //Area of water poured?
			PourHeightTrial = (float)LeftGlass.GetComponent<GlassScript> ().CurrentWaterHeight; //Height of water poured (on left glass)?
			float targetAreaToPour = TargetPourHeight [TrialI] * RightWaterWidth [TrialI];


			if (TrialI < 2 && SceneOrder==1) {
				IntroText1.SetActive (false);
				IntroText1.GetComponent<AudioSource> ().Stop ();
				IntroText2.SetActive (true);
				IntroText2.GetComponent<AudioSource> ().Play ();

			}

			TapScript.AllowFill = false;
			Score.SetActive (true);

			GlassFillRate = 80000;
			StartCoroutine(LeftGlass.GetComponent<GlassScript>().UnPour(0));
			//float fillArea = LeftGlass.GetComponent<GlassScript> ().CurrentWaterArea;
			StartCoroutine(RightGlass.GetComponent<GlassScript>().Pour(PourAreaTrial));


			//Wait untill both glasses have stopped pouring/unpouring
			//yield return new WaitUntil (() => LeftGlass.GetComponent<GlassScript>().CurrentWaterArea ==0 &&
			//								RightGlass.GetComponent<GlassScript>().Water.GetComponent<Rigidbody2D>().velocity.y == 0);

			//Wait untill Right GlassList is filled
			yield return new WaitUntil (() => RightGlass.GetComponent<GlassScript>().Water.GetComponent<Rigidbody2D>().velocity.y == 0);
			LeftGlass.GetComponent<GlassScript> ().StopPour ();
			RightGlass.GetComponent<GlassScript>().SetWaterFillToArea(PourAreaTrial);



			float scoreValue = (10 * (1 - (Mathf.Abs (targetAreaToPour - PourAreaTrial) / targetAreaToPour)));

			//if a high score is achieved play "win" sound then wait, otherwise just wait. 
			if (!(TrialI < 2 && SceneOrder == 1) && Mathf.Round(scoreValue) == 10) 
			{
				gameObject.GetComponent<AudioSource> ().PlayOneShot (WinSound);
				yield return new WaitUntil (() => gameObject.GetComponent<AudioSource> ().isPlaying == false);
				yield return new WaitForSecondsRealtime (.4f);
			} else {
				yield return new WaitForSecondsRealtime (1f);
			}
			yield return new WaitUntil (() => IntroText2.GetComponent<AudioSource>().isPlaying==false);


			//Output Data

			float PourStartRT = TimeOnFirstClick - TrialStartTime;
			float PourEndRT = TimeOnMouseUp - TrialStartTime;
			DataLogger.LogData ( new string[]{
				
					(TrialI+1).ToString(),
					TrialStartTime.ToString(),
					PourStartRT.ToString(),

					PourEndRT.ToString(),
					LeftGlass.GetComponent<GlassScript>().Width.ToString(),
					LeftGlass.GetComponent<GlassScript>().Height.ToString(),
					
					RightGlass.GetComponent<GlassScript>().Width.ToString(),
					RightGlass.GetComponent<GlassScript>().Height.ToString(),
					TargetPourHeight[TrialI].ToString(),

					(RightWaterWidth[TrialI]*TargetPourHeight[TrialI]).ToString(),
					PourHeightTrial.ToString(),
					PourAreaTrial.ToString(),

					scoreValue.ToString()

			});

			//Destroy GameObjects for end of task
			//print (string.Concat (new string[]{ "Score:    ", Score.GetComponent<LC_SetScore> ().Score.ToString () }));

			Destroy (LeftGlass);
			Destroy (RightGlass);
			Score.SetActive (false);
			TapScript.EndPour = false;


			if (TrialI < 2 && SceneOrder==1) {
				IntroText2.SetActive (false);
			}

//			print (string.Concat (new string[] {
//				"SM TargetAreatoPour:   ",targetAreaToPour.ToString(), "   ",
//				"SM Poured left Area Trial:    ",PourAreaTrial.ToString(), "   ",
//				//"SM Right Glass Water Area end:     ", (RightGlass.GetComponent<GlassScript>().Width*RightGlass.GetComponent<GlassScript>().CurrentWaterHeight).ToString(),
//				"SM Right Glass currentWaterArea:     ", (RightGlass.GetComponent<GlassScript>().CurrentWaterArea).ToString()
//
//			}));

		}


		DataLogger.Close ();
		if (SceneOrder<4){

			SceneManager.LoadScene(string.Concat(new string[]{"LC_MainTask",(SceneOrder+1).ToString()}));

		}
		if (SceneOrder == 4) {
			UnityEngine.SceneManagement.SceneManager.LoadScene ("InBetweenScene");
		}
		//Feedback.SetActive (true);
		//Feedback.transform.Find ("ScoreText").gameObject.GetComponent<Text> ().text = "Your Average Score= " + (Mathf.Round(Scores.Average()*10)/10).ToString();
	


		
	}
}
