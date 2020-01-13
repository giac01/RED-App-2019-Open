using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using System;

public class Davis_SceneControllerMainTask : MonoBehaviour
{

    //Required for logger 
    private bool _areWeDoneYet = false;

    //Task Parameters
    [Header("Time Paramaters of the Task")]
    public float observationtime = 5; //length of time that PhonoStimuli are shown for
    public float InterTrialInterval = 0; //length of time (secs) that fixation cross is shown for 
    //public float timetorespond; //length of time (secs) that pps are allowed to respond with. 
    public float WaitBetweenSounds = .5f; //length of time (secs) between two audio tracks being presented 
    public bool ShowWord = true;
    public bool UseSoundFeedback = false; 

    [Header("Game PhonoStimuli")]
    public AudioClip AudioInstructions;  //asigned in gui   
    public AudioClip CorrectAudio; //asigned in gui
    public AudioClip IncorrectAudio; //assigned in gui
    public GameObject secsleftx; // assigned in gui
    public GameObject whichhasmore; //asigned in gui
    public GameObject circle_white; //asssigned in gui, used for showing the time on a clock 

    private AudioSource AudioSourceObject;
    private GameObject WordToMatch_GameObject; 
    private AudioClip Beep;

    private GameObject continueButton; 
    private GameObject introSprites;
    private GameObject taskInstructions; 
    private GameObject CanvasOb;
    private GameObject sprite1;
    private GameObject sprite2;
    private GameObject Sprite1Mouth;
    private GameObject Sprite2Mouth;
    private GameObject spriteMiddle;

    public AudioClip[] FemaleAudioStimuli;
    public AudioClip[] AlienAudioStimuli;
    public Sprite[] ImageStimuli;
    public List<string> FemaleAudioStimuliNames;
    public List<string> PhonoStimulinames;
    public List<string> ImageStimuliNames;

    public GameObject responseclone;
    private GameObject feedbackclone;



    [Header("Game Logic")]
    public bool ContinueButtonPressed;

    public bool enableresponse;    
    public bool nextexamplepress = false;
    private float waitsecs;

    public int trialnum;
    public bool trialend = false; //This allows the scripts on the response buttons to set the end of the trial!
    private GameObject secsleftclone;


    public string[] WordToMatch;
    private int[] correctSide; //Which alien is correct. Left =1, right =0.
    private string[] phonoStimuliOrder;

    //private sprite[] imageStimuliOrder;

    //Data to Export
    public List<string> sidepress = new List<string>();  //Lists order in which PhonoStimuli will be presented. 
    public List<bool> correctPress = new List<bool>();  //Lists order in which PhonoStimuli will be presented. 

    [Header("Game Data")]
    public DataLogger Logger;
    public float trialRT;
    public string[] header;

    private float timeStartTrial;
    private float timeStartAllowResponse;
    private float timeStartTask;
    public float timeleft;
    private float timeTaskTaken;

    //  public List<string> sidepress;

