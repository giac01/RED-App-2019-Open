using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class StartTaskDemo : MonoBehaviour {

	[SerializeField] string TaskToLoad;

	// Default task order.
	private string[] _taskOrder = {"AppDemoStart","AppEnd"};

	// Use this for initialization
	void Start () {
		// Set the current task in the player preferences.
		PlayerPrefs.SetInt("CurrentTask", 0);
		// Set the participant number.
		PlayerPrefs.SetString ("ParticipantNumber", "DEMO");
		//Add starttime to playerprefs 
		PlayerPrefs.SetString ("StartTime", System.DateTime.Now.ToString ());
	}

	// Update is called once per frame
	void Update () {

	}

	// Bullshit convenience function because bullshit C# can't define variables 
	// within their bullshit if statements. C# is a garbage language with bullshit
	// global/local variable management. Bullshit.
	void SetTaskOrder(string[] taskOrder) {
		_taskOrder = taskOrder;
	}

	// OnMouseDown is called when the Button is clicked.
	void OnMouseDown()
	{
		// Set the correct task order.
		Debug.Log(TaskToLoad);
		if (TaskToLoad == "Questionnaire") 
		{
			Debug.Log("Questionnaire part of if statement.");
			string[] taskOrder = new string[] {
				"AppDemoStart",
				"Q1.Opening.Scene",
				"Q2.Opening.Scene",
				"Q3.Opening.Scene",
				"AppEnd"
			};
			SetTaskOrder (taskOrder);
		} 
		else 
		{
			// Determine the task order.
			string[] taskOrder = new string[] {
				"AppDemoStart",
				TaskToLoad,
				"AppEnd"
			};
			SetTaskOrder (taskOrder);
		}

		// Format the list of tasks in a single comma-separated string.
		string _taskList = string.Join (",", _taskOrder);
		// Save the task order in the player preferences.
		PlayerPrefs.SetString ("TaskList", _taskList);

		// Increment the current task to the actual task
		// (this replaces the InBetweenScene functionality).
		PlayerPrefs.SetInt("CurrentTask", PlayerPrefs.GetInt("CurrentTask")+1);

		// Find the current task.
		int _currentTask = PlayerPrefs.GetInt("CurrentTask");
		string SceneToLoad = _taskOrder[_currentTask];

		Debug.Log (_taskList);
		Debug.Log (SceneToLoad);

		// Load the task.
		UnityEngine.SceneManagement.SceneManager.LoadScene(SceneToLoad);
	}
}
