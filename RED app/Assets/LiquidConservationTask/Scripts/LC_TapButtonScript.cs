using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LC_TapButtonScript : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {
	public GameObject SceneManagerGO;
	public GameObject LeftGlass;
	public bool EndPour;
	public bool AllowFill;
	public AudioSource TapAudio;
	public GameObject Water;
	public Image TapImage;

	// Use this for initialization
	void Start () {
		Water = gameObject.transform.Find ("Water").gameObject;
		Water.SetActive (false);
		EndPour = false;
		TapImage = gameObject.GetComponent<Image>();

		TapAudio = gameObject.GetComponent<AudioSource> ();
		SceneManagerGO = GameObject.Find ("SceneManagerGO");
		//LeftGlass = SceneManagerGO.GetComponent<LC_SceneManagerScript> ().LeftGlass;

		//Button btn = gameObject.GetComponent<Button> ();
		//btn.onClick.AddListener(TaskOnClick);

	}

	// Update is called once per frame
	public void OnPointerDown (PointerEventData eventData){
		if (AllowFill) 
		{
			Water.SetActive (true);
			TapAudio.Play ();
			SceneManagerGO.GetComponent<LC_SceneManagerScript> ().TimeOnFirstClick = Time.time;
			StartCoroutine (LeftGlass.GetComponent<GlassScript> ().Pour (90000000));
			//TapImage.color = Color.blue;

		}
	}

	public void OnPointerUp (PointerEventData eventData){
		if (AllowFill) 
		{
			Water.SetActive (false);
			TapAudio.Stop();
			SceneManagerGO.GetComponent<LC_SceneManagerScript> ().TimeOnMouseUp = Time.time;
			LeftGlass.GetComponent<GlassScript> ().StopPour ();
			EndPour = true;
			//TapImage.color = Color.black;

		}

	}
    

}
