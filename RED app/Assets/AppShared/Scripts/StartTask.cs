using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class StartTask : MonoBehaviour {

	[SerializeField] string SceneToLoad;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // OnMouseDown is called when the Start Button is clicked.
    void OnMouseDown()
    {
		UnityEngine.SceneManagement.SceneManager.LoadScene(SceneToLoad);
    }
}
