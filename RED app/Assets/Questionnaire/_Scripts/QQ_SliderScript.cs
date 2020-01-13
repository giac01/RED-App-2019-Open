using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Linq;

public class QQ_SliderScript : MonoBehaviour, IPointerDownHandler {
    [Header("Slider Attributes")]
    public int NumberOfCategories;
    public string text1;
    public string text2;
    public string text3;
    public string text4;
    public string text5;
	public string text6;
	public string text7;
	//public string text8;

    public List<string> text = new List<string>();
    public string noresponsetext; // sets the no response feedback text when clickvirgin = true (before pps makes response or when the slider is moved back in the no response state), 

    public float maxvalue;
    public bool usebannedvalue; //if equal to false there are no banned values (see below)
    public float bannedvalue; //sets the slider to clickvirgin=true (non response) if there are values on the slider the user should not select.

	//public float TimeLastClick;

    public bool UseLikertFeedback = true; //default feedback style
    public bool UseHoursFeedback = false;
    public bool UseNumberFeedback = false;

    [Header("Logic For Slider to Work")]
    public bool clickvirgin = true; //Set true when the slider has not been touched, allows us to know when the question has been answered.

    public GameObject Background; //child gameObjects of slider which have properties we want to change
    public GameObject Fill; //child gameObjects of slider which have properties we want to change
    public GameObject ResponseFeedback;


    public string PreviousSelection = "null";
    public string CurrentSelection;

    public GameObject SceneManager;
    public GameObject VolumeIcon;

   // public MonoBehaviour GameControllerScriptB; //This is set in each scenecontroller!
    //public Component GameControllerScript; //This is set in each scenecontroller!

    //public string GameControllerScriptName;


    public List<float> locationOfEachScale = new List<float>();
    public List<float> minRange = new List<float>();
    public List<float> maxRange = new List<float>();

   
    public float halfDistance; //range to the left and right of each category on the scale
	[Header("Other")]
	public QQSceneManagerClass SceneManagerScript;


    // Use this for initialization
    void Awake () {
        SceneManager = GameObject.FindWithTag("GameController");
		SceneManagerScript = SceneManager.GetComponent<QQSceneManagerClass>();
        //GameControllerScript = SceneManager.GetComponent(typeof(MonoBehaviour)) as MonoBehaviour;
        //GameControllerScript = SceneManager.GetComponent<MonoBehaviour>() as MonoBehaviour;
        //GameControllerScriptB = SceneManager.GetComponent<MonoBehaviour>() as MonoBehaviour;
        //GameControllerScriptName = GameControllerScriptB.GetType().ToString();
        //GameControllerScript = SceneManager.GetComponent(GameControllerScriptName);
       //.Gam


        //print(GameControllerScript.GetComponent<Q2SceneControllerScript>().GameAudio);

        VolumeIcon = GameObject.FindWithTag("VolumeIcon");

        transform.Find("SliderText (1)").GetComponent<Text>().text = text1; //text which goes into the boxes
        transform.Find("SliderText (2)").GetComponent<Text>().text = text2;
        transform.Find("SliderText (3)").GetComponent<Text>().text = text3;
        transform.Find("SliderText (4)").GetComponent<Text>().text = text4;
        transform.Find("SliderText (5)").GetComponent<Text>().text = text5;
        GetComponent<Slider>().onValueChanged.AddListener(delegate { ValueChange(); }); //adds a listener for value change in slider

        text.Add(text1); text.Add(text2); text.Add(text3); text.Add(text4); text.Add(text5);

        if (UseHoursFeedback == false)
        {

            SetVariableRanges();
        }

        setcolour();
    }

    // Update is called once per frame
    void Update () {



    }

    public void ValueChange() //runs when slider is moved
    {
        clickvirgin = false;
		SceneManagerScript.TimeLastClick = Time.time;
        if (usebannedvalue == true) //sets clickvirgin back to true is default (banned) response is used. 
        {
            if (bannedvalue == GetComponent<Slider>().value)
            {
                clickvirgin = true;
            }
        }

        if (UseLikertFeedback == true)
        {
            setfeedbacktext();
        }
        if (UseHoursFeedback == true)
        {
            setfeedbacktext_cont();
        }
        if (UseNumberFeedback == true)
        {
            setfeedbacktext_cont100();
        }
		if (!UseHoursFeedback && !UseNumberFeedback) {
			StartCoroutine ("PlayAudioWhenSliderSelectionChanged");
		}
        setcolour();

    }

