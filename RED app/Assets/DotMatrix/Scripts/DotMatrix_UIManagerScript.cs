using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using System.Linq;
using UnityEngine.Video;

public class DotMatrix_UIManagerScript : MonoBehaviour {

	//Declare empty game objects for items to be manipulated
	public GameObject InsText;
	public GameObject InsText_2;
	public GameObject Ins_Prac_1;
	public GameObject StartButton;
	public GameObject MainCanvas;
	public GameObject endtext;
	public GameObject Targets;
	public GameObject Grid;
	public GameObject btnCnvs; 
	public GameObject video; 
	public GameObject Continue; 
	public bool practice; 
	public bool pracpress;
	public GameObject Start_Button_Prac;
	public Button StartPrac; 
	public GameObject nopress; 

	public GameObject enter_audio; 

	public bool newSpan = false; 
	//Data logger
	public GameObject DL_object; 

	public DataLogger Logger_2; 

	public List<float> pressed_rts = new List<float> (); 
	public float this_time_start; 

	public int total_presses; 

	//targets 
	public GameObject _1; public GameObject _2; public GameObject _3; public GameObject _4; public GameObject _5; public GameObject _6; public GameObject _7; public GameObject _8; public GameObject _9; 	public GameObject _10; public GameObject _11; public GameObject _12; public GameObject _13; public GameObject _14; public GameObject _15; public GameObject _16; 
	//button components of targets 
	public Button _1_btn; public Button _2_btn; public Button _3_btn; public Button _4_btn; public Button _5_btn; public Button _6_btn; public Button _7_btn; public Button _8_btn; public Button _9_btn; public Button _10_btn; public Button _11_btn; public Button _12_btn; public Button _13_btn; public Button _14_btn; public Button _15_btn; public Button _16_btn; 

