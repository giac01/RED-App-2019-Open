using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using System;




public class Vocab_SceneControllerMainTask : MonoBehaviour
{

private bool _areWeDoneYet = false;

    //time parameters in the game
    [Header("Time Paramaters of the Task")]
    public float observationtime; //length of time that stimuli are shown for
    public float fixationtime; //length of time (secs) that fixation cross is shown for 
    public float timetorespond; //length of time (secs) that pps are allowed to respond with. 
public bool UseSoundFeedback; 

    [Header("Game Stimuli")]
    public GameObject WordToMatch_GameObject; 

public AudioClip CorrectAudio;
public AudioClip IncorrectAudio;
public AudioSource AudioSourceFeedback;
    //public GameObject fixationcross; //asigned in gui
    public GameObject noresponsetext; // assigned in gui
    public GameObject secsleftx; // assigned in gui
    public GameObject whichhasmore; //asigned in gui
    public GameObject circle_white; //asssigned in gui, used for showing the time on a clock 
    //public GameObject yourscore;

    public Sprite[] stimuli;
    private GameObject sprite1;
    private GameObject sprite2;
    private GameObject sprite3;
    private GameObject sprite4;
    //private GameObject[] sprites;
    public List<string> stimulinames;
    //public List<string> presorder2;

    private int index1;
    private int index2;
    private int index3;
    private int index4;



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
    private float PicSize;
    public bool enableresponse;     //this prevents the buttons from doing anything when we don't want them to create a response. 
    public bool nextexamplepress = false;
    private float waitsecs;
    //  public GameObject secsleftclone;

    public int trialnum;
    public bool trialend = false; //This allows the scripts on the response buttons to set the end of the trial!
    private GameObject secsleftclone;
    public bool noresponse; //Assesses if a response has been given for a specific trial

    //Trial Data

public string[] WordToMatch;
private string[] correctSide; 
private string[] stimuliOrder;
    //Data to Export

    public List<string> sidepress = new List<string>();  //Lists order in which stimuli will be presented. 
    public List<bool> correctPress = new List<bool>();  //Lists order in which stimuli will be presented. 

    [Header("Game Data")]
public DataLogger Logger;
    public float trialRT;
    public string[] header;
    //public string[] dataheader = new string[] {"ReactionTime", "SidePress", "CorrectPress"};  //Lists order in which stimuli will be presented. 
    public string[][] exportdata = new string[104][];
    //public float dummmy; //DOES NOT DO ANYTHING.

    //Save responses 
    //  public List<string> sidepress;

    void Start()
    {
    PicSize = 1.4f;
    WordToMatch_GameObject = GameObject.Find("Canvas").gameObject.transform.Find("Word").gameObject;
	WordToMatch = new string[] {"bed","toy","balloon","fork","drum","engine","anchor","abacus","apostrophe","banquet","croissant","gauntlet","satelite","cerebrum","onyx","marmot","doppelganger","dossier","consulate"};
	stimuliOrder = new string[] { "sofa","chair","bed","desk","hat","cup","toy","pen","balloon","butterfly","bucket","tub","fork","knife","spoon","plate","pot","caterpillar","drum","gate","engine","maple","shed","jewel","ski","anchor","grill","fingerprint","barometer","compass","calculator","abacus","radiator","chessboard","hazard","apostrophe","banquet","warehouse","gnome","bowels","cake","baguette","macaroon","croissant","gauntlet","shield","corset","anvil","satelite","retina","computer","coliseum","spine","femur","abdomen","cerebrum","roundabout","onyx","fedora","forceps","finch","impala","marmot","monkey","soldier","doppelganger","manager","paparazzi","dossier","cloudburst","tungsten","bisque","consulate","church","mosque","garden"};
        //Set canvas elements to blank initially 
        secsleftx.SetActive(false);
        noresponsetext.SetActive(false);
        circle_white.SetActive(false);
        whichhasmore.SetActive(false);
        //yourscore.SetActive(false);
        
        //Load sprite objects (where images displayed on screen appear)
        sprite1 = GameObject.Find("Sprite1");
        sprite2 = GameObject.Find("Sprite2");
        sprite3 = GameObject.Find("Sprite3");
        sprite4 = GameObject.Find("Sprite4");


        //Load images into memory
        stimuli = Resources.LoadAll<Sprite>("VocabImages") as Sprite[];

        //Get names of imported stimuli objects <stimuli> and create names <stimulinames>
        foreach (Sprite i in stimuli)
        {
            stimulinames.Add(i.name.ToString());
        }

//Data Logger - Set header
Logger = GameObject.Find ("Logger").GetComponent<DataLogger> ();
header = new string[] {"QuestionNumber", "WordToMatch", "Option1", "Option2", "Option3", "Option4", "TimeStartQuestion", "RT", "ResponseSide", "ResponseCorrect"};
Logger.LogHeader( header);


AudioSourceFeedback = gameObject.GetComponent<AudioSource> ();

        timeStartTask = Time.time;
        StartCoroutine("PresentStimuli");
 
    }


