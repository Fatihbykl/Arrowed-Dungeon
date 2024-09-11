using System;
using UnityEngine;

namespace Utils
{
    public class TargetFrameRate : MonoBehaviour
    {
        private void Awake()
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 300;
        }
    }
}
