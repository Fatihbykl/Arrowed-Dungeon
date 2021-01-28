using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    public GameObject target;
    GameObject crossbow;
    // Start is called before the first frame update
    void Start()
    {
        crossbow = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        crossbow.transform.rotation = Quaternion.LookRotation(target.transform.position - crossbow.transform.position, crossbow.transform.up);
    }
}
