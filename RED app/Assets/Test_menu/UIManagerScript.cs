using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour {

	public void Start_cancellation() {
		SceneManager.LoadScene("CancellationStart");
	}
	public void Start_Digit_Span() {
		SceneManager.LoadScene ("Digit_Span");
	}
	public void Start_Dot_Matrix() {
		SceneManager.LoadScene ("Dot_Matrix");
	}
	public void Start_Cattell() {
		SceneManager.LoadScene ("cattell_start");
	}
	public void Start_Phono() {
		SceneManager.LoadScene ("PhonoAwareness");
	}
	public void Start_Q1() {
		SceneManager.LoadScene ("Q1.Opening.Scene");
	}
	public void Start_Q2() {
		SceneManager.LoadScene ("Q2.Opening.Scene");
	}
	public void Start_Num() {
		SceneManager.LoadScene ("ANS_StartScreen");
	}

	public void Go2Meny() {
		SceneManager.LoadScene ("test_menu");
	}
}