using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using System;




public class ANS_SceneControllerMainTask : MonoBehaviour
{

	private bool _areWeDoneYet = false;

    //time parameters in the game
    [Header("Time Paramaters of the Task")]
    public float observationtime; //length of time that stimuli are shown for
    public float fixationtime; //length of time (secs) that fixation cross is shown for 
    public float timetorespond; //length of time (secs) that pps are allowed to respond with. 
	public bool UseSoundFeedback; 

    [Header("Game Stimuli")]
	public AudioClip CorrectAudio;
	public AudioClip IncorrectAudio;
	public AudioSource AudioSourceFeedback;
	public GameObject MissPrompt;
    public GameObject fixationcross; //asigned in gui
    public GameObject noresponsetext; // assigned in gui
    public GameObject secsleftx; // assigned in gui
    public GameObject whichhasmore; //asigned in gui
    public GameObject circle_white; //asssigned in gui, used for showing the time on a clock 
    //public GameObject yourscore;

    private Sprite[] stimuli;
    private GameObject spriteleft;
    private GameObject spriteright;
    public List<string> stimulinames;
    public List<string> presorder2;

    private int indexleft;
    private int indexright;


    private float timeStartTrial;
    private float timeStartTask;
    private float timeleft;
    private float timeTaskTaken;

    //public bool endaftertimeout; //if set to TRUE then 
    //public Button leftbutton;
    //public Button rightbutton;

    private GameObject fixationclone;
    public GameObject responseclone;
    private GameObject feedbackclone;

    [Header("Game Logic")]
    public bool enableresponse;     //this prevents the buttons from doing anything when we don't want them to create a response. 
    public bool nextexamplepress = false;
    private float waitsecs;
    //  public GameObject secsleftclone;

    public int trialnum;
    public bool trialend = false; //This allows the scripts on the response buttons to set the end of the trial!
    private GameObject secsleftclone;
    public bool noresponse; //Assesses if a response has been given for a specific trial
	public bool MissPromptContinue;

    //Trial Data

	public string[] correctSide; //also used for the slider to know the number of trials!
	private string[] stimuliOrder; //
    //Data to Export

    public List<string> sidepress = new List<string>();  //Lists order in which stimuli will be presented. 
    public List<bool> correctPress = new List<bool>();  //Lists order in which stimuli will be presented. 

    [Header("Game Data")]
	public DataLogger Logger;
    public float trialRT;
    //public string[] dataheader = new string[] {"ReactionTime", "SidePress", "CorrectPress"};  //Lists order in which stimuli will be presented. 
    public string[][] exportdata = new string[104][];
    public float dummmy; //DOES NOT DO ANYTHING.


    //Save responses
    //  public List<string> sidepress;

