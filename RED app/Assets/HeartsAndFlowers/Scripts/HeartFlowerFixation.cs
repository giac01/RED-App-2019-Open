using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartFlowerFixation : MonoBehaviour
{

    [SerializeField] float durationSecs = 0.5f;

    private HeartFlowerPracticeSceneController controller;

    // Variables
    private float startTime;
    private float currentTime;
    private bool timedOut = false;

    // Use this for initialization
    void Start()
    {
        // Get the start time.
        startTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        // Compute the time since this stimulus came to be.
        currentTime = Time.realtimeSinceStartup - startTime;
        // Check whether the stimulus has been on for its maximum duration.
        if (currentTime >= durationSecs)
        {
            TimeOut(true);
        }
    }

    public void SetSceneController(HeartFlowerPracticeSceneController sceneController)
    {
        controller = sceneController;
    }

    // Called to kill the stimulus.
    public void TimeOut(bool timeout)
    {
        // Notify SceneController, but only if this is the first time the
        // timeout is noticed.
        if (timedOut == false)
        {
            // Notify the ScreenController.
            controller.OnFixTimeout();
            // Set the timedOut Boolean.
            timedOut = true;
        }
        // Suicide is painless!
        Destroy(gameObject);
    }
}
