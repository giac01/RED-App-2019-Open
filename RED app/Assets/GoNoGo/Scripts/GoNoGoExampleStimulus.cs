using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoNoGoExampleStimulus : MonoBehaviour {

	// SERIALISED FIELDS
	[SerializeField] private float waitDuration = 12.4f;
	[SerializeField] private float resetTime = 1.0f;
	[SerializeField] private float popupSeconds = 0.3f;
	[SerializeField] private GameObject popupText;

	// VARIABLES
	private AudioSource _audio;
	private bool isClicked = false;
	private float _startTime;
	private float _currentTime;
	private float _lastClickTime;

	// Use this for initialization
	void Start () {
		// Get the start time.
		_startTime = Time.realtimeSinceStartup;
		// Deactivate the popup.
		popupText.SetActive(false);
		// Get the audio source.
		_audio = GetComponent<AudioSource>();	
	}
	
	// Update is called once per frame
	void Update () {
		// Get the current time.
		_currentTime = Time.realtimeSinceStartup - _startTime;
		// Check if we should toggle the isClicked bool.
		if (_currentTime - _lastClickTime > resetTime) {
			isClicked = false;
		}
	}

	// OnMouseDown is called when the item is clicked/touched.
	void OnMouseDown() {
		// Only do things if the initial inactivity duration is over.
		if (isClicked == false & _currentTime > waitDuration) {
			// Update the most recent click time
			_lastClickTime = _currentTime;
			// Play the sound associated with this stimulus.
			_audio.Play();
			// Set isClicked to true to prevent further activation.
			isClicked = true;
			// Show the popup!
			StartCoroutine(Popup());
		}
	}

	private IEnumerator Popup () {
		// Show the popup.
		popupText.SetActive(true);
		// Wait for a short bit.
		yield return new WaitForSeconds(popupSeconds);
		// Hide the popup.
		popupText.SetActive(false);
	}
}
