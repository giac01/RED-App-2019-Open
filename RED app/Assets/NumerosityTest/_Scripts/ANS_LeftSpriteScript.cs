using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANS_LeftSpriteScript : MonoBehaviour
{

    public GameObject scenecontroller; //Assigned in gui. 

    void OnMouseDown()
    {
        if (scenecontroller.GetComponent<ANS_SceneControllerMainTask>() != null)
        {
            scenecontroller.GetComponent<ANS_SceneControllerMainTask>().TaskOnClickL();
        }

        if (scenecontroller.GetComponent<ANS_SceneControllerPracticeRounds>() != null)
        {
            scenecontroller.GetComponent<ANS_SceneControllerPracticeRounds>().TaskOnClickL();
        }
    }

}