using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public class PlaceWorldObjectToScreen : MonoBehaviour
    {
        public GameObject worldObject;
        public GameObject targetScreenPosition;

        private void Start()
        {
            PlaceObject();
        }
        
        private async void PlaceObject()
        {
            await UniTask.WaitForSeconds(0.05f);
            
            var cam = Camera.main;
            var objPos = worldObject.transform.position;
            
            var worldPos = cam.ScreenToWorldPoint(targetScreenPosition.transform.position);
            worldPos.y = objPos.y;
            worldPos.z = objPos.z;
            
            worldObject.transform.position = worldPos;
        }
    }
}
