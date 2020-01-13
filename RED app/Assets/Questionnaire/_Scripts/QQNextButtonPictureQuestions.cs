using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

public class QQNextButtonPictureQuestions : MonoBehaviour {
	public GameObject CanvasObject;
	public PictureQuestionControlScript DataScript;


	public string messageclicked;
	public string messagenoclick;

	// Use this for initialization
	void Start () {
		CanvasObject = gameObject.transform.parent.gameObject;
		DataScript = CanvasObject.GetComponent<PictureQuestionControlScript> ();
	}
	
	// Update is called once per frame
	void UpdateIgnore () { //ignore function
		if (DataScript.ItemSelected.Sum()>0){
			gameObject.transform.Find ("Text").GetComponent<Text> ().text = messageclicked;
		}

		if (DataScript.ItemSelected.Sum()==0){
			gameObject.transform.Find ("Text").GetComponent<Text> ().text = messagenoclick;
		}
		
	}




}
