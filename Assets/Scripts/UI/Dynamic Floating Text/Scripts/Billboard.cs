using UnityEngine;

namespace UI.Dynamic_Floating_Text.Scripts
{
    public class Billboard : MonoBehaviour
    {

        // Standard Billboard script which makes canvas objects always look
        // at the camera
    
        void LateUpdate()
        {
            transform.LookAt(transform.position + DynamicTextManager.mainCamera.forward);
        }
    }
}
