using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using System.Linq;

public class DigitSpan_UIManagerScript : MonoBehaviour {

	//Declare empty game objects for items to be manipulated
	public GameObject InsText;
	public GameObject InsText_2;
	public GameObject StartButton;
	public GameObject MainCanvas;
	public GameObject digit; 
	public GameObject NumPad;
	public GameObject endtext;
	public GameObject num_audio; 
	public AudioSource num_audio_src; 

	//number pad buttons 
	public GameObject num_1; public GameObject num_2; public GameObject num_3; public GameObject num_4; public GameObject num_5; public GameObject num_6; public GameObject num_7; public GameObject num_8; public GameObject num_9;
	public Button btn_1; public Button btn_2; public Button btn_3; public Button btn_4; public Button btn_5; public Button btn_6; public Button btn_7; public Button btn_8; public Button btn_9;
	public GameObject delete; public Button delete_btn; 

	//number pad screen 
	public GameObject num_output; 

	//Data logger
	public GameObject DL_object; 



	public DataLogger Logger_1; 

	public List<float> pressed_rts = new List<float> (); 
	public float this_time_start; 

	public int total_presses; 

	public bool newSpan = false; 

	public string str_no; 

	//Find GameObjects that must be hidden first
	//We do this, bcause the .Find() method will not find inactive objects 
	void Start(){
		//find objects
		InsText_2 = GameObject.Find("Ins_Text_2"); 
		digit = GameObject.Find ("digit");
		NumPad = GameObject.Find ("Num_Pad");
		num_output = GameObject.Find ("Out_Text"); 
		endtext = GameObject.Find ("end_text"); 

		total_presses = 0; 

		//set up scripts for numberpad - this has to be done before we hide the numberpad 
		//Get Each Button's GameObject, then it's associated Button component, then add a listener to run script on press, passing the number of that button
		num_1 = GameObject.Find ("One"); 
		btn_1 = num_1.GetComponent<Button> (); 
		btn_1.onClick.AddListener (delegate {
			TaskOnClick (1);
		}); 
		num_2 = GameObject.Find ("Two"); 
		btn_2 = num_2.GetComponent<Button> (); 
		btn_2.onClick.AddListener (delegate {
			TaskOnClick (2);
		}); 
		num_3 = GameObject.Find ("Three"); 
		btn_3 = num_3.GetComponent<Button> (); 
		btn_3.onClick.AddListener (delegate {
			TaskOnClick (3);
		}); 
		num_4 = GameObject.Find ("Four"); 
		btn_4 = num_4.GetComponent<Button> (); 
		btn_4.onClick.AddListener (delegate {
			TaskOnClick (4);
		}); 
		num_5 = GameObject.Find ("Five"); 
		btn_5 = num_5.GetComponent<Button> (); 
		btn_5.onClick.AddListener (delegate {
			TaskOnClick (5);
		}); 
		num_6 = GameObject.Find ("Six"); 
		btn_6 = num_6.GetComponent<Button> (); 
		btn_6.onClick.AddListener (delegate {
			TaskOnClick (6);
		}); 
		num_7 = GameObject.Find ("Seven"); 
		btn_7 = num_7.GetComponent<Button> (); 
		btn_7.onClick.AddListener (delegate {
			TaskOnClick (7);
		}); 
		num_8 = GameObject.Find ("Eight"); 
		btn_8 = num_8.GetComponent<Button> (); 
		btn_8.onClick.AddListener (delegate {
			TaskOnClick (8);
		}); 
		num_9 = GameObject.Find ("Nine"); 
		btn_9 = num_9.GetComponent<Button> (); 
		btn_9.onClick.AddListener (delegate {
			TaskOnClick (9);
		}); 

		delete = GameObject.Find ("delete"); 
		delete_btn = delete.GetComponent<Button> (); 
		delete_btn.onClick.AddListener (delegate {
			DelPress ();
		}); 

		//hide the things we don't want 
		InsText_2.SetActive (false);
		digit.SetActive(false);
		NumPad.SetActive(false);
		endtext.SetActive(false); 

		//Game object for audio 
		num_audio = GameObject.Find("num_audio"); 
		num_audio_src = num_audio.GetComponents<AudioSource> ()[0]; 


	}

