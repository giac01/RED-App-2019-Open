using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARTOL_ButtonScript : MonoBehaviour
{

	public ARTOL_SceneController MainScript; 
    public DataLogger Logger;
	public int OptionNumber; //Set in gui. Set 1 for first button, 3 for third button, etc. 
	//public bool AllowResponse = false;


    // Start is called before the first frame update
    void Awake()
    {
    	MainScript = GameObject.Find("SceneManager").GetComponent<ARTOL_SceneController>();
    	gameObject.GetComponent<Button>().onClick.AddListener(ButtonClick);
        Logger = GameObject.Find ("Logger").GetComponent<DataLogger> ();

        
    }

    void ButtonClick()
    {

    	//Only allows effect if AllowResponse = true! 
    	if (MainScript.AllowResponse){
    		//print("buttonpress");
    		MainScript.ResponseOption = OptionNumber;
            float RT = Time.time - MainScript.timeStartTrial;

            //MainScript.SelectionGO.GetComponent<Image>().sprite = 
            //print(MainScript.Stimuli_Used_Index[MainScript.trialnum]);
            //(Stimuli_Used_Index[trialnum] + Match_Option).Replace("_M_",("_T"+order[0].ToString()+"_"));

            //Display Reponse On Screen
                string imageName = (MainScript.Stimuli_Used_Index[MainScript.trialnum] + MainScript.Match_Option).Replace("_M_",("_T"+OptionNumber+"_")) ;
                MainScript.SelectionGO.GetComponent<Image>().sprite = MainScript.stimuli[MainScript.stimulinames.IndexOf(imageName)];
                MainScript.SelectionGO.SetActive(true);


            Logger.LogData( new string[] {
                (MainScript.trialnum+1).ToString(),
                MainScript.MatchName,
                MainScript.Option1Name,
                MainScript.Option2Name,
                MainScript.Option3Name,
                MainScript.Option4Name,
                RT.ToString(),
                MainScript.ResponseOption.ToString(),
            });

    	}


        
    }






}
