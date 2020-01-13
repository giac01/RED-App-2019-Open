using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net; //for string.Join function! 
using System.Linq;
using System.Text.RegularExpressions;

public class QQSceneManagerClass : MonoBehaviour {


    [Header("Data to Save")]
    public float TimeStartQuestion;
	public float TimeStartResponse; //Because children are only allowed to start a response AFTER 
    public float RTFirstSliderClick;
    public float RTLastSliderClick;
    public float RTClickNext;
	public float TimeLastClick; //When the latest click on the slider was performed. 
	public float TimeEndQuestion;
	public string Response;


    [Header("Game Logic")]
    public bool _areWeDoneYet = false;
    public int trialnum;
    public bool nextpress;
    public bool clickvirgin; //detects if slider has been touched yet, prevents skipping to next question.
    public string AudioTag; //This is used in SLIDERSCRIPT to know which audio to use!
	public bool AllowResponse;


    [Header("GameObjects")]
	public DataLogger Logger;
    public GameObject SliderClone;
    public AudioClip[] GameAudio;
    public List<string> AudioNames; //NAmes of each object in gameobject



    //Trial Questionnnaire Info
    public List<string> questiontext;

    //private string[] audioForEachQestion = new string[] {"Q1.Q1"};

    public int[] sliderselect;
        
    [Header("Slider Types")]
    public GameObject slider_binary;   //0
    public GameObject slider_cont;     //1
    public GameObject slider_likert5;  //2
    public GameObject slider_Grit;     //3
    public GameObject slider_RCADS;    //4
    public GameObject slider_Cont2;    //5
    public GameObject slider_Number;   //6
	public GameObject pictures_Tech;   //7
	public GameObject pictures_Allergies;   //8
	public GameObject slider_GrowthMindset;   //9



    public GameObject[] sliderlist;//all slider gameobjects in list.

    public GameObject VolumeIconSource; //Volume Icon Game Objects ASSIGNED IN GUI


    // Use this for initialization
    void Start()
    {
        trialnum = 0;
        LoadAudio();
        //sliderlist.Add(slider_binary); sliderlist.Add(slider_cont); sliderlist.Add(slider_likert5); sliderlist.Add(slider_Grit); sliderlist.Add(slider_RCADS);
        //print("start run");
        sliderlist =  new GameObject[] { slider_binary, slider_cont, slider_likert5, slider_Grit, slider_RCADS , slider_Cont2, slider_Number,
										pictures_Tech, pictures_Allergies, slider_GrowthMindset};

        //Add button listener 
        GameObject.Find("Canvas").transform.Find("Button").GetComponent<Button>().onClick.AddListener(TaskOnClick);

		//Retrieve data logger and set header 

		Logger = GameObject.Find ("Logger").GetComponent<DataLogger> ();
		string[] header = new string[] {
										"QuestionNumber", 
										"TimeStartQuestion", 
										"TimeStartResponse",
										"TimeEndQuestion", 

										"RTFirstSliderClick", 
										"RTLastSliderClick", 
										"RTClickNext", 

										"Response"};
		Logger.LogHeader( header);
		print(string.Concat(header[0]," ",header[1]," ",header[2]," ",header[3]," ",header[4]," ",header[5]," ",header[6])); 

        // Run trials.
        StartCoroutine("Q1StartTrial");
    }

    // Update is called once per frame
    void Update()
    {
        if (SliderClone != null)
        {
            //clickvirgin = SliderClone.transform.Find("Slider").GetComponent<Q1_SliderScript>().clickvirgin; //Temporary fix until i can get Q1_SliderScript to talk to the main scenemanager!!
        }

        // Check if the task is done yet.
        if (_areWeDoneYet)
        {
            // Load the next scene.
            UnityEngine.SceneManagement.SceneManager.LoadScene("InBetweenScene");
        }

    }
    public Color theColor;
    //public Slider SliderClone2;

