using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancellationCounter : MonoBehaviour {

	/// <summary>
	/// Counts the number of targets that were cancelled, and stops
	/// the task when all targets are cancelled.
	/// </summary>

	// SERIALIZED FIELDS
	[SerializeField] private CancellationStopTask stopTask;
	[SerializeField] int targetNumber = 40;

	// VARIABLES
	private int currentCount = 0;
	private bool taskStopped = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// AddOne is called when a target is cancelled.
	public void AddOne() {
		// Increment the counter.
		currentCount += 1;

		// Check if all targets are found yet.
		if ((currentCount >= targetNumber) & (taskStopped == false)) {
			// Stop the task.
			Debug.Log("All targets found!");
			taskStopped = true;
			stopTask.TaskStop();
		}
	}
}
