using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using System.Linq;
using UnityEngine.Networking; 
using System.IO; 

public class AlliterationTest_UIManagerScript : MonoBehaviour {

	//declare instance of alliteration test script 
	public DataLogger Logger; 
	public GameObject DL_object; 


	//variables for swivel 
	int SwivSpeed = 30; 
	int Limit = 22; 
	int spun = 0;
	bool clockwise = true;
	public Transform leftrect;
	public Transform rightrect; 

	//Declare empty game objects for items to be manipulated
	public GameObject InsText;
	public GameObject InsText_2;
	public GameObject StartButton;
	public GameObject EndText;

	//public GameObject Feedback; 

	public int chose_side; 
	public bool isleft  = false; 
	public bool isright = false; 
	public bool wait4response = false;

	//the first item
	public GameObject Cue; 
	//Sprite Renderer of Cue 
	public SpriteRenderer Cue_sprite;
	// the two options 
	public GameObject LeftOption;
	public SpriteRenderer Left_sprite;
	public GameObject RightOption; 
	public SpriteRenderer Right_sprite;
	//list for these plaecholders
	public List<GameObject> pHolderList = new List<GameObject>();

	public GameObject AudioObject; 
	public AudioSource AudioObjectSource; 

	//Main Canvas
	public GameObject MainCanvas; 

	// Errors 
	public int error_count = 0; 

	//completed at this span
	public int span_cnt = 0; 

	//Bool Flags for certain things 
	public bool endgame = false;
	public bool waiting4input = false; 

	public int indx; 


	// This trial's currently presented words
	// These will always follow the same order: Cue, Left, Right 
	public List<string> trial_words = new List<string>();

	//list of lists for all sprites relating to these words
	public List<string> trial_sprites = new List<string>();

	// This trial's selected option 
	public string side_chosen; 

	// This trials RT 
	public float reaction_time; 

	//This trials correct or not? 
	public int accuracy; 

	//List of lists with words this session
	public List<List<string>> session_words = new List<List<string>>(); 

	//List of selections this session
	public List<int> session_choices = new List<int>();

	//List of RT for each trial 
	public List<float> session_rt = new List<float>();

	// 3, 4, 6, 10, 2, 8 = new trial order
	// 2,3,4,6,8,10
	//public List<int> left_right = new List<int> {1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 1, 1, 0}; 
	public List<int> left_right = new List<int> {0, 1, 0, 0, 0, 0}; 

	//list of accuracy 
	public List<int> session_accuracy = new List<int>(); 

	//Vector 3 for angles 
	public Vector3 angle_left; 
	public Vector3 angle_right; 

	//Find GameObjects that must be hidden first
	//We do this, bcause the .Find() method will not find inactive objects 
	void Start(){
		Debug.Log ("started");
		//find gameobjects for place holders
		Cue = GameObject.Find("cue"); 
		LeftOption = GameObject.Find ("left_option");
		RightOption = GameObject.Find ("right_option"); 

		//Feedback = GameObject.Find ("Feedback"); 

		//get placeholders rect transform components
		leftrect = LeftOption.GetComponents<Transform>()[0]; 
		rightrect = RightOption.GetComponents<Transform>()[0];

		//Get sprite
		Cue_sprite = Cue.GetComponents<SpriteRenderer> ()[0]; 
		Left_sprite = LeftOption.GetComponents<SpriteRenderer> ()[0];
		Right_sprite = RightOption.GetComponents<SpriteRenderer> ()[0]; 

		//Game object for audio 
		AudioObject = GameObject.Find("Audio"); 
		AudioObjectSource = AudioObject.GetComponents<AudioSource> ()[0]; 

		//make a list of all the target Game objectes 
		pHolderList.Add(Cue); pHolderList.Add(LeftOption); pHolderList.Add(RightOption);

		//hide these 
		foreach (GameObject obj in pHolderList) {
			obj.SetActive (false);
		}

		//play the first audio message 
		AudioClip instr =  Resources.Load("Sounds/instuctions_allit", typeof(AudioClip)) as AudioClip;
		AudioObjectSource.clip = instr; 
		AudioObjectSource.Play (); 



	}

