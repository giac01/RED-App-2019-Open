using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Video;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems; 

public class RDK_short : MonoBehaviour {


	// Values for the experiment
	public int nTrials_single = 5; // number of trials
	public int nTrials_multiple = 5; 
	public float coherenceGain = 0.025f; // proportion to reduce the coherence by each trial
	public float speedGain = 0f; //change in speed each trial
	public float numberChange = 0f; //change in no of dots each trial
	public float lifeChange = 0f; //change in dot life time each trial
	public float waitTime = 0f; //time to wait before showing response button
	public float rampFrames = 120f; // This the length of a trial, and the number of frames we change the noise level over

	public int target = 30; //target frame rate 

	//values for the normal distribution noise function
	public float meanDis = 30f; //mean of the normal distribution, this is half of the total frames shown
	public float stdDis = 10f; //the width of the curve


	//objects for the experiment
	//holders for objects and components
	public GameObject DragObject; //the object for response
	public Transform DragTransform; //the transform for dragger
	public LineRenderer DragLine; // the component containing the line
	public CollisionTracker TrackerScript; // collision detector script attatched to the drag_object
	public GameObject LeftObject; //the left target
	public GameObject RightObject; //the right target
	public GameObject instructions; // GameObject for instructions
	public GameObject instructions_1; 
	public GameObject instructions_2; 
	public GameObject instructions_3; 
	public GameObject instructions_4; 
	public GameObject instructions_5;
	public GameObject instructions_6; 
	public GameObject slowdown; 
	public GameObject speedup; 
	public GameObject video; 
	public GameObject video_1; 
	public GameObject buttonOb;
	public Button buttonB; 
	public GameObject snowman; 

	//button

	public GameObject Play; 
	public Button PlayBut; 

	//logic flags and trackersVideo
	public bool touchTracker; 
	public float lastCoh; 
	public bool gameStarted = false; // flag for if the game has fully started or not
	public bool DraginNDrawin = true; // this flag tells us if the button is dragable and it is drawing
	public bool responded = false; //have they made a response for this trial
	public bool draggin = false; // are they dragging the DragObject
	public bool hascollided; //
	public bool showingTrial = false;
	public int introStage = 0; 
	public bool button_pressed = false; 
	public int slowCnt = 0;
	public int spedCnt = 0; 
	public bool giveFeedB = false; 
	public bool levelOff = false; 
		//metric containers
	public float initRT; //time taken to initiate response
	public float decRT; //time taken to make decision
	public float accuracy; //accuracy of decision
	public List<float> Ys; //points along the Y axis
	public List<float> Xs; //points along the X axis
		//calculator containers
	public float trialStart;
	public float respStart;

	//initial values for RDK
	public int nDots = 100; //number of dots on screen
	public int nSets = 1; // number of refreshes of dots
	public int direction = 0; // angle of coherent dots
	public float coherence = 0f; // percentage of dots actually moving along trajectory
	public float dotLife = 5f; // how long in seconds the dot exists before being redrawn on a random place
	public float moveDistance = 0.025f; // how fast a dot moves
	public float apertureWidth = 3.5f; //size of view window
	public string dotColour = "white"; //colour of dots
	public float xloc = 0f; //x location of view window in screen
	public float yloc = 3f; // y location of view window n screenVideo
	public float apertureCenterX = 0f;
	public float apertureCenterY = 3f;
	public float frameSpeed = 0.01f;

	public int reinsertType = 2;

	//initialise but give no values (these will be constantly set in update)
	public float coherentJumpSizeX;
	public float coherentJumpSizeY;

	public float nCoherentDots;
	public float nIncoherentDots;

	public List<List<dot>> dotArray2d;
	public List<dot> dotArray;

	public int currentSet;

	public bool stopDotMotion;
	public bool moveSnowman = false; 

	public float horizontalAxis;
	public float verticalAxis;

	public float currentSpeed; 
	public float lastLineNos; //for calculating speed

	public float signedCoherence; 

	public List<int> single_trial_list; 
	public List<int> multi_trial_list_0; 
	public List<int> multi_trial_list_1; 
	public List<int> multi_trial_list_2; 

	//Data logger
	public GameObject DL_object; 

	public DataLogger Logger_1; 

	public EvidJSON EvJson; 
	public EvidVecJSON EvVJSON; 

	public List<int> JS_Trial;
	public List<int> JS_Type;
	public List<string> JS_Dirs;
	public List<int> JS_Resp;
	public List<int> JS_Accu;
	public List<float> JS_RT;
	public List<float> JS_initRT;

	public List<string> JS_Xvect;
	public List<string> JS_Yvect;
	public List<string> JS_CohVect;

	public List<string> JS_leftprop; 
	public List<string> JS_rightprop; 

	public string vectJSON;
	public string outJSON; 

	public int corrDir; 

//	//Load external JS function for communicating with browsers
//	[DllImport("__Internal")]
//	public static extern void AddDataToFirebase(string inputJSONRaw);
//
//	[DllImport("__Internal")]
//	public static extern void Hello();

	//"Trial", "Type", "Dirs", "XVect", "YVect", "CohVect","Resp", "Accu", "RT", "initRT"

	//Logged vars 
	public int Type; 
	public List<float> XVect;
	public List<float> YVect;
	public List<float> CohVect;
	public List<float> LeftVect;
	public List<float> RightVect;
	  
	public int Resp; 
	public List<int> Dirs; 

//	public Texture2D cursorTexture; 
//	public CursorMode cursorMode = CursorMode.Auto; 
//	public Vector2 hotSpot = Vector2.zero;

	void Awake () {

	
		// set target framerate!N; 

		QualitySettings.vSyncCount = 0;  // VSync must be disabled
		Application.targetFrameRate = target;

		//Get gameobjects and components
		DragObject = GameObject.Find("drag_object");
		DragTransform = DragObject.GetComponent<Transform>();
		DragLine = DragObject.GetComponent<LineRenderer>();
		TrackerScript = DragObject.GetComponent<CollisionTracker>();
		LeftObject = GameObject.Find ("left");
		RightObject = GameObject.Find ("right");
		instructions = GameObject.Find ("instructions");
		instructions_1 = GameObject.Find ("instructions_1");
		instructions_2 = GameObject.Find ("instructions_2");
		instructions_3 = GameObject.Find ("instructions_3");
		instructions_4 = GameObject.Find ("instructions_4");
		instructions_5 = GameObject.Find ("instructions_5");
		instructions_6 = GameObject.Find ("instructions_6");
		slowdown = GameObject.Find ("SlowDown");
		speedup = GameObject.Find ("KeepMoving"); 
		slowdown.SetActive (false);
		speedup.SetActive (false); 
		instructions_1.SetActive (false); 
		instructions_2.SetActive (false); 
		instructions_3.SetActive(false);
		instructions_4.SetActive(false);
		instructions_5.SetActive (false);
		instructions_6.SetActive (false);

		DragObject.SetActive (false);

		snowman = GameObject.Find ("snowman"); 


		video = GameObject.Find ("Video"); 
		video.SetActive (false); 
		video_1 = GameObject.Find ("Video_1"); 
		video_1.SetActive (false); 
		buttonOb = GameObject.Find ("Button");
		buttonB = buttonOb.GetComponent<Button> (); 
		buttonB.onClick.AddListener (delegate {
			buttonClick();
		}); 
		buttonOb.SetActive (false); 

		Play = GameObject.Find ("Play"); 
		PlayBut = Play.GetComponent<Button> (); 
		PlayBut.onClick.AddListener (delegate {
			if (Time.time >14f){
				StartCoroutine(collisionHandler(1));
				Play.SetActive(false);
				button_pressed = true; 
			}
		}); 

		//set trial lists: generated in https://repl.it/@AlexanderIrvine/AlertImpressiveList 

		single_trial_list = new List<int> (){ 0, 180, 180, 0, 180, 0};


		//list 0 refers to direction
		//list 1 refers to type of trial (same or switch)
		multi_trial_list_0 = new List<int>(){0, 180, 180, 0, 180, 0};
		multi_trial_list_1 = new List<int>(){0, 1, 1, 0, 0, 1};



		XVect = new List<float> (){ }; 
		YVect = new List<float> (){ }; 
		CohVect = new List<float> (){ }; 



		//Calculate the x and y jump sizes for coherent dots
		coherentJumpSizeX = calculateCoherentJumpSizeX(direction);
		coherentJumpSizeY = calculateCoherentJumpSizeY(direction);

		horizontalAxis = apertureWidth / 2f;
		verticalAxis = apertureWidth / 2f;

		nCoherentDots = nDots * coherence;
		nIncoherentDots = nDots - nCoherentDots;

		//make dot arrayN; 
		dotArray2d = makeDotArray2d();

		//current set at 0
		currentSet = 0;

		//stop is false
		stopDotMotion = false;

		//load in present dotArray
		dotArray = dotArray2d[currentSet]; //Global variable, so the draw function also uses this array

		draw ();

		//start coroutine for animating

		StartCoroutine (updateSpeed ()); 

	}


