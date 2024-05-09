using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedJoystick : Joystick
{
    public GameObject indicator;
    private bool _skillPressed = false;
    private GameObject _indicator;
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        _skillPressed = true;
        Debug.Log("Pointer down.");
    }

    protected override void Start()
    {
        base.Start();

        _indicator = Instantiate(indicator, GameManager.instance.playerObject.transform);
        _indicator.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_skillPressed)
        {
            _indicator.SetActive(true);
            //_indicator.transform.LookAt(Direction);
            _indicator.transform.rotation = Quaternion.FromToRotation(Vector3.back, new Vector3(Horizontal, 0f , Vertical));
            Debug.Log(Direction);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        _indicator.SetActive(false);
        _skillPressed = false;
        Debug.Log("Pointer down.");
    }
}