using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq; 


public class ARTOL_SceneController : MonoBehaviour
{

	[Header("Game Resources")]
	public AudioClip Intro1;
	public AudioClip correctAudio;
	public AudioClip incorrectAudio;
	public AudioSource MainAudioSource;
	public List<string> Stimuli_Used_Index; //List of stimuli used!
	public List<string> Match_text; //List of stimuli used!

	public Sprite[] stimuli;
    public List<string> stimulinames;

    public ARTOL_Square BlinkingSquare;
	
	[Header("Game Objects")]
	public GameObject MatchGO;
	public GameObject Option1GO;
	public GameObject Option2GO;
	public GameObject Option3GO;
	public GameObject Option4GO;
	public GameObject NextGO; 
	public GameObject SelectionGO;


	[Header("Game Parameters")] //May want to alter these! 
	public int NumberPracticeRounds; 
	public string Match_Option;

	[Header("Game Logic")] //May want to alter these! 
	public bool AllowResponse = false;
	public bool NextPress = false;

    [Header("Game Data")] 
    public int trialnum;
	public DataLogger Logger;
	public int ResponseOption; //Box selected on a given trial. the button script changes this!
	public float timeStartTrial;

	public string Option1Name;
	public string Option2Name;
	public string Option3Name;
	public string Option4Name;
	public string MatchName;


    void Start()
    {
    	AllowResponse = false;

    	//Game Logic that can be changed
    	Match_Option = "_md";
    	NumberPracticeRounds = 3;

    	BlinkingSquare = GameObject.Find("Square").GetComponent<ARTOL_Square>();




        //Edwin's data logger stuff
        Logger = GameObject.Find ("Logger").GetComponent<DataLogger> ();
		string[] header = new string[] {"QuestionNumber", "MatchNamme", "Option1Name", "Option2Name","Option3Name", "Option4Name", "RT", "ResponseOption", "ResponseCorrect"};
		Logger.LogHeader( header);

		//Load basic things required
		stimuli = Resources.LoadAll<Sprite>("Stimuli") as Sprite[];
		MainAudioSource = gameObject.GetComponent<AudioSource>();
		MatchGO = GameObject.Find("Match");
		Option1GO = GameObject.Find("option1");
		Option2GO = GameObject.Find("option2");
		Option3GO = GameObject.Find("option3");
		Option4GO = GameObject.Find("option4");
		NextGO = GameObject.Find("NEXT");
		SelectionGO = GameObject.Find("Selection");


		 //Get names of imported stimuli objects <stimuli> and create names <stimulinames>
        foreach (Sprite i in stimuli)
        {
            stimulinames.Add(i.name.ToString());
        }


		//Load stimuli used
		Stimuli_Used_Index = new List<string>{"PS1_4_M_ss3","PS1_5_M_ss1","PS1_68_M_ss1","PS1_77_M_ss1","PS1_7_M_ss3","PS1_2_M_ss1","PS1_3_M_ss2","PS1_1_M_ss3","PS1_9_M_ss2","PS1_8_M_ss1","PS1_41_M_ss1","PS1_16_M_ss3","PS1_38_M_ss1","PS1_23_M_ss1","PS1_22_M_ss3","PS1_19_M_ss3","PS1_20_M_ss1","PS1_6_M_ss2","PS1_25_M_ss3","PS1_58_M_ss3","PS1_30_M_ss2","PS1_64_M_ss3","PS1_75_M_ss2","PS1_29_M_ss1","PS1_17_M_ss1","PS1_33_M_ss2","PS1_32_M_ss1","PS1_27_M_ss2","PS1_10_M_ss3","PS1_35_M_ss1"};
		

				//print(Stimuli_Used_Index.Count);
		//for (int i = 0; i < Stimuli_Used_Index.Count; i++){
		//	Match_text.Insert(i,  Concat() );
		//}


		StartCoroutine("Task");




    }

