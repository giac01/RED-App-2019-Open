using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QQButtonIHaveNoAllergies : MonoBehaviour {

	//public Button yourButton;
	[Header("GameObjects")]
	public GameObject AttachedObject;
	public Button btn ;
	public GameObject SliderClone; 
	public QQPictureScript[] OtherAllergies;

	[Header("Game Logic")]
	public bool NoAllergiesBool;
	public int ItemIndex;
	public string ButtonText; 



	void Awake()
	{
		AttachedObject = gameObject;
		btn = AttachedObject.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		ButtonText = gameObject.transform.Find ("Label").gameObject.GetComponent<Text> ().text;
		SliderClone = gameObject.transform.parent.gameObject;

		ItemIndex = SliderClone.GetComponent<PictureQuestionControlScript> ().ItemNames.IndexOf (ButtonText);

		OtherAllergies = SliderClone.GetComponentsInChildren<QQPictureScript> ();



		//print (ButtonText);
	}

	public void TaskOnClick(){
		//print ("I have no allergies");
		NoAllergiesBool = !NoAllergiesBool;

		if (NoAllergiesBool) {
			Selected ();
		}
		if (!NoAllergiesBool){
			NotSelected ();
		}
	}

	public void Selected(){
		//gameObject.GetComponent<QQPictureScript> ().IsSelected = true;
		//print(SliderClone.GetComponent<PictureQuestionControlScript>().ItemNames.IndexOf(ButtonText).ToString());
		SliderClone.GetComponent<PictureQuestionControlScript>().ItemSelected[ItemIndex] =1;
		//Color RedCol = Color.red;
		AttachedObject.GetComponent<Image>().color = Color.yellow;
		if (ButtonText=="I have no allergies"){
			foreach (QQPictureScript i in OtherAllergies) {
				i.glowImage.SetActive(true);
				i.OnClickPic ();
			}
			SliderClone.transform.Find ("OtherAllergy").GetComponent<QQButtonIHaveNoAllergies> ().NoAllergiesBool = true;
			SliderClone.transform.Find ("OtherAllergy").GetComponent<QQButtonIHaveNoAllergies> ().TaskOnClick ();
		}
		if (ButtonText=="I also have an allergy not listed here"){
			SliderClone.transform.Find ("NoAllergies").GetComponent<QQButtonIHaveNoAllergies> ().NoAllergiesBool = true;
			SliderClone.transform.Find ("NoAllergies").GetComponent<QQButtonIHaveNoAllergies> ().TaskOnClick ();		}

	}

	public void NotSelected(){
		SliderClone.GetComponent<PictureQuestionControlScript> ().ItemSelected [ItemIndex] = 0;
		AttachedObject.GetComponent<Image>().color = Color.white;

	}



}
