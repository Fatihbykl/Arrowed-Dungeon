using UnityEngine;

namespace Gameplay.TangramGame.GridObjects
{
    public class TileContentElement : MonoBehaviour
    {
        
        [SerializeField] private MeshRenderer meshRenderer;

        private int originalOrder;

        private void Awake()
        {
            originalOrder = meshRenderer.sortingOrder;
        }

        public void Setup(Color color)
        {
            //sprite.color = color;
            meshRenderer.material.color = color;
        }

        public void SetOrder(int order) => meshRenderer.sortingOrder = order;
    }
}