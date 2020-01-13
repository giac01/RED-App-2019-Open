using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QQNoTechButton : MonoBehaviour {
	public PictureQuestionControlScript ControlScript;
	public string buttonText;
	public Button btn;
	public QQPictureScript[] OtherSelection;


	// Use this for initialization
	void Start () {
		
		ControlScript = gameObject.transform.parent.gameObject.GetComponent<PictureQuestionControlScript>();
		btn = gameObject.GetComponent<Button> ();
		btn.onClick.AddListener (ButtonClick);

		buttonText = gameObject.transform.Find ("Label").gameObject.GetComponent<Text> ().text;
		//print (buttonText);

		OtherSelection = gameObject.transform.parent.gameObject.GetComponentsInChildren<QQPictureScript> ();

		
	}


	void ButtonClick(){





		if (ControlScript.ItemSelected[ControlScript.ItemNames.IndexOf (buttonText)] == (int)0) {

			//Sets all other items to unselected
			foreach (QQPictureScript i in OtherSelection) { 
				i.glowImage.SetActive(true);
				i.OnClickPic ();
			}

			ControlScript.ItemSelected[ControlScript.ItemNames.IndexOf(buttonText)]=(int)1;
			gameObject.GetComponent<Image> ().color = Color.yellow;
		} 
		else if  (ControlScript.ItemSelected[ControlScript.ItemNames.IndexOf (buttonText)] == (int)1) {
			ControlScript.ItemSelected[ControlScript.ItemNames.IndexOf(buttonText)]=(int)0;
			gameObject.GetComponent<Image> ().color = Color.white;

		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
