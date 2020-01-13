using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoNoGoSceneController : MonoBehaviour {

	// SERIALISED FIELDS
    [SerializeField] private float taskDurationSeconds = 240.0f;
    [SerializeField] private string nextScene;
    [SerializeField] private bool waitUntilClicked = true;
	[SerializeField] private float targetProbability = 0.8f;
	[SerializeField] private float minDelay = 0.3f;
	[SerializeField] private float maxDelay = 0.8f;
	[SerializeField] private float popupSeconds = 0.3f;
	[SerializeField] private int targetPoints = 50;
	[SerializeField] private int distractorPoints = -50;
	[SerializeField] private Sprite target;
	[SerializeField] private Sprite distractor;
	[SerializeField] private AudioClip targetSound;
	[SerializeField] private AudioClip distractorSound;
	[SerializeField] private GoNoGoStimulus originalStimulus;
	[SerializeField] private DataLogger logger;
	[SerializeField] private TextMesh pointTextMesh;
	[SerializeField] private TextMesh popupTextMesh;

	// VARIABLES
	// The current stimulus and its properties.
	private int stimNr;
	private GoNoGoStimulus stim;
	private string stimType;
	private Sprite stimSprite;
	private AudioClip stimSound;
	// Game variables.
	private int pointChange = 0;
	private int pointCount = 0;

	// CONSTANTS
	// The screen dimensions in pixels. Note that these must be made to
	// correspond with the settings manually!
	private float leftBound;
	private float rightBound;
	private float lowerBound;
	private float upperBound;
	// Position and buffers for potential positions of the stimuli.
	private float posX;
	private float posY;
	private float[] xBuffer = {1.5f, 1.5f};
	private float[] yBuffer = {1.5f, 3.5f};
	private float screenWidth;
	private float screenHeight;
	// Pixel values for logging convenience.
	private int xPixelPos;
	private int yPixelPos;
	// The depth is hard-coded, and determines what other information elements
	// can obscure the stimuli.
	private float posZ = -10.0f;
	// Timing: Start time registered in Start(), current in Update()
	private float _startTime;
	private float _currentTime;
    private float _lastSpawnTime;
    private float _spawnDelay;

	// Use this for initialization
	void Start () {
		// Get the start time.
		_startTime = Time.realtimeSinceStartup;

		// Set the screen bounds.
		lowerBound = -1.0f * Camera.main.orthographicSize;
		upperBound = Camera.main.orthographicSize;
		leftBound = -1.0f * Camera.main.aspect * Camera.main.orthographicSize;
		rightBound = Camera.main.aspect * Camera.main.orthographicSize;
		screenWidth = 2.0f * (Camera.main.aspect * Camera.main.orthographicSize);
		screenHeight = 2.0f * Camera.main.orthographicSize;

		// Use to the original stimulus.
		stim = Instantiate(originalStimulus) as GoNoGoStimulus;
		// Set the stimulus to the centre of the display.
		stimType = "target";
		stimSprite = target;
		stimSound = targetSound;
		// Set the new stimulus properties.
		SetStim(false);
        // Set the last spawn time.
        _lastSpawnTime = Time.realtimeSinceStartup - _startTime;
        // Set the first spawn delay.
        _spawnDelay = UnityEngine.Random.Range(minDelay, maxDelay);

        // Log a header.
		string[] header = {
			"stimnr", 
			"time", 
			"stimtype", 
			"x", 
			"y", 
			"xpixel", 
			"ypixel", 
			"RT", 
			"timeout", 
			"pointchange", 
			"pointtotal"};
		logger.LogHeader(header);
	}

	// Use this to end the task.
	public void Stop(string nextScene) {
		// Close the Logger.
		logger.Close();
		// Load the next scene.
		UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
	}
	
	// Update is called once per frame
	void Update () {
		// Get the current time.
		_currentTime = Time.realtimeSinceStartup - _startTime;
        // Check if the current time is beyond the duration, but only if the
        // duration is a positive number.
        if (taskDurationSeconds > 0) {
            if (_currentTime > taskDurationSeconds) {
                Stop(nextScene);
            }
        }

        // If we're not waiting until the previous stimulus has been clicked
        // to spawn new ones, we need to keep track of time here.
        if (waitUntilClicked == false) {
            // Check the time since the last stimulus appeared.
            if (_currentTime - _lastSpawnTime > _spawnDelay) {
                // Make the new stimulus.
                StartCoroutine(WaitThenMakeNewStim(0.0f));
                // Randomly choose the next waiting time.
                _spawnDelay = UnityEngine.Random.Range(minDelay, maxDelay);
            }
        }
	}

	private void InitStim() {
		// Initialise a new stimulus (always a target).
		stim = Instantiate(originalStimulus) as GoNoGoStimulus;
		// Update the stimulus number.
		stimNr ++;
	}

	private void RandomiseStim() {
		// Choose a random location on the screen.
		posX = UnityEngine.Random.Range(leftBound+xBuffer[0], rightBound-xBuffer[1]);
		posY = UnityEngine.Random.Range(lowerBound+yBuffer[0], upperBound-yBuffer[1]);
		// Choose a target type.
		float p = UnityEngine.Random.Range(0.0f, 1.0f);
		if (p < targetProbability) {
			stimType = "target";
		}
		else {
			stimType = "distractor";
		}
		// Copy target/distractor to stimulus properties.
		if (stimType == "target") {
			stimSprite = target;
			stimSound = targetSound;
		} 
		else {
			stimSprite = distractor;
			stimSound = distractorSound;
		}
	}

	private void SetStim(bool randomise) {
		if (randomise) {
			RandomiseStim ();
		}

		// Set the stimulus properties.
        stim.SetStimulus(this, stimNr, posX, posY, stimType, stimSprite, stimSound);

		// Move the stimulus to its new position.
		stim.transform.position = new Vector3(posX, posY, posZ);
	}

    public void ClickedCurrentStim(int _stimNr, float _posX, float _posY, string _stimulusType, bool timeOut, float respTime) {
		// Update the pointcount and the point popup.
		if (timeOut == false) {
			if (_stimulusType == "target") {
				pointChange = targetPoints;
			} else {
				pointChange = distractorPoints;
			}
			pointCount += pointChange;
			StartCoroutine (PointPopup (pointChange, _posX, _posY + 2.0f));
			pointTextMesh.text = pointCount.ToString();
		}
		else {
			pointChange = 0;
		}
		// Compute the positions in pixels, with (0,0) as the top left.
		int _xPixelPos = (int) (100.0f * ((screenWidth/2.0f)  + _posX));
		int _yPixelPos = (int) (100.0f * ((screenHeight/2.0f) - _posY));
		// Log click on the current stimulus.
		string[] data = {
			_stimNr.ToString(), 
			_currentTime.ToString(), 
			_stimulusType, 
			_posX.ToString(), 
			_posY.ToString(), 
			_xPixelPos.ToString(), 
			_yPixelPos.ToString(), 
			respTime.ToString(), 
			timeOut.ToString(), 
			pointChange.ToString(), 
			pointCount.ToString()};
		logger.LogData(data);

        // Create the new stimulus upon the clicking of the old, but
        // only if this option is set.
        if (waitUntilClicked) {
            // Randomly choose the waiting time.
            float delaySecs = UnityEngine.Random.Range(minDelay, maxDelay);
            // Wait and then make the new stimulus.
            StartCoroutine(WaitThenMakeNewStim(delaySecs));
        }
	}

	private IEnumerator PointPopup (int points, float x, float y) {
		// Update the point popup.
		popupTextMesh.text = points.ToString();
		if (points == 0) {
			popupTextMesh.color = UnityEngine.Color.white;
		} 
		else if (points > 0) {
			popupTextMesh.text = "+" + points.ToString();
			popupTextMesh.color = UnityEngine.Color.green;
		} 
		else {
			popupTextMesh.color = UnityEngine.Color.red;
		}
		popupTextMesh.transform.position = new Vector3(x, y, posZ);
		// Wait for a short bit.
		yield return new WaitForSeconds(popupSeconds);
		// Reset the text.
		popupTextMesh.text = "";
		popupTextMesh.color = UnityEngine.Color.white;
	}

	private IEnumerator WaitThenMakeNewStim(float delaySeconds)
	{
		// Wait for the requested amount of seconds.
		yield return new WaitForSeconds(delaySeconds);
		// Initialise a new stimulus.
		InitStim();
		// Randomise and set the new stimulus' properties.
		SetStim(true);
        // Set the last spawn time.
        _lastSpawnTime = Time.realtimeSinceStartup - _startTime;
	}
}
