using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public class PlaceWorldObjectToScreen : MonoBehaviour
    {
        public GameObject worldObject;
        public GameObject targetScreenPosition;
        public Canvas canvas;

        private void Start()
        {
            PlaceObject();
        }
        
        private async void PlaceObject()
        {
            await UniTask.WaitForSeconds(0.05f);
            
            var cam = Camera.main;
            var objPos = worldObject.transform.position;
            
            var position = cam.WorldToScreenPoint(targetScreenPosition.transform.position);
            position.z = (canvas.transform.position - cam.transform.position).magnitude;
            
            var worldPos = cam.ScreenToWorldPoint(position);
            worldPos.y = objPos.y;
            worldPos.z = objPos.z;
            
            worldObject.transform.position = worldPos;
        }
    }
}
