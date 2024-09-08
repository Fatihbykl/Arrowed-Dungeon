using System;
using UnityEngine;

namespace Utils
{
    public class TargetFrameRate : MonoBehaviour
    {
        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}
