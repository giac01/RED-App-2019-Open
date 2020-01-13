using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


//During the practice rounds, this controlls the button to go to the next example.
public class Vocab_NextExampleButton : MonoBehaviour {
    //public Button yourButton;
    public GameObject SceneController;


    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(TaskOnClick);
        SceneController = GameObject.Find("SceneControllerMain");
    }
    // Use this for initialization

    void TaskOnClick()
    {
        print("ButtonDown");
        SceneController.GetComponent<ANS_SceneControllerPracticeRounds>().nextexamplepress = true;
    }
}
