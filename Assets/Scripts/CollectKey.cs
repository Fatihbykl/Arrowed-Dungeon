using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectKey : MonoBehaviour
{
    public GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //animasyon, anahtar alındı
            gameManager.CollectedKey();
            Destroy(this.gameObject);
        }
    }
}
