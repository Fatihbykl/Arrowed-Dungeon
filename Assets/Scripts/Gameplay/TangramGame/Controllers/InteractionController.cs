using TangramGame.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.TangramGame.Controllers
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private Camera cam;

        private IInteractable current;

        private bool shouldDrag;
        private Vector3 touch;

        private void Update()
        {
            transform.position = cam.transform.position;
            var screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2f);
            
            if (shouldDrag)
            {
                var worldPos = cam.ScreenToWorldPoint(screenPos);
                current.OnDrag(worldPos);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (current != null)
                {
                    current.OnDrop(current.Transform.position);
                    current = null;
                    shouldDrag = false;
                }
                
                var worldPos = cam.ScreenToWorldPoint(screenPos);
                var ray = cam.ScreenPointToRay(screenPos);
                var hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);
                RaycastHit2D hit = new RaycastHit2D();
                
                foreach (var h in hits)
                {
                    if (hit.collider == null) hit = h;
                    else if (h.collider.transform.position.z > hit.collider.transform.position.z) hit = h;
                }
                
                if (hit.collider != null && hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    current = interactable;
                    current.OnGrab(worldPos);
                    shouldDrag = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && current != null)
            {
                var worldPos = cam.ScreenToWorldPoint(screenPos);
                current.OnDrop(worldPos);
                current = null;
                shouldDrag = false;
            }
        }
    }
}