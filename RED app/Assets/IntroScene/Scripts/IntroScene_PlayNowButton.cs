using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroScene_PlayNowButton : MonoBehaviour {
	public GameObject MainCamera;
	public Button PlayButton;
	public AudioClip PlaySound; //assigned in gui

	// Use this for initialization
	void Start () {
		PlayButton = gameObject.GetComponent<Button>();
		PlayButton.onClick.AddListener(TaskOnClick);
	}
	
	// Update is called once per frame
	void TaskOnClick () {
		print ("click");
		MainCamera.GetComponent<AudioSource> ().Stop ();
		PlayButton.GetComponent<AudioSource> ().Play();
		StartCoroutine(WaitTillSoundStopped ());
	}

	IEnumerator WaitTillSoundStopped()
	{
		//Debug.Log("Waiting for princess to be rescued...");
		print("test1");
		yield return new WaitUntil(()=> !PlayButton.GetComponent<AudioSource>().isPlaying);
		print ("stop audio");

		SceneManager.LoadScene ("Intro_Rules");

	}
}