    void Start()
    {
        CanvasOb = GameObject.Find("Canvas");
        WordToMatch_GameObject = CanvasOb.gameObject.transform.Find("Word").gameObject;
        WordToMatch = new string[] {
                "fan","sand","bow","seat","pen","bed","sign","rug","gate","goat","knife","shelf","hot","sink","shave","yawn","lock","peach","tape","mug","cat","sauce","wipe","bear","leap","top","kite","yell","soap","bib","knob","bag","shake","sweet","hay","leaf","ant","sheet","seed","sing","swing","shirt","wag","glass"
        };
        phonoStimuliOrder = new string[] {
                "fan.wav_van.wav_td_1.00","fan.wav_van.wav_td_0.00","sand.wav_hand.wav_td_1.00","sand.wav_hand.wav_td_0.00","bow.wav_go.wav_td_0.00","bow.wav_go.wav_td_1.00","seat.wav_heat.wav_td_0.00","seat.wav_heat.wav_td_1.00","pen.wav_ten.wav_td_0.27","pen.wav_ten.wav_td_0.73","bed.wav_beg.wav_td_0.55","bed.wav_beg.wav_td_0.45","sign.wav_shine.wav_td_0.27","sign.wav_shine.wav_td_0.73","rug.wav_rub.wav_td_0.45","rug.wav_rub.wav_td_0.55","gate.wav_date.wav_td_0.82","gate.wav_date.wav_td_0.18","goat.wav_boat.wav_td_0.64","goat.wav_boat.wav_td_0.36","knife.wav_nice.wav_td_0.64","knife.wav_nice.wav_td_0.36","shelf.wav_self.wav_td_0.36","shelf.wav_self.wav_td_0.64","hot.wav_hop.wav_td_0.27","hot.wav_hop.wav_td_0.73","sink.wav_think.wav_td_0.73","sink.wav_think.wav_td_0.27","shave.wav_save.wav_td_0.64","shave.wav_save.wav_td_0.36","yawn.wav_lawn.wav_td_0.27","yawn.wav_lawn.wav_td_0.73","lock.wav_rock.wav_td_0.27","lock.wav_rock.wav_td_0.73","peach.wav_teach.wav_td_0.73","peach.wav_teach.wav_td_0.27","tape.wav_take.wav_td_0.18","tape.wav_take.wav_td_0.82","mug.wav_mud.wav_td_0.45","mug.wav_mud.wav_td_0.55","cat.wav_cap.wav_td_0.73","cat.wav_cap.wav_td_0.27","sauce.wav_force.wav_td_0.64","sauce.wav_force.wav_td_0.36","wipe.wav_white.wav_td_0.64","wipe.wav_white.wav_td_0.36","bear.wav_pear.wav_td_0.45","bear.wav_pear.wav_td_0.55","leap.wav_leak.wav_td_0.45","leap.wav_leak.wav_td_0.55","top.wav_pop.wav_td_0.55","top.wav_pop.wav_td_0.45","kite.wav_tight.wav_td_0.73","kite.wav_tight.wav_td_0.27","yell.wav_well.wav_td_0.36","yell.wav_well.wav_td_0.64","soap.wav_hope.wav_td_0.45","soap.wav_hope.wav_td_0.55","bib.wav_big.wav_td_0.73","bib.wav_big.wav_td_0.27","knob.wav_nod.wav_td_0.55","knob.wav_nod.wav_td_0.45","bag.wav_back.wav_td_0.27","bag.wav_back.wav_td_0.73","shake.wav_fake.wav_td_0.45","shake.wav_fake.wav_td_0.55","sweet.wav_sweep.wav_td_0.82","sweet.wav_sweep.wav_td_0.18","hay.wav_say.wav_td_0.64","hay.wav_say.wav_td_0.36","leaf.wav_leave.wav_td_0.45","leaf.wav_leave.wav_td_0.55","ant.wav_and.wav_td_0.45","ant.wav_and.wav_td_0.55","sheet.wav_sheep.wav_td_0.64","sheet.wav_sheep.wav_td_0.36","seed.wav_feed.wav_td_0.55","seed.wav_feed.wav_td_0.45","sing.wav_thing.wav_td_0.64","sing.wav_thing.wav_td_0.36","swing.wav_swim.wav_td_0.36","swing.wav_swim.wav_td_0.64","shirt.wav_hurt.wav_td_0.36","shirt.wav_hurt.wav_td_0.64","wag.wav_rag.wav_td_0.73","wag.wav_rag.wav_td_0.27","glass.wav_class.wav_td_0.18","glass.wav_class.wav_td_0.82"
        };
        correctSide = new int[] {
                0,0,1,1,1,0,1,1,0,0,0,1,1,0,0,1,1,0,1,1,0,0,0,1,1,0,0,1,1,0,0,1,1,0,0,1,1,0,0,0,1,1,0,1
        };


        //print("test");

        //Set canvas elements to blank initially 
        secsleftx.SetActive(false);
        circle_white.SetActive(false);
        whichhasmore.SetActive(false);
        //yourscore.SetActive(false);
        
        //Load sprite objects (where images displayed on screen appear)
        introSprites = GameObject.Find("IntroSprites");
        taskInstructions = GameObject.Find("Task Instructions");
        continueButton = taskInstructions.transform.Find("ContinueButton").gameObject;
        sprite1 = GameObject.Find("Sprite1");
        sprite2 = GameObject.Find("Sprite2");
        Sprite1Mouth = GameObject.Find("Sprite1Mouth");
        Sprite2Mouth = GameObject.Find("Sprite2Mouth");
        spriteMiddle = GameObject.Find("SpriteMiddle");
        sprite1.SetActive(false);sprite2.SetActive(false); spriteMiddle.SetActive(false); continueButton.SetActive(false);
        //noTouchSign = GameObject.Find("NoTouchSign");

        //Load images and audio into memory
        FemaleAudioStimuli = Resources.LoadAll<AudioClip>("Audio_Female") as AudioClip[];
        AlienAudioStimuli = Resources.LoadAll<AudioClip>("Audio_Alien") as AudioClip[];
        ImageStimuli = Resources.LoadAll<Sprite>("Images") as Sprite[];
        Beep = Resources.Load<AudioClip>("Audio/incorrect_ding");

        //Get names of imported PhonoStimuli objects <PhonoStimuli> and create names <PhonoStimulinames>
        foreach (AudioClip i in FemaleAudioStimuli)
        {
            FemaleAudioStimuliNames.Add(i.name.ToString());
        }
        foreach (AudioClip i in AlienAudioStimuli)
        {
            PhonoStimulinames.Add(i.name.ToString());
        }

        foreach (Sprite i in ImageStimuli)
        {
            ImageStimuliNames.Add(i.name.ToString());
        }

        //Data Logger - Set header
        Logger = GameObject.Find ("Logger").GetComponent<DataLogger> ();
        header = new string[] {"QuestionNumber", "Option1", "Option2", "TimeStartQuestion", "TimeStartAllowResponse", "RT", "ResponseSide", "ResponseCorrect"};
        Logger.LogHeader( header);
        AudioSourceObject = gameObject.GetComponent<AudioSource> ();
        timeStartTask = Time.time;

        StartCoroutine("PresentPhonoStimuli");
 
    }


