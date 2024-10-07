using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI
{
    public enum TargetPositionType
    {
        World,
        Screen
    }

    [Serializable]
    public struct HintTarget
    {
        public string hintId;
        public TargetPositionType targetPositionType;
        public GameObject targetObject;
        [TextArea]
        public string hintText;
    }
    
    public class HintManager : MonoBehaviour
    {
        public GameObject hintUI;
        public GameObject hintPopup;
        public TextMeshProUGUI hintPopupText;
        public HintTarget[] hints;

        public static HintManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Found more than one Hint Manager in the scene.");
                Destroy(this);
                return;
            }
            Instance = this;
        }
        
        public void OpenHint(string id)
        {
            var hint = hints.FirstOrDefault(h => h.hintId == id);
            var position = GetPositionOfTarget(hint);
            var popupPosition = GetPositionOfPopup(position);
            
            Time.timeScale = 0;
            hintUI.transform.position = position;
            hintPopup.transform.localPosition = popupPosition;
            hintPopupText.text = hint.hintText;
            hintUI.SetActive(true);
        }

        public void CloseHint()
        {
            Time.timeScale = 1;
            hintUI.SetActive(false);
        }

        private Vector3 GetPositionOfTarget(HintTarget target)
        {
            var pos = target.targetObject.transform.position;
            
            return target.targetPositionType switch
            {
                TargetPositionType.Screen => pos,
                TargetPositionType.World => Camera.main.WorldToScreenPoint(pos),
                _ => Vector3.zero
            };
        }

        private Vector2 GetPositionOfPopup(Vector3 targetPos)
        {
            Vector3 pos = new Vector3(0,-100,0);
            var boundX = 960;

            if (targetPos.x > boundX) { pos.x = -450; }
            else { pos.x = 450; }

            return pos;
        }
    }
}
