using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroRules_SceneManager : MonoBehaviour {

	//All below objects are assigned in the gui
	public GameObject RulesImage, ContinueButt, VolumeReminder, Text1, Text2, Text3, Text4,StudentImage, TeacherImage, SilenceImage;
	public AudioClip Audio1, Audio2, Audio3, Audio4;
	public Button ContinueButton; 
	public AudioSource APlayer;

	[Header("GameLogic")]
	public bool ButtonClick;
	public float yButton; //Button Location


	// Use this for initialization
	void Start () {


		yButton = ContinueButt.GetComponent<RectTransform> ().localPosition.y;
		APlayer = gameObject.GetComponent<AudioSource> ();
		ContinueButton = ContinueButt.GetComponent<Button>();
		ContinueButton.onClick.AddListener(TaskOnClick);

		ButtonClick = false;
        StudentImage.SetActive(false);
        TeacherImage.SetActive(false);
		ContinueButt.SetActive (false);
        SilenceImage.SetActive(false);
		StartCoroutine ("PlayScene");

	}
	
	// Sorry this is not really efficient coding!
	IEnumerator PlayScene(){

		yield return new WaitForSecondsRealtime (1f);

		//Show 1st slide
		APlayer.PlayOneShot (Audio1);
		yield return new WaitUntil (() => !APlayer.isPlaying);
		ContinueButt.SetActive (true);
		yield return new WaitUntil (() => ButtonClick==true);
		ButtonClick = false;

		//Show 2nd Slide
		APlayer.PlayOneShot (Audio2);
		ContinueButt.SetActive (false);
		//ContinueButt.SetActive (false);
		Text1.SetActive (false);
		VolumeReminder.SetActive (false);
		RulesImage.SetActive (true);
		Text2.SetActive (true);
        StudentImage.SetActive(true);
        yield return new WaitUntil (() => !APlayer.isPlaying);
		ContinueButt.SetActive (true);
		yield return new WaitUntil (() => ButtonClick);
		ButtonClick = false;

		//Show 3rd Slide
		APlayer.PlayOneShot (Audio3);
		ContinueButt.SetActive (false);
		Text2.SetActive (false);
		Text3.SetActive (true);
        StudentImage.SetActive(false);
        TeacherImage.SetActive(true);
		yield return new WaitUntil (() => !APlayer.isPlaying);
		ContinueButt.SetActive (true);
		yield return new WaitUntil (() => ButtonClick);
		ButtonClick = false;

		//Show 4th Slide
		APlayer.PlayOneShot (Audio4);
		ContinueButt.SetActive (false);
        TeacherImage.SetActive(false);
        SilenceImage.SetActive(true);

		Text3.SetActive (false);
		Text4.SetActive (true);
		yield return new WaitUntil (() => !APlayer.isPlaying);
		ContinueButt.SetActive (true);
		yield return new WaitUntil (() => ButtonClick);
		ButtonClick = false;

		print ("end");
		EndScene ();
	

	}

	void EndScene(){
		UnityEngine.SceneManagement.SceneManager.LoadScene("InBetweenScene");

	
	}

	void TaskOnClick(){
		ButtonClick = true;

	}
}
