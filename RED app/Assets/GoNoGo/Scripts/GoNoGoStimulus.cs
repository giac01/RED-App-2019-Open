using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoNoGoStimulus : MonoBehaviour {

	// SERIALISED FIELDS
	// Timeout in seconds.
	[SerializeField] float selfDestruct = 2.5f;
	[SerializeField] float selfDestructDelay = 0.3f;

	// VARIABLES
	private bool isClicked = false;
	private bool isDying = false;
	private float _startTime;
	private float _currentTime;
	private float _respTime;

	// The following variables are set by the SceneController.
	private GoNoGoSceneController _controller;
    private int _stimNr;
    private float _posX;
    private float _posY;
	private string _stimulusType;
	public string stimulusType
	{
		get { return _stimulusType; }
	}
	private SpriteRenderer _image;
	private AudioSource _audio;

	// Set the current stimulus' properties from the
	public void SetStimulus(GoNoGoSceneController controller, int stimNr, float posX, float posY, string stimulusType, Sprite image, AudioClip audio)
	{
		// Log the start time.
		_startTime = Time.realtimeSinceStartup;
		// Set the properties of this stimulus.
		_controller = controller;
        _stimNr = stimNr;
        _posX = posX;
        _posY = posY;
		_stimulusType = stimulusType;
		_image = GetComponent<SpriteRenderer>();
		_image.sprite = image;
		_audio = GetComponent<AudioSource>();
		_audio.clip = audio;
	}


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// Get the current time.
		_currentTime = Time.realtimeSinceStartup - _startTime;
		// To be, or not to be? That kinda depends on how long we've been alive.
		if (_currentTime > selfDestruct) {
			// Suicide!
			LifeEnd(true);
		}
	}

	// OnMouseDown is called when the item is clicked/touched.
	void OnMouseDown() {
		if (isClicked == false) {
			// Play the sound associated with this stimulus.
			_audio.Play();
			// Set isClicked to true to prevent further activation.
			isClicked = true;
			// Let the SceneController know this item got clicked.
			LifeEnd(false);
		}
	}

	// Function to clean up (destroy handled via SceneController)
	public void LifeEnd (bool timeOut) {
		// Only call this function once.
		if (isDying == false) {
			// Set the isDying bool.
			isDying = true;
			// Compute the response time, or set it to the maximum time
			// if a timeout occurred.
			if (timeOut) {
				_respTime = selfDestruct;
			} else {
				_respTime = _currentTime;
			}
			// Let the SceneController know the timeout status.
            _controller.ClickedCurrentStim(_stimNr, _posX, _posY, _stimulusType, timeOut, _respTime);
			// Wait for a bit, then commit suicide.
			StartCoroutine (SuicideIsPainless (selfDestructDelay));
		}
	}

	private IEnumerator SuicideIsPainless(float delaySeconds)
	{
		// Wait for the requested amount of seconds.
		yield return new WaitForSeconds(delaySeconds);
		// Suicide!
		Destroy(gameObject);
	}

}
