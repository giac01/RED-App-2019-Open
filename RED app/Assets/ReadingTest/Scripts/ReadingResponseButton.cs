using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadingResponseButton : MonoBehaviour {

	[SerializeField] private ReadingSceneController sceneController;
	[SerializeField] private ReadingTimer timer;
	[SerializeField] private DataLogger logger;
	[SerializeField] private TextMesh sentence;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// OnMouseDown is called when the button is clicked.
	void OnMouseDown() {

		// Get the time, and convert it to milliseconds.
		int _time = (int)(timer.getCurrentTime() * 1000.0f);

		// Log the touch on this stimulus.
		string[] data = {_time.ToString(), sentence.text, this.name};
		logger.LogData(data);
		//Debug.Log (data);

		// Update the sentence.
		sceneController.NextSentence();
	}
}
