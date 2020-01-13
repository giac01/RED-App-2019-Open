using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;



public class LE_SceneManager : MonoBehaviour {

    [Header("GameLogic")]
    public int[] TrialNumbers;
    public int TrialN;
    public bool NextTrialButtonPress = false;
    public bool DisableButton = true;

    [Header("GameObjects")]
	public GameObject SliderCanvas, SliderClone, Arrow, ProgressBar; //assigned in gui
    //public GameObject SliderClone;
	public AudioClip Intro1, example1, example2; //asigned in gui
	private AudioSource AudioS;
	private Slider ProgressSlider;

    [Header("Game Data")]
	public int SceneOrder;
	public GameObject Logger; //asigned in gui
	public DataLogger DataLoggerComponent;
    public string[] VarHeader;
    public GameObject NumberText; //0-100 Number text
	public List<string[]> SaveData = new List<string[]>();
	private float timeStartTrial;
	private float endTrialRT;
	//private float RT;

	// Use this for initialization
	void Awake () {
		//Set Up Data Logger
		DataLoggerComponent = Logger.GetComponent<DataLogger> ();
		string[] header = new string[] {"QuestionNumber", "NumberToMatch", "Value", "FirstClickRT", "LastClickRT", "EndTrialRT"};
		DataLoggerComponent.LogHeader( header);

		AudioS = gameObject.GetComponent<AudioSource> ();

        //VarHeader = new string[] {"Val", "RT_Start", "RT_End"};
        NumberText = GameObject.Find("NumberText");
		Arrow.SetActive (false);

		ProgressSlider = ProgressBar.GetComponent<Slider> ();
		//ProgressSlider.value = 0;

        //Numbers to present in trials
        TrialNumbers = new int[] { 
			50, 0, //Practice Trials
			88, 42, 7, 88, 20, 26, 35, 26, 68, 75, 32, 42, 99, 91, 90, 83, 4, 62, 34, 15, 91, 47, 55, 46, 14, 67, 78, 29, 13, 79, 57, 93, 3, 26, 1, 60, 61, 40,
		}; 

        StartCoroutine("TaskCoroutine");



    }

	void Update() {
		//ProgressSlider.value = TrialN / (TrialNumbers.Length-1);
	
	}


    IEnumerator TaskCoroutine()
    {
		TrialN = 0 + SceneOrder*10;
		while (TrialN < SceneOrder*10 +10)
        {
        	ProgressSlider.value = (float)TrialN / 39f;
			//Play explanatory audio on first few trials
			if (TrialN < 2) {
				AudioS.PlayOneShot (Intro1);
				//yield return new WaitUntil (() => !AudioS.isPlaying);
			}


            //Set up game logic
            NextTrialButtonPress = false; DisableButton = true;

            GameObject.Find("NextButton").GetComponent<Image>().color = Color.red;
            GameObject.Find("NumberToMatchText").GetComponent<Text>().text = TrialNumbers[TrialN].ToString();
            SliderClone = Instantiate(SliderCanvas) as GameObject;
			SliderClone.SetActive (true);
            timeStartTrial = Time.time;


			//yield return new WaitUntil (() => !AudioS.isPlaying);
            yield return new WaitUntil(()=> SliderClone.transform.Find("Slider").GetComponent<SliderScript>().ClickVirgin == false);
            float firstValueChangeRT = Time.time - timeStartTrial;
            GameObject.Find("NextButton").GetComponent<Image>().color = Color.green;

			//yield return new WaitUntil (() => !AudioS.isPlaying);
			yield return new WaitUntil (() => NextTrialButtonPress == true);
			AudioS.Stop ();
            endTrialRT = Time.time - timeStartTrial;
            float lastValueChangeRT = SliderClone.transform.Find("Slider").GetComponent<SliderScript>().TimeLastTouched - timeStartTrial;

			//Last version
			if (TrialN < 2) {
				if (TrialN == 0) {
					AudioS.PlayOneShot (example1);
					Arrow.SetActive (true);

				}
				if (TrialN == 1) {
					AudioS.PlayOneShot (example2);
					Arrow.SetActive (true);
					Arrow.transform.localPosition = new Vector3 (-445.13f, 11.34f, 0f);
				}
				yield return new WaitUntil (() => !AudioS.isPlaying);
			}

			Arrow.SetActive (false);

			//Log Data

			DataLoggerComponent.LogData( new string[] {
				(TrialN+1).ToString(),
				TrialNumbers[TrialN].ToString(),
				(SliderClone.transform.Find("Slider").GetComponent<Slider>().value*100).ToString(),
				firstValueChangeRT.ToString(),
                lastValueChangeRT.ToString(),
				endTrialRT.ToString()}
            );
 //           SaveData.Add(new string[] {
 //               SliderClone.transform.Find("Slider").GetComponent<Slider>().value.ToString(), 
 //               firstValueChangeRT.ToString(),
 //               lastValueChangeRT.ToString(),
 //                   endTrialRT.ToString() });


            Destroy(SliderClone);

			//Update Progress Bar

            TrialN++;
			//print ((float)TrialN / 31f);

        }

	//End Task
		DataLoggerComponent.Close ();
		if (SceneOrder<3){
			UnityEngine.SceneManagement.SceneManager.LoadScene(string.Concat("LE_MainTask",(SceneOrder+1).ToString()));
		}
		if (SceneOrder == 3) {
			UnityEngine.SceneManagement.SceneManager.LoadScene ("InBetweenScene");
		}
    }

//    double WorkOutAverageSuccess(List<string[]> inputList )
//    {
//        double[] responseDeviation = new double[inputList.Count];
//        for (int i = 0; i < SaveData.Count; i++)
//        {
//           // print(((double)TrialNumbers[i] / 100));
//           // print(float.Parse(inputList[i][0]));
//            responseDeviation[i] = Mathf.Pow(Mathf.Pow((((float)TrialNumbers[i]/100) - float.Parse(inputList[i][0])), 2f), .5f);
//
//        }
//        return responseDeviation.Sum() / responseDeviation.Length *100;
//    }




}
