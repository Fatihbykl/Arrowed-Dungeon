using Cinemachine;
using TangramGame.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.TangramGame.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera cam;

        public void MoveToGrid(GridController controller)
        {
            cam.transform.position = controller.GridWorldPos + new Vector3(-0.5f, -0.5f, -10);
            cam.m_Lens.OrthographicSize = controller.Grid.width + 2;
        }
    }
}