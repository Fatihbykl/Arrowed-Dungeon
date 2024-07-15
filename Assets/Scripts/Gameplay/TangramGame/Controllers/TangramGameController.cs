using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Gameplay.InventorySystem;
using Gameplay.Managers;
using Gameplay.TangramGame.GridObjects;
using Gameplay.TangramGame.GridSystem;
using TangramGame.Scripts;
using TangramGame.Scripts.GridSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.TangramGame.Controllers
{
    [Serializable]
    public class TileContentInput
    {
        public string id;
        public Vector2Int key;
        public Vector2Int[] pieces;
    }
    public class TangramGameController : MonoBehaviour
    {
        [SerializeField] private GridController grid;
        [SerializeField] private float cellScale;
        [SerializeField] private int width, height;
        [SerializeField] private TileContentObject contentPrefab;
        [SerializeField] private Transform parent;
        [SerializeField] private TileContentInput[] tileContents;

        private List<TileContentObject> placedContents = new List<TileContentObject>();
        private List<TileContentObject> spawnedContents = new List<TileContentObject>();

        private TileContent currentContent;
        private Vector2 lastPos;

        public Dictionary<Vector2Int, TileContent> CreateContents()
        {
            var contents = new Dictionary<Vector2Int, TileContent>();
            foreach (var content in tileContents)
            {
                contents.Add(content.key, new TileContent(content.id, content.pieces.ToHashSet(), new Color(132 / 255f,148 / 255f,188 / 255f)));
            }

            return contents;
        }

        public void Start()
        {
            ClearGame();
            
            var w = width;
            var h = height;
            grid.CreateGrid(w, h, cellScale);
            
            var patterns = CreateContents();
            var placementOffset = Mathf.Max(w, h);

            float delay = 0f;
        
            foreach (var pattern in patterns)
            {
                var circle = Random.insideUnitCircle.normalized * cellScale;
                var oval = new Vector2(circle.x / 1.8f, circle.y);
                var pos = oval * placementOffset / 2f;

                //var pos = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                
                var obj = Instantiate(contentPrefab.gameObject, pos, Quaternion.identity, parent);
                var controller = obj.GetComponent<TileContentObject>();

                obj.transform.localPosition = pos;
                
                controller.Setup(pattern.Value, cellScale, OnContentPicked, OnContentDropped, OnContentDragged);
                controller.transform.localScale = Vector3.zero;
                controller.transform.DOScale(cellScale, 0.5f);
                controller.gameObject.SetActive(false);
                spawnedContents.Add(controller);
            }
        }

        public void ClearGame()
        {
            grid.ClearGrid();
            foreach (var content in spawnedContents)
                Destroy(content.gameObject);
            
            spawnedContents.Clear();
            placedContents.Clear();
        }

        public void ShowFoundPieces()
        {
            foreach (var spawnedContent in spawnedContents)
            {
                if (Inventory.Instance.HasId(spawnedContent.Content.id))
                {
                    Debug.Log("test");
                    spawnedContent.gameObject.SetActive(true);
                }
            }
        }

        private void CheckForWin()
        {
            if (grid.IsAllFilled()) EndRound();
        }

        private void EndRound()
        {
            GameManager.Instance.PuzzleCompleted();
        }

        private void OnContentPicked(TileContentObject obj)
        {
            currentContent = obj.Content;
            if (placedContents.Contains(obj))
            {
                placedContents.Remove(obj);
                grid.RemovePiece(obj.transform.position);
            }
        }

        private void OnContentDropped(TileContentObject obj)
        {
            currentContent = null;
            
            grid.ClearLastPreShows();
            if (!grid.IsValid(obj.Content, lastPos))
            {
                //obj.ResetPos();
                return;
            }
            grid.PlacePiece(obj.Content, lastPos);
            var actualPos = grid.GridToWorldPos(grid.WorldToGridPos(lastPos));
            obj.transform.position = actualPos;
            obj.initPos = actualPos;
            obj.SetOrder(0);
            placedContents.Add(obj);

            CheckForWin();
        }

        private void OnContentDragged(TileContentObject obj, Vector2 pos)
        {
            lastPos = pos;
            
            grid.PreShowTile(obj.Content, pos);
        }
    }
}