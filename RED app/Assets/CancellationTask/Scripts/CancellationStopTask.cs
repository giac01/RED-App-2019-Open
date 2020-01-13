using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancellationStopTask : MonoBehaviour {

	// SERIALIZED FIELDS
	[SerializeField] private DataLogger logger;
	[SerializeField] string SceneToLoad;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // TaskStop is called when the Stop Button is clicked, or when the timer
    // runs out.
    void OnMouseDown()
    {
        {
            TaskStop();
        }
    }
    public void TaskStop()
    {
		// Close the log file.
		logger.Close();
		// Stop the task after a 0.5 second delay.
        StartCoroutine(TaskStopAfterDelay(0.5f));
    }

    IEnumerator TaskStopAfterDelay(float delaySeconds)
    {
        // Pause for the requested time.
        yield return new WaitForSeconds(delaySeconds);
        // Load the scene that follows the cancellation task.
		UnityEngine.SceneManagement.SceneManager.LoadScene(SceneToLoad);
    }
}
