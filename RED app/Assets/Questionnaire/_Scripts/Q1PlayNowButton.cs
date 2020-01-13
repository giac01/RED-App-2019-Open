using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Q1PlayNowButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnMouseDown()
    {
        SceneManager.UnloadSceneAsync("Q1_Opening_Scene");
        SceneManager.LoadScene("Q1_Questions");


    }
}