	// Update is called once per frame
	void Update (){


		//Debug.Log (touchTracker); 

		//try and keep at target framerate 
		if (Application.targetFrameRate != target) {
			Application.targetFrameRate = target; 
		}

		// check for timeouts 
		if (showingTrial == true && gameStarted == true) {
			if (respStart > 0f) {
				float thisTime = Time.fixedTime;
				Debug.Log (thisTime - respStart); 
				if (thisTime - respStart > 10f) {

					var input = EventSystem.current.GetComponent<StandaloneInputModule>();
					input.DeactivateModule();

					//StartCoroutine (collisionHandler (999));
					showingTrial = false;
					responded = true; 

					giveFeedB = false;
					Resp = 999; //for logging
					Debug.Log ("responded");
					//decide if correct or nor

					accuracy = float.NaN;

					decRT = respStart - Time.fixedTime; //record response time
					responded = true;

					//get rid of trial and reset position
					DraginNDrawin = false; //prevent the DragObject from being dragged
					DragTransform.position = new Vector3 (0.02f, -5f, 0); //reset DragObject's position
					DragTransform.transform.rotation = Quaternion.Euler (0f, 0f, 0f);
					DragLine.SetVertexCount (0); //delete the line that has been drawn
					DragObject.SetActive (false); //hide Drag Object
					giveFeedB = true; 
				}
			}
		}



		//calculate current values ( these vars may have been manipulated since initial

		horizontalAxis = apertureWidth / 2f;
		verticalAxis = apertureWidth / 2f;

		nCoherentDots = nDots * coherence;
		nIncoherentDots = nDots - nCoherentDots;

		//check for collisions!N; 
		TrackerScript = DragObject.GetComponent<CollisionTracker>();

		if (TrackerScript.isLeftChoice == true) {
			StartCoroutine(collisionHandler (180));
            TrackerScript.isLeftChoice = false;
		}

		if (TrackerScript.isRightChoice == true) {
			StartCoroutine(collisionHandler (0));
            TrackerScript.isRightChoice = false;
		}

		//information update
		//Count number of dots
		var Dots = GameObject.FindGameObjectsWithTag("clone");
		int len = Dots.Length;


		//find info object
		GameObject info = GameObject.Find("info");

		//info.GetComponent<TextMesh> ().text = " Dots on Display: " + len.ToString () + " Coherence: " + coherence.ToString () + " Direction: " + direction.ToString ();


	}

