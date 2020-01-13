using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancellationTimeOut : MonoBehaviour {

    /// <summary>
    /// Counts down until the preset amount of time is reached, and displays
    /// a timer by updating the text on a Text Mesh.
    /// </summary>

    // SERIALIZED FIELDS
    [SerializeField] private TextMesh timeLabel;
	[SerializeField] private CancellationStopTask stopTask;
    [SerializeField] float taskDurationSeconds = 120.0f;
    
    // VARIABLES
    // A Boolean that is used to track whether the task has started.
    // It gets set to true when the task begins, and to false once it ends.
    private bool _taskStarted = false;
    // A Boolean that is used to track whether the task has stopped.
    // It gets set to true when the task ends.
    private bool _taskStopped = false;
    // Floats that keep track of the starting time, the current time, and the
    // amount of time that's left (all in seconds).
    private float _startTime;
    public float _currentTime;
    private float _leftTime;

    // Use this for initialization
    void Start()
    {
        // Wait for a second to start (this will make the transition from the
        // starting screen seem less abrupt).
        StartCoroutine(StartAfterPause(1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        // Only update the time if the task is still running.
        if (_taskStarted == true && _taskStopped == false)
        {
            // Get the current time.
            _currentTime = Time.realtimeSinceStartup - _startTime;
            _leftTime = taskDurationSeconds - _currentTime;
            int timeLeft = (int)_leftTime;
            // Subtract the starting time to get the elapsed time.
            // Update the time text.
            int minutes = timeLeft / 60;
            int seconds = timeLeft % 60;
            string timeString = string.Format("{0}:{1}",
                minutes.ToString().PadLeft(2, '0'),
                seconds.ToString().PadLeft(2, '0'));
            timeLabel.text = timeString;
        }
        // If the task isn't running yet, set the timer to the task duration.
        else if (_taskStarted == false && _taskStopped == false)
        {
            // Convert the task duration to an integer.
            int timeLeft = (int)taskDurationSeconds;
            // Subtract the starting time to get the elapsed time.
            // Update the time text.
            int minutes = timeLeft / 60;
            int seconds = timeLeft % 60;
            string timeString = string.Format("{0}:{1}",
                minutes.ToString().PadLeft(2, '0'),
                seconds.ToString().PadLeft(2, '0'));
            timeLabel.text = timeString;
        }
        // If the task has ended, set the timer to "00:00".
        else
        { 
            timeLabel.text = "00:00";
        }
    }

    public IEnumerator CountDown(float seconds)
    {
        // Wait for the requested amount of seconds.
        yield return new WaitForSeconds(seconds);

        // End the game.
        _taskStopped = true;
        stopTask.TaskStop();
    }

    public IEnumerator StartAfterPause(float seconds)
    {
        // Wait for the requested amount of seconds.
        yield return new WaitForSeconds(seconds);

        // Start the countdown.
        _taskStarted = true;
        _startTime = Time.realtimeSinceStartup;
        StartCoroutine(CountDown(taskDurationSeconds));
    }
}
