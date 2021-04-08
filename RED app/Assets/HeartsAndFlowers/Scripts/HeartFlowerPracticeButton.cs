using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartFlowerPracticeButton : MonoBehaviour
{

    [SerializeField] HeartFlowerPracticeSceneController controller;

    private bool _active = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (_active)
        {
            controller.OnResponse(this.name);
        }
    }

    public void ActivationStation(bool activate)
    {
        _active = activate;
    }
}
