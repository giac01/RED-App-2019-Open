using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANS_RightSpriteScript : MonoBehaviour
{

    public GameObject scenecontroller; //Assigned in gui. 


    void OnMouseDown()
    {
        if (scenecontroller.GetComponent<ANS_SceneControllerMainTask>() != null)
        {
            scenecontroller.GetComponent<ANS_SceneControllerMainTask>().TaskOnClickR();
        }

        if (scenecontroller.GetComponent<ANS_SceneControllerPracticeRounds>() != null)
        {
            scenecontroller.GetComponent<ANS_SceneControllerPracticeRounds>().TaskOnClickR();
        }
    }
}