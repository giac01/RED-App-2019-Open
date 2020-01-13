using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Davis_SpriteScript : MonoBehaviour
{

    public GameObject scenecontroller; //Assigned in gui. 
    public float SpriteN;


    void OnMouseDown()
    {
        //print("mouse down");
        //print("MouseDown");
        if (scenecontroller.GetComponent<Davis_SceneControllerMainTask>() != null)
        {
            scenecontroller.GetComponent<Davis_SceneControllerMainTask>().TaskOnClick(SpriteN);
        }

        //if (scenecontroller.GetComponent<Vocab_SceneControllerPracticeRounds>() != null)
        //{
        //    scenecontroller.GetComponent<Vocab_SceneControllerPracticeRounds>().TaskOnClickR();
        //}
    }
}