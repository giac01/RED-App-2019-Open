using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class CancellationStartTask : MonoBehaviour {

	[SerializeField] string SceneToLoad;

	// VARIABLES
	private AudioSource _audio;
	private bool _audioPlayed = false;

	// Use this for initialization
	void Start () {
		_audio = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {

	}

	// OnMouseDown is called when the Start Button is clicked.
	void OnMouseDown()
	{
		if (_audioPlayed == false) {
			// Play the audio.
			_audio.Play();
			// Wait for a bit.
			StartCoroutine (WaitThenStart (4.0f));
			// Toggle the audio bool.
			_audioPlayed = true;
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
