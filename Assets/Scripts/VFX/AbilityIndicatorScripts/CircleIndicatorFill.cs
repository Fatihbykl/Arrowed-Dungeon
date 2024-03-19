using System;
using UnityEngine;

namespace VFX.AbilityIndicatorScripts
{
    public class CircleIndicatorFill : MonoBehaviour
    {
        [Range(0, 1)]
        public float fillProgress;
        private MeshRenderer circleRenderer;
    
        private static readonly int ProgressShaderID = Shader.PropertyToID("_FillProgress");

        private void Start()
        {
            circleRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (circleRenderer == null)
                return;

            circleRenderer.sharedMaterial.SetFloat(ProgressShaderID, fillProgress);
        }
    }
}