	//Find GameObjects that must be hidden first
	//We do this, bcause the .Find() method will not find inactive objects 
	void Start(){

		//find targets 
		_1 = GameObject.Find("1"); _2 = GameObject.Find("2"); _3 = GameObject.Find("3"); _4 = GameObject.Find("4"); _5 = GameObject.Find("5");  _6 = GameObject.Find("6"); _7 = GameObject.Find("7"); _8 = GameObject.Find("8"); _9 = GameObject.Find("9"); _10 = GameObject.Find("10"); _11 = GameObject.Find("11"); _12 = GameObject.Find("12"); _13 = GameObject.Find("13"); _14 = GameObject.Find("14"); _15 = GameObject.Find("15"); _16 = GameObject.Find("16"); 

		total_presses = 0; 

		//set up listeners 

		_1_btn = _1.GetComponent<Button> (); 
		_1_btn.onClick.AddListener (delegate {TaskOnClick (1);}); 
		_2_btn = _2.GetComponent<Button> (); 
		_2_btn.onClick.AddListener (delegate {TaskOnClick (2);}); 
		_3_btn = _3.GetComponent<Button> (); 
		_3_btn.onClick.AddListener (delegate {TaskOnClick (3);}); 
		_4_btn = _4.GetComponent<Button> (); 
		_4_btn.onClick.AddListener (delegate {TaskOnClick (4);}); 
		_5_btn = _5.GetComponent<Button> (); 
		_5_btn.onClick.AddListener (delegate {TaskOnClick (5);}); 
		_6_btn = _6.GetComponent<Button> (); 
		_6_btn.onClick.AddListener (delegate {TaskOnClick (6);}); 
		_7_btn = _7.GetComponent<Button> (); 
		_7_btn.onClick.AddListener (delegate {TaskOnClick (7);}); 
		_8_btn = _8.GetComponent<Button> (); 
		_8_btn.onClick.AddListener (delegate {TaskOnClick (8);}); 
		_9_btn = _9.GetComponent<Button> (); 
		_9_btn.onClick.AddListener (delegate {TaskOnClick (9);}); 
		_10_btn = _10.GetComponent<Button> (); 
		_10_btn.onClick.AddListener (delegate {TaskOnClick (10);}); 
		_11_btn = _11.GetComponent<Button> (); 
		_11_btn.onClick.AddListener (delegate {TaskOnClick (11);}); 
		_12_btn = _12.GetComponent<Button> (); 
		_12_btn.onClick.AddListener (delegate {TaskOnClick (12);}); 
		_13_btn = _13.GetComponent<Button> (); 
		_13_btn.onClick.AddListener (delegate {TaskOnClick (13);}); 
		_14_btn = _14.GetComponent<Button> (); 
		_14_btn.onClick.AddListener (delegate {TaskOnClick (14);}); 
		_15_btn = _15.GetComponent<Button> (); 
		_15_btn.onClick.AddListener (delegate {TaskOnClick (15);}); 
		_16_btn = _16.GetComponent<Button> (); 
		_16_btn.onClick.AddListener (delegate {TaskOnClick (16);}); 

		//hide them
		_1.SetActive (false);_2.SetActive (false);_3.SetActive (false);_4.SetActive (false);_5.SetActive (false);_6.SetActive (false); _7.SetActive (false);_8.SetActive (false);_9.SetActive (false);_10.SetActive (false);_11.SetActive (false);_12.SetActive (false);_13.SetActive (false);_14.SetActive (false);_15.SetActive (false);_16.SetActive (false);


		//find objects
 	
		InsText_2 = GameObject.Find("Ins_Text_2"); 
		endtext = GameObject.Find ("end_text"); 
		Grid = GameObject.Find ("Grid");
		Start_Button_Prac = GameObject.Find ("Start_Button_Prac"); 
		enter_audio = GameObject.Find ("enter_audio");
		video = GameObject.Find ("Video"); 
		Continue = GameObject.Find ("Continue");
		nopress = GameObject.Find ("no_press"); 
		//Set listener for button 


		//set up scripts for numberpad - this has to be done before we hide the numberpad 
		//Get Each Button's GameObject, then it's associated Button component, then add a listener to run script on press, passing the number of that button


		//hide the things we don't want 
		video.SetActive(false); 
		InsText_2.SetActive (false);
		endtext.SetActive(false); 
		Grid.SetActive (false);
		Start_Button_Prac.SetActive(false); 
		enter_audio.SetActive (false); 
		Continue.SetActive (false); 
		nopress.SetActive (false); 
	}

	void Update() {

		//Check for total button presses being beyond span (prevents bugs)
		if (total_presses == span){
			Debug.Log (total_presses); 
			_1_btn.interactable = false; 
			_2_btn.interactable = false; 
			_3_btn.interactable = false; 
			_4_btn.interactable = false; 
			_5_btn.interactable = false; 
			_6_btn.interactable = false; 
			_7_btn.interactable = false; 
			_8_btn.interactable = false; 
			_9_btn.interactable = false; 
			_10_btn.interactable = false; 
			_11_btn.interactable = false; 
			_12_btn.interactable = false; 
			_13_btn.interactable = false; 
			_14_btn.interactable = false; 
			_15_btn.interactable = false; 
			_16_btn.interactable = false; 



		}


	}

	// Starting Span for the game 
	public int span = 3; 
	// Errors 
	public int error_count = 0; 
	//completed at this span
	public int span_cnt = 0; 

	//Bool Flags for certain things 
	public bool endgame = false;
	public bool waiting4input = false; 
 

	// This trial's currently presented numbers
	public List<int> trial_numbers = new List<int>();

	// This trial's recalled list of numbers
	public List<int> trial_recalled = new List<int>();

	//List of lists with numbers this session
	public List<List<int>> session_numbers = new List<List<int>>();

	//List of lists with recalled numbers this session
	public List<List<int>> session_recalled = new List<List<int>>();

	//List of spans for each trial 
	public List<int> session_spans = new List<int>();

	//list of performance 
	public List<int> session_accuracy = new List<int>(); 

	//List of numbers for task
	public List<List<List<int>>> span_numbers = new List<List<List<int>>>(); 



