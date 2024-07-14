using Gameplay.TangramGame.GridSystem;
using TangramGame.Scripts.GridSystem;
using UnityEngine;

namespace Gameplay.TangramGame.GridObjects
{
    public class TileObject : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Color normal, valid, invalid;

        public Tile Tile { get; private set; }

        public void Setup(Tile t, float scale)
        {
            this.Tile = t;
            Tile.OnContentChanged += OnContentChanged;
            transform.localScale = Vector3.one * scale;
        }

        private void OnDestroy()
        {
            if (Tile != null) Tile.OnContentChanged -= OnContentChanged;
        }

        private void OnContentChanged(TileContent newContent)
        {
            
        }

        public void SetPreShow(bool isOn, bool isValid)
        {
            if (Tile.CurrentContent != null) return;
            meshRenderer.material.color = !isOn ? normal : isValid ? valid : invalid;
        }
    }
}