using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class HeartFlowerSoundButton : MonoBehaviour
{

    [SerializeField] HeartFlowerPracticeSceneController sceneController;
    [SerializeField] string SceneToLoad;
    [SerializeField] float soundDuration;
    [SerializeField] bool playOnAwake = true;
    [SerializeField] bool activateHighlight = true;

    // VARIABLES
    private AudioSource _audio;
    private bool _activated = false;
    private bool _audioPlayed = false;

    // Use this for initialization
    void Start()
    {
        // Find the audio associated with this object.
        _audio = GetComponent<AudioSource>();

        // Play the audio on awakening.
        if (playOnAwake)
        {
            // Play the audio.
            _audio.Play();
            // Wait for a bit.
            StartCoroutine(WaitThenActivate(soundDuration));
            // Toggle the audio bool.
            _audioPlayed = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // OnMouseDown is called when the Start Button is clicked.
    void OnMouseDown()
    {
        // Play audio if it's required on click.
        if (_audioPlayed == false)
        {
            // Play the audio.
            _audio.Play();
            // Wait for a bit.
            StartCoroutine(WaitThenActivate(soundDuration));
            // Toggle the audio bool.
            _audioPlayed = true;
        }
        // Load the next scene if the button is activated.
        if (_activated)
        {
            // Load the task.
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneToLoad);
        }
    }

    public IEnumerator WaitThenActivate(float delaySeconds)
    {
        // Wait for the requested amount of seconds.
        yield return new WaitForSeconds(delaySeconds);
        // Activate the button.
        _activated = true;
        // Optionally activate the highlight.
        if (activateHighlight) {
            sceneController.Highlight(false,true);
        }
    }
}
