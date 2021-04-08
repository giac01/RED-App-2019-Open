using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using android;

public class AppVersionText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    	string appVersion = Application.version;
    	gameObject.GetComponent<Text>().text = "App Version: " + appVersion + "\n Saved Data Location:  " + Application.persistentDataPath;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
