using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BowString : MonoBehaviour
{
    [SerializeField] private Transform stringFollowPos;
    [SerializeField] private Transform stringStartPos;
    [SerializeField] private Transform stringEndPos;
    [SerializeField] private Transform stringCenterPos;
    [SerializeField] private GameObject arrow;
    private LineRenderer lineRenderer;
    private bool isHolding = false;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        GameplayEvents.HoldBowString += OnHoldString;
        GameplayEvents.ReleaseBowString += OnReleaseString;
    }

    private void Update()
    {
        if (stringFollowPos == null) { return; }
        
        lineRenderer.SetPosition(0, stringStartPos.position);
        lineRenderer.SetPosition(1, isHolding ? stringFollowPos.position : stringCenterPos.position);
        lineRenderer.SetPosition(2, stringEndPos.position);

        arrow.transform.position = lineRenderer.GetPosition(1);
    }

    private void OnHoldString()
    {
        isHolding = true;
        arrow.gameObject.SetActive(true);
    }

    private void OnReleaseString()
    {
        isHolding = false;
        arrow.gameObject.SetActive(false);
    }
}
