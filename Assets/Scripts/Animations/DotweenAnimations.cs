using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

namespace Animations
{
    public class DotweenAnimations
    {

        public static void DoPunchAnimation(VisualElement elem, Vector3 direction, float duration, Ease ease)
        {
            DOTween.Punch(
                () => elem.transform.scale,
                x => elem.transform.scale = x,
                direction,
                duration
            ).SetEase(ease);
        }

        public static void DoScaleFromZeroAnimation(VisualElement elem)
        {
            DOTween.To(
                () => elem.transform.scale,
                x => elem.transform.scale = x,
                new Vector3(1f, 1f, 1f),
                0.5f
            ).SetEase(Ease.InSine).Restart();
        }
    }
}