	void Update() {

		//Check for total button presses being beyond span (prevents bugs)
		if (total_presses == span){
			Debug.Log (total_presses); 
			btn_1.interactable = false; 
			btn_2.interactable = false; 
			btn_3.interactable = false; 
			btn_4.interactable = false; 
			btn_5.interactable = false; 
			btn_6.interactable = false; 
			btn_7.interactable = false; 
			btn_8.interactable = false; 
			btn_9.interactable = false; 
			delete_btn.interactable = false; 

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
			//when endgame flag happens, do the stuff below 
			//display end text
			//close text file 
			Logger_1.Close(); 

			endtext.SetActive (true);
			//wait 2 seconds
			yield return new WaitForSecondsRealtime (2); 
			//load main menu 
			SceneManager.LoadScene ("InBetweenScene"); 
		}
	}

	public void StartDigitSpan(){

		//Get datalogger object 
		DL_object = GameObject.Find ("DataLogger"); 
		Logger_1 = DL_object.GetComponent<DataLogger> ();
		//Write header to files 
		//header string 
		string[] header = {"Trial", "RT", "Accuracy", "Span", "Trial_Numbers", "Pressed_Numbers", "Pressed_RTs"};
		Logger_1.LogHeader (header);

		//FIRST WE NEED TO HIDE THE INITIAL STUFF 

		//Find the two objects to get rid of 
		InsText = GameObject.Find ("Ins_Text");
		StartButton	= GameObject.Find("Start_Button");

		//Stop showing these objects 
		InsText.SetActive(false); 
		StartButton.SetActive(false);

		//Set up the numbers 

		//add list holder for 3 span 
		span_numbers.Add(new List<List<int>>());
		//3 span trials
		span_numbers[0].Add(new List<int>(new int[] { 9, 2, 5 } ));
		span_numbers[0].Add(new List<int>(new int[] { 5, 8, 4 } ));
		span_numbers[0].Add(new List<int>(new int[] { 6, 1, 2 } ));
		span_numbers[0].Add(new List<int>(new int[] { 8, 3, 5 } ));
		span_numbers[0].Add(new List<int>(new int[] { 5, 2, 8 } ));
		span_numbers[0].Add(new List<int>(new int[] { 2, 9, 3 } ));

		//add list holder for 4 span 
		span_numbers.Add(new List<List<int>>());
		//4 span trials
		span_numbers[1].Add(new List<int>(new int[] { 7,4,9,3} ));
		span_numbers[1].Add(new List<int>(new int[] { 2,1,4,6} ));
		span_numbers[1].Add(new List<int>(new int[] { 4,8,9,7} ));
		span_numbers[1].Add(new List<int>(new int[] { 2,7,4,3} ));		
		span_numbers[1].Add(new List<int>(new int[] { 7,2,1,3} ));
		span_numbers[1].Add(new List<int>(new int[] { 8,5,4,7} ));

		//add list holder for 5 span 
		span_numbers.Add(new List<List<int>>());
		//5 span trials
		span_numbers[2].Add(new List<int>(new int[] { 2,3,6,5,1} ));
		span_numbers[2].Add(new List<int>(new int[] { 1,9,3,7,5} ));
		span_numbers[2].Add(new List<int>(new int[] { 5,1,6,9,3} ));
		span_numbers[2].Add(new List<int>(new int[] { 1,8,6,5,2} ));
		span_numbers[2].Add(new List<int>(new int[] { 5,3,4,8,1} ));
		span_numbers[2].Add(new List<int>(new int[] { 9,6,5,1,4} ));

		//add list holder for 6 span 
		span_numbers.Add(new List<List<int>>());
		//6 span trials
		span_numbers[3].Add(new List<int>(new int[] { 1, 9, 8, 4, 2, 3} ));
		span_numbers[3].Add(new List<int>(new int[] { 9, 5, 6, 2, 3, 8} ));
		span_numbers[3].Add(new List<int>(new int[] { 1, 8, 3, 5, 2, 9} ));
		span_numbers[3].Add(new List<int>(new int[] { 3, 8, 2, 6, 1, 5} ));
		span_numbers[3].Add(new List<int>(new int[] { 9, 5, 2, 3, 1, 4} ));
		span_numbers[3].Add(new List<int>(new int[] { 9, 1, 8, 7, 6, 5} ));


		//add list holder for 7 span 
		span_numbers.Add(new List<List<int>>());
		//7 span trials
		span_numbers[4].Add(new List<int>(new int[] { 1, 4, 7, 3, 8, 2, 6} ));
		span_numbers[4].Add(new List<int>(new int[] { 9, 3, 7, 2, 1, 6, 8} ));
		span_numbers[4].Add(new List<int>(new int[] { 1, 5, 3, 6, 8, 9, 7} ));
		span_numbers[4].Add(new List<int>(new int[] { 3, 5, 2, 8, 9, 7, 1} ));
		span_numbers[4].Add(new List<int>(new int[] { 6, 5, 7, 8, 1, 9, 2} ));
		span_numbers[4].Add(new List<int>(new int[] { 4, 1, 3, 2, 5, 9, 7} ));

		//add list holder for 8 span 
		span_numbers.Add(new List<List<int>>());
		//7 span trials
		span_numbers[5].Add(new List<int>(new int[] {8,3,2,4,5,9,6,1} ));
		span_numbers[5].Add(new List<int>(new int[] {4,8,5,7,1,9,2,3} ));
		span_numbers[5].Add(new List<int>(new int[] {1,5,3,2,7,8,4,9} ));
		span_numbers[5].Add(new List<int>(new int[] {2,9,3,8,5,3,6,1} ));
		span_numbers[5].Add(new List<int>(new int[] {8,1,5,9,2,6,3,4} ));
		span_numbers[5].Add(new List<int>(new int[] {3,6,2,9,4,8,1,5} ));

		//add list holder for 9 span 
		span_numbers.Add(new List<List<int>>());
		//7 span trials
		span_numbers[5].Add(new List<int>(new int[] {3,5,9,1,8,7,6,2,4} ));
		span_numbers[5].Add(new List<int>(new int[] {3,2,1,6,7,4,9,5,8} ));
		span_numbers[5].Add(new List<int>(new int[] {2,7,5,8,4,1,3,6,9} ));
		span_numbers[5].Add(new List<int>(new int[] {1,2,3,6,8,5,9,4,7} ));
		span_numbers[5].Add(new List<int>(new int[] {7,9,6,1,4,2,8,5,3} ));
		span_numbers[5].Add(new List<int>(new int[] {2,4,1,6,3,9,7,5,8} ));

		// Find the main canvas and other things, which get called later 
		MainCanvas = GameObject.Find("Canvas");

		//Call co-routine which begins the actual game 
		StartCoroutine(MainLoop());

	}