	//Main Game logic
	public IEnumerator MainLoop() {



		#if UNITY_WEBGL && !UNITY_EDITOR

		Application.ExternalCall ("Hello"); 

		#else

		// something else to emulate what you want to do

		#endif


		// Data Logger stuff
		//Get datalogger object 
		DL_object = GameObject.Find ("DataLogger"); 
		Logger_1 = DL_object.GetComponent<DataLogger> ();
		//Write header to files 
		//header string 
		string[] header = {"Trial", "Type", "Dirs", "XVect", "YVect", "CohVect","LeftVect", "RightVect","Resp", "Accu", "RT", "initRT"};
		Logger_1.LogHeader (header);

		//JSON stuff 
		GameObject temp = GameObject.Find("EvidVecJSON"); 
		EvVJSON = temp.GetComponent<EvidVecJSON> ();
		temp = GameObject.Find ("EvidJSON"); 
	
		EvJson = temp.GetComponent<EvidJSON> (); 

		yield return new WaitForSecondsRealtime (0.2f); 

		clearDots (); 

		//do single trials first 
		for (int i = 0; i < nTrials_single; i++) {

			//clear lists for last trial!
			XVect = new List<float> (){ }; 
			YVect = new List<float> (){ }; 
			CohVect = new List<float> (){ };
			LeftVect = new List<float> (){ }; 
			RightVect = new List<float> (){ }; 
			Dirs = new List<int> (){ };

			//Get the direction for this trial 
			direction = single_trial_list[i]; 

			corrDir = direction; 

			//reset some stuff that may be left over fromz previous trials
			draggin = false; // reset draggin flag for this trial
			responded = false;
			coherence = 0f;
			giveFeedB = false; 

			dotArray2d = makeDotArray2d (); //make a new array based on current settings

			stopDotMotion = false;

			yield return new WaitForSecondsRealtime (waitTime); //wait for designated response delay

            hascollided = false; // do this to prevent collisionhandler sending multiple responses
			DragObject.SetActive (true); // show the response button
			DraginNDrawin = true; //allow user to drag the response button	public List<int> JS_Trial;
            Debug.Log("Snowman lives!");
;

			trialStart = Time.fixedTime; //record begining of response window
            Debug.Log("Trial start time: " + trialStart.ToString());


			//wait for beginning of drag - this is set to true by the DragNDraw script when an object is moved
            Debug.Log("Waiting for dragging to start...");
			yield return new WaitUntil (() => draggin == true);
            Debug.Log("Dragging has started!");
			giveFeedB = true; 

			showingTrial = true;

            Debug.Log("Starting RDK stuff...");
			draw(); //draw the RDK
			StartCoroutine (animate ()); //animate the RDK we just made


			//start the single trial thing
			StartCoroutine (SingleDirecTrial ());
//			StartCoroutine (SingleMultiTrial (multi_trial_list_0[i], multi_trial_list_1[i], multi_trial_list_2[i])); 
            Debug.Log("RDK stuff has started!");

			//
			respStart = Time.fixedTime; //begining of decision window
            initRT = Time.fixedTime - trialStart; //initial RT



            //wait for response
            Debug.Log("Waiting for a response...");
            yield return new WaitUntil (() => responded == true);
            Debug.Log("Response has been made!");
			//reset last coherence 
			lastCoh = 0f; 
			float trialRT = respStart - Time.fixedTime; 

			showingTrial = false;
			stopDotMotion = true; //stop the animation

			yield return new WaitForSeconds (0.1f); //neccesary to give a few ms for the animation to stop

            Debug.Log("Clearing the dots...");
			clearDots (); //stop showing the RDK
            Debug.Log("Dots cleared!");

            //increment the difficulty up
			coherence = coherence + coherenceGain;




			//			//UNCOMMENT IF YOU WANT RANDOM change the direction randomlypublic 
//			Random.seed = System.DateTime.Now.Millisecond;
//
//			if (Random.value < .5) {
//				direction = 0;
//			} else {
//				direction = 180;
//			}
			yield return new WaitForSeconds (0.1f); //wait before continuing to allow computations
			hascollided = false; // do this to prevent collisionhandler sending multiple responses

			yield return new WaitForSeconds (0.1f);


			//"Trial", "Type", "Dirs", "XVect", "YVect", "CohVect","Resp", "Accu", "RT", "initRT"

			Dirs = new List<int> (){direction}; 

			int type = 0; 



			//add to lists for JSON 
//			public List<int> JS_Trial;
//			public List<int> JS_Type;
//			public List<int> JS_Dirs;
//			public List<int> JS_Resp;
//			public List<int> JS_Accu;
//			public List<float> JS_RT;
//			public List<float> JS_initRT;
//
//			public List<string> JS_Xvect;
//			public List<string> JS_Yvect;
//			public List<string> JS_CohVect;

			JS_Trial.Add(i); 
			JS_Type.Add(type);
			JS_Dirs.Add (string.Join (" ", Dirs.Select (n => n.ToString ()).ToArray ()));
			JS_Resp.Add (Resp); 
		
			JS_Accu.Add ((int)accuracy); 
			JS_RT.Add (trialRT);
			JS_initRT.Add (initRT); 

			JS_Xvect.Add (string.Join (" ", XVect.Select (n => n.ToString ()).ToArray ()));
			JS_Yvect.Add (string.Join (" ", YVect.Select (n => n.ToString ()).ToArray ())); 
			JS_CohVect.Add (string.Join (" ", CohVect.Select (n => n.ToString ()).ToArray ())); 

			JS_leftprop.Add(string.Join (" ", LeftVect.Select (n => n.ToString ()).ToArray ())); 
			JS_rightprop.Add(string.Join (" ", RightVect.Select (n => n.ToString ()).ToArray ())); 


			//{"Trial", "Type", "Dirs", "XVect", "YVect", "CohVect","LeftVect", "RightVect","Resp", "Accu", "RT", "initRT"};
			//record responses in log file 
			string[] results = {
				i.ToString(),
				type.ToString(),
				string.Join(" ", Dirs.Select(n => n.ToString()).ToArray()),
				string.Join(" ", XVect.Select(n => n.ToString()).ToArray()),
				string.Join(" ", YVect.Select(n => n.ToString()).ToArray()),
				string.Join(" ", CohVect.Select(n => n.ToString()).ToArray()),
				string.Join (" ", LeftVect.Select (n => n.ToString ()).ToArray ()), 
				string.Join (" ", RightVect.Select (n => n.ToString ()).ToArray ()),
				Resp.ToString(), 
				accuracy.ToString(), 
				trialRT.ToString(),
				initRT.ToString()

			};

			Logger_1.LogData (results);

		}

		//Now it's time to do multiple directions 

		// Firstly we are going to display a second set of instructions 
		button_pressed = false; 
		instructions_5.SetActive(true); 

		Play.SetActive (true); 

		giveFeedB = false; 

		yield return new WaitUntil (() => button_pressed == true); 

		instructions_5.SetActive (false); 
		buttonOb.SetActive (false); 
		Play.SetActive (false); 

		StartCoroutine (playThenHide()); 


		//wait until the video has finished playing 
		yield return new WaitUntil (() => introStage == 7); 

		button_pressed = false; 

		buttonOb.SetActive (true);
		instructions_6.SetActive (true); 

		yield return new WaitUntil (() => button_pressed == true); 

		buttonOb.SetActive (false);
		instructions_6.SetActive (false); 

		// now make it faster (increase frame speed by 2x); 
		rampFrames = rampFrames/2; 

		for (int i = 0; i < nTrials_multiple; i++) {

			giveFeedB = false; 

			//clear lists for last trial!
			XVect = new List<float> (){ }; 
			YVect = new List<float> (){ }; 
			CohVect = new List<float> (){ }; 
			Dirs = new List<int> (){ };

			LeftVect = new List<float> (){ }; 
			RightVect = new List<float> (){ }; 

			//reset some stuff that may be left over fromz previous trials
			draggin = false; // reset draggin flag for this trial
			responded = false;
			coherence = 0f;
			giveFeedB = false; 

			dotArray2d = makeDotArray2d (); //make a new array based on current settings

			stopDotMotion = false;

			yield return new WaitForSecondsRealtime (waitTime); //wait for designated response delay

			hascollided = false; // do this to prevent collisionhandler sending multiple responses

			DragObject.SetActive (true); // show the response button
			DraginNDrawin = true; //allow user to drag the response button

			trialStart = Time.fixedTime; //record begining of response window


			//wait for beginning of drag - this is set to true by the DragNDraw script when an object is moved
			yield return new WaitUntil (() => draggin == true);
			giveFeedB = true; 
			showingTrial = true;

			draw(); //draw the RDK
			StartCoroutine (animate ()); //animate the RDK we just made

			//start the single trial thing
			//			StartCoroutine (SingleDirecTrial ());
			//StartCoroutine (SingleMultiTrial (multi_trial_list_0[i], multi_trial_list_1[i], multi_trial_list_2[i])); 
			if (multi_trial_list_1 [i] == 0) {
				corrDir = multi_trial_list_0 [i]; 
			} else {
				if (multi_trial_list_0 [i] == 0) { corrDir = 180;} else {corrDir = 0;}
			}
			StartCoroutine (SingleMultiTrial (multi_trial_list_0 [i], multi_trial_list_1[i]));
			//
			respStart = Time.fixedTime; //begining of decision window
			initRT =  Time.fixedTime - trialStart; //initial RT



			//wait for response
			yield return new WaitUntil (() => responded == true);

			float trialRT = Time.fixedTime - respStart;

			showingTrial = false;
			stopDotMotion = true; //stop the animation

			yield return new WaitForSeconds (0.1f); //neccesary to give a few ms for the animation to stop

			clearDots (); //stop showing the RDKLog

			//increment the difficulty up
			coherence = coherence + coherenceGain;




			//			//UNCOMMENT IF YOU WANT RANDOM change the direction randomly
			//			Random.seed = System.DateTime.Now.Millisecond;
			//
			//			if (Random.value < .5) {
			//				direction = 0;
			//			} else {
			//				direction = 180;
			//			}
			yield return new WaitForSeconds (0.1f); //wait before continuing to allow computations
			hascollided = false; // do this to prevent collisionhandler sending multiple responses

			yield return new WaitForSeconds (0.1f);

			//"Trial", "Type", "Dirs", "XVect", "YVect", "CohVect","Resp", "Accu", "RT", "initRT"

			Dirs = new List<int> (){multi_trial_list_0[i], multi_trial_list_1[i]}; 

			int type = 1; 


			JS_Trial.Add(i); 
			JS_Type.Add(type);
			JS_Dirs.Add (string.Join (" ", Dirs.Select (n => n.ToString ()).ToArray ()));
			JS_Resp.Add (Resp); 

			JS_Accu.Add ((int)accuracy); 
			JS_RT.Add (trialRT);
			JS_initRT.Add (initRT); 

			JS_Xvect.Add (string.Join (" ", XVect.Select (n => n.ToString ()).ToArray ()));
			JS_Yvect.Add (string.Join (" ", YVect.Select (n => n.ToString ()).ToArray ())); 
			JS_CohVect.Add (string.Join (" ", CohVect.Select (n => n.ToString ()).ToArray ())); 

			JS_leftprop.Add(string.Join (" ", LeftVect.Select (n => n.ToString ()).ToArray ())); 
			JS_rightprop.Add(string.Join (" ", RightVect.Select (n => n.ToString ()).ToArray ())); 

			//record responses in log file 
			string[] results = {
				i.ToString(),
				type.ToString(),
				string.Join(" ", Dirs.Select(n => n.ToString()).ToArray()),
				string.Join(" ", XVect.Select(n => n.ToString()).ToArray()),
				string.Join(" ", YVect.Select(n => n.ToString()).ToArray()),
				string.Join(" ", CohVect.Select(n => n.ToString()).ToArray()),
				string.Join (" ", LeftVect.Select (n => n.ToString ()).ToArray ()), 
				string.Join (" ", RightVect.Select (n => n.ToString ()).ToArray ()),
				Resp.ToString(), 
				accuracy.ToString(), 
				trialRT.ToString(),
				initRT.ToString()

			};
			Logger_1.LogData (results);

			Debug.Log (type);
			Debug.Log("left"); 
			Debug.Log (LeftVect);
			Debug.Log("right");
			Debug.Log (RightVect); 

		}

		//save log file.
		Logger_1.Close(); 

		//JSON stuff 

		//add to lists for JSON 
		//			public List<int> JS_Trial;
		//			public List<int> JS_Type;
		//			public List<int> JS_Dirs;
		//			public List<int> JS_Resp;
		//			public List<int> JS_Accu;
		//			public List<float> JS_RT;
		//			public List<float> JS_initRT;
		//
		//			public List<string> JS_Xvect;
		//			public List<string> JS_Yvect;
		//			public List<string> JS_CohVect;

		EvJson.Trial = string.Join (",", JS_Trial.Select (n => n.ToString ()).ToArray ());
		EvJson.Type = string.Join (",", JS_Type.Select (n => n.ToString ()).ToArray ());

		EvJson.Dirs = string.Join (",", JS_Dirs.Select (n => n.ToString ()).ToArray ());
		EvJson.Resp = string.Join (",", JS_Resp.Select (n => n.ToString ()).ToArray ());
		EvJson.Accu = string.Join (",", JS_Accu.Select (n => n.ToString ()).ToArray ());
		EvJson.RT= string.Join (",", JS_RT.Select (n => n.ToString ()).ToArray ());
		EvJson.initRT = string.Join (",", JS_initRT.Select (n => n.ToString ()).ToArray ());


//		EvVJSON.Xvect = string.Join (",", JS_Xvect.Select (n => n.ToString ()).ToArray ());
//		EvVJSON.Yvect = string.Join (",", JS_Yvect.Select (n => n.ToString ()).ToArray ());
//		EvVJSON.CohVect = string.Join (",", JS_CohVect.Select (n => n.ToString ()).ToArray ());

		EvVJSON.Xvect = JS_Xvect;
		EvVJSON.Yvect = JS_Yvect;
		EvVJSON.CohVect = JS_CohVect;

		vectJSON = JsonUtility.ToJson (EvVJSON); 
		outJSON = JsonUtility.ToJson (EvJson); 



		Application.ExternalCall("AddDataToFirebase", vectJSON, "vects");
		Application.ExternalCall("AddDataToFirebase", outJSON, "trials");


		Debug.Log (outJSON); 
		//reload scene
		SceneManager.LoadScene("InBetweenScene");
		Application.ExternalCall ("EndGame"); 
	}

