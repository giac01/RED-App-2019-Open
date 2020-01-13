using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LC_IntroSceneController : MonoBehaviour {

	public GameObject PlayButton, QuitButton; //assigned in gui
	//public GameObject QuitButton; //assigned in gui


	void Start()
	{
		Button PB = PlayButton.GetComponent<Button>();
		Button QB = QuitButton.GetComponent<Button>();
		QB.onClick.AddListener(TaskOnClickQuit);
		PB.onClick.AddListener(TaskOnClickPlay);
	}

	void TaskOnClickQuit()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("InBetweenScene");
	}

	void TaskOnClickPlay()
	{
		//Logger.Close ();
		// Load the next scene.
		UnityEngine.SceneManagement.SceneManager.LoadScene("LC_MainTask1");
	}

	void TaskWithParameters(string message)
	{
		//Output this to console when the Button is clicked
		Debug.Log(message);
	}
}

