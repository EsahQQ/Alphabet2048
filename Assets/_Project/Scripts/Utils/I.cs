using System;
using UnityEngine;

namespace _Project.Scripts.Utils
{
    public class I : MonoBehaviour
    {
        private void Awake()
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value; 
        }
    }
}