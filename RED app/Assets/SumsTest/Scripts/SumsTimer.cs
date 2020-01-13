using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumsTimer : MonoBehaviour {

	[SerializeField] private float taskDuration;
	[SerializeField] private TextMesh timeLeftText;
	[SerializeField] private SumsSceneController sceneController;

	private float _startTime;
	private float _currentTime;
	private float _timeLeft;

	// Use this for initialization
	void Start () {
		// Get the start time of this scene.
		// (Note that this is an arbitrary number which future
		// calls to realTimeSinceStartup will be compared against.)
		_startTime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {

		// Update the current time.
		_currentTime = Time.realtimeSinceStartup - _startTime;

		// Compute how much time is left.
		_timeLeft = taskDuration - _currentTime;

		// Stop the task if the time is up.
		if (_timeLeft <= 0.0f) 
		{
			sceneController.TimeUp();
			timeLeftText.text = "00:00";
		}
		else
		{
			// Update the time text.
			int minutes = (int)_timeLeft / 60;
			int seconds = (int)_timeLeft % 60;
			string timeString = string.Format ("{0}:{1}",
				                   minutes.ToString ().PadLeft (2, '0'),
				                   seconds.ToString ().PadLeft (2, '0'));
			timeLeftText.text = timeString;
		}
	}

	// This function allows other objects to get the current task
	// time in seconds.
	public float getCurrentTime() {
		return _currentTime;
	}
}
