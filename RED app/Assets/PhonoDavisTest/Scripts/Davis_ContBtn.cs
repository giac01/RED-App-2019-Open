using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Davis_ContBtn : MonoBehaviour {

    //Make sure to attach these Buttons in the Inspector
    public Button btn1;
    public GameObject SceneControllerOb;
    public Davis_SceneControllerMainTask SceneScript;

    void Start()
    {
        Button btn1 = gameObject.GetComponent<Button>();
        SceneScript = GameObject.Find("SceneControllerMain").GetComponent<Davis_SceneControllerMainTask>();

        //Calls the TaskOnClick/TaskWithParameters method when you click the Button
        btn1.onClick.AddListener(TaskOnClick);
        //btn2.onClick.AddListener(delegate {TaskWithParameters("Hello"); });
    }

    void TaskOnClick()
    {
        //Output this to console when the Button is clicked
        
        SceneScript.ContinueButtonPressed = true;
    }


}