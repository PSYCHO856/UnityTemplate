#pragma warning disable 0649

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Watermelon
{
    [DefaultExecutionOrder(-999)]
    public class Initialiser : MonoBehaviour
    {
        // [SerializeField] Canvas systemCanvas;

        // public static Canvas SystemCanvas;
        public static GameObject InitialiserGameObject;
        [SerializeField] private ProjectInitSettings initSettings;

        private void Awake()
        {
            if (!initSettings.IsInititalized)
            {
                // SystemCanvas = systemCanvas;
                var o = gameObject;
                InitialiserGameObject = o;
                DontDestroyOnLoad(o);

                initSettings.Init(this);

                Destroy(this);
            }
            else
            {
                Debug.Log("[Initialiser]: Game is already initialized!");

                Destroy(gameObject);
            }
        }

#if UNITY_EDITOR
        [InitializeOnLoad]
        private static class InitCallbackHandler
        {
            public static ProjectInitSettings initSettings;

            static InitCallbackHandler()
            {
                EditorApplication.playModeStateChanged += ModeChanged;
            }

            [DidReloadScripts]
            private static void CreateAssetWhenReady()
            {
                if ((EditorApplication.isCompiling || EditorApplication.isUpdating) && EditorApplication.isPlaying)
                    EditorApplication.delayCall += delegate
                    {
                        initSettings = RuntimeEditorUtils.GetAssetByName<ProjectInitSettings>("Project Init Settings");

                        if (initSettings != null)
                            initSettings.Destroy();
                    };
            }

            private static void ModeChanged(PlayModeStateChange playModeState)
            {
                if (playModeState == PlayModeStateChange.ExitingPlayMode)
                {
                    initSettings = RuntimeEditorUtils.GetAssetByName<ProjectInitSettings>("Project Init Settings");

                    if (initSettings != null)
                        initSettings.Destroy();
                }
            }
        }
#endif
    }
}

// -----------------
// Initialiser v 0.3
// -----------------

// Changelog
// v 0.3
// • Initializer renamed to Initialiser
// • Fixed problem with recompilation
// v 0.2
// • Added sorting feature
// • Initialiser MonoBehaviour will destroy after initialization
// v 0.1
// • Added basic version