using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class GoNoGoStartTask : MonoBehaviour {

	// SERIALISED FIELDS
	[SerializeField] private float waitDuration = 12.4f;
	[SerializeField] string SceneToLoad;

	// VARIABLES
	private float _startTime;
	private float _currentTime;

	// Use this for initialization
	void Start () {
		// Get the start time.
		_startTime = Time.realtimeSinceStartup;
	}

	// Update is called once per frame
	void Update () {
		// Get the current time.
		_currentTime = Time.realtimeSinceStartup - _startTime;
	}

	// OnMouseDown is called when the Start Button is clicked.
	void OnMouseDown() {
		// Only become responsive after the initial wait.
		if (_currentTime > waitDuration) {
			// Wait for a bit.
			StartCoroutine (WaitThenStart (0.5f));
		}
	}

	public IEnumerator WaitThenStart(float delaySeconds)
	{
		// Wait for the requested amount of seconds.
		yield return new WaitForSeconds(delaySeconds);
		// Load the task.
		UnityEngine.SceneManagement.SceneManager.LoadScene(SceneToLoad);
	}
}
