using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    
    private float spawnTime;
    private GameObject target;
    private float arrowSpeed;
    private Color particleColor;
    
    // Start is called before the first frame update
    void Start()
    {
        var arrow = prefab.GetComponent<Arrow>();
        var crossbowRenderer = GetComponentInParent<Renderer>();

        target = GameObject.FindGameObjectWithTag("Player");
        arrowSpeed = arrow.arrowType.baseSpeed;
        spawnTime = arrow.arrowType.spawnSeconds;
        particleColor = arrow.arrowType.arrowMetalSideColor;

        // Set crossbow colors
        crossbowRenderer.materials[0].color = arrow.arrowType.arrowTailColor;
        crossbowRenderer.materials[1].color = arrow.arrowType.arrowMetalSideColor;

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            var obj = Instantiate(prefab, this.transform.position, this.transform.rotation) as GameObject;
            var ps = obj.GetComponentInChildren<ParticleSystem>();
            ParticleSystem.MainModule ma = ps.main;
            ma.startColor = particleColor;

            Vector3 v = (obj.transform.position - this.gameObject.transform.parent.position).normalized;
            obj.GetComponent<Rigidbody>().AddForce(v * Time.fixedDeltaTime * arrowSpeed, ForceMode.Impulse);
            obj.transform.rotation = LookAtTarget(target.transform.position - obj.transform.position);

            yield return new WaitForSeconds(spawnTime);
        }
    }

    Quaternion LookAtTarget(Vector3 v)
    {
        return Quaternion.Euler(0, Mathf.Atan2(v.z, v.x) * -Mathf.Rad2Deg + 90, 0);
    }

}
