using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LE_NextButtonScript : MonoBehaviour {

    public GameObject SceneManagerObject;

    public Button btn;

    public ColorBlock cb;

	// Use this for initialization
	void Awake () {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    
        SceneManagerObject = GameObject.Find("SceneManager");

        //print(gameObject.name);

    }


	
	// Update is called once per frame
	void TaskOnClick () {

        if (SceneManagerObject.GetComponent<LE_SceneManager>().SliderClone.transform.Find("Slider").GetComponent<SliderScript>().ClickVirgin ==false)
        {
            //print("Buttonclick");
            SceneManagerObject.GetComponent<LE_SceneManager>().NextTrialButtonPress = true;
			SceneManagerObject.GetComponent<LE_SceneManager> ().SliderClone.transform.Find ("Slider").gameObject.transform.Find ("Handle Slide Area").gameObject.SetActive (false);
        }

    }
}
