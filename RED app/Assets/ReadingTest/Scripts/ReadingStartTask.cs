using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class ReadingStartTask : MonoBehaviour {

	[SerializeField] string SceneToLoad;

	// VARIABLES
	private AudioSource _audio;

	// Use this for initialization
	void Start () 
	{
		// Get the Audio Source component.
		//_audio = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {

	}

	// OnMouseDown is called when the Start Button is clicked.
	void OnMouseDown()
	{
		// Play the sound.
		//_audio.Play();
		// Wait for a bit.
		StartCoroutine(WaitThenStart(0.1f));
	}

	public IEnumerator WaitThenStart(float delaySeconds)
	{
		// Wait for the requested amount of seconds.
		yield return new WaitForSeconds(delaySeconds);

		// Load the task.
		UnityEngine.SceneManagement.SceneManager.LoadScene(SceneToLoad);
	}
}
