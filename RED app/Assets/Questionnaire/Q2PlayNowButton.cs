using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Q2PlayNowButton : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {


    }

    private void OnMouseDown()
    {
        SceneManager.UnloadSceneAsync("Q2_Opening_Scene");
        SceneManager.LoadScene("Q2_Questions");


    }
}