	public int thisNumber;

	// couroutine for checking if the game has ended 
	public IEnumerator CheckEnd(){
		Debug.Log ("checking for end"); 
		while (endgame == true) {
			//close logger
			Logger_2.Close(); 
			//when endgame flag happens, do the stuff below 
			//display end text
			endtext.SetActive (true);
			//wait 2 seconds
			yield return new WaitForSecondsRealtime (2); 
			//load main menu 
			SceneManager.LoadScene ("InBetweenScene"); 
		}
	}


	public void PlayVideo_Enum(){
		StartCoroutine (PlayVideo ()); 
	}

	public IEnumerator PlayVideo(){

		video.SetActive (true);
		InsText = GameObject.Find ("Ins_Text"); 
		InsText.SetActive (false); 
		StartButton = GameObject.Find("Start_Button"); 
		StartButton.SetActive (false); 
		VideoPlayer videoComp = video.GetComponent<VideoPlayer>(); 
		//change the button's target 
		yield return new WaitForSecondsRealtime(0.2f); //wait a bit to give time for the video to start playing
		yield return new WaitUntil (() => videoComp.isPlaying == false); //wait until the video is not playing 
		video.SetActive (false); //hide the gameObject of the video

		Start_Button_Prac.SetActive (true);
		Continue.SetActive (true); 



	}

	public void StartDotMatrix(){

		//FIRST WE NEED TO HIDE THE INITIAL STUFF 


		//Stop showing these objects 
		InsText.SetActive(false); 
		StartButton.SetActive(false);

		Continue.SetActive (false); 
		Start_Button_Prac.SetActive (false); 
		//Set up the numbers 

		//add list holder for 3 span 
		span_numbers.Add(new List<List<int>>());
		//3 span trials
		span_numbers[0].Add(new List<int>(new int[] { 3, 5, 10 } ));
		span_numbers[0].Add(new List<int>(new int[] { 10, 16, 7 } ));
		span_numbers[0].Add(new List<int>(new int[] { 8, 14, 11 } ));
		span_numbers[0].Add(new List<int>(new int[] { 13, 1, 8 } ));
		span_numbers[0].Add(new List<int>(new int[] { 7, 10, 3 } ));
		span_numbers[0].Add(new List<int>(new int[] { 6, 4, 3 } ));

		//add list holder for 4 span 
		span_numbers.Add(new List<List<int>>());
		//4 span trials
		span_numbers[1].Add(new List<int>(new int[] { 9, 1, 7, 2} ));
		span_numbers[1].Add(new List<int>(new int[] { 6, 12, 5, 15} ));
		span_numbers[1].Add(new List<int>(new int[] { 10, 4, 1, 8} ));
		span_numbers[1].Add(new List<int>(new int[] { 12, 1, 4, 6} ));		
		span_numbers[1].Add(new List<int>(new int[] { 2, 16, 14, 6} ));
		span_numbers[1].Add(new List<int>(new int[] { 1, 10, 11, 13} ));

		//add list holder for 5 span 
		span_numbers.Add(new List<List<int>>());
		//5 span trials
		span_numbers[2].Add(new List<int>(new int[] { 4, 12, 9, 3, 4} ));
		span_numbers[2].Add(new List<int>(new int[] { 5, 6, 10, 15, 13} ));
		span_numbers[2].Add(new List<int>(new int[] { 16, 7, 2,9, 15} ));
		span_numbers[2].Add(new List<int>(new int[] { 13, 9, 6, 12, 16} ));
		span_numbers[2].Add(new List<int>(new int[] { 7, 4, 16, 13, 3} ));
		span_numbers[2].Add(new List<int>(new int[] { 1, 8, 7, 12, 11} ));

		//add list holder for 6 span 
		span_numbers.Add(new List<List<int>>());
		//6 span trials
		span_numbers[3].Add(new List<int>(new int[] { 5, 10, 8, 7, 2, 13} ));
		span_numbers[3].Add(new List<int>(new int[] { 11, 10, 5, 15, 12, 14} ));
		span_numbers[3].Add(new List<int>(new int[] { 9, 1, 13, 6, 14, 8} ));
		span_numbers[3].Add(new List<int>(new int[] { 4, 14, 11, 7, 13, 6} ));
		span_numbers[3].Add(new List<int>(new int[] { 3, 15, 7, 16, 9, 12} ));
		span_numbers[3].Add(new List<int>(new int[] { 7, 12, 9, 11, 13, 2} ));


		//add list holder for 7 span 
		span_numbers.Add(new List<List<int>>());
		//7 span trials
		span_numbers[4].Add(new List<int>(new int[] { 8, 12, 10, 15, 5, 3, 10} ));
		span_numbers[4].Add(new List<int>(new int[] { 12, 7, 13, 8, 3, 15, 5} ));
		span_numbers[4].Add(new List<int>(new int[] { 13, 5, 10, 4, 9, 16, 14} ));
		span_numbers[4].Add(new List<int>(new int[] { 7, 10, 11, 13, 6, 8, 7} ));
		span_numbers[4].Add(new List<int>(new int[] { 15, 13, 11, 14, 6, 10, 2} ));
		span_numbers[4].Add(new List<int>(new int[] { 8, 2, 10, 7, 1, 4, 3} ));

		//add list holder for 8 span 
		span_numbers.Add(new List<List<int>>());
		//7 span trials
		span_numbers[5].Add(new List<int>(new int[] {4,7,2,5,9,3,6,1} ));
		span_numbers[5].Add(new List<int>(new int[] {3,8,1,7,2,4,9,6} ));
		span_numbers[5].Add(new List<int>(new int[] {8,6,2,7,5,9,1,4} ));
		span_numbers[5].Add(new List<int>(new int[] {6,1,3,7,2,8,4,5} ));
		span_numbers[5].Add(new List<int>(new int[] {3,5,1,9,2,8,4,7} ));
		span_numbers[5].Add(new List<int>(new int[] {1,7,9,4,6,8,2,5} ));




		// Find the main canvas and other things, which get called later 
		MainCanvas = GameObject.Find("Canvas");

		//Set Practice to false 
		practice = false; 

		//Get datalogger object 
		DL_object = GameObject.Find ("DataLogger"); 
		Logger_2 = DL_object.GetComponent<DataLogger> ();
		//Write header to files 
		//header string 
		string[] header = {"Trial", "RT", "Accuracy", "Span", "Trial_Numbers", "Pressed_Numbers", "Pressed_RT"};
		Logger_2.LogHeader (header);

		//Call co-routine which begins the actual game 
		StartCoroutine(MainLoop());

	}