	//function for a trial when we see one direction only 
	public IEnumerator SingleDirecTrial (){

		//conditional if this is a practice trial 
		if (introStage == 2 || introStage == 3 || introStage == 4) {
			giveFeedB = false; 

			Debug.Log("introStage_Direct_trial"); 
			//clear previous locations

			//wait for beginning of drag - this is set to true by the DragNDraw script when an object is moved


			yield return new WaitForSecondsRealtime (0.5f); 

			clearDots(); 

			stopDotMotion = false; 
			showingTrial = true; 

			draggin = false; //make this false 
			DraginNDrawin = true; //reset the bool so we can move the target
			hascollided = false; //reset collission tracker



			yield return new WaitUntil (() => draggin == true);
			clearDots(); 

			giveFeedB = true; 

			//hide instructions 
			instructions_1.SetActive(false); 
			instructions_2.SetActive (false);
			instructions_3.SetActive (false); 
			stopDotMotion = false; 
			showingTrial = true; 

			draggin = false; //make this false 
			DraginNDrawin = true; //reset the bool so we can move the target
			hascollided = false; //reset collission tracker
			clearDots (); 

			dotArray2d = makeDotArray2d (); //make a new array based on current settings
			draw (); //draw the RDK

			StartCoroutine (animate ()); //animate the RDK
		}

		//Calculate the x and y jump sizes for coherent dots
		coherentJumpSizeX = calculateCoherentJumpSizeX(direction);
		coherentJumpSizeY = calculateCoherentJumpSizeY(direction);


		// loop through the ramp frames!

		for (int i = 0; i < rampFrames; i++) {

			if (showingTrial == false) {break;} // dont execute if we are not showing 

			float one_over_2pi = (float)(1.0 / (stdDis * Mathf.Sqrt (2 * Mathf.PI)));
			float varDis = stdDis * stdDis;

			//over the trial we want to change the noise level acording to a normal distribution
			//We input the current frame as X and they Y we get out is the %of coherence
			float thisNoise = F (i, one_over_2pi, meanDis, stdDis, varDis);
			//thisNoise = CumDensity (i); 

			//wait for the given frame time
			yield return new WaitForSecondsRealtime (frameSpeed);

			//update coherence public variable
			coherence = thisNoise * 30;

			//update dot array to match this value as close as permitted (i.e. we cant have half a dot).

			//load dot array
			dotArray = dotArray2d [currentSet];

			//count number of constant direction dots
			float NoCoherent = dotArray.Count (p => p.updateType == "constant direction");

			//compare this to the current coherence level, and tell us how many dots need to be changed
			//desired N coherent dots
			float desiredN = coherence * nDots;

			float toChange = desiredN - NoCoherent;
			toChange = Mathf.Round (toChange);

			//add stuff
			//current coherence adjusted for direction 

			if (direction == 0) {
				signedCoherence = coherence;
				RightVect.Add (1f);
				LeftVect.Add (0f); 
			} else {
				signedCoherence = coherence * -1f; 
				RightVect.Add (0f);
				LeftVect.Add (1f);
			}
			CohVect.Add (signedCoherence);
			//X Y object positions 
			XVect.Add( DragTransform.position.x);
			YVect.Add (DragTransform.position.y); 



			//loop through the dots individually IF we have dots to change
			//while (toChange != 0f) {
			for (int ii = 0; ii < nDots; ii++) {

				//load the current dotdem
				dot thisDot = dotArray [ii];

				//if negative we need to change x constant to random
				if (toChange < 0f) {
					if (thisDot.updateType == "constant direction") {
						//change to random
						thisDot.updateType = "random direction";
						setvx2vy2 (thisDot); // Set dot.vx and dot.vy
						//we have one less dot to change
						toChange = toChange + 1f;

					} else if (thisDot.updateType == "random direction") {
						//do nothing
					}
				}

				//if postive we need to change x random to positive!!
				//if negative we need to change x constant to random
				if (toChange > 0f) {
					if (thisDot.updateType == "constant direction") {
						//do nothing
					} else if (thisDot.updateType == "random direction") {
						//change to constant
						thisDot.updateType = "constant direction";
						setvxvy (thisDot); // Set dot.vx and dot.vy for this dot
						//we have one less dot to change
						toChange = toChange - 1f;
						Debug.Log (toChange);
						Debug.Log (ii);
						Debug.Log ("updated to constant");
					}
				}
					
				if (toChange == 0f) {
					break;
				}


			}
				
				//}

		}
		if (introStage == 2 || introStage == 3 || introStage == 4 || instructions_4.active) {
		} else {
			bool breakFlag = false; 
			//record responses after the ramp frames has finished
			while (!breakFlag && !responded) {
				yield return new WaitForSecondsRealtime (frameSpeed);

				if (direction == 0) {
					signedCoherence = coherence;
					RightVect.Add (1f);
					LeftVect.Add (0f); 
				} else {
					signedCoherence = coherence * -1f; 
					RightVect.Add (0f);
					LeftVect.Add (1f);
				}
				CohVect.Add (signedCoherence);
				//X Y object positions 
				XVect.Add (DragTransform.position.x);
				YVect.Add (DragTransform.position.y); 
				Debug.Log ("added responses"); 
				if (responded == true) {
					Debug.Log ("breaking");
					breakFlag = true; 
					break; 
				}
			}
		}

	}


//	//function for a trial where we see multiple directions 
	public IEnumerator SingleMultiTrial (int dir1, int type){

		// loop through the ramp frames over multiple directions 

		//Two types: 
			// 1)overall average in one direction - but evidence is less in time 2 
			// 2)overall average weight changes - 

		//Both have a ramping up of noise, but not a ramping down 
		//The transition between directions is gradual - but in a linear manner. 


		//Direction Number one ! 
		// start with 70% going one way (coherent jump size in 'direction'), 30% going other (coherent jump size in opposite direction)


		direction = dir1; 

		levelOff = false; 

		float Prop = 0f; 
		//Step one 

		for (int i = 0; i < rampFrames; i++) {

			if (showingTrial == false) {break;} // dont execute if we are not showing 

			float one_over_2pi = (float)(1.0 / (stdDis * Mathf.Sqrt (2 * Mathf.PI)));
			float varDis = stdDis * stdDis;

			//over the trial we want to change the noise level acording to a normal distribution
			//We input the current frame as X and they Y we get out is the %of coherence
			float thisNoise = F (i, one_over_2pi, meanDis, stdDis, varDis);

			float maxLevel = F (meanDis, one_over_2pi, meanDis, stdDis, varDis); 

			//if we have ramped up, maintain that coherence level! 
			if (thisNoise == maxLevel) {
				levelOff = true; 
			}
			if (levelOff == true){ thisNoise = maxLevel; }

			//Debug.Log (thisNoise); 
			//Debug.Log (meanDis); 
			//wait for the given frame time
			yield return new WaitForSecondsRealtime (frameSpeed);

			//update coherence public variable
			coherence = thisNoise * 30;

			//update dot array to match this value as close as permitted (i.e. we cant have half a dot).

			//load dot array
			dotArray = dotArray2d [currentSet];

			//count number of constant direction dots
			float NoCoherent = dotArray.Count (p => p.updateType == "constant direction");

			//compare this to the current coherence level, and tell us how many dots need to be changed
			//desired N coherent dots
			float desiredN = coherence * nDots;

			float toChange = desiredN - NoCoherent;
			toChange = Mathf.Round (toChange);


			//add stuff
			//current coherence adjusted for direction 


			if (direction == 0) {
				signedCoherence = coherence; 
			} else {
				signedCoherence = coherence * -1f; 
			}
			CohVect.Add (signedCoherence);
			//X Y object positions 
			XVect.Add( DragTransform.position.x);
			YVect.Add (DragTransform.position.y); 

			//loop through the dots individually IF we have dots to change
			//while (toChange != 0f) {

			for (int ii = 0; ii < nDots; ii++) {

				//load the current dotdem
				dot thisDot = dotArray [ii];


				//if negative we need to change x constant to random
				if (toChange < 0f) {
					if (thisDot.updateType == "constant direction") {
						//change to random
						thisDot.updateType = "random direction";
						setvx2vy2 (thisDot); // Set dot.vx and dot.vy
						//we have one less dot to change
						toChange = toChange + 1f;

					} else if (thisDot.updateType == "random direction") {
						//do nothing
					}
				}

				//if postive we need to change x random to positive!!
				//if negative we need to change x constant to random
				if (toChange > 0f) {
					if (thisDot.updateType == "constant direction") {
						//do nothing
					} else if (thisDot.updateType == "random direction") {
						//change to constant
						thisDot.updateType = "constant direction";
						setvxvy (thisDot); // Set dot.vx and dot.vy for this dot
						//we have one less dot to change
						toChange = toChange - 1f;
//						Debug.Log (toChange);
//						Debug.Log (ii);
//						Debug.Log ("updated to constant");
					}
				}
					
				if (toChange == 0f) {
					break;
				}
					
			}

			//update direction proportions for the constant dots 
			//proportion for this type of trial 
			Prop = 0.7f; 

			//a list of directions for all our dots 
			List<int> dirArray = new List<int>(); 

			int antidirection; 

			if (direction == 0){ 
				antidirection = 180;
				RightVect.Add (Prop); 
				LeftVect.Add (1 - Prop); 
			} else { 
				antidirection = 0;
				RightVect.Add (1- Prop); 
				LeftVect.Add (Prop); 
			};   

			//we need to only loop through the coherent dots 
			//add our direction! 

			//count the number of coherent dots
			int noDotsConst = 0; 
			for (int ii = 0; ii < nDots; ii++) { 
				if (dotArray [ii].updateType == "constant direction") {
					noDotsConst = noDotsConst + 1;
				}
			}

			for (int ii = 0; ii < noDotsConst * Prop; ii++) {

				if (dotArray [ii].updateType == "constant direction") {
					dirArray.Add (direction); 
				}
			}
			for (int ii = 0; ii < noDotsConst * (1 - Prop); ii++) {
				if (dotArray [ii].updateType == "constant direction") {
					dirArray.Add (antidirection); 
				}
			}
				
			int tmpCounter = 0; 

			//change the array to represent this proportion
			for (int ii = 0; ii < nDots; ii++) {
				dot thisDot = dotArray [ii]; 
				if (thisDot.updateType == "constant direction") {
					coherentJumpSizeX = calculateCoherentJumpSizeX (dirArray[tmpCounter]);
					coherentJumpSizeY = calculateCoherentJumpSizeY (dirArray[tmpCounter]); 
					setvxvy (thisDot); 
					tmpCounter = tmpCounter + 1; 
				}

			}

			Debug.Log("Dot info"); 
			for (int ii = 0; ii < nDots; ii ++){
				Debug.Log (dotArray2d [0] [ii].vx);
			}
			Debug.Log ("end Dot info");



			//}

		}

//		//step two 
		// we change to the second ratio

		//depending on trial type this is a switch in total evidence, or a maintenance of total evidence 
//		direction = dir2; 
//

		// target % of coherent dots in direction
		float targPerc = 0f; 

		// target % change on coherence  
		float targCohCha = 0f; 

		//value for incremental direction change 
		float targDirCha = 0f; 

		int antidirection2;
		if (direction == 0){ antidirection2 = 180;} else { antidirection2 = 0;}; 

		//we need to decide values 
		//same direction, change ratio 
		if (type == 0) { 
			targCohCha = -0.3f; // but make it noiser 
			targPerc = 1f; // all dots moving in one at the end 


		} else if (type == 1) {
			targCohCha = 0; //no change in coherence 
			targPerc = 0f; // All dots will be moving in new direction 
		}


		// we need to transition using the above values over ramp frame 
		float loopCohInc = targCohCha / rampFrames; //each frame how much do we change coherence by?
		float loopPercInc = (targPerc - Prop)/ rampFrames; //subtract current proportion from target then devide 


		//calculate all of this to give us the first initial coherence value 
		float one_over_2pi_2 = (float)(1.0 / (stdDis * Mathf.Sqrt (2 * Mathf.PI)));
		float varDis_2 = stdDis * stdDis;

		//the max of the previous stage's normal distribution 
		float maxLevel_2 = F (meanDis, one_over_2pi_2, meanDis, stdDis, varDis_2);
		float thisNoise_2 = maxLevel_2;
		coherence = thisNoise_2 * 30;

		// this time we are not ramping from 0 noise, so we will not need the ramping function 

		for (int i = 0; i < rampFrames; i++) {

			if (showingTrial == false) {break;} // dont execute if we are not showing 

			//modulate coherence up or down by frame perceptage increment 
			coherence = coherence + loopCohInc; 


			//wait for the given frame time
			yield return new WaitForSecondsRealtime (frameSpeed);

			//update coherence public variable


			//update dot array to match this value as close as permitted (i.e. we cant have half a dot).

			//load dot array
			dotArray = dotArray2d [currentSet];

			//count number of constant direction dots
			float NoCoherent = dotArray.Count (p => p.updateType == "constant direction");

			//compare this to the current coherence level, and tell us how many dots need to be changed
			//desired N coherent dots
			float desiredN = coherence * nDots;

			float toChange = desiredN - NoCoherent;
			toChange = Mathf.Round (toChange);


			//add stuff
			//current coherence adjusted for direction 


			if (direction == 0) {
				signedCoherence = coherence; 
			} else {
				signedCoherence = coherence * -1f; 
			}

			CohVect.Add (signedCoherence);
			//X Y object positions 
			XVect.Add( DragTransform.position.x);
			YVect.Add (DragTransform.position.y); 

			//loop through the dots individually IF we have dots to change
			//while (toChange != 0f) {

			for (int ii = 0; ii < nDots; ii++) {

				//load the current dotdem
				dot thisDot = dotArray [ii];


				//if negative we need to change x constant to random
				if (toChange < 0f) {
					if (thisDot.updateType == "constant direction") {
						//change to random
						thisDot.updateType = "random direction";
						setvx2vy2 (thisDot); // Set dot.vx and dot.vy
						//we have one less dot to change
						toChange = toChange + 1f;

					} else if (thisDot.updateType == "random direction") {
						//do nothing
					}
				}

				//if postive we need to change x random to positive!!
				//if negative we need to change x constant to random
				if (toChange > 0f) {
					if (thisDot.updateType == "constant direction") {
						//do nothing
					} else if (thisDot.updateType == "random direction") {
						//change to constant
						thisDot.updateType = "constant direction";
						setvxvy (thisDot); // Set dot.vx and dot.vy for this dot
						//we have one less dot to change
						toChange = toChange - 1f;
						//						Debug.Log (toChange);
						//						Debug.Log (ii);
						//						Debug.Log ("updated to constant");
					}
				}

				if (toChange == 0f) {
					break;
				}

			}

			Debug.Log("prop");
			Debug.Log (Prop); 
			//change the Prop value by our increment 
			Prop = Prop + loopPercInc; 
			Debug.Log (Prop); 
			Debug.Log (loopPercInc); 

			//a list of directions for all our dots 
			List<int> dirArray = new List<int>(); 

			int antidirection; 

			if (direction == 0){ 
				antidirection = 180;
				LeftVect.Add (1-Prop); 
				RightVect.Add (Prop);
			} else { 
				antidirection = 0;
				LeftVect.Add (Prop); 
				RightVect.Add (1-Prop);
			};   
				
			//we need to only loop through the coherent dots 
			//add our direction! 

			//count the number of coherent dots
			int noDotsConst = 0; 
			for (int ii = 0; ii < nDots; ii++) { 
				if (dotArray [ii].updateType == "constant direction") {
					noDotsConst++;
				}
			}

			Debug.Log (noDotsConst); 
			Debug.Log (Mathf.Round (noDotsConst * Prop));
			Debug.Log (Mathf.Round(noDotsConst *(1-Prop)));
			//we need to round proportion to fit with the number of dots, this means 

			for (int ii = 0; ii < Mathf.CeilToInt(noDotsConst * Prop); ii++) {
				dirArray.Add (direction); 
			}
			for (int ii = 0; ii < Mathf.CeilToInt(noDotsConst * (1 - Prop)); ii++) {
				dirArray.Add (antidirection); 
			}



			Debug.Log (dirArray.Count); 
			Debug.Log (dirArray [dirArray.Count-1]); 
				
			int tmpCounter = 0; 

			//change the array to represent this proportion
			for (int ii = 0; ii < nDots; ii++) {
				dot thisDot = dotArray [ii]; 
				if (dotArray[ii].updateType == "constant direction") {
					coherentJumpSizeX = calculateCoherentJumpSizeX (dirArray[tmpCounter]);
					coherentJumpSizeY = calculateCoherentJumpSizeY (dirArray[tmpCounter]); 
					setvxvy (thisDot); 
					tmpCounter = tmpCounter + 1; 
				}

			}

			Debug.Log("Dot info"); 
			for (int ii = 0; ii < nDots; ii ++){
				Debug.Log (dotArray2d [0] [ii].vx);
			}
			Debug.Log ("end Dot info");



			//}

		}


		if (introStage == 2 || introStage == 3 || introStage == 4 || instructions_4.active) {
		} else {
			bool breakFlag = false; 
			//record responses after the ramp frames has finished
			while (!breakFlag && !responded) {
				yield return new WaitForSecondsRealtime (frameSpeed);

				if (direction == 0) {
					signedCoherence = coherence;
					RightVect.Add (1f);
					LeftVect.Add (0f); 
				} else {
					signedCoherence = coherence * -1f; 
					RightVect.Add (0f);
					LeftVect.Add (1f);
				}
				CohVect.Add (signedCoherence);
				//X Y object positions 
				XVect.Add (DragTransform.position.x);
				YVect.Add (DragTransform.position.y); 
				Debug.Log ("added responses"); 
				if (responded == true) {
					Debug.Log ("breaking");
					breakFlag = true; 
					break; 
				}
			}
		}
	}

