using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Q3PlayNowButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    private void OnMouseDown()
    {
        SceneManager.UnloadSceneAsync("Q3_Opening_Scene");
        SceneManager.LoadScene("Q3_Questions");
    }
}
