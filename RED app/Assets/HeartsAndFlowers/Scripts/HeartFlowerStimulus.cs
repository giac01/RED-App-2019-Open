using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartFlowerStimulus : MonoBehaviour {

    [SerializeField] bool isPractice = false;
    [SerializeField] float durationSecs = 1.5f;

    private HeartFlowerPracticeSceneController controller;

    // Variables
    private float startTime;
    private float currentTime;
    private bool timedOut = false;

    // Use this for initialization
    void Start () {
        // Get the start time.
        startTime = Time.realtimeSinceStartup;
        // Set the duration to -1 if this is a practice.
        if (isPractice)
        {
            durationSecs = -1;
        }
    }

    // Update is called once per frame
    void Update () {
        // Compute the time since this stimulus came to be.
        currentTime = Time.realtimeSinceStartup - startTime;
        // Check whether the stimulus has been on for its maximum duration, but
        // only do so if there is a timeout. Also, practice stimuli don't have
        // a timeout per definition.
        if (durationSecs > 0 & currentTime >= durationSecs)
        {
            TimeOut(true);
        }
    }

    public void SetSceneController(HeartFlowerPracticeSceneController sceneController) {
        controller = sceneController;
    }

    // Called to kill the stimulus.
    public void TimeOut(bool timeout)
    {
        // Notify SceneController, but only if this is the first time the
        // timeout is noticed.
        if (timedOut == false)
        {
            controller.OnStimTimeout();
            // Set the timedOut Boolean.
            timedOut = true;
        }
        Begone();
    }

    public void Begone() {
        // Suicide is painless!
        Destroy(gameObject);
    }
}