	public IEnumerator practiceTrial(){

		//If we are doing practice number 1 
		if (introStage == 1) {
			//wait for beginning of drag - this is set to true by the DragNDraw script when an object is moved
			yield return new WaitForSecondsRealtime (0.5f); 
			draggin = false; //make this false 
			DraginNDrawin = true; //reset the bool so we can move the target
			hascollided = false; //reset collission tracker

			yield return new WaitUntil (() => draggin == true);

			dotArray2d = makeDotArray2d (); //make a new array based on current settings
			draw (); //draw the RDK
			StartCoroutine (animate ());
		} if (introStage == 2) {
			yield return new WaitForSecondsRealtime (0.2f); 
			clearDots (); 

			draggin = false; //make this false 
			DraginNDrawin = true; //reset the bool so we can move the target
			hascollided = false; //reset collission tracker
		}

	}

	public IEnumerator prevideo(){
		yield return new WaitForSecondsRealtime (0.2f); 
		clearDots(); 
		stopDotMotion = false; 
		showingTrial = true; 
		draggin = false; //make this false 
		DraginNDrawin = true; //reset the bool so we can move the target
		hascollided = false; //reset collission tracker

		#if UNITY_WEBGL && !UNITY_EDITOR
			VideoPlayer videocomp = video_1.GetComponent<VideoPlayer>(); 
			//videocomp.VideoSource.url = "~/demo_2.mov"; 
			videocomp.url = "~/demo_2.mov"; 
			
			VideoPlayer videocomp2 = video.GetComponent<VideoPlayer>(); 
			//videocomp2.VideoSource.url = "~/demo_1.mov"; 
			videocomp2.url = "~/demo_1.mov"; 
		#else
			
		#endif

	}

