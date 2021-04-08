using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARTOL_Square : MonoBehaviour
{

	private bool blink;

	void Start()
	{
		//StartCoroutine("BlinkGameObject");

	}

 

    IEnumerator BlinkGameObject(float time)  
    {
    	blink = true;
        //GameObject.Find("Square").SetActive(true);

        while (blink)
        {
            gameObject.GetComponent<Image>().enabled = true;
            gameObject.GetComponent<Image>().color = Color.black;
            //gameObject.SetActive(true); - this doesn't work :'(
            yield return new WaitForSecondsRealtime(1.5f);
            gameObject.GetComponent<Image>().color= Color.white;
            //gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(1.5f/3f);
            
       	}
       	gameObject.GetComponent<Image>().color= Color.black;

        yield return null; 

    }

    public void StopBlink(){
    	blink = false;
        gameObject.GetComponent<Image>().enabled = false;

    }
    
}
