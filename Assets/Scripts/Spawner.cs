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
            obj.GetComponent<Rigidbody>().AddForce(target.transform.position * Time.fixedDeltaTime * ballSpeed, ForceMode.Impulse);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    void ChooseRandomly()
    {
        prefab = prefabs[Random.Range(0, prefabs.Length)];
        ballSpeed = prefab.GetComponent<Ball>().speed;
    }
}
