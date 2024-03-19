using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class test2 : MonoBehaviour
{
    public Transform endPosition;
    public float t;
    private float x;
    public float y;
    private float Vyi;
    private float Vxi;

    public float g;
    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        
        Vector3 direction = endPosition.transform.position - transform.position;
        Vector3 directionXZ = direction.normalized; 
        directionXZ.y = 0;
        
        //Vx = x / t
        float Vxz = direction.magnitude / t;
        
        //Vy0 = y/t + 1/2 * g * t
        float Vy = direction.y / t + 0.5f * Mathf.Abs(Physics.gravity.y) * t;

        Vector3 result = directionXZ * Vxz;
        result.y = Vy;
        rb.velocity = result;
        //return result;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
