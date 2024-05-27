
namespace UnityEngine.UI.Extensions
{
    [AddComponentMenu("Layout/Extensions/Radial Layout")]
    public class RadialLayout : LayoutGroup
    {
        public float fDistance;
        [Range(0f, 360f)]
        public float MinAngle, MaxAngle, StartAngle;
        public bool OnlyLayoutVisible = false;
        protected override void OnEnable() { base.OnEnable(); CalculateRadial(); }
        public override void SetLayoutHorizontal()
        {
        }
        public override void SetLayoutVertical()
        {
        }
        public override void CalculateLayoutInputVertical()
        {
            CalculateRadial();
        }
        public override void CalculateLayoutInputHorizontal()
        {
            CalculateRadial();
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            CalculateRadial();
        }
#endif

        protected override void OnDisable()
        {
            m_Tracker.Clear(); // key change - do not restore - false
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

        void CalculateRadial()
        {
            m_Tracker.Clear();
            if (transform.childCount == 0)
                return;

            int ChildrenToFormat = 0;
            if (OnlyLayoutVisible)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    RectTransform child = (RectTransform)transform.GetChild(i);
                    if ((child != null) && child.gameObject.activeSelf)
                        ++ChildrenToFormat;
                }
            }
            else
            {
                ChildrenToFormat = transform.childCount;
            }

            float fOffsetAngle = (MaxAngle - MinAngle) / ChildrenToFormat;

            float fAngle = StartAngle;
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform child = (RectTransform)transform.GetChild(i);
                if ((child != null) && (!OnlyLayoutVisible || child.gameObject.activeSelf))
                {
                    //Adding the elements to the tracker stops the user from modifying their positions via the editor.
                    m_Tracker.Add(this, child,
                    DrivenTransformProperties.Anchors |
                    DrivenTransformProperties.AnchoredPosition |
                    DrivenTransformProperties.Pivot);
                    Vector3 vPos = new Vector3(Mathf.Cos(fAngle * Mathf.Deg2Rad), Mathf.Sin(fAngle * Mathf.Deg2Rad), 0);
                    child.localPosition = vPos * fDistance;
                    //Force objects to be center aligned, this can be changed however I'd suggest you keep all of the objects with the same anchor points.
                    //child.anchorMin = child.anchorMax = child.pivot = new Vector2(0.5f, 0.5f);
                    fAngle += fOffsetAngle;
                }
            }
        }
    }
}