    IEnumerator PresentStimuli()
    {
    	float BoxColliderSize = sprite1.GetComponent<BoxCollider2D>().size.x;

        yield return new WaitForSecondsRealtime(.5f);
        //taskstarted = true;
        for (trialnum = 0; trialnum < WordToMatch.Length; trialnum++)
        {
            //Set up trial logic
           trialend = false; nextexamplepress = false; noresponse = true; enableresponse = false;
           sprite1.SetActive(false); sprite2.SetActive(false);sprite3.SetActive(false);WordToMatch_GameObject.SetActive(false);

            //Present Fixation Cross
                //fixationclone = Instantiate(fixationcross) as GameObject;

                yield return new WaitForSecondsRealtime(fixationtime);
                sprite1.SetActive(true); sprite2.SetActive(true);sprite3.SetActive(true);WordToMatch_GameObject.SetActive(true);
                //Destroy(fixationclone);

                enableresponse = true;
                timeStartTrial = Time.time; //Time and start of response

            //Present Dot Blobs

                index1 = stimulinames.IndexOf((stimuliOrder[trialnum * 4]));
                index2 = stimulinames.IndexOf((stimuliOrder[trialnum * 4 + 1]));
                index3 = stimulinames.IndexOf((stimuliOrder[trialnum * 4 + 2]));
                index4 = stimulinames.IndexOf((stimuliOrder[trialnum * 4 + 3]));


                //print(index1);
                //print((stimuliOrder[trialnum * 4]));
                sprite1.GetComponent<SpriteRenderer>().sprite = stimuli[index1];
                sprite2.GetComponent<SpriteRenderer>().sprite = stimuli[index2];
                sprite3.GetComponent<SpriteRenderer>().sprite = stimuli[index3];
                sprite4.GetComponent<SpriteRenderer>().sprite = stimuli[index4];


                
                float sprite1Adjustment = PicSize/Mathf.Max(sprite1.GetComponent<SpriteRenderer>().sprite.bounds.extents.x,sprite1.GetComponent<SpriteRenderer>().sprite.bounds.extents.y);
                float sprite2Adjustment = PicSize/Mathf.Max(sprite2.GetComponent<SpriteRenderer>().sprite.bounds.extents.x,sprite2.GetComponent<SpriteRenderer>().sprite.bounds.extents.y);
                float sprite3Adjustment = PicSize/Mathf.Max(sprite3.GetComponent<SpriteRenderer>().sprite.bounds.extents.x,sprite3.GetComponent<SpriteRenderer>().sprite.bounds.extents.y);
                float sprite4Adjustment = PicSize/Mathf.Max(sprite4.GetComponent<SpriteRenderer>().sprite.bounds.extents.x,sprite4.GetComponent<SpriteRenderer>().sprite.bounds.extents.y);

                //print("AdjustmentIndicies" + sprite1Adjustment.ToString() + "     " + sprite2Adjustment.ToString() +"     " + sprite3Adjustment.ToString() +"     " + sprite4Adjustment.ToString() );

                sprite1.transform.localScale = new Vector3( sprite1Adjustment,sprite1Adjustment,1f);
                sprite2.transform.localScale = new Vector3( sprite2Adjustment,sprite2Adjustment,1f);
                sprite3.transform.localScale = new Vector3( sprite3Adjustment,sprite3Adjustment,1f);
                sprite4.transform.localScale = new Vector3( sprite4Adjustment,sprite4Adjustment,1f);

                sprite1.GetComponent<BoxCollider2D>().size= new Vector3( BoxColliderSize/sprite1Adjustment,BoxColliderSize/sprite1Adjustment,1f);
                sprite2.GetComponent<BoxCollider2D>().size= new Vector3( BoxColliderSize/sprite2Adjustment,BoxColliderSize/sprite2Adjustment,1f);
                sprite3.GetComponent<BoxCollider2D>().size= new Vector3( BoxColliderSize/sprite3Adjustment,BoxColliderSize/sprite3Adjustment,1f);
                sprite4.GetComponent<BoxCollider2D>().size= new Vector3( BoxColliderSize/sprite4Adjustment,BoxColliderSize/sprite4Adjustment,1f);


                //sprite2.transform.localScale = new Vector3( PicSize/sprite2.GetComponent<SpriteRenderer>().sprite.bounds.extents.x/2,
                //PicSize/sprite2.GetComponent<SpriteRenderer>().sprite.bounds.extents.y/2,1f);
                //sprite3.transform.localScale = new Vector3(PicSize/sprite3.GetComponent<SpriteRenderer>().sprite.bounds.extents.x/2,
                //PicSize/sprite3.GetComponent<SpriteRenderer>().sprite.bounds.extents.y/2,1f);
                //sprite4.transform.localScale = new Vector3(PicSize/sprite4.GetComponent<SpriteRenderer>().sprite.bounds.extents.x/2,
                //PicSize/sprite4.GetComponent<SpriteRenderer>().sprite.bounds.extents.y/2,1f);
                
                //sprite1.GetComponent<BoxCollider2D>().size= new Vector3(4.8f/(.66f*sprite1.transform.localScale.x),5.29f*.66f/sprite1.transform.localScale.y,0f);



        //Put word to be matched on screen
                WordToMatch_GameObject.GetComponent<Text>().text = WordToMatch[trialnum];
                whichhasmore.SetActive(true);

            //Wait untill response - n.b. there is no time out of response!
                yield return new WaitUntil(() => ((Time.time - timeStartTrial) > observationtime) | trialend == true);

                print ("start blink");
                StartCoroutine("BlinkGameObject");
            yield return new WaitUntil(() => (trialend == true));


            //Delete number blobs and show text if no response is given. 
           sprite1.GetComponent<SpriteRenderer>().sprite = null;
           sprite2.GetComponent<SpriteRenderer>().sprite = null;
           sprite3.GetComponent<SpriteRenderer>().sprite = null;
           sprite4.GetComponent<SpriteRenderer>().sprite = null;


           noresponsetext.SetActive(false);
           trialRT = (Time.time - timeStartTrial);
           PlayFeedbackNoise ();

            //Save data 
           Logger.LogData( new string[] {
			(trialnum+1).ToString(),
			WordToMatch[trialnum].ToString(),
			(stimuliOrder[trialnum * 4]).ToString(),
			(stimuliOrder[trialnum * 4+1]).ToString(),
			(stimuliOrder[trialnum * 4+2]).ToString(),
			(stimuliOrder[trialnum * 4+3]).ToString(),

			timeStartTrial.ToString(),
			trialRT.ToString(),
			sidepress[trialnum].ToString(),
			correctPress[trialnum].ToString()
			});

            //Next trial
           trialend = true; //This should be set anyway
           enableresponse = false;
           StopCoroutine("BlinkGameObject");

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
//print ("correct");
AudioSourceFeedback.PlayOneShot (CorrectAudio);
}
if (correctPress [trialnum] == false) {
//print ("incorrect");
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
        if (sprite1.GetComponent<SpriteRenderer>().sprite == null)
        {
            secsleftx.SetActive(false); circle_white.SetActive(false);
        }

        if (sprite1.GetComponent<SpriteRenderer>().sprite != null) //This updates the seconds remaining gui elements.
        {
            secsleftx.SetActive(true); circle_white.SetActive(true);
            timeleft = Mathf.Max(Mathf.Round(observationtime + .49f + (timeStartTrial - Time.time)), 0f);
            // timeleft = string timeleft.ToString("R");
            GameObject.Find("SecsLeftX").GetComponent<Text>().text = timeleft.ToString("R");
            //Update the clock
            circle_white.GetComponent<Image>().fillAmount = (Time.time - timeStartTrial) / observationtime;
        }

    }

    public void TaskOnClick(float SpriteN) //activates when the response buttons are clicked! these are activated my onmousedown() functions in a script attached to the object.
    {
        if (enableresponse == true)
        {
            trialend = true;

            //print(SpriteN.ToString());
            sidepress.Add(SpriteN.ToString());

//            print("Side press " + SpriteN.ToString());


			float correctSprite = stimuliOrder.ToList().GetRange(trialnum*4,4).IndexOf(WordToMatch[trialnum]) + 1; //Which sprite is teh correct one to pick
//			print ("Correct response: " + correctSprite.ToString());
			//print(WordToMatch[Enumerable.Range(trialnum*3, trialnum*3+2)]);


            correctPress.Add(SpriteN == correctSprite);

            //print(correctSide[trialnum]);
            //print(correctPress[trialnum]);

            noresponse = false;
            enableresponse = false;

        }
    }






    IEnumerator BlinkGameObject()  
    {
        while (trialend == false)
        {

            circle_white.GetComponent<Image>().color= GameObject.Find("Main Camera (2)").gameObject.GetComponent<Camera>().backgroundColor;
            yield return new WaitForSecondsRealtime(.75f);
            circle_white.GetComponent<Image>().color= Color.white;
            yield return new WaitForSecondsRealtime(.75f);
            
        }

    }




}


