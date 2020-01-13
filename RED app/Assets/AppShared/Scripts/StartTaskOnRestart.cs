using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTaskOnRestart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// OnMouseDown is called when the Start Button is clicked.
	void OnMouseDown()
	{
		// Get the list of tasks from the Player Preferences.
		string _taskList = PlayerPrefs.GetString("TaskList");
		// The list is formatted as comma-separated string, and should be converted to an array.
		string[] _taskOrder = _taskList.Split(',');
		// Find the current task.
		int _currentTask = PlayerPrefs.GetInt("CurrentTask");
		string SceneToLoad = _taskOrder[_currentTask];
		// Load the scene for the current task.
		UnityEngine.SceneManagement.SceneManager.LoadScene(SceneToLoad);
	}
}
