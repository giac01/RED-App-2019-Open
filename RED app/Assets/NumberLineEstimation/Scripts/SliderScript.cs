using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;



public class SliderScript : MonoBehaviour {

    [Header("GameLogic")]
    public bool ClickVirgin = true;


    [Header("GameObjects")]
    public GameObject SliderHandle;
    public GameObject NumberAboveHandle;
    public GameObject SceneManager;
    public LE_SceneManager SceneManagerVariables;
    
    [Header("Data")]
    public float TimeLastTouched;

	// Use this for initialization
	void Awake () {

        SceneManager = GameObject.Find("SceneManager");
        SliderHandle = gameObject.transform.Find("Handle Slide Area").gameObject;
        SliderHandle.SetActive(false);
        gameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate { ValueChange(); });

        SceneManagerVariables = SceneManager.GetComponent<LE_SceneManager>();
    }

    public void ValueChange()
    {
        if (ClickVirgin == true) SliderSetColour();
        TimeLastTouched = Time.time;
        ClickVirgin = false;
    }

    public void SliderSetColour()
    {
        SliderHandle.SetActive(true);
        NumberAboveHandle.GetComponent<Text>().text = SceneManagerVariables.TrialNumbers[SceneManagerVariables.TrialN].ToString();
    }



}