    IEnumerator Q1StartTrial()
    {
        // int[] sliderselect = new int[] { 0, 0, 0, 1 };                                                                                                                                                                                                                                             
        yield return new WaitForSecondsRealtime(.1f);
        while (trialnum < questiontext.Count)
        {
			nextpress = false; clickvirgin = true; Response = "NULL"; TimeStartResponse = 0; TimeEndQuestion = 0; TimeStartQuestion = 0;RTFirstSliderClick = 0;RTLastSliderClick=0; TimeLastClick = 0; RTClickNext = 0;

            //Add Question, manipulate text, etc.

            SliderClone = Instantiate(sliderlist[sliderselect[trialnum]]) as GameObject;


            TimeStartQuestion = Time.time;

            SliderClone.name = "Slider_Clone";

			//Hide slider till end of question audio!
			if (SliderClone.GetComponent<PictureQuestionControlScript> () == null) { 
				SliderClone.transform.Find ("Slider").gameObject.SetActive (false);
			}
				
            SliderClone.transform.Find("QuestionText").GetComponent<Text>().text = questiontext[trialnum];
            SliderClone.transform.Find("QuestionNumber").GetComponent<Text>().text = string.Concat("Question ", trialnum + 1, ")");

	

			if (SliderClone.GetComponent<PictureQuestionControlScript> () == null) { 												//ignore for picture responses
				SliderClone.transform.Find ("Slider").GetComponent<Slider> ().onValueChanged.AddListener (delegate {
					OnSliderMove (); //N.B. this function is also called by a separate script on the slider which detects clicks without value changes!
				});
			}
            //Play Question Audio
            VolumeIconSource.GetComponent<AudioSource>().Stop();
            VolumeIconSource.GetComponent<VolumeIconClickScript>().OnMouseDown();

			//Wait till audio stops to show slider
			if (SliderClone.GetComponent<PictureQuestionControlScript> () == null) { 
				yield return new WaitUntil (() => VolumeIconSource.GetComponent<AudioSource> ().isPlaying == false);
				print ("AudioEnd");
				SliderClone.transform.Find ("Slider").gameObject.SetActive (true);
			}
			TimeStartResponse = Time.time ; //Time that the slider appears on the question


            //Wait till choice has been made (slider touched)
            yield return new WaitUntil(() => clickvirgin == false);
            RTFirstSliderClick = Time.time - TimeStartQuestion; //TIme that the first click appears on the screen

            GameObject.Find("Button").GetComponent<Image>().color = Color.green;

            //Wait till nextquestion button is pressed
			//print("wait");
			if (SliderClone.GetComponent<PictureQuestionControlScript> () == null) { 																								//ignore for picture responses
				yield return new WaitUntil (() => (nextpress == true) && (SliderClone.transform.Find ("Slider").GetComponent<QQ_SliderScript> ().clickvirgin == false));
			}
			if (SliderClone.GetComponent<PictureQuestionControlScript> () != null) { 																								//use for picture responses
				yield return new WaitUntil (() => (nextpress == true) && (clickvirgin == false));
			}


            // End of trial - calculate output data
            RTClickNext = Time.time - TimeStartQuestion;
			RTLastSliderClick = TimeLastClick - TimeStartQuestion;

            TimeEndQuestion = Time.time;
			if (SliderClone.GetComponent<PictureQuestionControlScript> () == null) { 
				Response = SliderClone.transform.Find("Slider").GetComponent<Slider>().value.ToString();//ignore for picture responses
				};
			if (SliderClone.GetComponent<PictureQuestionControlScript> () != null) { 

				string[] ItemSelectedString = SliderClone.GetComponent<PictureQuestionControlScript>().ItemSelected.Select(x=>x.ToString()).ToArray();
				string[] ResponseOptions = SliderClone.GetComponent<PictureQuestionControlScript>().ItemNames.Select(x=>Regex.Replace(x.ToString(), @"\s+", string.Empty)).ToArray();

				Response = Regex.Replace(
							string.Concat(
								string.Join("_", ItemSelectedString),
								"//",
								string.Join("_", ResponseOptions)),
						   ",", "")
							;
				//);
					//SliderClone.transform.Find("Slider").GetComponent<Slider>().value.ToString();//ignore for picture responses
			};

			Logger.LogData(new string[] 
				{
					(trialnum+1).ToString(),

					TimeStartQuestion.ToString(),
					TimeStartResponse.ToString(),
					TimeEndQuestion.ToString(),

					RTFirstSliderClick.ToString(),
					RTLastSliderClick.ToString(),
					RTClickNext.ToString(),

					Response.ToString()
				});
				

//            print(string.Concat(trialnum + 1, "     ",
//                               // SliderClone.transform.Find("Slider").GetComponent<Slider>().value, "      ",
//                                TimeStartQuestion, "     ",
//                                RTFirstSliderClick, "     ",
//                                RTClickNext, "     ",
//                                TimeEndQuestion));

            trialnum += 1;
            Destroy(SliderClone);

        }
		if (SceneManager.GetActiveScene().name=="Q3.Questions"){  //Load picture 
			GameObject.Find("Canvas").transform.Find("Button").gameObject.SetActive(false);

			//yield return new WaitUntil(()=> (_areWeDoneYet==true));
		}
		Logger.Close ();
		// Signal the Update function that we're done.
        _areWeDoneYet = true;

        yield return null;
    }

    public void OnSliderMove()
    {
        clickvirgin = false;

        //The next section avoids the situation where a binary response slider can be moved back to the middle (no response) after setting clickvirgin to true

        if (GameObject.Find("Slider_Binary(Clone)") != null)
        {
            if (GameObject.Find("Slider_Binary(Clone)").transform.Find("Slider").GetComponent<Slider>().value == 1)
            {
                clickvirgin = true;
                GameObject.Find("Button").GetComponent<Image>().color = Color.red;
				//print ("revertColour");

            }
            if (GameObject.Find("Slider_Binary(Clone)").transform.Find("Slider").GetComponent<Slider>().value != 1)
            {
                clickvirgin = false;
                GameObject.Find("Button").GetComponent<Image>().color = Color.green;
            }
        }

    }

    void TaskOnClick() //on "Next Question" Button click
    {
		if (SliderClone.GetComponent<PictureQuestionControlScript> () != null) { //next press for picture responses
			nextpress = true;
			//clickvirgin = false; //do not wait for response to be given!

		}
        else if (SliderClone.transform.Find("Slider").GetComponent<QQ_SliderScript>().clickvirgin == false) //this condition stops the button working when no response has been made yet!
        {
            nextpress = true;

        }

    }

    void LoadAudio()
    {
        GameAudio = Resources.LoadAll<AudioClip>("Audio") as AudioClip[];
        foreach (AudioClip i in GameAudio)
        {
            AudioNames.Add(i.name.ToString());

        }

        //VolumeIconSource.GetComponent<AudioSource>().PlayOneShot(GameAudio[1]);

    }




}

