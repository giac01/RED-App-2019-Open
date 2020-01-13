using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LE_EndGameButton : MonoBehaviour {
	public bool ContinuePress;
	public DataLogger Logger;

	// Use this for initialization
	void Start () {
		Logger = GameObject.Find ("Logger").GetComponent<DataLogger> ();
		ContinuePress = false;
		Button btn = gameObject.GetComponent<Button> ();
		btn.onClick.AddListener (ClickFunction);

	}

	// Update is called once per frame
	void ClickFunction () {
		ContinuePress = true; //This doesn't do anything yet...


		Logger.Close ();
		UnityEngine.SceneManagement.SceneManager.LoadScene("InBetweenScene");
	}
}
