using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumsShowButtonAfterIntro : MonoBehaviour {

	[SerializeField] GameObject startButton;
	[SerializeField] float explainTime;

	// Use this for initialization
	void Start () {

		// De-activate the start button.
		startButton.SetActive(false);

		// Re-activate the butten after the explanation.
		StartCoroutine(WaitThenActivate(explainTime));
	}

	// Update is called once per frame
	void Update () {

	}

	public IEnumerator WaitThenActivate(float delaySeconds)
	{
		// Wait for the requested amount of seconds.
		yield return new WaitForSeconds(delaySeconds);

		// Activate the start button.
		startButton.SetActive(true);
	}
}
