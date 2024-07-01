using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.Managers
{
    public class ManageScenes : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadAsyncScene(sceneName));
        }

        IEnumerator LoadAsyncScene(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    
    
    }
}
