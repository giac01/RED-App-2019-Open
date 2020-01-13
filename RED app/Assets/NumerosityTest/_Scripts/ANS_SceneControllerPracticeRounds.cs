using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ANS_SceneControllerPracticeRounds : MonoBehaviour
{
    //time parameters in the game
    public float observationtime; //length of time that stimuli are shown for
    public float fixationtime; //length of time (secs) that fixation cross is shown for 
    public float timetorespond; //length of time (secs) that pps are allowed to respond with. 

    public Sprite[] stimuli;
    private GameObject spriteleft;
    private GameObject spriteright;
    public List<string> stimulinames;
    public List<string> presorder2;
    private int indexleft;
    private int indexright;
    public GameObject fixationcross; //asigned in gui
    public GameObject noresponsetext; // assigned in gui
    public GameObject secsleftx; // assigned in gui
    public GameObject whichhasmore; //asigned in gui
    public GameObject circle_white; //asssigned in gui, used for showing the time on a clock 
    public GameObject feedback_canvas;
    public GameObject IntroAudio;

    public bool AllowResponse;


    private float timevar1;
    private float timeleft;



    //public bool endaftertimeout; //if set to TRUE then 
    //public Button leftbutton;
    //public Button rightbutton;

    private GameObject fixationclone;
    public GameObject responseclone;
    private GameObject feedbackclone;

    public bool nextexamplepress = false;
    private float waitsecs;
    //  public GameObject secsleftclone;

    public int trialnum;
    public bool trialend; //This allows the scripts on the response buttons to set the end of the trial!
    private GameObject secsleftclone;
    public bool noresponse; //Assesses if a response has been given for a specific trial

    //Save responses
	public DataLogger Logger;
    public List<string> sidepress;
    public List<string> correctside = new List<string>(new string[] { "R", "L", "L" });
    private List<string> feedbacktext = new List<string>(new string[] {
        "Remember that even though the dots on the right are smaller, there are three dots on the right, and only two dots on the left, so right was the correct choice.",
        "Remember that even though the dots on the left are less spread out, we can count more dots on the left, so left was the correct choice.",
        "In the examples that follow, you will be presented with many dots, and you won't have time to count them all. Look carefully and try to make your best guess. In this example, left was the correct choice." });

	public AudioClip Correctbeep;
	public AudioClip Incorrectbeep;


    void Awake()
    {
        AllowResponse = false;
		trialnum = 0;
        //Set canvas elements to blank initially 
        secsleftx.SetActive(false);
        noresponsetext.SetActive(false);
        circle_white.SetActive(false);
        whichhasmore.SetActive(false);

        IntroAudio = GameObject.Find("IntroAudio");



        presorder2 = new List<string>(new string[] {
                        "1A", "1B",
                        "2B", "2A",
                        "3A", "3B"});

        spriteleft = GameObject.Find("SpriteLeft");
        spriteright = GameObject.Find("SpriteRight");
        stimuli = Resources.LoadAll<Sprite>("PracticeStimuli") as Sprite[];
        //print(SecsLeftCanvas);

        //Get names of imported stimuli objects <stimuli> and create names <stimulinames>
        foreach (Sprite i in stimuli)
        {
            stimulinames.Add(i.name.ToString());
            //print(i);
        }

		Logger = GameObject.Find ("Logger").GetComponent<DataLogger> ();
		string[] header = new string[] {"QuestionNumber", "TimeStartQuestion", "RT", "ResponseSide", "ResponseCorrect"};
		Logger.LogHeader( header);
		print(string.Concat(header[0]," ",header[1]," ",header[2]," ",header[3]," ",header[4]));


		StartCoroutine("PresentStimuliPR");



    }


    IEnumerator PresentStimuliPR()
    {
        while (trialnum < 3)
        {
            trialend = false; nextexamplepress = false; noresponse = true;
			float timeStartTrial = Time.time;
            //Show Fixation Cross for X seconds
            fixationclone = Instantiate(fixationcross) as GameObject;
            yield return new WaitForSecondsRealtime(fixationtime);
            Destroy(fixationclone);
            //Show Number blobs for X seconds
            timevar1 = Time.time; //Time and start of response
            indexleft = stimulinames.IndexOf(presorder2[trialnum * 2]); //Finds which stimuli element in the list <stimuli> should be presented on a given trial
            indexright = stimulinames.IndexOf(presorder2[trialnum * 2 + 1]);
            spriteleft.GetComponent<SpriteRenderer>().sprite = stimuli[indexleft];
            spriteright.GetComponent<SpriteRenderer>().sprite = stimuli[indexright];
            whichhasmore.SetActive(true);
            circle_white.SetActive(true);
            AllowResponse = true;

            StartCoroutine("BlinkGameObject");

            yield return new WaitUntil(() => (trialend == true));
			float trialRT = Time.time - timeStartTrial;
            circle_white.SetActive(false);
            AllowResponse = false;

            //Delete number blobs and show text if no response is given. 
            spriteleft.GetComponent<SpriteRenderer>().sprite = null;
            spriteright.GetComponent<SpriteRenderer>().sprite = null;
            whichhasmore.SetActive(false);


            StartCoroutine("SetFeedbackText");
            yield return new WaitUntil(()=> nextexamplepress == true);

            Destroy(feedbackclone);
            IntroAudio.GetComponent<AudioSource>().Stop();

			Logger.LogData( new string[] {
				(trialnum+1).ToString(),
				timeStartTrial.ToString(),
				trialRT.ToString(),
				sidepress[trialnum].ToString(),
				(sidepress[trialnum] == correctside[trialnum]).ToString()
			});
            trialnum++;
            circle_white.SetActive(false);
        }
			Logger.Close ();
			SceneManager.LoadScene("ANS_MainTask");

    }



    void Update()
    {
        SetCanvasTimer(); //here the countdown timer in the response section is running

    }

    public void SetCanvasTimer() //here the countdown timer in the response section is running
    {
        if (spriteleft.GetComponent<SpriteRenderer>().sprite == null)
        {
            secsleftx.SetActive(false); //circle_white.SetActive(false);
        }

        if (spriteleft.GetComponent<SpriteRenderer>().sprite != null) //This updates the seconds remaining gui elements.
        {
            secsleftx.SetActive(true); //circle_white.SetActive(true);
            timeleft = Mathf.Max(Mathf.Round(observationtime + .49f + (timevar1 - Time.time)), 0f);
            // timeleft = string timeleft.ToString("R");
            GameObject.Find("SecsLeftX").GetComponent<Text>().text = timeleft.ToString("R");
            //Update the clock
            circle_white.GetComponent<Image>().fillAmount = (Time.time - timevar1) / observationtime;
        }

    }


    public void TaskOnClickL() //activates when the response buttons are clicked! these are activated my onmousedown() functions in a script attached to the object.
    {
        if (AllowResponse){
            print("left click");
            trialend = true;
            sidepress.Add("L");
            noresponse = false;
            AllowResponse = false; //don't allow additional button presses. 
        }


    }

    public void TaskOnClickR() //activates when the response buttons are clicked! these are activated my onmousedown() functions in a script attached to the object.
    {
        if (AllowResponse){
            print("right click");
            trialend = true;
            sidepress.Add("R");
            noresponse = false;
            AllowResponse = false; //don't allow additional button presses. 

        }   
    }

    void TaskOnClickNA() //activates when no button is pressed- DEPRECIATED? 
    {
        trialend = true;
        sidepress.Add("NA");
    }

    IEnumerator SetFeedbackText() //Sets feedback test (e.g. CORRECT or INCORRECT) 
	{
		feedbackclone = Instantiate (feedback_canvas) as GameObject;

		feedbackclone.transform.Find ("Button").gameObject.SetActive (false);

		GameObject.Find ("Explanation").GetComponent<Text> ().text = feedbacktext [trialnum];
		if (sidepress [trialnum] == correctside [trialnum]) {//Correct response
			GameObject.Find ("Result").GetComponent<Text> ().text = "CORRECT";
			GameObject.Find ("Result").GetComponent<Text> ().color = Color.green;
			IntroAudio.GetComponent<AudioSource> ().PlayOneShot (Correctbeep);
			yield return new WaitUntil (() => !IntroAudio.GetComponent<AudioSource> ().isPlaying);
			IntroAudio.GetComponent<AudioSource> ().PlayOneShot (IntroAudio.GetComponent<IntroAudioScript> ().GameAudio [IntroAudio.GetComponent<IntroAudioScript> ().GameAudioNames.IndexOf ("correct")]);
			yield return new WaitUntil (() => !IntroAudio.GetComponent<AudioSource> ().isPlaying);

		}
		if (sidepress [trialnum] != correctside [trialnum]) {//Incorect response
			GameObject.Find ("Result").GetComponent<Text> ().text = "INCORRECT";
			GameObject.Find ("Result").GetComponent<Text> ().color = Color.red;
			IntroAudio.GetComponent<AudioSource> ().PlayOneShot (Incorrectbeep);
			yield return new WaitUntil (() => !IntroAudio.GetComponent<AudioSource> ().isPlaying);
			IntroAudio.GetComponent<AudioSource> ().PlayOneShot (IntroAudio.GetComponent<IntroAudioScript> ().GameAudio [IntroAudio.GetComponent<IntroAudioScript> ().GameAudioNames.IndexOf ("incorrect")]);
			yield return new WaitUntil (() => !IntroAudio.GetComponent<AudioSource> ().isPlaying);

		}

        

		IntroAudio.GetComponent<AudioSource> ().PlayOneShot (IntroAudio.GetComponent<IntroAudioScript> ().GameAudio [IntroAudio.GetComponent<IntroAudioScript> ().GameAudioNames.IndexOf (string.Concat ("p", (trialnum + 1)))]);

		//Activate next button after audio stopped playing
		yield return new WaitUntil (() => !IntroAudio.GetComponent<AudioSource> ().isPlaying);
		feedbackclone.transform.Find ("Button").gameObject.SetActive (true);
		if (trialnum == 2) {
			GameObject.Find ("Button").transform.Find ("Text").GetComponent<Text> ().text = "Start Main Task";
			//GameObject.Find("Button").GetComponent<Button>().transition.;
			GameObject.Find ("Button").GetComponent<RectTransform> ().localPosition = new Vector3 (0f, -250f, 0f);
			GameObject.Find ("Button").GetComponent<RectTransform> ().localScale = new Vector3 (1.5f, 1.5f, 1.5f);
		}

	}
    IEnumerator SetTrialEndTrueAfterSecs() //this coroutine ends the trial after (var timetosrepond) seconds, but does not activate trial end if button is already pressed!
    {
        yield return new WaitUntil(() => (Time.time - timevar1) > timetorespond);
        if (GameObject.Find("ResponseCanvas(Clone)") != null) //only activate endtrial if button has not already been pressed. 
        {
            TaskOnClickNA();
           // print("Executed");
        }
        yield break;
    }

    IEnumerator BlinkGameObject()  //NO LONGER USED!
    {
        yield return new WaitUntil(() => (Time.time - timevar1) > observationtime);
        while (trialend == false)
        {
                circle_white.SetActive(false);
                yield return new WaitForSeconds(.5f);
                circle_white.SetActive(true);
                yield return new WaitForSeconds(.5f);
            
        }
        circle_white.SetActive(false);
    }
}