    void Awake()
    {
		stimuliOrder = new string[] {"65_1557_1", "65_1558_2", "44_1047_1", "44_1048_2", "32_758_2", "32_757_1", "77_1840_2", "77_1839_1", "44_1038_2", "44_1037_1", "100_2397_1", "100_2398_2", "94_2246_2", "94_2245_1", "25_587_1", "25_588_2", "20_474_2", "20_473_1", "17_404_2", "17_403_1", "7_163_1", "7_164_2", "66_1577_1", "66_1578_2", "93_2222_2", "93_2221_1", "47_1125_1", "47_1126_2", "18_429_1", "18_430_2", "63_1494_2", "63_1493_1", "71_1684_2", "71_1683_1", "44_1054_2", "44_1053_1", "54_1288_2", "54_1287_1", "36_845_1", "36_846_2", "49_1169_1", "49_1170_2", "98_2347_1", "98_2348_2", "18_421_1", "18_422_2", "18_414_2", "18_413_1", "49_1158_2", "49_1157_1", "51_1208_2", "51_1207_1", "68_1617_1", "68_1618_2", "42_992_2", "42_991_1", "16_369_1", "16_370_2", "85_2037_1", "85_2038_2", "50_1194_2", "50_1193_1", "49_1166_2", "49_1165_1", "31_742_2", "31_741_1", "78_1864_2", "78_1863_1", "75_1782_2", "75_1781_1", "22_525_1", "22_526_2", "43_1025_1", "43_1026_2", "59_1415_1", "59_1416_2", "67_1596_2", "67_1595_1", "20_463_1", "20_464_2", "58_1388_2", "58_1387_1", "6_128_2", "6_127_1", "52_1245_1", "52_1246_2", "3_70_2", "3_69_1", "63_1507_1", "63_1508_2", "96_2283_1", "96_2284_2", "13_289_1", "13_290_2", "91_2162_2", "91_2161_1", "15_348_2", "15_347_1", "6_125_1", "6_126_2", "58_1379_1", "58_1380_2", "2_44_2", "2_43_1", "79_1885_1", "79_1886_2", "16_367_1", "16_368_2", "39_933_1", "39_934_2", "24_572_2", "24_571_1", "55_1305_1", "55_1306_2", "34_811_1", "34_812_2", "99_2367_1", "99_2368_2", "72_1711_1", "72_1712_2", "77_1837_1", "77_1838_2", "8_180_2", "8_179_1", "59_1394_2", "59_1393_1", "3_50_2", "3_49_1", "46_1100_2", "46_1099_1", "46_1104_2", "46_1103_1", "17_387_1", "17_388_2", "66_1562_2", "66_1561_1", "54_1280_2", "54_1279_1", "36_856_2", "36_855_1", "43_1030_2", "43_1029_1", "32_745_1", "32_746_2", "69_1641_1", "69_1642_2", "24_563_1", "24_564_2", "47_1114_2", "47_1113_1", "90_2141_1", "90_2142_2", "74_1756_2", "74_1755_1", "58_1384_2", "58_1383_1", "84_2007_1", "84_2008_2", "23_540_2", "23_539_1", "82_1955_1", "82_1956_2", "35_830_2", "35_829_1", "27_648_2", "27_647_1", "51_1217_1", "51_1218_2", "62_1467_1", "62_1468_2", "25_583_1", "25_584_2", "100_2379_1", "100_2380_2", "66_1566_2", "66_1565_1", "77_1844_2", "77_1843_1", "79_1877_1", "79_1878_2", "37_883_1", "37_884_2", "96_2290_2", "96_2289_1", "36_841_1", "36_842_2", "55_1316_2", "55_1315_1", "81_1924_2", "81_1923_1", "88_2108_2", "88_2107_1", "45_1080_2", "45_1079_1", "95_2259_1", "95_2260_2", "5_97_1", "5_98_2", "24_557_1", "24_558_2", "67_1600_2", "67_1599_1", "32_764_2", "32_763_1", "57_1363_1", "57_1364_2", "96_2294_2", "96_2293_1", "45_1069_1", "45_1070_2", "72_1713_1", "72_1714_2", "56_1341_1", "56_1342_2", "56_1339_1", "56_1340_2", "26_613_1", "26_614_2", "14_322_2", "14_321_1", "16_362_2", "16_361_1", "28_653_1", "28_654_2", "83_1990_2", "83_1989_1", "19_454_2", "19_453_1", "88_2110_2", "88_2109_1", "66_1571_1", "66_1572_2"};

		correctSide = new string[] { "R", "L", "R", "L", "L", "R", "R", "R", "L", "L", "L", "R", "L", "R", "R", "L", "L", "R", "L", "R", "R", "L", "R", "L", "R", "L", "R", "L", "R", "R", "L", "L", "L", "R", "L", "R", "L", "R", "L", "R", "R", "L", "R", "L", "R", "L", "R", "L", "L", "R", "R", "L", "L", "R", "R", "R", "L", "R", "L", "L", "L", "R", "L", "R", "L", "R", "L", "R", "L", "L", "R", "R", "L", "R", "R", "L", "R", "R", "L", "L", "L", "R", "R", "L", "L", "R", "L", "R", "R", "L", "R", "L", "L", "R", "R", "R", "L", "L", "L", "L", "R", "R", "L", "R", "L", "R", "R", "L", "L", "R", "R", "L", "L", "R", "R", "L"};

		trialnum = 0;
        //Set canvas elements to blank initially 
        secsleftx.SetActive(false);
        noresponsetext.SetActive(false);
        circle_white.SetActive(false);
        whichhasmore.SetActive(false);
        //yourscore.SetActive(false);

        //Set Which Side Is Correct
        
        print(String.Concat("Number of stimuli:", stimuliOrder.Length/2));
        spriteleft = GameObject.Find("SpriteLeft");
        spriteright = GameObject.Find("SpriteRight");
        stimuli = Resources.LoadAll<Sprite>("Stimuli") as Sprite[];

        spriteleft.transform.localScale = new Vector3(583f/stimuli[0].rect.width, 583f / stimuli[0].rect.width, 1f);
        spriteright.transform.localScale = new Vector3(583f / stimuli[0].rect.width, 583f / stimuli[0].rect.width, 1f);

        //Get names of imported stimuli objects <stimuli> and create names <stimulinames>
        foreach (Sprite i in stimuli)
        {
            stimulinames.Add(i.name.ToString());
        }

		//Data Logger - Set header
		Logger = GameObject.Find ("Logger").GetComponent<DataLogger> ();
		string[] header = new string[] {"QuestionNumber", "LeftStimuliName", "RightStimuliName", "TimeStartQuestion", "RT", "ResponseSide", "ResponseCorrect"};
		Logger.LogHeader( header);
		print(string.Concat(header[0]," ",header[1]," ",header[2]," ",header[3]," ",header[4]));

		//Set up the miss prompter
		MissPrompt.SetActive(false);
		MissPromptContinue = false;
		MissPrompt.transform.Find ("Button").gameObject.GetComponent<Button> ().onClick.AddListener (MissPromptClick);

		AudioSourceFeedback = gameObject.GetComponent<AudioSource> ();

        timeStartTask = Time.time;
        StartCoroutine("PresentStimuli");
 



    }


