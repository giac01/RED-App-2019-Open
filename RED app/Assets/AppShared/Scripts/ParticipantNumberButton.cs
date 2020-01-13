using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticipantNumberButton : MonoBehaviour {

	// SERIALISED FIELDS
	[SerializeField] private string playerPrefsVar;
	[SerializeField] private TextMesh buttonText;
	[SerializeField] private InputField buttonInput;
	[SerializeField] private GameObject startButton;
	[SerializeField] private AppStartPromptSave participantInput;

	// CONSTANTS
	private string participantNr;

	// Use this for initialization
	void Start () {
		// Find the current participant number/name.
		if (PlayerPrefs.HasKey (playerPrefsVar)) {
			participantNr = PlayerPrefs.GetString (playerPrefsVar);
		}
		else {
			participantNr = "000000";
		}
		// Set the participant number on the button.
		buttonText.text = participantNr;
		// Make the input field call the SubmitName function when its content is changed.
		buttonInput.onValueChanged.AddListener(SetButton);
	}

	// Update is called once per frame
	void Update () {
	}

	void OnMouseDown () {
		// Set the current text to the main input field.
		participantInput.SetNameInInputField(participantNr);
		// Set the name in the PlayerPrefs.
		participantInput.SetNameInPlayerPrefs(participantNr);
		// Activate the start button.
		startButton.SetActive(true);
	}

	private void SetButton(string name) {
		// Get the button's current text.
		participantNr = name;
		// Update the button's text.
		buttonText.text = participantNr;
		// Set the participant number.
		PlayerPrefs.SetString(playerPrefsVar, participantNr);
	}
}
