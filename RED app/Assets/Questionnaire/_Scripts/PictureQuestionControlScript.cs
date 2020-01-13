using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PictureQuestionControlScript : MonoBehaviour {

	public List<string> ItemNames; //Names of items that can be selected

	public int[] ItemSelected;

	public Component[] components; //Each text component within the picture responses 
	// Use this for initialization

	public QQSceneManagerClass SceneControllerScript;

	public GameObject NextQuestionButton; 

	void Awake () {

		SceneControllerScript = GameObject.Find ("Q3.SceneController").GetComponent<QQSceneManagerClass> ();

		SceneControllerScript.clickvirgin = false;

		NextQuestionButton = GameObject.Find ("Canvas").transform.Find ("Button").gameObject;

		components = GetComponentsInChildren<Text> ();
		for (int i = 0; i < components.Length; i++) {
//			print (components [i].name);
			if (components [i].name == "Label") {
				//ItemNames.Add(components[i].Text)
				ItemNames.Add(components[i].GetComponent<Text>().text);
			}
		
		}
		//ItemNames.Add ("No Item Selected");

		ItemSelected = Enumerable.Repeat (0, ItemNames.Count).ToArray();

	}
	
	// Update is called once per frame
	void Update () {

		if (ItemSelected.Sum() > 0) {
			//print ("A");
			SceneControllerScript.clickvirgin = false;
			NextQuestionButton.GetComponent<Image> ().color = Color.green;

		}
		if (ItemSelected.Sum() == 0) {
			//print ("B");
			SceneControllerScript.clickvirgin = true;
			SceneControllerScript.nextpress = false;
			NextQuestionButton.GetComponent<Image> ().color = Color.red;

		}


		
	}
}
