using System;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.TangramGame.GridSystem;
using TangramGame.Scripts;
using TangramGame.Scripts.GridSystem;
using UnityEngine;

namespace Gameplay.TangramGame.GridObjects
{
    public class TileContentObject : MonoBehaviour, IInteractable
    {
        private static int LastOrder = -10;
        
        [SerializeField] private TileContentElement originElement;
        [SerializeField] private TileContentElement elementPrefab;
        
        private Action<TileContentObject> OnContentPicked;
        private Action<TileContentObject> OnContentDropped;
        private Action<TileContentObject, Vector2> OnContentDragged;

        private List<TileContentElement> elements = new List<TileContentElement>();
        private Camera cam;
        private Vector3 pickedUpOffset;
        private float cellScale;
        
        public Vector2 initPos;

        private int scaleAnimId = Int32.MaxValue;
        
        public TileContent Content { get; private set; }

        public void Setup(TileContent tp, float cellScale,
            Action<TileContentObject> onPicked = null, 
            Action<TileContentObject> onDropped = null, 
            Action<TileContentObject, Vector2> onDragged = null)
        {
            Content = tp;
            cam = Camera.main;
            this.cellScale = cellScale;
            
            LastOrder++;

            originElement.Setup(tp.color);
            SetZ(LastOrder);
            originElement.SetOrder(LastOrder);
            elements.Add(originElement);

            foreach (var offsetPiece in tp.OffsetPieces)
            {
                var element = Instantiate(elementPrefab.gameObject, transform).GetComponent<TileContentElement>();
                element.gameObject.transform.localPosition = (Vector2) offsetPiece;
                element.Setup(tp.color);
                element.SetOrder(LastOrder);
                
                elements.Add(element);
            }

            if (onPicked != null) OnContentPicked += onPicked;
            if (onDropped != null) OnContentDropped += onDropped;
            if (onDragged != null) OnContentDragged += onDragged;
        }

        public void OnDestroy()
        {
            OnContentPicked = null;
            OnContentDropped = null;
            OnContentDragged = null;
        }

        public void AnimateScale(float from, float to, float duration)
        {
            if (scaleAnimId != Int32.MaxValue) DOTween.Kill(scaleAnimId);
            
            transform.localScale = Vector3.one * from;
            scaleAnimId = gameObject.transform.DOScale(Vector3.one * to, duration).SetEase(Ease.OutSine).intId;
        }

        public Transform Transform => transform;

        public void OnGrab(Vector2 worldPosition)
        {
            initPos = transform.localPosition;

            var worldPos = new Vector3(worldPosition.x, worldPosition.y, LastOrder / 100f);
            pickedUpOffset = transform.localPosition - worldPos;
            var finalPos = worldPos + pickedUpOffset;
            finalPos.z = 0;
            transform.localPosition = finalPos;
            OnContentPicked?.Invoke(this);
            LastOrder++;
            SetOrder(LastOrder);
            
            transform.localScale = Vector3.one * cellScale * 1.05f;
        }

        public void OnDrag(Vector2 worldPosition)
        {
            var finalPos = new Vector3(worldPosition.x, worldPosition.y, LastOrder / 100f) + pickedUpOffset;
            finalPos.z = 0;
            transform.localPosition = finalPos;
            OnContentDragged?.Invoke(this, transform.position);
        }

        public void OnDrop(Vector2 worldPosition)
        {
            OnContentDropped?.Invoke(this);
            /*foreach (var element in elements)
                element.ResetOrder();*/
            transform.localScale = Vector3.one * cellScale;
        }
        
        public void ResetPos() => transform.localPosition = initPos;

        public void SetOrder(int order)
        {
            SetZ(order);
            foreach (var element in elements)
                element.SetOrder(order);
        }
        
        public void SetZ(int order) => transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, order / 100f);
    }
}