using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallingSloth
{
    public class SceneManager : SingletonBehaviour<SceneManager>
    {
        public void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        public void LoadScene(int sceneIndex)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }

        public void LoadSceneAsync(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        }

        public void LoadSceneAsync(int sceneIndex)
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
        }
    }
}