	public IEnumerator playThenHide(){

		if (button_pressed == true) {
			video_1.SetActive (true);
			VideoPlayer videoComp = video_1.GetComponent<VideoPlayer>(); 		//get player component 
			yield return new WaitForSecondsRealtime(0.2f); //wait a bit to give time for the video to start playing
			yield return new WaitUntil (() => videoComp.isPlaying == false); //wait until the video is not playing 
			video_1.SetActive (false); //hide the gameObject of the video
			Debug.Log("video ended"); 
			introStage = 7; 
		} else {
			video.SetActive (true); //show the video, and automatically it plays
			VideoPlayer videoComp = video.GetComponent<VideoPlayer>(); 		//get player component 
			yield return new WaitForSecondsRealtime(0.2f); //wait a bit to give time for the video to start playing
			yield return new WaitUntil (() => videoComp.isPlaying == false); //wait until the video is not playing 
			video.SetActive (false); //hide the gameObject of the video
			Debug.Log("video ended"); 
			StartCoroutine(collisionHandler (1)); //call collision handler to move on to the next stage 
		}
	}

	//collision handler
	public IEnumerator collisionHandler(int respDir){
		var input = EventSystem.current.GetComponent<StandaloneInputModule>();
		input.DeactivateModule();

		if (hascollided == false) {
			hascollided = true;
            Debug.Log("gameStarted is " + gameStarted.ToString());
			if (gameStarted == false) { // if we are still showing instructions

				Debug.Log ("early collision"); 
				DraginNDrawin = false; //prevent the DragObject from being dragged

				DragTransform.position = new Vector3 (0.02f, -5f, 0); //reset DragObject's position
				DragTransform.transform.rotation = Quaternion.Euler(0f,0f,0f);
				DragLine.SetVertexCount (0); //delete the line that has been drawn

				//instruction stage 0 
				if (introStage == 0) {
					instructions.SetActive (false); //hide instructions
					introStage +=1;
					hascollided = false;
					StartCoroutine (playThenHide ());
					Play.SetActive (false); 
					snowman.SetActive (false); 
					DragObject.SetActive (true); 
//					instructions_1.SetActive (true);//show more instructions
//					clearDots (); //clear the old dots
//					introStage += 1; //move introduction stage tracker up one 
//					coherence = 0.6f; //set coherence to a high level for this example trual
//					StartCoroutine (practiceTrial ()); //start practice trial coroutine

				} else if (introStage == 1) {
					instructions_1.SetActive (true);//show more instructions
					clearDots (); //clear the old dots
					introStage += 1; //move introduction stage tracker up one 
					coherence = 0.6f; //set coherence to a high level for this example trual
					StartCoroutine (SingleDirecTrial ()); //start practice trial coroutine



				} else if (introStage == 2) {
					lastCoh = 0f; 
					giveFeedB = false; 
					showingTrial = false;
					stopDotMotion = true; 

					instructions_2.SetActive (true);
					instructions_1.SetActive (false); 



					introStage += 1; //move introduction stage tracker up one 
					coherence = 0.6f; //set coherence to a high level for this example trial

					StartCoroutine (SingleDirecTrial ()); //start practice trial coroutine


//
//					showingTrial = false; //reset flag that we set to true in last instruction stage. 
//					stopDotMotion = true; 
//
//					instructions_2.SetActive (false); 
//					instructions_3.SetActive (true);  
//
//					StartCoroutine (prevideo ()); 
//					introStage += 1; 
				} else if (introStage == 3) {
					lastCoh = 0f; 

					giveFeedB = false; 

					instructions_3.SetActive (true); 

					giveFeedB = false; 
					showingTrial = false;
					stopDotMotion = true;

					introStage += 1; //move introduction stage tracker up one 
					coherence = 0.6f; //set coherence to a high level for this example trial

					StartCoroutine (SingleDirecTrial ()); //start practice trial coroutine
//					instructions_3.SetActive (false); 
//					introStage += 1; 
//					hascollided = false;
//					StartCoroutine (playThenHide ());

				
				} else if (introStage == 4) {
					lastCoh = 0f; 
					yield return new WaitForSecondsRealtime (0.5f);
					draggin = false; //make this false 
					DraginNDrawin = true; //reset the bool so we can move the target
					hascollided = false; //reset collission tracker
					giveFeedB = false; 
					stopDotMotion = true; 
					clearDots(); 
					Debug.Log ("intro stage 4"); 
					instructions_4.SetActive (true);
					introStage += 1;


					
				} else if (introStage == 5) { 
					lastCoh = 0f; 
					instructions_4.SetActive (false); // hide instructions
					gameStarted = true;
					DragObject.SetActive (false); //hide Drag Object
					StartCoroutine (MainLoop ()); //start the main experiment
				}; 



			} else {
				giveFeedB = false;
				Resp = respDir; //for logging
				Debug.Log ("responded");
				//decide if correct or nor
				if (corrDir == respDir) {
					accuracy = 1;
				} else {
					accuracy = 0;
				}
				decRT = respStart - Time.fixedTime; //record response time
				responded = true;

				//get rid of trial and reset position
				DraginNDrawin = false; //prevent the DragObject from being dragged
				DragTransform.position = new Vector3 (0.02f, -5f, 0); //reset DragObject's position
				DragTransform.transform.rotation = Quaternion.Euler(0f,0f,0f);
				DragLine.SetVertexCount (0); //delete the line that has been drawn
				DragObject.SetActive (false); //hide Drag Object
				giveFeedB = true; 
			}
		}

	}

