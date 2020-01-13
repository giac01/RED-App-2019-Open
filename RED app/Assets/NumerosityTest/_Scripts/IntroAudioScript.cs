using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAudioScript : MonoBehaviour {
    public AudioClip[] GameAudio;
    public List<string> GameAudioNames;

	// Use this for initialization
	void Awake () {
        //print("test");
        GameAudio = Resources.LoadAll<AudioClip>("Audio") as AudioClip[] ;
        for (int i=0; i<GameAudio.Length; i++)
        {
            GameAudioNames.Add(GameAudio[i].name);
        }
        //print(GameAudioNames.IndexOf("taskintro"));
        //gameObject.GetComponent<AudioSource>().PlayOneShot(GameAudio[GameAudioNames.IndexOf("taskintro")]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
