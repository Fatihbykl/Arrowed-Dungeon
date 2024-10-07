using System;
using UnityEngine;

namespace UI
{
    public class HintCollider : MonoBehaviour
    {
        public string hintId;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            HintManager.Instance.OpenHint(hintId);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
