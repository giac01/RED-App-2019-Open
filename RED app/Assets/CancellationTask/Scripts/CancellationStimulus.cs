using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancellationStimulus : MonoBehaviour {
    
    /// <summary>
    /// This class handles interactions with the stimuli. Specifically, it
    /// registers when a stimulus is tapped, and changes the stimulus Sprite
    /// accordingly. An event is also logged.
    /// </summary>

    // SERIALIZED FIELDS
	[SerializeField] private CancellationTimeOut timer;
    [SerializeField] private DataLogger logger;
	[SerializeField] private CancellationCounter counter;

    // VARIABLES
	private AudioSource _audio;
    // The following variables are set by SceneController.
	private bool _unclicked = true;
    private int _posX;
    private int _posY;
    private string _stimulusType;
    public string stimulusType
    {
        get { return _stimulusType; }
    }
    private Sprite _cancelledImage;


    // PUBLIC METHODS
    public void SetStimulus(int posX, int posY, string stimulusType, Sprite image, Sprite cancelledImage)
    {
        _posX = posX;
        _posY = posY;
        _stimulusType = stimulusType;
        _cancelledImage = cancelledImage;
        GetComponent<SpriteRenderer>().sprite = image;
    }


    // METHODS

    // Use this for initialization.
    void Start ()
    {
		// Get the Audio Source component.
		_audio = GetComponent<AudioSource>();
    }

    void OnMouseDown ()
    {
		// Get the time, and convert it to milliseconds.
		int _time = (int)(timer._currentTime * 1000.0f);
        // Log the touch on this stimulus.
		string[] data = {_time.ToString(), _posX.ToString(), _posY.ToString(), stimulusType};
		logger.LogData(data);
        // If this stimulus is a target, change its sprite.
        GetComponent<SpriteRenderer>().sprite = _cancelledImage;

		// Play the sound, but only if this is a target.
		if (_stimulusType == "target") {
			_audio.Play();
		}

		// Notify the cancellation counter if this target wasn't cancelled yet.
		if ((_stimulusType == "target") & (_unclicked)) {
			counter.AddOne ();
		}
		// Set the _unclicked Boolean to False.
		_unclicked = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
