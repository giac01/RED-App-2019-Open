using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoNoGoStopTask : MonoBehaviour {

	// SERIALIZED FIELDS
	[SerializeField] private GoNoGoSceneController controller;
	[SerializeField] string SceneToLoad;

	// Variables
	private bool taskStopped = false;

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
		if (taskStopped == false ) {
            TaskStop();
			taskStopped = true;
        }
    }

	private void TaskStop()
    {
		// Stop the task after a delay (in seconds).
        StartCoroutine(TaskStopAfterDelay(0.3f));
    }

    private IEnumerator TaskStopAfterDelay(float delaySeconds)
    {
        // Pause for the requested time.
        yield return new WaitForSeconds(delaySeconds);
        // Tell the SceneController to stop and load the next scene.
		controller.Stop(SceneToLoad);
    }
}
