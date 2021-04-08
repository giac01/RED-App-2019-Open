using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARTOL_NextButton : MonoBehaviour
{

	public ARTOL_SceneController MainScript; 
	public GameObject WhiteBackground;

	public bool ready;

	//public bool AllowResponse = false;


    // Start is called before the first frame update
    void Start()
    {
    	ready = false;
    	MainScript = GameObject.Find("SceneManager").GetComponent<ARTOL_SceneController>();
    	gameObject.GetComponent<Button>().onClick.AddListener(ButtonClick);

        
    }

    void ButtonClick()
    {

    	//Only allows effect if AllowResponse = true! 
    	if (MainScript.AllowResponse){
    		
    		MainScript.NextPress = true;


    	}


        
    }






}