    IEnumerator PresentStimuli()
    {
        yield return new WaitForSecondsRealtime(.5f);
        //taskstarted = true;
        while (trialnum < (stimuliOrder.Length/2))
        {
            //Set up trial logic
            trialend = false; nextexamplepress = false; noresponse = true; enableresponse = false;

			if (trialnum > 1) { //prompt user after two missed rounds 
				if(sidepress[trialnum-1].ToString()=="NA" && sidepress[trialnum-2].ToString()=="NA"){
					print ("runMISSPROMPT");
					MissPrompt.SetActive (true);
					yield return new WaitUntil (() => MissPromptContinue == true);
					MissPrompt.SetActive (false);
					MissPromptContinue = false;

				}
			}

            //Present Fixation Cross
                fixationclone = Instantiate(fixationcross) as GameObject;

            //Present Dot Blobs
                yield return new WaitForSecondsRealtime(fixationtime);
                Destroy(fixationclone);
                enableresponse = true;
                timeStartTrial = Time.time; //Time and start of response

                indexleft = stimulinames.IndexOf((stimuliOrder[trialnum * 2]));
                indexright = stimulinames.IndexOf((stimuliOrder[trialnum * 2 + 1]));

                spriteleft.GetComponent<SpriteRenderer>().sprite = stimuli[indexleft];
                spriteright.GetComponent<SpriteRenderer>().sprite = stimuli[indexright];
                whichhasmore.SetActive(true);

            //Wait untill response or time out
                yield return new WaitUntil(() => ((Time.time - timeStartTrial) > observationtime) | trialend == true);


            //Delete number blobs and show text if no response is given. 
            spriteleft.GetComponent<SpriteRenderer>().sprite = null;
            spriteright.GetComponent<SpriteRenderer>().sprite = null;
            noresponsetext.SetActive(false);

            if (noresponse == true)
            {
                TaskOnClickNA(); //Sets an response to FALSE (incorrect) and marks the trial as misseed. 
                noresponsetext.SetActive(true);
                yield return new WaitForSecondsRealtime(.5f);
                noresponsetext.SetActive(false);
            }

            if (noresponse == false)
            {
                //Save Reaction Time
                trialRT = (Time.time - timeStartTrial);
				PlayFeedbackNoise ();
            }


            //Save data 
            Logger.LogData( new string[] {
				(trialnum+1).ToString(),
				(stimuliOrder[trialnum * 2]).ToString(),
				(stimuliOrder[trialnum * 2+1]).ToString(),
				timeStartTrial.ToString(),
				trialRT.ToString(),
				sidepress[trialnum].ToString(),
				correctPress[trialnum].ToString()
			});



            //Next trial
            trialnum++;
            trialend = true; //This should be set anyway
            enableresponse = false;

        }
        timeTaskTaken = Time.time - timeStartTask;
		print(String.Concat("Your Score: ", ConvertBoolArrayToMean(correctPress).ToString(".###")));

        whichhasmore.SetActive(false);


		// Signal the Update function that we're done.
		_areWeDoneYet = true;

        yield return null;

    }

