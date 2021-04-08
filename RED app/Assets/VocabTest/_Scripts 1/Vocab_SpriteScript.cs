using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vocab_SpriteScript : MonoBehaviour
{

    public GameObject scenecontroller; //Assigned in gui. 
    public float SpriteN;

    public Button myButton;

    void Start(){
        gameObject.GetComponent<Button>().onClick.AddListener(TaskOnClick);


    }




    void TaskOnClick()
    {
        //print("mouse down: " + SpriteN.ToString());

        //print("MouseDown");
        //if (scenecontroller.GetComponent<Vocab_SceneControllerMainTask>() != null)
        //{
            //print("click image");
            scenecontroller.GetComponent<Vocab_SceneControllerMainTask>().TaskOnClick(SpriteN);
        //}

        //if (scenecontroller.GetComponent<Vocab_SceneControllerPracticeRounds>() != null)
        //{
        //    scenecontroller.GetComponent<Vocab_SceneControllerPracticeRounds>().TaskOnClickR();
        //}
    }
}