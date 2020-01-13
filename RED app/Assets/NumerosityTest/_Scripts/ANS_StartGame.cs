using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ANS_StartGame : MonoBehaviour {
    public GameObject startbutton;
    public GameObject introtext;
    public GameObject IntroAudio;


    void Awake()
    {
        startbutton = GameObject.Find("PlayNowButton");
        introtext = GameObject.Find("IntroText");
        IntroAudio = GameObject.Find("IntroAudio");

        //Below is now set to "Play On Awake" in the audiosource ! 
        //IntroAudio.GetComponent<AudioSource>().PlayOneShot(IntroAudio.GetComponent<IntroAudioScript>().GameAudio[IntroAudio.GetComponent<IntroAudioScript>().GameAudioNames.IndexOf("taskintro")]);

    }


    // Use this for initialization
    public void OnMouseDown()
    {
        startbutton.SetActive(false);
        introtext.SetActive(false);
        SceneManager.LoadScene("ANS_PracticeRounds");

    }
}
