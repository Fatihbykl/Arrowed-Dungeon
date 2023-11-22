using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    private GameObject target;
    private GameObject crossbow;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        crossbow = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //crossbow.transform.rotation = Quaternion.LookRotation(new Vector3(0, target.transform.position.y - crossbow.transform.position.y, 0), crossbow.transform.up);
        crossbow.transform.LookAt(new Vector3(target.transform.position.x, crossbow.transform.position.y, target.transform.position.z));
    }
}