	//This is the Coroutine for the main trial 
	// The IEnumerator functions will run until they reach a 'yield return' statement, and then restart in the next frame 
	// As we want timing, we use WaitForSecondsRealTime(seconds) after yield return to control timing. 
	public IEnumerator MainLoop(){

		int trial_ind = 0; 
		while (endgame == false) {

			total_presses = 0; 
			// RESET SOME THINGS FROM POTENTIAL LAST RUN (forsome reason this wasn't working within another function). 
			//reset these trial values from the last trial
			trial_numbers = new List<int>();
			trial_recalled = new List<int>();

			pressed_rts = new List<float> (); 

			//clear this text from the hidden number output window 
			num_output.GetComponent<Text> ().text = "";

			//TELL THEM TO WAIT! 
			//show second instruction text2 then wait for 5 seconds 
			InsText_2.SetActive (true); 
			yield return new WaitForSecondsRealtime (2); 

			//Hide the instructions 
			InsText_2.SetActive (false); 


			//TRIAL TIME
			//now show the series of digits to length of span 
			for (int i = 0; i < span; i++) {
				//randomly choose a value - but it can NOT be a previously seen value 
				bool foundNumber = false;
				//loop for generating number until appropriate one is flag 
				while (!foundNumber) {
					//generate between 1 and 9
					thisNumber = span_numbers[span-3][span_cnt][i];
					// compare to previous numbers 
					bool isInList = trial_numbers.IndexOf(thisNumber)!= -1;
					//if not in list, then we have found our number :-) 
					if (!isInList) { foundNumber = true;}
				}
				//Make this the text of the digit GameObject
				digit.GetComponent<Text> ().text = thisNumber.ToString ();
				// Show the GameObject
				digit.SetActive (true);

				//load the audio number

				str_no = "Sounds/Digit_Span_" + thisNumber.ToString() ;
				Debug.Log (str_no); 
				AudioClip file_audio =  Resources.Load(str_no, typeof(AudioClip)) as AudioClip;
				num_audio_src.clip = file_audio; 
				//play
				num_audio_src.Play(); 
				//Wait a second 
				yield return new WaitForSecondsRealtime (1);
				//Hide the text
				digit.SetActive (false);
				//wait another second 
				yield return new WaitForSecondsRealtime (1); 
				//We will have to record this value in order to establish accuracy
				trial_numbers.Add (thisNumber); 
			}

			// Show the Input field 
			NumPad.SetActive (true); 

			btn_1.interactable = true; 
			btn_2.interactable = true; 
			btn_3.interactable = true; 
			btn_4.interactable = true; 
			btn_5.interactable = true; 
			btn_6.interactable = true; 
			btn_7.interactable = true; 
			btn_8.interactable = true; 
			btn_9.interactable = true; 
			delete_btn.interactable = true; 

			//turn wait for input flag to true 
			waiting4input = true; 

			float start_time = Time.fixedTime; 

			//set timer for first button pressed
			this_time_start = Time.fixedTime; 

			//stay in this loop until we get a response (i.e. one of the options has been selected)

			//Only continue if waiting4input is false i.e. they have entered enough digits on the numberpad 
			yield return new WaitUntil(() => waiting4input == false);

			float rt = Time.fixedTime - start_time; 

			//Log line in the txt file 
			//{"Trial", "RT", "Accuracy", "Span", "Trial_Numbers", "Pressed_Numbers", "RT_numbers"};

			//Log results of this file 
			//Make row


			string[] results = {
				trial_ind.ToString(),
				rt.ToString(),
				session_accuracy[trial_ind].ToString(),
				span.ToString(),
				string.Join(" ", span_numbers[span-3][span_cnt-1].Select(n => n.ToString()).ToArray()),
				string.Join(" ", trial_recalled.Select(n => n.ToString()).ToArray()),
				string.Join(" ", pressed_rts.Select(n => n.ToString()).ToArray()),
			};

			Logger_1.LogData (results);

			//reset some stuff if we have gone up a span 
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
			}
			//Call coroutine which checks for end and exetutes the exit
			StartCoroutine(CheckEnd()); 

			//count up index
			trial_ind += 1; 
		} 

	}

	//function for dealing with the keypad (which is called in the coroutine above) 
	public void TaskOnClick(int number){
		total_presses += 1; 
		StartCoroutine(Clickfunc(number));
	}

	public void DelPress(){
		//if greater than 1
		if (trial_recalled.Count >= 1){
			//remove last item 
			trial_recalled.RemoveAt (trial_recalled.Count - 1);
			total_presses -= 1; 

			//update screen 
			//Then change the text of the numberpad screen to this list (convert list to string sepearted with spaces)
			//int list to string list using LINQ 
			string[] templist = trial_recalled.Select (i => i.ToString ()).ToArray ();

			// get numoutput game object then change text component 
			num_output = GameObject.Find ("Out_Text"); 
			num_output.GetComponent<Text> ().text = string.Join (" ", templist);
		}
	}

	//Coroutine/loop so we can do this across frames
	public IEnumerator Clickfunc(int number){

		//add click RT to our list 
		pressed_rts.Add(Time.fixedTime - this_time_start);

		//First add the value of the button to recalled list 
		trial_recalled.Add(number); 

		//Then change the text of the numberpad screen to this list (convert list to string sepearted with spaces)
		//int list to string list using LINQ 
		string[] templist = trial_recalled.Select (i => i.ToString ()).ToArray ();

		// get numoutput game object then change text component 
		num_output = GameObject.Find ("Out_Text"); 
		num_output.GetComponent<Text> ().text = string.Join (" ", templist);

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



			// end game if span is greater than 8 
			if (span > 8) { 
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
			//add the presented number sequence to session list 
			session_numbers.Add(trial_numbers); 
			//add the recalled numbers to session list 
			session_recalled.Add(trial_recalled);
			//add the length of this span 
			session_spans.Add(span); 

			//wait for 0.5 second 
			yield return new WaitForSecondsRealtime(1);

			//hide the numberpad 
			NumPad.SetActive (false);

			//continue with next trial
			waiting4input = false; 

		}

		//reset the timer for next button press 
		this_time_start = Time.fixedTime; 
	}


}