	//This is the Coroutine for the main trial 
	// The IEnumerator functions will run until they reach a 'yield return' statement, and then restart in the next frame 
	// As we want timing, we use WaitForSecondsRealTime(seconds) after yield return to control timing. 
	public IEnumerator MainLoop(){

		int trial_cnt = 0;


		while (endgame == false && practice == false) {
 
			// RESET SOME THINGS FROM POTENTIAL LAST RUN (forsome reason this wasn't working within another function). 
			//reset these trial values from the last trial
			trial_numbers = new List<int>();
			trial_recalled = new List<int>();
			pressed_rts = new List<float> (); 

			total_presses = 0; 

			_1_btn.interactable = false; 
			_2_btn.interactable = false; 
			_3_btn.interactable = false; 
			_4_btn.interactable = false; 
			_5_btn.interactable = false; 
			_6_btn.interactable = false; 
			_7_btn.interactable = false; 
			_8_btn.interactable = false; 
			_9_btn.interactable = false; 
			_10_btn.interactable = false; 
			_11_btn.interactable = false; 
			_12_btn.interactable = false; 
			_13_btn.interactable = false; 
			_14_btn.interactable = false; 
			_15_btn.interactable = false; 
			_16_btn.interactable = false; 

			//TELL THEM TO WAIT! 
			//show second instruction text2 then wait for 2 seconds 
			InsText_2.SetActive (true); 
			yield return new WaitForSecondsRealtime (2); 

			//Hide the instructions 
			InsText_2.SetActive (false); 

			//Show grid
			Grid.SetActive(true);

			//show stop press image 
			nopress.SetActive(true); 

			//TRIAL TIME
			//now loop through the span of these dot sequences
			for (int i = 0; i < span; i++) {
				//randomly choose a value - but it can NOT be a previously seen value 
				bool foundNumber = false;
				//loop for generating number until appropriate one is flag 
				while (!foundNumber) {
					//get number to use
					thisNumber = span_numbers[span-3][span_cnt][i];
					// compare to previous numbers 
//					bool isInList = trial_numbers.IndexOf(thisNumber)!= -1;
					//if not in list, then we have found our number :-) 
//					if (!isInList) { foundNumber = true;}
					foundNumber = true;
				}

				//wait another second 
				yield return new WaitForSecondsRealtime (1); 

				//which object we gonna show? 

				if (thisNumber == 1){_1.SetActive (true);}; 
				if (thisNumber == 2){_2.SetActive (true);}; 
				if (thisNumber == 3){_3.SetActive (true);}; 
				if (thisNumber == 4){_4.SetActive (true);}; 
				if (thisNumber == 5){_5.SetActive (true);}; 
				if (thisNumber == 6){_6.SetActive (true);}; 
				if (thisNumber == 7){_7.SetActive (true);}; 
				if (thisNumber == 8){_8.SetActive (true);}; 
				if (thisNumber == 9){_9.SetActive (true);}; 
				if (thisNumber == 10){_10.SetActive (true);}; 
				if (thisNumber == 11){_11.SetActive (true);}; 
				if (thisNumber == 12){_12.SetActive (true);}; 
				if (thisNumber == 13){_13.SetActive (true);}; 
				if (thisNumber == 14){_14.SetActive (true);}; 
				if (thisNumber == 15){_15.SetActive (true);}; 
				if (thisNumber == 16){_16.SetActive (true);}; 

				// Show the GameObject

				//Wait for another second 
				yield return new WaitForSecondsRealtime (1);

				//Hide the target 
				if (thisNumber == 1){_1.SetActive (false);}; 
				if (thisNumber == 2){_2.SetActive (false);}; 
				if (thisNumber == 3){_3.SetActive (false);}; 
				if (thisNumber == 4){_4.SetActive (false);}; 
				if (thisNumber == 5){_5.SetActive (false);}; 
				if (thisNumber == 6){_6.SetActive (false);}; 
				if (thisNumber == 7){_7.SetActive (false);}; 
				if (thisNumber == 8){_8.SetActive (false);}; 
				if (thisNumber == 9){_9.SetActive (false);}; 
				if (thisNumber == 10){_10.SetActive (false);}; 
				if (thisNumber == 11){_11.SetActive (false);}; 
				if (thisNumber == 12){_12.SetActive (false);}; 
				if (thisNumber == 13){_13.SetActive (false);}; 
				if (thisNumber == 14){_14.SetActive (false);}; 
				if (thisNumber == 15){_15.SetActive (false);}; 
				if (thisNumber == 16){_16.SetActive (false);}; 



				//We will have to record this value in order to establish accuracy
				trial_numbers.Add (thisNumber); 
			}


			_1_btn.interactable = true; 
			_2_btn.interactable = true; 
			_3_btn.interactable = true; 
			_4_btn.interactable = true; 
			_5_btn.interactable = true; 
			_6_btn.interactable = true; 
			_7_btn.interactable = true; 
			_8_btn.interactable = true; 
			_9_btn.interactable = true; 
			_10_btn.interactable = true; 
			_11_btn.interactable = true; 
			_12_btn.interactable = true; 
			_13_btn.interactable = true; 
			_14_btn.interactable = true; 
			_15_btn.interactable = true; 
			_16_btn.interactable = true; 

			//show buttons 
			_1.SetActive (true);_2.SetActive (true);_3.SetActive (true);_4.SetActive (true);_5.SetActive (true);_6.SetActive (true);_7.SetActive (true);_8.SetActive (true);_9.SetActive (true);_10.SetActive (true);_11.SetActive (true);_12.SetActive (true);_13.SetActive (true);_14.SetActive (true);_15.SetActive (true);_16.SetActive (true);

			//set alpha to invisible, but also increase clickable area to fill grid 
			ShowHide(_1, false, 2); 
			ShowHide(_2, false, 2); 
			ShowHide(_3, false, 2); 
			ShowHide(_4, false, 2); 
			ShowHide(_5, false, 2); 
			ShowHide(_6, false, 2); 
			ShowHide(_7, false, 2); 
			ShowHide(_8, false, 2); 
			ShowHide(_9, false, 2); 
			ShowHide(_10, false, 2); 
			ShowHide(_11, false, 2);
			ShowHide(_12, false, 2); 
			ShowHide(_13, false, 2); 
			ShowHide(_14, false, 2); 
			ShowHide(_15, false, 2); 
			ShowHide(_16, false, 2); 



			//turn wait for input flag to true 
			waiting4input = true; 
			//hide the no press button
			nopress.SetActive (false); 
			float start_time = Time.fixedTime;

			//set timer for first button pressed
			this_time_start = Time.fixedTime; 

			//play enter audio message by setting gameobject to active 
			enter_audio.SetActive(true); 

			//Only continue if waiting4input is false i.e. they have entered enough digits on the numberpad 
			yield return new WaitUntil(() => waiting4input == false);

			//return enter audio gameobject to not-active

			enter_audio.SetActive (false); 

			float rt = Time.fixedTime - start_time; 

			//Log line in the txt file 
			//{"Trial", "RT", "Accuracy", "Span", "Trial_Numbers", "Pressed_Numbers", "RT_numbers"};

			//Log results of this file 
			//Make row
			string[] results = {
				trial_cnt.ToString(),
				rt.ToString(),
				session_accuracy[trial_cnt].ToString(),
				span.ToString(),
				string.Join(" ", span_numbers[span-3][span_cnt-1].Select(n => n.ToString()).ToArray()),
				string.Join(" ", trial_recalled.Select(n => n.ToString()).ToArray()),
				string.Join(" ", pressed_rts.Select(n => n.ToString()).ToArray()),
			};
			trial_cnt += 1; 

			//if sequence length is going up reset some stuff. 
			if (newSpan == true) {
				span_cnt = 0; 
				span += 1;
				//reset error counter for next span 
				error_count = 0; 

				// end game if span is greater than 7 
				if (span > 8) { 
					endgame = true;
				}

				newSpan = false; 
			}; 

			Debug.Log (results); 

			Logger_2.LogData (results);

			//Call coroutine which checks for end and exetutes the exit
			StartCoroutine(CheckEnd()); 



			} 

	
	}

