using System;
using System.Collections;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gameplay.Managers
{
    public class AsyncSceneLoader : MonoBehaviour
    {
        public GameObject loadingScreen;
        public Slider slider;
        public TextMeshProUGUI text;

        private void Start()
        {
            EventManager.EmitEvent(EventStrings.SceneLoaded);
        }

        public void LoadScene(string sceneName)
        {
            if (loadingScreen) { loadingScreen.SetActive(true); }
            
            EventManager.EmitEvent(EventStrings.SceneChanged);
            StartCoroutine(LoadAsyncScene(sceneName));
        }

        IEnumerator LoadAsyncScene(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                if (slider)
                {
                    var progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                    slider.value = progress;
                    text.text = $"Loading...{progress * 100}%";
                }

                yield return null;
            }
        }
    
    
    }
}
