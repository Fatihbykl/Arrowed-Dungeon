using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Utils
{
    public class FPSCounter : MonoBehaviour
    {
        public enum DeltaTimeType
        {
            Smooth,
            Unscaled
        }

        public TextMeshProUGUI fpsText;
        [Tooltip("Unscaled is more accurate, but jumpy, or if your game modifies Time.timeScale. Use Smooth for smoothDeltaTime.")]
        public DeltaTimeType deltaType = DeltaTimeType.Smooth;

        private Dictionary<int, string> _cachedNumberStrings = new();

        private int[] _frameRateSamples;
        private int _cacheNumbersAmount = 300;
        private int _averageFromAmount = 30;
        private int _averageCounter;
        private int _currentAveraged;

        private FPSCounter Instance;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Found more than one FPS Counter in the scene.");
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            {
                for (int i = 0; i < _cacheNumbersAmount; i++) {
                    _cachedNumberStrings[i] = i.ToString();
                }

                _frameRateSamples = new int[_averageFromAmount];
            }
        }
        
        void Update()
        {
            // Sample
            {
                var currentFrame = (int)Math.Round(1f / deltaType switch
                {
                    DeltaTimeType.Smooth => Time.smoothDeltaTime,
                    DeltaTimeType.Unscaled => Time.unscaledDeltaTime,
                    _ => Time.unscaledDeltaTime
                });
                _frameRateSamples[_averageCounter] = currentFrame;
            }

            // Average
            {
                var average = 0f;

                foreach (var frameRate in _frameRateSamples) {
                    average += frameRate;
                }

                _currentAveraged = (int)Math.Round(average / _averageFromAmount);
                _averageCounter = (_averageCounter + 1) % _averageFromAmount;
            }

            // Assign to UI
            {
                fpsText.text = _currentAveraged switch
                {
                    var x when x >= 0 && x < _cacheNumbersAmount => _cachedNumberStrings[x],
                    var x when x >= _cacheNumbersAmount => $"> {_cacheNumbersAmount}",
                    var x when x < 0 => "< 0",
                    _ => "?"
                };
            }
        }
    }
}
