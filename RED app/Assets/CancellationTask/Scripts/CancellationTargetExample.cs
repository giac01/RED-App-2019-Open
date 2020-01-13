using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancellationTargetExample : MonoBehaviour
{

    /// <summary>
    /// Allows for interaction with the example target. Will hide the target
    /// on click, to show a different sprite, and simultaneously play a sound.
    /// </summary>

    // SERIALIZED FIELDS
	[SerializeField] private bool isTarget;
    [SerializeField] private Sprite replacementSprite;
    [SerializeField] private GameObject startButton;
	[SerializeField] private float explanationDelay;

    // VARIABLES
    private AudioSource _audio;
    private Sprite _targetSprite;
	private bool _cancelled = false;
	private bool _active = false;


    // FUNCTIONS

    // Use this for initialization.
    void Start()
    {
        // Get the Audio Source component.
        _audio = GetComponent<AudioSource>();
        // Get the current sprite.
        _targetSprite = GetComponent<SpriteRenderer>().sprite;
        // Deactivate the start button.
        startButton.SetActive(false);

		// Wait and self-activate.
		StartCoroutine(ActivateSelf(explanationDelay));
    }

    // Update is called once per frame.
    void Update()
    {
        // Nothing to see here, move along.
    }

    // OnMouseDown is called when the mouse clicks on the target.
    public void OnMouseDown()
    {
		// Only do the following if the target is yet uncancelled.
		if (_cancelled == false & _active == true) 
		{
			// Show the replacement sprite.
			GetComponent<SpriteRenderer> ().sprite = replacementSprite;
			// Play the sound.
			_audio.Play ();

			// Wait for a bit (in seconds) to reset.
			// EDIT: Decided to not reset it.
			//StartCoroutine(ResetInSeconds(1.0f));

			// Activate the start button (with a delay in seconds).
			// Only do this if this example is a target, though.
			if (isTarget) {
				StartCoroutine (ActivateStartButton (5.0f));
			}

			// Toggle the cancelled Boolean.
			_cancelled = true;
		}
    }

    public IEnumerator ResetInSeconds(float seconds)
    {
        // Wait for the requested amount of seconds.
        yield return new WaitForSeconds(seconds);
        // Redraw the original sprite.
        GetComponent<SpriteRenderer>().sprite = _targetSprite;
    }

    public IEnumerator ActivateSelf(float delaySeconds)
    {
        // Wait for the requested amount of seconds.
        yield return new WaitForSeconds(delaySeconds);
        // Activate ourself.
		_active = true;

	}

	public IEnumerator ActivateStartButton(float delaySeconds)
	{
		// Wait for the requested amount of seconds.
		yield return new WaitForSeconds(delaySeconds);
		// Activate the start button if it isn't active yet.
		if (startButton.activeSelf != true)
		{
			startButton.SetActive(true);
		}

	}
}