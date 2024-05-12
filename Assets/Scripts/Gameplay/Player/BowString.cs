using UnityEngine;

namespace Gameplay.Player
{
    [RequireComponent(typeof(LineRenderer))]
    public class BowString : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private Transform stringFollowPos;
        [SerializeField] private Transform stringStartPos;
        [SerializeField] private Transform stringEndPos;
        [SerializeField] private Transform stringCenterPos;
        [SerializeField] private ParticleSystem arrow;
        private LineRenderer _lineRenderer;
        private bool _isHolding;

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            player.HoldBowString += OnHoldString;
            player.ReleaseBowString += OnReleaseString;
        }

        private void Update()
        {
            if (stringFollowPos == null) { return; }
        
            _lineRenderer.SetPosition(0, stringStartPos.position);
            _lineRenderer.SetPosition(1, _isHolding ? stringFollowPos.position : stringCenterPos.position);
            _lineRenderer.SetPosition(2, stringEndPos.position);

            arrow.transform.position = _lineRenderer.GetPosition(1);
        }

        private void OnHoldString()
        {
            _isHolding = true;
            arrow.gameObject.SetActive(true);
        }

        private void OnReleaseString()
        {
            _isHolding = false;
            arrow.gameObject.SetActive(false);
        }
    }
}
