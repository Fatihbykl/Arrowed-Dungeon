using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject target;
    public float spawnTime = 1f;
    float ballSpeed;
    GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            ChooseRandomly();
            var obj = Instantiate(prefab, this.transform.position, this.transform.rotation) as GameObject;
            var rb = obj.GetComponent<Rigidbody>();
            Vector3 v = target.transform.position - obj.transform.position;
            rb.AddForce(v * Time.fixedDeltaTime * ballSpeed, ForceMode.Impulse);
            obj.transform.rotation = LookAtTarget(target.transform.position - obj.transform.position);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    Quaternion LookAtTarget(Vector3 v)
    {
        return Quaternion.Euler(0, Mathf.Atan2(v.z, v.x) * -Mathf.Rad2Deg + 180, 90);
    }

    void ChooseRandomly()
    {
        prefab = prefabs[Random.Range(0, prefabs.Length)];
        ballSpeed = prefab.GetComponent<Ball>().speed;
    }
}
