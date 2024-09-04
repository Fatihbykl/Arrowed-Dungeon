using System;
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
        public TargetPositionType targetPositionType;
        public RectTransform targetRect;
        [TextArea]
        public string hintText;
    }
    
    public class HintManager : MonoBehaviour
    {
        public GameObject hintUI;
        public GameObject hintPopup;
        public TextMeshProUGUI hintPopupText;
        public HintTarget[] hints;

        private int _nextHintIndex;

        private void Start()
        {
            TestHint();
        }

        private async void TestHint()
        {
            await UniTask.WaitForSeconds(2);
            OpenHint();
            // await UniTask.WaitForSeconds(5);
            // CloseHint();
            // await UniTask.WaitForSeconds(2);
            // OpenHint();
            // await UniTask.WaitForSeconds(5);
            // CloseHint();
        }

        public void OpenHint()
        {
            var hint = hints[_nextHintIndex];
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
            _nextHintIndex++;
        }

        private Vector3 GetPositionOfTarget(HintTarget target)
        {
            var pos = target.targetRect.position;
            
            return target.targetPositionType switch
            {
                TargetPositionType.Screen => pos,
                TargetPositionType.World => Camera.main.WorldToScreenPoint(pos),
                _ => Vector3.zero
            };
        }

        private Vector2 GetPositionOfPopup(Vector3 targetPos)
        {
            Vector3 pos = new Vector3(0,100,0);
            var boundX = 960;

            if (targetPos.x > boundX) { pos.x = -450; }
            else { pos.x = 450; }

            return pos;
        }
    }
}