    IEnumerator PlayAudioWhenSliderSelectionChanged()
    {
        //print("Audiochange");
        yield return new WaitForSecondsRealtime(.3f);
        if (PreviousSelection != CurrentSelection)
        {
            //print("Audio Play");
			if (VolumeIcon.GetComponent<AudioSource>().isPlaying == false | VolumeIcon.GetComponent<AudioSource>().isPlaying == true)
            {
                //print("play audio");
                //VolumeIcon.GetComponent<AudioSource>().PlayOneShot(AudioNames.IndexOf(CurrentSelection.ToLower())]);
				VolumeIcon.GetComponent<AudioSource>().Stop();

                VolumeIcon.GetComponent<AudioSource>().PlayOneShot(SceneManager.GetComponent<QQSceneManagerClass>().GameAudio[SceneManager.GetComponent<QQSceneManagerClass>().AudioNames.IndexOf(CurrentSelection.ToLower())]);
                PreviousSelection = CurrentSelection;

            }
            //VolumeIconSource.GetComponent<AudioSource>().PlayOneShot(GameAudio[AudioNames.IndexOf("Q1.Q1")]);
        }

    }


    void SetVariableRanges()
    {
        halfDistance = maxvalue / ((NumberOfCategories - 1)*2);
        //print(halfDistance);
        foreach (int i in Enumerable.Range(0, NumberOfCategories).ToArray())
        {
            //print(i);
            locationOfEachScale.Add((float)(0 + i * 2 * halfDistance));
            minRange.Add((float)(-halfDistance + i * 2 * halfDistance));
            maxRange.Add((float)(halfDistance + i * 2 * halfDistance));

        }

    }
    void setfeedbacktext() //sets the feedback response text box for STANDARD LIKERT RESPONSES
    {
        
        maxvalue = GetComponent<Slider>().maxValue; //this allows the correct feedback text to be set when the max value is not 1
        foreach (int i in Enumerable.Range(0, NumberOfCategories).ToArray())
        {
            if ( (minRange[i] < GetComponent<Slider>().value) &&  (GetComponent<Slider>().value <= maxRange[i]))
            {
                ResponseFeedback.GetComponent<Text>().text = text[i];
                CurrentSelection = text[i];


            }


        }

      
    }
    void setfeedbacktext_cont() //sets the feedback response text FOR TIME SCALE RESPONSES
    {
        if (GetComponent<Slider>().value == 0)
        {
            ResponseFeedback.GetComponent<Text>().text = "Never";
        }
        if ((GetComponent<Slider>().value > 0 ) && ((GetComponent<Slider>().value*maxvalue < 1)) )
        {
            ResponseFeedback.GetComponent<Text>().text = string.Concat(Mathf.Round(GetComponent<Slider>().value*maxvalue*60).ToString(), " Minutes");
        }
        if ((GetComponent<Slider>().value*maxvalue >= 1) && ((GetComponent<Slider>().value < 1)))
        {
            ResponseFeedback.GetComponent<Text>().text = string.Concat(Mathf.Floor(GetComponent<Slider>().value*maxvalue).ToString(), " Hours and ",
                                                                        Mathf.Round(GetComponent<Slider>().value*maxvalue*60- Mathf.Floor(GetComponent<Slider>().value *maxvalue)*60), " Minutes");
        }
        if ((GetComponent<Slider>().value * maxvalue == maxvalue))
        {
            ResponseFeedback.GetComponent<Text>().text = string.Concat("More than ", Mathf.Floor(GetComponent<Slider>().value * maxvalue).ToString(), " Hours");
        }


        //ResponseFeedback.GetComponent<Text>().text = (GetComponent<Slider>().value*maxvalue).ToString();



        CurrentSelection = GetComponent<Slider>().value.ToString();

    }
    void setfeedbacktext_cont100() //sets the feedback response text FOR TIME SCALE RESPONSES
    {
        if (GetComponent<Slider>().value == 0)
        {
            ResponseFeedback.GetComponent<Text>().text = "None";
        }
		if ((GetComponent<Slider>().value > 0) && ((GetComponent<Slider>().value < maxvalue)))
        {
            ResponseFeedback.GetComponent<Text>().text = Mathf.Round(GetComponent<Slider>().value * 100).ToString();
        }

        if ((GetComponent<Slider>().value == maxvalue))
        {
            ResponseFeedback.GetComponent<Text>().text = "More than 200";
        }


        //ResponseFeedback.GetComponent<Text>().text = (GetComponent<Slider>().value*maxvalue).ToString();



        CurrentSelection = GetComponent<Slider>().value.ToString();

    }




