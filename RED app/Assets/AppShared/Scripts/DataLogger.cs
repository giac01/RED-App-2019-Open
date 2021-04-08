using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLogger : MonoBehaviour {

	// SERIALISED FIELDS
	[SerializeField] private string separator = ",";

	// VARIABLES.
	private bool _headerLogged = false;
	private string _participantNumber;
	private string _fileName;
	private string _filePath;
	private StreamWriter _streamWriter;

	public string filePath
	{
		get {return _filePath;}
	}

	// Use this for initialization
	void Awake () 
	{
		// Get application version number
		string appVersion = "AppVersion" + Application.version;
		// Load the participant number form the PlayerPrefs.
		_participantNumber = PlayerPrefs.GetString("ParticipantNumber");
		// Get the current scene. We need this to find the task name.
		UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
		// Get the current time.
		string time = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
		// Construct the file name.
		_fileName = string.Format("{0}_{1}_{2}_{3}.txt", scene.name, _participantNumber, time, appVersion);
		// Open a file for this participant.
		_filePath = Application.persistentDataPath + Path.DirectorySeparatorChar + _fileName;

		//Create file to write within
		_streamWriter = File.CreateText(_filePath);
		// TODO: Take out this temporary debug log to the console.
		Debug.Log("Opened file: " + _filePath);
	}


    // PUBLIC FUNCTIONS

	// A function to log the header to a file.
	public void LogHeader(string[] header)
	{
		if (_headerLogged) 
		{
			// TODO: Throw Exception?
		} 
		else 
		{
			// Create line using a separator.
			_streamWriter.WriteLine(string.Join(separator, header));
		}
	}

    // A function to log a line of data variables.
    public void LogData(string[] data)
	{
		// Log the data.
		string line = string.Join (separator, data);
		_streamWriter.WriteLine (line);
		// TODO: Take out this temporary debug log to the console.
		Debug.Log (line);
	}

	public void Close ()
	{
		_streamWriter.Close ();
		Debug.Log("Closed file: " + _filePath);
	}

}