    IEnumerator Task()
	{
		trialnum = 0 ;


		while(trialnum<Stimuli_Used_Index.Count)
		{

			

		//Reset Values 
			AllowResponse = false;
			NextPress = false;
			ResponseOption = 0;
			SelectionGO.SetActive(false);
			//BlinkingSquare.SetActive(true);


		//Present images Images 

			//Randomise order of images 
	    	System.Random rnd = new System.Random();
	        var order = Enumerable.Range(1, 4).OrderBy(x => rnd.NextDouble()).ToList(); //List 1-4 in random order

			//Names of images
			MatchName = (Stimuli_Used_Index[trialnum]);

			Option1Name = (Stimuli_Used_Index[trialnum] + Match_Option).Replace("_M_",("_T"+order[0].ToString()+"_"));
			Option2Name = (Stimuli_Used_Index[trialnum] + Match_Option).Replace("_M_",("_T"+order[1].ToString()+"_"));
			Option3Name = (Stimuli_Used_Index[trialnum] + Match_Option).Replace("_M_",("_T"+order[2].ToString()+"_"));
			Option4Name = (Stimuli_Used_Index[trialnum] + Match_Option).Replace("_M_",("_T"+order[3].ToString()+"_"));

			//print(MatchName);
			//print(Option1Name);

			int indexM = stimulinames.IndexOf(MatchName);
			int indexO1 = stimulinames.IndexOf(Option1Name);
			int indexO2 = stimulinames.IndexOf(Option2Name);
			int indexO3 = stimulinames.IndexOf(Option3Name);
			int indexO4 = stimulinames.IndexOf(Option4Name);

			MatchGO.GetComponent<Image>().sprite = stimuli[indexM];
			Option1GO.GetComponent<Image>().sprite = stimuli[indexO1];
			Option2GO.GetComponent<Image>().sprite = stimuli[indexO2];
			Option3GO.GetComponent<Image>().sprite = stimuli[indexO3];
			Option4GO.GetComponent<Image>().sprite = stimuli[indexO4];

			//Set buttons so that the correct number is shown
			Option1GO.GetComponent<ARTOL_ButtonScript>().OptionNumber = order[0];
			Option2GO.GetComponent<ARTOL_ButtonScript>().OptionNumber = order[1];
			Option3GO.GetComponent<ARTOL_ButtonScript>().OptionNumber = order[2];
			Option4GO.GetComponent<ARTOL_ButtonScript>().OptionNumber = order[3];

			//Blink Dashed Square

			BlinkingSquare.StartCoroutine("BlinkGameObject",1f); //parameter is length of time of blink! 
			NextGO.SetActive(false);


		//Play Explanation & wait for it to finish. Only do this for "practice rounds"
			if (trialnum < 1){ //only play instructions on first round 
				yield return new WaitForSecondsRealtime(1f);
				MainAudioSource.PlayOneShot(Intro1);
				yield return new WaitUntil(() => MainAudioSource.isPlaying==false);
			}

		//Record Start of Trial & allow response
			timeStartTrial = Time.time; //Time and start of response
			AllowResponse = true;

		//Wait for response
			yield return new WaitUntil(() => ResponseOption > 0);
			BlinkingSquare.StopBlink();
			//BlinkingSquare.SetActive(false);
			NextGO.SetActive(true);

			yield return new WaitUntil(() => NextPress);




			//Play Feedback!
			if (trialnum < NumberPracticeRounds){
				if(ResponseOption==1){
					MainAudioSource.PlayOneShot(correctAudio);
					yield return new WaitUntil(() => MainAudioSource.isPlaying==false);
				}
				if(ResponseOption!=1){
					MainAudioSource.PlayOneShot(incorrectAudio);
					yield return new WaitUntil(() => MainAudioSource.isPlaying==false);
				}
	
			}
		
		//Record data
			//float RT = Time.time - timeStartTrial;
			//print(RT);

			




		trialnum++;

		}
		Logger.Close();
		// Load the next scene.
		UnityEngine.SceneManagement.SceneManager.LoadScene("InBetweenScene");

		yield return null;
	

    }


    
}