	void Update(){

		//swivel l/R sprites if we are waiting for a response (this let's user know they are clickable)
		// using SwivSpeed, Limit, Spun, clockwise
		if (wait4response == true) {

			//get current rotation quaternion
			Quaternion curQ_left = leftrect.rotation;
			Quaternion curQ_right = rightrect.rotation; 

			//get euler angle 
			angle_left = curQ_left.eulerAngles;
			angle_right = curQ_right.eulerAngles;

			//we are adding degrees 
			if (clockwise == true) {
				//add rotation 
				angle_left.z += 0.5f; 
				angle_right.z += 0.5f;

			}

			//we are subtracting degrees
			if (clockwise == false) {
				//subtract rotation 
				angle_left.z += -0.5f; 
				angle_right.z += -0.5f;

			}

			//apply transforms 
			//change this on the quarts
			curQ_left.eulerAngles = angle_left; 
			curQ_right.eulerAngles = angle_right;

			//change this in the actual rects
			leftrect.rotation = curQ_left;
			rightrect.rotation = curQ_right; 

			//check if we have reached limits and change direction if needed
			Debug.Log(angle_left.z);
			if (angle_left.z >= 10) { clockwise = false; }   
			if (angle_left.z <= 0.5) { clockwise = true; }

		}

		//monitor for keypress flags
		//action changes if flagged
		if (left_right [indx] == 0) {
			//correct
			if (isleft == true){
				chose_side = 1; 
				session_choices.Add (1);
				wait4response = false; 
				isleft = false; 
			}
			//incorrect
			if (isright == true){
				chose_side = 2; 
				session_choices.Add (0);
				wait4response = false; 
				isright = false;
			}

		} 
		if (left_right[indx] == 1){
			//incorrect
			if (isleft == true){
				chose_side = 1; 
				session_choices.Add (0);
				wait4response = false; 
				isleft = false; 
			}

			//correct
			if (isright == true){
				chose_side = 2; 
				session_choices.Add (1);
				wait4response = false; 
				isright = false;
			}
		}
	}




	// couroutine for checking if the game has ended 
	public IEnumerator CheckEnd(){
		Debug.Log ("checking for end"); 
		while (endgame == true) {
			//when endgame flag happens, do the stuff below 
			//wait 2 seconds
			yield return new WaitForSecondsRealtime (2); 
			//load main menu 
			SceneManager.LoadScene ("InBetweenScene"); 
		}
	}
		
	//func that is called from the script attatched to left and right sprites
	public void SelectOption(string name){

		//if wait for response flag is true, change option flags
		if (wait4response == true) {
			if (name == "left_option") {isleft = true;}
			if (name == "right_option") {isright = true;}
		}
	
	}

	public void StartAlliterationTest(){

		//FIRST WE NEED TO HIDE THE INITIAL STUFF 

		//Find the two objects to get rid of 
		InsText = GameObject.Find ("ins_text");
		StartButton	= GameObject.Find("StartButton");

		//Stop showing these objects 
		InsText.SetActive(false); 
		StartButton.SetActive(false);

		//Set up the numbers 

		//add list holder for 3 words


		//trials
		// 1st = Cue, 2nd = Left, 3rd = Right 
		// 2,3,4,6,8,10    ZERO INDEXED
		//Simple Rhymes/Non-rhyme distractors
//		session_words.Add(new List<string>{"Bunny", "Money", "Lightning"} ); //0
//		session_words.Add(new List<string>{"Shower", "Flower", "Classroom"} ); //1
		session_words.Add(new List<string>{"Rocket", "Pocket", "Chipmunk"} ); //2
		session_words.Add(new List<string>{"Jelly", "Belly", "Table"} ); //3
		session_words.Add(new List<string>{"Winter", "Splinter", "Bedroom"} ); //4
//		session_words.Add(new List<string>{"Shoelace", "Bookcase", "Chicken"} ); //5
		session_words.Add(new List<string>{"Sweater", "Letter", "Lion"} ); //6
//		session_words.Add(new List<string>{"Teaspoon", "Balloon", "Blanket"} ); //7
		session_words.Add(new List<string>{"Cherry", "Fairy", "Button"} ); //8

		// three sylabbles 
//		session_words.Add(new List<string>{"Raspberry", "Library", "Magician"} ); //9
		session_words.Add(new List<string>{"Elevator", "Alligator", "Rhinoceros"} ); //10
//		session_words.Add(new List<string>{"Waterfall", "Basketball", "Rectangle"} );
//		session_words.Add(new List<string>{"Bicycle", "Icicle", "Stereo"} );

		// Near rhymes
//		session_words.Add(new List<string>{"Daughter", "Water", "Doctor"} );
//		session_words.Add(new List<string>{"Mitten", "Kitten", "Ribbon"} );
//		session_words.Add(new List<string>{"Weather", "Feather", "Pepper"} );
//		session_words.Add(new List<string>{"Sandal", "Candle", "Camel"} );
//		session_words.Add(new List<string>{"Sailboat", "Raincoat", "Rainbow"} );
//		session_words.Add(new List<string>{"Zipper", "Slipper", "Sticker"} );
//		session_words.Add(new List<string>{"Mother", "Brother", "Father"} );
//		session_words.Add(new List<string>{"Costume", "Bathroom", "Cartoon"} );
//		session_words.Add(new List<string>{"Gorilla", "Vanilla", "Umbrella"} );

		// Add left right bins for each trial

		Debug.Log (session_words [0]);
		// Find the main canvas and other things, which get called later 
		MainCanvas = GameObject.Find("Canvas");

		DL_object = GameObject.Find ("DataLogger"); 
		Logger = DL_object.GetComponent<DataLogger> ();

		//Write header to files 
		//header string 
		string[] header = {"Trial", "RT", "Accuracy", "Word", "Correct", "Incorrect", "Side", "Pressed"  };
		Logger.LogHeader (header);




		//Downloaded results from json to object 


		//Call co-routine which begins the actual game 
		StartCoroutine(MainLoop());

	}


