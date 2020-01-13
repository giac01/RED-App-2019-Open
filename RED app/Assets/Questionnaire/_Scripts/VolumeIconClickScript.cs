using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeIconClickScript : MonoBehaviour {
    public GameObject SceneManager;
    private Sprite VolumeIcon;
    private Sprite VolumeIconGlow;

    
	// Use this for initialization
	void Awake () {
		SceneManager = GameObject.FindWithTag("GameController");
        VolumeIcon = Resources.Load<Sprite>("Sprites/volume");
        VolumeIconGlow = Resources.Load<Sprite>("Sprites/volume_glow");

        //print(VolumeIconGlow); print(VolumeIconGlow);

    }

    // Update is called once per frame
    void Update () {
        //gameObject.GetComponent<SpriteRenderer>().sprite = VolumeIcon;



    }

    public void OnMouseDown()
    {

		PlayQuestionAudio ();
        //print("OnMouseDown in VOLUMECLUCKSCRIPT RUN");
        //GameObject.Find("Slider_Clone").transform.Find("Slider").GetComponent<QQ_SliderScript>().PlayQuestionAudio();
        StartCoroutine("GlowVolumeIcon");

    }

    IEnumerator GlowVolumeIcon()
    {
        //print("GLOWVOLUMEICON() RUN");
        //print(VolumeIconGlow);
        gameObject.GetComponent<SpriteRenderer>().sprite = VolumeIconGlow;
        yield return new WaitUntil(() => !gameObject.GetComponent<AudioSource>().isPlaying);
        gameObject.GetComponent<SpriteRenderer>().sprite = VolumeIcon;
        yield return null;

    }


			public void PlayQuestionAudio() //Depreciated
			{
				if (gameObject.GetComponent<AudioSource>().isPlaying == false)
				{
			//print("PlayQuestionAudio ran22");

					gameObject.GetComponent<AudioSource>().PlayOneShot(SceneManager.GetComponent<QQSceneManagerClass>().GameAudio[SceneManager.GetComponent<QQSceneManagerClass>().AudioNames.IndexOf(string.Concat(SceneManager.GetComponent<QQSceneManagerClass>().AudioTag, (SceneManager.GetComponent<QQSceneManagerClass>().trialnum+ 1).ToString()))]);
				}
			}

}