	//BELOW WE CREATE ALL FUNCTIONS AND CLASSES NEEDED

	// coroutine for animating

	public IEnumerator animate(){


		while (stopDotMotion == false) {
			updateDots ();
			yield return new WaitForSecondsRealtime (frameSpeed);
			draw ();
		}
	}

	// Use this for initialization, we will use instatuation of a bunch of game objects
	// These are circle sprites
	public void draw(){
		System.DateTime timestart = System.DateTime.Now;

		//clear previous locations
		var oldDots = GameObject.FindGameObjectsWithTag("clone");
		foreach(GameObject item in oldDots){
			Destroy(item);
		}

		//Loop through the dots one by one and draw them
		for (int i = 0; i < nDots; i++) {
			dot thisDot = dotArray[i];
			GameObject prefab_dot = GameObject.Find ("prefab_dot");
			Instantiate (prefab_dot, new Vector3 (thisDot.x, thisDot.y, 0), Quaternion.identity).tag ="clone";

		}
		System.DateTime timeafter = System.DateTime.Now;
		System.TimeSpan duration = timeafter.Subtract (timestart);

		//Debug.Log (duration.Milliseconds);
	}

	public void updateDots(){

		//cycle through the next set
		if (currentSet == nSets - 1) {
			currentSet = 0;

		} else {
			currentSet += 1;
		}

		//load current dot set
		dotArray = dotArray2d[currentSet];

		//loop through the dots individually and update them accordingly
		for (int i = 0; i < nDots; i++) {
			//load the current dotdem
			dot thisDot = dotArray[i];

			// move it randomly or in a direction depending on it's label
			if (thisDot.updateType == "constant direction") {
				thisDot = constantDirectionUpdate (thisDot);
			} else if (thisDot.updateType == "random direction") {
				thisDot = randomDirectionUpdate (thisDot);
			}

			//Increment it's life
			thisDot.lifecount += 1f;

			//check if life has ended for this poor dot?
			if (lifeEnded (thisDot)) {
				thisDot = resetLocation (thisDot); //reset location if dead
			}

			//check if the dot is escapeing out the aperture!
			if (outOfBounds(thisDot)) {
				switch (reinsertType) {
				case 1:
					thisDot = resetLocation(thisDot);
					break;
				case 2:
					thisDot = reinsertOnOppositeEdge(thisDot);
					break;
				} //End of switch statement
			} //End of if


		}
	}

	//Calculate coherent jump size in the x direction
	public float calculateCoherentJumpSizeX(int coherentDir) {
		float angleInRadians = coherentDir * Mathf.PI / 180f;
		return moveDistance * Mathf.Cos (angleInRadians);
	}

	//Calculate coherent jump size in the x direction
	public float calculateCoherentJumpSizeY(int coherentDir) {
		float angleInRadians = coherentDir * Mathf.PI / 180f;
		return moveDistance * Mathf.Sin (angleInRadians);
	}



	// normal distribution function returns a y given x
	// Our X is time in a given trial, and our Y is the coherence value of the Random Dot kinematagram

	public float F(float x, float one_over_2pi, float mean,
		float stddev, float var)
	{	
		float curr = one_over_2pi * Mathf.Exp (-(x - mean) * (x - mean) / (2 * var));
		if (curr < lastCoh){
			return lastCoh; 
		} else {
			lastCoh = curr; 
			return curr; 
		}
	}



	//class for dots
	public class dot
	{
		public float x;
		public float y;
		public float vx;
		public float vy;
		public float vx2;
		public float vy2;
		public float latestXMove;
		public float latestYMove;
		public float lifecount;
		public string updateType;
	}


	//make the 2d array, which is an array of array of dots
	public List<List<dot>> makeDotArray2d(){

		List<List<dot>> tempArray2 = new List<List<dot>> ();

		for (int i = 0; i < nSets; i++) {

			tempArray2.Add (makeDotArray());
		}

		return tempArray2;
	}




	//make dot array
	public List<dot> makeDotArray(){

		//a list to contain information for all our dots
		List<dot> tempArray = new List<dot>();

		//now loop through and instantiate the dots we need
		for (int i = 0; i < nDots; i++) {

			//create a dot object
			dot tempDot = new dot();

			//randomly set the location of our dot
			resetLocation(tempDot);

			tempDot.lifecount = Mathf.Floor(Random.Range(0f, dotLife));

			//For the same && random direction RDK type
			//For coherent dots
			if (i < nCoherentDots) {
				setvxvy(tempDot); // Set dot.vx and dot.vy
				tempDot.updateType = "constant direction";
			}
			//For incoherent dots
			else {
				setvx2vy2(tempDot); // Set dot.vx2 and dot.vy2
				tempDot.updateType = "random direction";
			}

			tempArray.Add (tempDot);
	     }
		return tempArray;
	}



	public dot setvxvy(dot inDot) {
		inDot.vx = coherentJumpSizeX;
		inDot.vy = coherentJumpSizeY;
		return inDot;
	}

