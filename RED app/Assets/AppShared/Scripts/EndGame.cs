using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

	[SerializeField] private DataLogger logger;

	private bool endTimeLogged = false;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		// Log the end time.
		// (Must be done here, as logger isn't initialised yet during Start call.)
		if (endTimeLogged == false) 
		{
			//Add starttime to playerprefs then log data 
			PlayerPrefs.SetString ("EndTime", System.DateTime.Now.ToString()); 
			string[] header = { "StartTime", "EndTime" }; 
			logger.LogHeader (header);
			string[] info = { PlayerPrefs.GetString ("StartTime"), PlayerPrefs.GetString ("EndTime") };
			logger.LogData (info);
			logger.Close ();
			endTimeLogged = true;
		}
	}


}
