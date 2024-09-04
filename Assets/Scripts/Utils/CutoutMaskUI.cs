using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Utils
{
    public class CutoutMaskUI : Image
    {
        public override Material materialForRendering
        {
            get
            {
                var material = new Material(base.materialForRendering);
                material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
                return material;
            }
        }
    }
}