	void PlayFeedbackNoise(){

		if (UseSoundFeedback) {
			if (correctPress [trialnum] == true) {
				print ("correct");
				AudioSourceFeedback.PlayOneShot (CorrectAudio);
			}
			if (correctPress [trialnum] == false) {
				print ("incorrect");
				AudioSourceFeedback.PlayOneShot (IncorrectAudio);

			}
		}

	}



    void Update() 
    {
        SetCanvasTimer(); //here the countdown timer in the response section is running

		// Check if the task is done yet.
		if (_areWeDoneYet)
		{
			Logger.Close ();
			// Load the next scene.
			UnityEngine.SceneManagement.SceneManager.LoadScene("InBetweenScene");
		}
    }

    //Functions and that

	void MissPromptClick(){
		MissPromptContinue = true;
	}


    float ConvertBoolArrayToMean(List<bool> inputarray)
    {

        List<float> floatlist = new List<float>();
        foreach (bool i in inputarray)
        {
            //print(Convert.ToSingle(i));
            floatlist.Add(Convert.ToSingle(i));
            //print(floatlist[0]);
        }
        return floatlist.Average();

    }

    public void SetCanvasTimer() //here the countdown timer in the response section is running
    {
        if (spriteleft.GetComponent<SpriteRenderer>().sprite == null)
        {
            secsleftx.SetActive(false); circle_white.SetActive(false);
        }

        if (spriteleft.GetComponent<SpriteRenderer>().sprite != null) //This updates the seconds remaining gui elements.
        {
            secsleftx.SetActive(true); circle_white.SetActive(true);
            timeleft = Mathf.Max(Mathf.Round(observationtime + .49f + (timeStartTrial - Time.time)), 0f);
            // timeleft = string timeleft.ToString("R");
            GameObject.Find("SecsLeftX").GetComponent<Text>().text = timeleft.ToString("R");
            //Update the clock
            circle_white.GetComponent<Image>().fillAmount = (Time.time - timeStartTrial) / observationtime;
        }

    }

    public void TaskOnClickL() //activates when the response buttons are clicked! these are activated my onmousedown() functions in a script attached to the object.
    {
        if (enableresponse == true)
        {
            trialend = true;

            sidepress.Add("L");
            correctPress.Add("L" == correctSide[trialnum]);

            //print(correctSide[trialnum]);
            //print(correctPress[trialnum]);

            noresponse = false;
            enableresponse = false;

        }
    }

    public void TaskOnClickR() //activates when the response buttons are clicked! these are activated my onmousedown() functions in a script attached to the object.
    {
        if (enableresponse == true)
        {
            trialend = true;
            sidepress.Add("R");
            correctPress.Add("R" == correctSide[trialnum]);

            noresponse = false;
            enableresponse = false;

        }
    }

    void TaskOnClickNA() //activates when no button is pressed
    {
        //if (enableresponse == true)
        //{
            trialRT = (999);
            trialend = true;
            sidepress.Add("NA");
            correctPress.Add(false);
            enableresponse = false;

        //}
    }


    //DEPRECIATED??
    IEnumerator SetTrialEndTrueAfterSecs() //this coroutine ends the trial after (var timetosrepond) seconds, but does not activate trial end if button is already pressed!
    {
        yield return new WaitUntil(() => (Time.time - timeStartTrial) > timetorespond);
        if (GameObject.Find("ResponseCanvas(Clone)") != null) //only activate endtrial if button has not already been pressed. 
        {
            TaskOnClickNA();
        }

        yield break;
    }

    IEnumerator BlinkGameObject()  //NO LONGER USED!
    {
        while (trialend == false)
        {
            //   if ((Time.time - timeStartTrial) > timetorespond)
            // {
            circle_white.SetActive(false);
            yield return new WaitForSecondsRealtime(.5f);
            circle_white.SetActive(true);
            yield return new WaitForSecondsRealtime(.5f);
            
        }

    }




}



