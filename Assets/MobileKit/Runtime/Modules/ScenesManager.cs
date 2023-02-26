using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MobileKit
{
    public enum SceneLoadStatus
    {
        Loading,
        WaitForActivation,
        Activating,
        Activated
    }
    public class ScenesManager : Singleton<ScenesManager>
    {
        public static float LoadingProgress { get; set; }
        public static SceneLoadStatus SceneLoadStatus { get; set; } = SceneLoadStatus.Activated;

        public static string SceneName { get; private set; }
        
        private static AsyncOperation asyncOperation = null;
        
        public static void LoadScene(string sceneName, bool isAutoActive = false)
        {
            SceneName = sceneName;
            Instance.StartCoroutine(LoadSceneAsync(sceneName, isAutoActive));
        }
        
        private static IEnumerator LoadSceneAsync(string sceneName, bool isAutoActive)
        {
            asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            LoadingProgress = 0;
            SceneLoadStatus = SceneLoadStatus.Loading;
            
            while (!asyncOperation.isDone)
            {
                LoadingProgress = asyncOperation.progress;
                if (asyncOperation.progress >= 0.9f)
                { 
                    if (isAutoActive || SceneLoadStatus == SceneLoadStatus.Activating)
                    {
                        asyncOperation.allowSceneActivation = true;
                    }
                    else
                    {
                        SceneLoadStatus = SceneLoadStatus.WaitForActivation;
                    }
                }
                yield return null;
            }
            SceneLoadStatus = SceneLoadStatus.Activated;
        }

        public T[] FindAllComponentsInScene<T>() where T : Component
        {
            var components = Resources.FindObjectsOfTypeAll<T>();
            foreach (var go in components)
            {
                Debug.Log($"找到Componet {typeof(T)} In GameObject: {go.gameObject.name} Scene: {go.gameObject.scene.name}");
            }
            return components;
        }
    }
}
