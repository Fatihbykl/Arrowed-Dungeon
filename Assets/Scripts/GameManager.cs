using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int remainingKeyCount;
    public GameObject keysObject;
    public GameObject gate;

    private void Start()
    {
        remainingKeyCount = keysObject.transform.childCount;
    }

    public void CollectedKey()
    {
        remainingKeyCount--;
        if (remainingKeyCount <= 0)
        {
            gate.GetComponent<Animator>().Play("Open");
        }
    }
}