	//This is the Coroutine for the main trial 
	// The IEnumerator functions will run until they reach a 'yield return' statement, and then restart in the next frame 
	// As we want timing, we use WaitForSecondsRealTime(seconds) after yield return to control timing. 
	public IEnumerator MainLoop(){



//		//loop through the main list and choose words/sprites

		indx = 0; 
		foreach (List<string> word_trial in session_words) {

			//reset target rect positions 
			LeftOption.transform.rotation = Quaternion.Euler(0f,0f,0.1f);
			RightOption.transform.rotation = Quaternion.Euler(0f,0f,0.1f);
			clockwise = true; 


			yield return new WaitForSeconds (0.5f); 

			Debug.Log (word_trial [0]);
			//Set Cue image 
			string str_cue_main = "Sprites/" + word_trial [0];
			string str_cue_audio = "Sounds/Phono_Awareness_" + word_trial [0];
			Sprite sCue_main = Resources.Load (str_cue_main, typeof(Sprite)) as Sprite;
			Debug.Log (sCue_main); 
			Cue_sprite.sprite = sCue_main;

			Cue.SetActive (true);


			AudioClip Aclip = Resources.Load (str_cue_audio, typeof(AudioClip)) as AudioClip;
			AudioObjectSource.clip = Aclip; 
			AudioObjectSource.Play (); 

			yield return new WaitForSecondsRealtime (2); 


			yield return new WaitForSecondsRealtime (0.95f); 
			//what side is the correct target going on? 

			//if correct option will be on the left
			if (left_right [indx] == 0) {

				//' WORD1 '
				// Left cue is 1st word in list
				string str_cue_left = "Sprites/" + word_trial [1];
				Sprite sCue_left = Resources.Load (str_cue_left, typeof(Sprite)) as Sprite;
				Debug.Log (sCue_left); 
				Left_sprite.sprite = sCue_left;

				LeftOption.SetActive (true);


				// ' OR'
				yield return new WaitForSecondsRealtime (2); 


				yield return new WaitForSecondsRealtime (0.5f); 

				// ' WORD2' 
				//right cue is second word in list 
				string str_cue_right = "Sprites/" + word_trial [2];

				Sprite sCue_right = Resources.Load (str_cue_right, typeof(Sprite)) as Sprite;
				Debug.Log (sCue_right); 
 

				Right_sprite.sprite = sCue_right;

				RightOption.SetActive (true); 

			} 

			if (left_right [indx] == 1) {

				// Left cue is 2nd word in list
				string str_cue_left = "Sprites/" + word_trial [2];

				Sprite sCue_left = Resources.Load (str_cue_left, typeof(Sprite)) as Sprite;
				Debug.Log (sCue_left); 
				Left_sprite.sprite = sCue_left;
 

				LeftOption.SetActive (true);

				// ' OR'
				yield return new WaitForSecondsRealtime (2); 

				yield return new WaitForSecondsRealtime (0.5f); 

				//right cue is  1st word in list 
				string str_cue_right = "Sprites/" + word_trial [1];
				Sprite sCue_right = Resources.Load (str_cue_right, typeof(Sprite)) as Sprite;
				Debug.Log (sCue_right); 
				Right_sprite.sprite = sCue_right;


				RightOption.SetActive (true);

			} 

			//Now we have to wait for a response 
			//temporarily wait for a second
			yield return new WaitForSecondsRealtime (2); 

			//flag that we are waiting for response
			wait4response = true; 

			float start_time = Time.fixedTime; 

			//stay in this loop until we get a response (i.e. one of the options has been selected)
			yield return new WaitWhile (() => wait4response == true );

			float rt = Time.fixedTime - start_time; 
			session_rt.Add (rt); 

			//Log results of this file 
			//Make row
			string[] results = {
				indx.ToString(),
				rt.ToString(),
				session_choices [indx].ToString(),
				session_words [indx] [0],
				session_words [indx] [1],
				session_words [indx] [2],
				left_right [indx].ToString(),
				chose_side.ToString()
			};

			Logger.LogData (results);



			//proceed if flagged response (left= true OR right = true) 

			// make function that is called on pressing 

			//Hide all of the objects 
			foreach (GameObject objects in pHolderList) {

				objects.SetActive (false);
			}



			//update index and reset flags
			wait4response = false; 
			isleft= false;
			isright = false; 
			indx += 1; 
		}	


		//Close file 
		Logger.Close(); 

		endgame = true; 
		StartCoroutine(CheckEnd()); 

	}









}
