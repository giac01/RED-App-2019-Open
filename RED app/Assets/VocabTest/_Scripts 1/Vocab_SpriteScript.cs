using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vocab_SpriteScript : MonoBehaviour
{

    public GameObject scenecontroller; //Assigned in gui. 
    public float SpriteN;


    void OnMouseDown()
    {
        //print("mouse down: " + SpriteN.ToString());

        //print("MouseDown");
        if (scenecontroller.GetComponent<Vocab_SceneControllerMainTask>() != null)
        {
            scenecontroller.GetComponent<Vocab_SceneControllerMainTask>().TaskOnClick(SpriteN);
        }

        //if (scenecontroller.GetComponent<Vocab_SceneControllerPracticeRounds>() != null)
        //{
        //    scenecontroller.GetComponent<Vocab_SceneControllerPracticeRounds>().TaskOnClickR();
        //}
    }
}