	//function for dealing with the keypad (which is called in the coroutine above) 
	public void TaskOnClick(int number){
		total_presses += 1; 

		StartCoroutine(Clickfunc(number));
	}

	//Coroutine/loop so we can do this across frames
	public IEnumerator Clickfunc(int number){

		//add click RT to our list 
		pressed_rts.Add(Time.fixedTime - this_time_start);

		//First add the value of the button to recalled list 
		trial_recalled.Add(number); 

		//make all other buttons unpressable until this animation is over 
		_1_btn.interactable = false; 
		_2_btn.interactable = false; 
		_3_btn.interactable = false; 
		_4_btn.interactable = false; 
		_5_btn.interactable = false; 
		_6_btn.interactable = false; 
		_7_btn.interactable = false; 
		_8_btn.interactable = false; 
		_9_btn.interactable = false; 
		_10_btn.interactable = false; 
		_11_btn.interactable = false; 
		_12_btn.interactable = false; 
		_13_btn.interactable = false; 
		_14_btn.interactable = false; 
		_15_btn.interactable = false; 
		_16_btn.interactable = false; 

		//call Coroutine to expand the button then shrink AND call function to show the item 
		if (number == 1){ShowHide(_1, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_1, 5, 0.01f, 0.01f)); }; 
		if (number == 2){ShowHide(_2, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_2, 5, 0.01f, 0.01f)); }; 
		if (number == 3){ShowHide(_3, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_3, 5, 0.01f, 0.01f)); }; 
		if (number == 4){ShowHide(_4, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_4, 5, 0.01f, 0.01f)); }; 
		if (number == 5){ShowHide(_5, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_5, 5, 0.01f, 0.01f)); }; 
		if (number == 6){ShowHide(_6, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_6, 5, 0.01f, 0.01f)); }; 
		if (number == 7){ShowHide(_7, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_7, 5, 0.01f, 0.01f)); }; 
		if (number == 8){ShowHide(_8, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_8, 5, 0.01f, 0.01f)); }; 
		if (number == 9){ShowHide(_9, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_9, 5, 0.01f, 0.01f)); }; 
		if (number == 10){ShowHide(_10, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_10, 5, 0.01f, 0.01f)); }; 
		if (number == 11){ShowHide(_11, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_11, 5, 0.01f, 0.01f)); }; 
		if (number == 12){ShowHide(_12, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_12, 5, 0.01f, 0.01f)); }; 
		if (number == 13){ShowHide(_13, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_13, 5, 0.01f, 0.01f)); }; 
		if (number == 14){ShowHide(_14, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_14, 5, 0.01f, 0.01f)); }; 
		if (number == 15){ShowHide(_15, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_15, 5, 0.01f, 0.01f)); }; 
		if (number == 16){ShowHide(_16, true, 2); yield return new WaitForSecondsRealtime(0.1f);StartCoroutine(ExpandContract(_16, 5, 0.01f, 0.01f)); }; 

		//then return that number to it's previous hidden state 

		if (number ==1){ ShowHide (_1, false, 2); };
		if (number ==2){ ShowHide (_2, false, 2); };
		if (number ==3){ ShowHide (_3, false, 2); };
		if (number ==4){ ShowHide (_4, false, 2); };
		if (number ==5){ ShowHide (_5, false, 2); };
		if (number ==6){ ShowHide (_6, false, 2); };
		if (number ==7){ ShowHide (_7, false, 2); };
		if (number ==8){ ShowHide (_8, false, 2); };
		if (number ==9){ ShowHide (_9, false, 2); };
		if (number ==10){ ShowHide (_10, false, 2); };
		if (number ==11){ ShowHide (_11, false, 2); };
		if (number ==12){ ShowHide (_12, false, 2); };
		if (number ==13){ ShowHide (_13, false, 2); };
		if (number ==14){ ShowHide (_14, false, 2); };
		if (number ==15){ ShowHide (_15, false, 2); };
		if (number ==16){ ShowHide (_16, false, 2); };

		//If we have entered the number of digits shown move to next trial and do some logic flag, trial tracking stuff

		if (trial_recalled.Count >= span) {

			span_cnt += 1; 

			//calculate if the answer was correct or not and add to counters appropriately
			if (trial_recalled.SequenceEqual (trial_numbers)) {
				error_count += 0;
				session_accuracy.Add (1);
			} else {
				error_count += 1;
				session_accuracy.Add (0);
			}

			//if the error count is three or greater quit the game 
			if (error_count >= 3) {
				endgame = true; 
			}

			//if they have 4 or more correct trials go up one level
			if (span_cnt-error_count >= 4) {
//				span_cnt = 0; 
//				span += 1;
//				//reset error counter for next span 
//				error_count = 0; 
				newSpan = true; 
			}

			// end game if span is greater than 7 
			if (span > 8) { 
				endgame = true;
			}
			//add the presented number sequence to session list 
			session_numbers.Add(trial_numbers); 
			//add the recalled numbers to session list 
			session_recalled.Add(trial_recalled);
			//add the length of this span 
			session_spans.Add(span); 

			//wait for 0.5 second 
			yield return new WaitForSecondsRealtime(1);


			// reset alpha and size 
			ShowHide (_1, true, 1); 
			ShowHide (_2, true, 1); 
			ShowHide (_3, true, 1); 
			ShowHide (_4, true, 1); 
			ShowHide (_5, true, 1); 
			ShowHide (_6, true, 1); 
			ShowHide (_7, true, 1); 
			ShowHide (_8, true, 1); 
			ShowHide (_9, true, 1); 
			ShowHide (_10, true, 1); 
			ShowHide (_11, true, 1); 
			ShowHide (_12, true, 1); 
			ShowHide (_13, true, 1); 
			ShowHide (_14, true, 1); 
			ShowHide (_15, true, 1);
			ShowHide (_16, true, 1);

			//hide grid
			_1.SetActive (false);_2.SetActive (false);_3.SetActive (false);_4.SetActive (false);_5.SetActive (false);_6.SetActive (false); _7.SetActive (false);_8.SetActive (false);_9.SetActive (false);_10.SetActive (false);_11.SetActive (false);_12.SetActive (false);_13.SetActive (false);_14.SetActive (false);_15.SetActive (false);_16.SetActive (false);


			//continue with next trial
			waiting4input = false; 



		}

		this_time_start = Time.fixedTime; 

		//Allow them to be pressed again 
		_1_btn.interactable = true; 
		_2_btn.interactable = true; 
		_3_btn.interactable = true; 
		_4_btn.interactable = true; 
		_5_btn.interactable = true; 
		_6_btn.interactable = true; 
		_7_btn.interactable = true; 
		_8_btn.interactable = true; 
		_9_btn.interactable = true; 
		_10_btn.interactable = true; 
		_11_btn.interactable = true; 
		_12_btn.interactable = true; 
		_13_btn.interactable = true; 
		_14_btn.interactable = true; 
		_15_btn.interactable = true; 
		_16_btn.interactable = true; 
	}

	//function which expands and contracts the GameObject passed to it 
	public IEnumerator ExpandContract(GameObject game_object, int points, float time, float increment){

		//Get Rect Transform component of object
		RectTransform rect_comp;
		rect_comp = game_object.GetComponent<RectTransform>(); 

		//15 points 
		for (int i = 1; i < points; i++){

			//expand width and height by 1%
			rect_comp.sizeDelta = new Vector2 (rect_comp.sizeDelta[0]+ increment, rect_comp.sizeDelta[1] +increment);

			//pause for 10ms 
			yield return new WaitForSeconds (time);
		}

		//15 points 
		for (int i = 1; i < points; i++){

			//expand width and height by 1%
			rect_comp.sizeDelta = new Vector2 (rect_comp.sizeDelta[0]- increment, rect_comp.sizeDelta[1] -increment);

			//pause for 10ms 
			yield return new WaitForSeconds (time);
		}

		Debug.Log ("swell", game_object); 

	}

	//function to show or hide buttons by setting alpha to zero and expanding to fill the grid square
	// if show is set to false this reverses the action, we must pass size argument as 0.7 

	public void ShowHide(GameObject game_object, bool show, float size){

		//Get Rect Transform component of object
		RectTransform rect_comp;
		rect_comp = game_object.GetComponent<RectTransform>(); 

		//get Canvas Group component of object
		CanvasGroup btn_cnvs;
		//assign the Canvas Group component
		btn_cnvs = game_object.GetComponent<CanvasGroup>(); 


		//to size and show 
		if (show == true) {
			//change alpha to 1 
			btn_cnvs.alpha = 1; 
			// set size
			rect_comp.sizeDelta = new Vector2 (size, size); 
			Debug.Log ("alpha = 1",game_object); 
		//to size and hide
		} else {
			
			//alpha to 0
			btn_cnvs.alpha = 0;
			// set size
			rect_comp.sizeDelta = new Vector2 (size, size);
			Debug.Log ("alpha = 0",game_object); 
			
		}
	}


}
