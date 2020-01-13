using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QQPictureScript : MonoBehaviour {

	[Header("GameObjects")]
	public AudioClip ObjectAudio; //assigned in gui
	public GameObject CanvasObject;
	public PictureQuestionControlScript DataScript;
	public GameObject regularImage;
	public GameObject glowImage;
	//public GameObject colliderObject;
	public Button btn;
	public QQSceneManagerClass SceneManagerScript;
	public AudioSource ParentAudioSource;
	public AudioSource QuestionAudioSource;

	[Header("Jiggle Parameters")]
	public float MaxRotation = 10;
	public float SpeedRotation = 1;

	[Header("Data")]
	public bool IsSelected;
	public string ItemName;


	// Use this for initialization
	void Start () {
		QuestionAudioSource = GameObject.Find ("Canvas").transform.Find ("VolumeIcon").GetComponent<AudioSource> ();
		ParentAudioSource = gameObject.transform.parent.gameObject.GetComponent<AudioSource> ();
		SceneManagerScript = GameObject.Find ("Q3.SceneController").GetComponent<QQSceneManagerClass> ();
		CanvasObject = gameObject.transform.parent.gameObject;
		DataScript = CanvasObject.GetComponent<PictureQuestionControlScript> ();
		Button btn = gameObject.transform.Find ("Button").gameObject.GetComponent<Button>();
		glowImage = gameObject.transform.Find ("glow").gameObject;
		regularImage = gameObject.transform.Find ("regularImage").gameObject;
		//colliderObject = gameObject.transform.Find ("colliderObject").gameObject;
		ItemName = gameObject.transform.Find("Label").GetComponent<Text>().text;

		btn.onClick.AddListener(OnClickPic);
		glowImage.SetActive (false);

		IsSelected = false;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickPic() {
		//print ("run0");


		if (glowImage.activeSelf==true)
		{
			//print ("run1");
			glowImage.SetActive (false);
			gameObject.transform.Find ("Label").GetComponent<Text> ().fontStyle = FontStyle.Normal;
			IsSelected = false;
			DataScript.ItemSelected [DataScript.ItemNames.IndexOf (ItemName)] = 0;
		}
		else
		{
			//print ("run2");
			ParentAudioSource.Stop();
			ParentAudioSource.PlayOneShot(ObjectAudio);
			if (QuestionAudioSource.isPlaying) {
				ParentAudioSource.volume = .2f;
			} else {
				ParentAudioSource.volume = 1f;
			}


			glowImage.SetActive (true);
			gameObject.transform.Find ("Label").GetComponent<Text> ().fontStyle = FontStyle.Bold;
			IsSelected = true;
			DataScript.ItemSelected [DataScript.ItemNames.IndexOf (ItemName)] = 1;
			if (SceneManagerScript.trialnum == 13) {
				CanvasObject.transform.Find ("NoAllergies").GetComponent<QQButtonIHaveNoAllergies> ().NoAllergiesBool = true;
				CanvasObject.transform.Find ("NoAllergies").GetComponent<QQButtonIHaveNoAllergies> ().TaskOnClick ();
			}

		}


	}


	public void jiggle() {


	}
}
