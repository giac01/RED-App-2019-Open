using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppStartPromptSave : MonoBehaviour {

	// SERIALISED FIELDS
	[SerializeField] private GameObject startButton;

	// VARIABLES
	private InputField input;

	// Use this for initialization
	void Start () 
	{
		// Set the current task in the player preferences.
		PlayerPrefs.SetInt("CurrentTask", 0);
		// Determine the task order.
		string[] _taskOrder = new string[] {
			"AppStartPrompt",
			"Intro_Scene",
			"ReadingStart", 
			"SumsStart",
			"CancellationStart",
			"Q1_Opening_Scene",
			"Digit_Span",
			"DavisTest",
			"Q2_Opening_Scene",
			"Dot_Matrix",
            "GoNoGoStart",
            
            "Q3_Opening_Scene",
            "ANS_StartScreen",
            "Ev_Accu",
            "PhonoAwareness",
            "LC_MainTask1",
			"LE_MainTask",
			"GoNoGoGame",
			"AppEnd"
		};
		// Format the list of tasks in a single comma-separated string.
		string _taskList = string.Join(",", _taskOrder);
		// Save the task order in the player preferences.
		PlayerPrefs.SetString("TaskList", _taskList);

		// Deactivate the start button.
		startButton.SetActive(false);

		// Make the input field call the SubmitName function when completed.
		input = gameObject.GetComponent<InputField>();
		input.onEndEdit.AddListener(SubmitName);
		//input.onValueChanged.AddListener(SubmitName);

	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	private void SubmitName(string name)
	{
		// Set the participant number.
		SetNameInPlayerPrefs (name);
		// Reactivate the start button.
		startButton.SetActive(true);
	}

	public void SetNameInInputField(string name) {
		// Set the name on the input field.
		input.text = name;
	}

	public void SetNameInPlayerPrefs (string name) {
		// Set the participant number.
		PlayerPrefs.SetString("ParticipantNumber", name);
		//Add starttime to playerprefs 
		PlayerPrefs.SetString("StartTime", System.DateTime.Now.ToString()); 
		// Debug info.
		Debug.Log ("Set new participant number in PlayerPrefs: " + PlayerPrefs.GetString("ParticipantNumber"));
		Debug.Log ("Set new start time in PlayerPrefs: " + PlayerPrefs.GetString("StartTime"));
	}
}