	//Set the vx2 and vy2 based on a random angle
	public dot setvx2vy2(dot inDot) {
		//Generate a random angle of movement
		float theta = Random.Range(-Mathf.PI, Mathf.PI);
		//Update properties vx2 and vy2 with the alternate directions
		inDot.vx2 = Mathf.Cos(theta) * moveDistance;
		inDot.vy2 = -Mathf.Sin(theta) * moveDistance;
		return inDot;
	}

	//Updates the x and y coordinates by moving it in the x and y coherent directions
	public dot constantDirectionUpdate(dot inDot) {
		inDot.x += inDot.vx;
		inDot.y += inDot.vy;
		inDot.latestXMove = inDot.vx;
		inDot.latestYMove = inDot.vy;
		return inDot;
	}

	public dot randomDirectionUpdate (dot inDot){

		inDot.x += inDot.vx2;
		inDot.y += inDot.vy2;
		inDot.latestXMove = inDot.vx2;
		inDot.latestYMove = inDot.vy2;
		return inDot;

	}

	//Function to check if dot life has ended
	public bool lifeEnded(dot inDot) {
		//If we want infinite dot life
		if (dotLife < 0f) {
			inDot.lifecount = 0f; //resetting to zero to save memory. Otherwise it might increment to huge numbers.
			return false;
		}
		//Else if the dot's life has reached its end
		else if (inDot.lifecount >= dotLife) {
			inDot.lifecount = 0f;
			return true;
		}
		//Else the dot's life has not reached its end
		else {
			return false;
		}
	}

	//Function to check if dot is out of bounds
	public bool outOfBounds(dot inDot) {
		//For circle and ellipse
		if (inDot.x < xValueNegative(inDot.y) || inDot.x > xValuePositive(inDot.y) || inDot.y < yValueNegative(inDot.x) || inDot.y > yValuePositive(inDot.x)) {
			return true;
		} else {
			return false;
		}

	}

	public dot reinsertOnOppositeEdge(dot inDot){

		//Bring the dot back into the aperture by moving back one step
		inDot.x -= inDot.latestXMove;
		inDot.y -= inDot.latestYMove;

		//Move the dot to the position relative to the origin to be reflected about the origin
		inDot.x -= apertureCenterX;
		inDot.y -= apertureCenterY;

		//Reflect the dot about the origin
		inDot.x = -inDot.x;
		inDot.y = -inDot.y;

		//Move the dot back to the center of the screen
		inDot.x += apertureCenterX;
		inDot.y += apertureCenterY;

		return inDot;
	}

	//Calculating different coordinate values from edges
	public float yValuePositive(float x) {
		x = x - (apertureCenterX); //Bring it back to the (0,0) center to calculate accurately (ignore the y-coordinate because it is not necessary for calculation)
		return verticalAxis * Mathf.Sqrt(1f - (Mathf.Pow(x, 2f) / Mathf.Pow(horizontalAxis, 2f))) + apertureCenterY; //Calculated the positive y value and added height/2 to recenter it on the screen

	}

	//Calculate the NEGATIVE y value of a point on the edge of the ellipse given an x-value
	public float yValueNegative(float x) {
		x = x - (apertureCenterX); //Bring it back to the (0,0) center to calculate accurately (ignore the y-coordinate because it is not necessary for calculation)
		return -verticalAxis * Mathf.Sqrt(1f - (Mathf.Pow(x, 2f) / Mathf.Pow(horizontalAxis, 2f))) + apertureCenterY; //Calculated the negative y value and added height/2 to recenter it on the screen
	}


	public float xValuePositive(float y) {
		y = y - (apertureCenterY); //Bring it back to the (0,0) center to calculate accurately (ignore the x-coordinate because it is not necessary for calculation)
		return horizontalAxis * Mathf.Sqrt(1f - (Mathf.Pow(y, 2f) / Mathf.Pow(verticalAxis, 2f))) + apertureCenterX; //Calculated the positive x value and added width/2 to recenter it on the screen
	}

	//Calculate the NEGATIVE x value of a point on the edge of the ellipse given a y-value
	public float xValueNegative(float y) {
		y = y - (apertureCenterY); //Bring it back to the (0,0) center to calculate accurately (ignore the x-coordinate because it is not necessary for calculation)
		return -horizontalAxis * Mathf.Sqrt(1f - (Mathf.Pow(y, 2f) / Mathf.Pow(verticalAxis, 2f))) + apertureCenterX; //Calculated the negative x value and added width/2 to recenter it on the screen
	}

	//reset locations (only for circular apertures
	public dot resetLocation(dot inDot){

		float phi = Random.Range(-Mathf.PI, Mathf.PI);
		float rho = Random.value;

		float x = Mathf.Sqrt (rho) * Mathf.Cos (phi);
		float y = Mathf.Sqrt (rho) * Mathf.Sin (phi);

		//might have to change this all to work in unity coordinate system

		// problem is unity coordinate system works on a -x:+x, -y:+y system. 0,0 is in the center of the screen
		// Javascript works with 0,0 at the top left of the screen
		// so this generates movement in a concentric way (not what we want).

		x = x * horizontalAxis + xloc;
		y = y * verticalAxis + yloc;

		inDot.x = x;
		inDot.y = y;

		return inDot;
	}

	public void buttonClick(){
		button_pressed = true; 
	}

	//Update speed calculates the distance moved in one second, it's bassically coords/seconds 
	// We probably want this to be higher than around 0.1, but lower that around 1
	public IEnumerator updateSpeed(){

		bool loop4eva = true; 

		while (loop4eva == true) {


			yield return new WaitForSecondsRealtime (0.2f); 
			//get Line Renderer commponent of drag object
			LineRenderer dragLine = DragObject.GetComponent<LineRenderer> (); 

			//get the the coordinates of all points on line 
			//Get old Position Length
			Vector3[] Points = new Vector3[dragLine.positionCount]; //empty list of Vector3s, length of currnt points
			dragLine.GetPositions(Points); //populate that list  

			//calculate the distance between first and last 
			Vector3 last = new Vector3 (); 
			Vector3 first = new Vector3 ();

			try {
				 last = Points.Last(); 
				 first = Points[0]; 
			} catch {
				last = new Vector3 (); 
				first = new Vector3 ();
			}

			float distance = Vector3.Distance (first, last); 

			//compare this to the last time 
			float ThisMinusLast = distance - lastLineNos; 

			//divide this by time since last frame

			currentSpeed = ThisMinusLast/0.2f;

			//update for next time round 
			lastLineNos = distance; 
			//Debug.Log (currentSpeed); 
//			if (giveFeedB == false) { speedup.SetActive(false); slowdown.SetActive(false); }; // hide potential stuff if we arent tracking
//			//add to counters or show/hide warning if we are dragging 
//			if (giveFeedB == true) {
//				//speed up
//				if (Mathf.Abs (currentSpeed) < 0.1f) {
//
//					//do stuff with speed flags - 'cos ya' get a few chances to change speed 
//					if (slowCnt > 3) {
//						speedup.SetActive (true);
//						slowdown.SetActive (false); 
//					} else {
//						slowCnt += 1; 
//					}
//	
//					//slow down
//				} else if (Mathf.Abs (currentSpeed) > 6f) {
//
//					//do stuff with speed flags - 'cos ya' get a few chances to change speed 
//					if (spedCnt > 3) {
//						slowdown.SetActive (true);
//						speedup.SetActive (false); 
//					} else {
//						spedCnt += 1; 
//					}
//
//					//good speed pal - doing a great job
//				} else {
//
//					//check if we were nagging them, if we were hide this and reset counters
//					if (speedup.activeSelf == true) {
//						speedup.SetActive (false);
//						slowCnt = 0; 
//					}
//					if (slowdown.activeSelf == true) {
//						slowdown.SetActive (false);
//						spedCnt = 0; 
//					}
//				}
//			}

		}

	
	}




	//stop showing all the dots
	public void clearDots(){
		Debug.Log("clearingdots");
		var oldDots = GameObject.FindGameObjectsWithTag("clone");
		foreach(GameObject item in oldDots){
			Destroy(item);
		}

	}
		
}