    IEnumerator PresentPhonoStimuli()
    {
        ContinueButtonPressed = false;
        for (trialnum = 0; trialnum < WordToMatch.Length; trialnum++)
        {
            //Set up trial logic
                trialend = false; nextexamplepress = false; enableresponse = false;
                sprite1.SetActive(false); sprite2.SetActive(false);
                WordToMatch_GameObject.SetActive(false); spriteMiddle.SetActive(false);
                secsleftx.SetActive(false); 
                circle_white.SetActive(false); 
                Sprite1Mouth.SetActive(false); 
                Sprite2Mouth.SetActive(false);

                //noTouchSign.SetActive(true); 

            //Inter-trial interval
                yield return new WaitUntil(() => AudioSourceObject.isPlaying==false);
                yield return new WaitForSecondsRealtime(InterTrialInterval);


            //Present Text to be matched
                WordToMatch_GameObject.GetComponent<Text>().text = WordToMatch[trialnum];
                whichhasmore.SetActive(true); 
                if (ShowWord == true){
                    WordToMatch_GameObject.SetActive(true);
                }

            //Play Audio + present image in middle of screen
                //Find index of audio/image
                int femaleaudioindex = FemaleAudioStimuliNames.IndexOf(WordToMatch[trialnum]);
                int index1 = PhonoStimulinames.IndexOf((phonoStimuliOrder[trialnum * 2]));
                int index2 = PhonoStimulinames.IndexOf((phonoStimuliOrder[trialnum * 2 + 1]));
                int imageindex = ImageStimuliNames.IndexOf((WordToMatch[trialnum]));
                //print(imageindex);

                //Present image


            //Play Audio instructions for first few trials!
                if (trialnum < 1) { //Play on first trial always
                    CanvasOb.SetActive(false);
                    sprite1.SetActive(true); sprite2.SetActive(true);
                    AudioSourceObject.PlayOneShot (AudioInstructions);
                    yield return new WaitUntil(() => AudioSourceObject.isPlaying==false);
                    continueButton.SetActive(true); 
                    yield return new WaitUntil(() => ContinueButtonPressed);
                    CanvasOb.SetActive(true); taskInstructions.SetActive(false); introSprites.SetActive(false);
                    yield return new WaitForSecondsRealtime(.75f);
                } else if (trialnum > 0  & trialnum <4){ //Play on first 4 trials if previous trial incorrect
                    if (!correctPress[trialnum-1]){
                        AudioSourceObject.PlayOneShot (AudioInstructions);
                        yield return new WaitUntil(() => AudioSourceObject.isPlaying==false);
                        yield return new WaitForSecondsRealtime(1);

                    }
                }

            //Play audio sounds to match to work! 
                AudioSourceObject.Stop();
                
                timeStartTrial = Time.time; //Time and start of response

                //Play Target word
                AudioSourceObject.PlayOneShot (FemaleAudioStimuli[femaleaudioindex]);
                spriteMiddle.GetComponent<SpriteRenderer>().sprite = ImageStimuli[imageindex];
                spriteMiddle.SetActive(true);
                yield return new WaitUntil(() => AudioSourceObject.isPlaying==false);

                if (trialnum<4){
                    yield return new WaitForSecondsRealtime(WaitBetweenSounds*2);
                }

                yield return new WaitForSecondsRealtime(WaitBetweenSounds*2);

                //Play sprite one
                AudioSourceObject.PlayOneShot (AlienAudioStimuli[index1]);
                sprite1.SetActive(true);
                StartCoroutine(ActivateGameObjectDelay(Sprite1Mouth));
                yield return new WaitUntil(() => AudioSourceObject.isPlaying==false);
                Sprite1Mouth.SetActive(false);

                yield return new WaitForSecondsRealtime(WaitBetweenSounds);

                //Play sprite 2
                AudioSourceObject.PlayOneShot (AlienAudioStimuli[index2]);
                sprite2.SetActive(true);
                StartCoroutine(ActivateGameObjectDelay(Sprite2Mouth));
                yield return new WaitUntil(() => AudioSourceObject.isPlaying==false);
                Sprite2Mouth.SetActive(false);


            //Allow Response
                timeStartAllowResponse = Time.time;
                enableresponse = true;
                //noTouchSign.SetActive(false);

            //Wait untill response - n.b. there is no time out of response!
                yield return new WaitUntil(() => ((Time.time - timeStartAllowResponse) > observationtime) | trialend == true);
                StartCoroutine(BlinkGameObject(circle_white));
                yield return new WaitUntil(() => (trialend == true));
                trialRT = (Time.time - timeStartAllowResponse);

                //yield return new WaitForSecondsRealtime(.2f);
                PlayFeedbackNoise();

            //Save data 
                Logger.LogData( new string[] {
                (trialnum+1).ToString(),
                (phonoStimuliOrder[trialnum * 2]).ToString(),
                (phonoStimuliOrder[trialnum * 2+1]).ToString(),

                timeStartTrial.ToString(),
                timeStartAllowResponse.ToString(),
                trialRT.ToString(),
                sidepress[trialnum].ToString(),
                correctPress[trialnum].ToString()
                });

            //Next trial
           trialend = true; //This should be set anyway
           enableresponse = false;

        }
        timeTaskTaken = Time.time - timeStartTask;
        whichhasmore.SetActive(false);
        // Signal the Update function that we're done.
        _areWeDoneYet = true;
        yield return null;
    }

IEnumerator ActivateGameObjectDelay(GameObject GO){ //This is just to make the mouth movements more realistic (delay onset a little)
    yield return new WaitForSecondsRealtime(.2f);
    GO.SetActive(true);
}



void PlayFeedbackNoise(){

    if (UseSoundFeedback) {

        if (correctPress [trialnum] == true) {
            //print ("correct");
            AudioSourceObject.PlayOneShot (CorrectAudio);
        }
        if (correctPress [trialnum] == false) {
            //print ("incorrect");
            AudioSourceObject.PlayOneShot (IncorrectAudio);
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

//        if (Input.GetMouseButtonDown(0)){
  //          while (enableresponse==false){
    //            print("NO TOUCHY");
      //          AudioSourceObject.PlayOneShot(Beep);
       //         noTouchSign.SetActive(true);
        //        //StartCoroutine(BlinkGameObject2(noTouchSign, trialnum));
         //   } 

        //}


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
        if (enableresponse == false)
        {
            secsleftx.SetActive(false); circle_white.SetActive(false);
        }

        if (enableresponse == true) //This updates the seconds remaining gui elements.
        {
            secsleftx.SetActive(true); circle_white.SetActive(true);
            timeleft = Mathf.Max(Mathf.Round(observationtime +  .49f + (timeStartAllowResponse - Time.time)), 0f);
            // timeleft = string timeleft.ToString("R");
            GameObject.Find("SecsLeftX").GetComponent<Text>().text = timeleft.ToString("R");
            //Update the clock
            circle_white.GetComponent<Image>().fillAmount = (Time.time - timeStartAllowResponse) / observationtime;
        }
    }

    public void TaskOnClick(float SpriteN) //activates when the response buttons are clicked! these are activated my onmousedown() functions in a script attached to the object.
    {
        if (enableresponse == true)
        {
            trialend = true;
            //print(SpriteN.ToString());
            if (SpriteN==0){
                sidepress.Add("left");
            }

            if (SpriteN==1){
                sidepress.Add("right");
            }

            //sidepress.Add(SpriteN.ToString());
            //print("Side press " + SpriteN.ToString());
            //print ("Correct response: " + correctSide[trialnum].ToString());
            correctPress.Add(SpriteN != correctSide[trialnum]);
            enableresponse = false;
        }
    }




    IEnumerator BlinkGameObject(GameObject blinkObject)  
    {
        blinkObject.SetActive(true);
        float currentTrialNum = trialnum;
        while (trialnum == currentTrialNum)
        {
            blinkObject.GetComponent<Image>().color= Color.black;
            yield return new WaitForSecondsRealtime(.75f);
            blinkObject.GetComponent<Image>().color= Color.red;
            yield return new WaitForSecondsRealtime(.75f);   
        }
        yield return null;
    }

    IEnumerator BlinkGameObject2(GameObject blinkObject, float currentTrialNum)  //depreciated?
    {
        blinkObject.SetActive(true);
        //float currentTrialNum = trialnum;
        while (trialnum == currentTrialNum & enableresponse==false)
        {
            blinkObject.SetActive(false);
            yield return new WaitForSecondsRealtime(.75f);
            blinkObject.SetActive(true);
            yield return new WaitForSecondsRealtime(.75f);   
        }
        if (trialnum==currentTrialNum){
            blinkObject.SetActive(false);
        }

        yield return null;
    }

}


