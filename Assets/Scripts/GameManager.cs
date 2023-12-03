using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject keysObject;
    public GameObject gate;
    public int currentLevelCoin;
    public int currentLevelBrokenArrows;
    public int collectedKeyCount = 0;
    public int totalKeyCount = 0;

    private void OnEnable()
    {
        GameplayEvents.ArrowDead += onArrowDead;
    }

    private void OnDisable()
    {
        GameplayEvents.ArrowDead += onArrowDead;
    }
    private void Start()
    {
        totalKeyCount = keysObject.transform.childCount;
    }

    public void CollectedKey()
    {
        collectedKeyCount++;
        if (collectedKeyCount >= totalKeyCount)
        {
            gate.GetComponent<Animator>().Play("Open");
        }
        GameplayEvents.KeyCollected.Invoke(collectedKeyCount, totalKeyCount);
    }

    public void onArrowDead(string arrowType, int coinReward)
    {
        currentLevelCoin += coinReward;
        currentLevelBrokenArrows++;
    }
}