    void setcolour() //On the first click it sets the colour of the text and slider to green
    {
        if (clickvirgin == false) //after pps made response
        {
            Background.GetComponent<Image>().color = Color.white;
            Fill.GetComponent<Image>().color = new Color(133/255f, 133/255f, 133/255f, 255/255f);
            transform.Find("responsefeedback").GetComponent<Text>().color = Color.green;
            transform.Find("Handle Slide Area").transform.Find("Handle").GetComponent<Image>().color = Color.green;
            GameObject.Find("Button").GetComponent<Image>().color = Color.green;


            //Set text above the slider (e.g. Never, rarely, sometimes, usually, always) to white
            transform.Find("SliderText (1)").GetComponent<Text>().color = Color.white;
            transform.Find("SliderText (2)").GetComponent<Text>().color = Color.white;
            transform.Find("SliderText (3)").GetComponent<Text>().color = Color.white;
            transform.Find("SliderText (4)").GetComponent<Text>().color = Color.white;
            transform.Find("SliderText (5)").GetComponent<Text>().color = Color.white;

			if (UseHoursFeedback == true) {
				transform.Find("SliderText (6)").GetComponent<Text>().color = Color.white;
				transform.Find("SliderText (7)").GetComponent<Text>().color = Color.white;
			
			}


        }

        if (clickvirgin == true) //before pps made response - this is so the slider can be set BACK to this mode in some cases!
        {
            transform.Find("responsefeedback").GetComponent<Text>().color = Color.red;
            Background.GetComponent<Image>().color = Color.red;
            Fill.GetComponent<Image>().color = Color.red;
            transform.Find("responsefeedback").GetComponent<Text>().text = noresponsetext;
            transform.Find("Handle Slide Area").transform.Find("Handle").GetComponent<Image>().color = Color.red;

            //Set text above the slider (e.g. Never, rarely, sometimes, usually, always) to red
            transform.Find("SliderText (1)").GetComponent<Text>().color = Color.red;
            transform.Find("SliderText (2)").GetComponent<Text>().color = Color.red;
            transform.Find("SliderText (3)").GetComponent<Text>().color = Color.red;
            transform.Find("SliderText (4)").GetComponent<Text>().color = Color.red;
            transform.Find("SliderText (5)").GetComponent<Text>().color = Color.red;
            GameObject.Find("Button").GetComponent<Image>().color = Color.red;

			if (UseHoursFeedback == true) {
				transform.Find("SliderText (6)").GetComponent<Text>().color = Color.red;
				transform.Find("SliderText (7)").GetComponent<Text>().color = Color.red;

			}



        }
    }

	public void OnPointerDown(PointerEventData eventData) 
	{

		if (usebannedvalue == true) { // Prevent slider going green / allowing next question if banned value 
			if (bannedvalue == GetComponent<Slider> ().value) {
				//Do nothing
			}
		} else 
		{
			clickvirgin = false;
			setcolour();
			SceneManager.GetComponent<QQSceneManagerClass> ().OnSliderMove ();
			ValueChange ();
		}



	}


}



//N.B. You can use GetComponent<Slider>().value
//N.B. This works too:         GetComponentInChildren<Image>().color = Color.green;
//transform.Find("Background").GetComponent<Image>().color = Color.red;