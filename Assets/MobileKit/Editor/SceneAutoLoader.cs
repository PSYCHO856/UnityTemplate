using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MobileKit.Editor
{
    /// <summary>
    /// Scene auto loader.
    /// </summary>
    /// <description>
    /// This class adds a File > Scene Autoload menu containing options to select
    /// a "master scene" enable it to be auto-loaded when the user presses play
    /// in the editor. When enabled, the selected scene will be loaded on play,
    /// then the original scene will be reloaded on stop.
    ///
    /// Based on an idea on this thread:
    /// http://forum.unity3d.com/threads/157502-Executing-first-scene-in-build-settings-when-pressing-play-button-in-editor
    /// </description>
    [InitializeOnLoad]
    public static class SceneAutoLoader 
    {
        // Static constructor binds a playmode-changed callback.
        // [InitializeOnLoad] above makes sure this gets executed.
        static SceneAutoLoader() {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        // Menu items to select the "master" scene and control whether or not to load it.
        [MenuItem("Tools/主场景自动载入/Select Master Scene...")]
        private static void SelectMasterScene() {
            string masterScene = EditorUtility.OpenFilePanel("Select Master Scene", Application.dataPath, "unity");
            masterScene = masterScene.Replace(Application.dataPath, "Assets"); //project relative instead of absolute path
            Debug.Log(masterScene);
            if (!string.IsNullOrEmpty(masterScene)) {
                MasterScene = masterScene;
                LoadMasterOnPlay = true;
            }
        }

        [MenuItem("Tools/主场景自动载入/Load Master On Play", true)]
        private static bool ShowLoadMasterOnPlay() 
        {
            return !LoadMasterOnPlay;
        }

        [MenuItem("Tools/主场景自动载入/Load Master On Play")]
        private static void EnableLoadMasterOnPlay() 
        {
            LoadMasterOnPlay = true;
        }

        [MenuItem("Tools/主场景自动载入/Don't Load Master On Play", true)]
        private static bool ShowDontLoadMasterOnPlay() 
        {
            return LoadMasterOnPlay;
        }

        [MenuItem("Tools/主场景自动载入/Don't Load Master On Play")]
        private static void DisableLoadMasterOnPlay() 
        {
            LoadMasterOnPlay = false;
        }

        // Play mode change callback handles the scene load/reload.
        private static void OnPlayModeChanged(PlayModeStateChange state) {
            if (!LoadMasterOnPlay) {
                return;
            }

            if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode) {
                // User pressed play -- autoload master scene.
                PreviousScene = SceneManager.GetActiveScene().path;
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                    try {
                        EditorSceneManager.OpenScene(MasterScene);
                    } catch {
                        Debug.LogError($"error: scene not found: {MasterScene}");
                        EditorApplication.isPlaying = false;

                    }
                } else {
                    // User cancelled the save operation -- cancel play as well.
                    EditorApplication.isPlaying = false;
                }
            }

            // isPlaying check required because cannot OpenScene while playing
            if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode) {
                // User pressed stop -- reload previous scene.
                try {
                    EditorSceneManager.OpenScene(PreviousScene);
                } catch {
                    Debug.LogError($"error: scene not found: {PreviousScene}");
                }
            }
        }

        // Properties are remembered as editor preferences.
        private const string editorPrefLoadMasterOnPlay = "SceneAutoLoader.LoadMasterOnPlay";
        private const string editorPrefMasterScene = "SceneAutoLoader.MasterScene";
        private const string editorPrefPreviousScene = "SceneAutoLoader.PreviousScene";

        private static bool LoadMasterOnPlay {
            get => EditorPrefs.GetBool(editorPrefLoadMasterOnPlay, true);
            set => EditorPrefs.SetBool(editorPrefLoadMasterOnPlay, value);
        }

        private static string MasterScene {
            get => EditorPrefs.GetString(editorPrefMasterScene, "Assets/MobileKit/Scenes/StartUp.unity");
            set => EditorPrefs.SetString(editorPrefMasterScene, value);
        }

        private static string PreviousScene {
            get => EditorPrefs.GetString(editorPrefPreviousScene, SceneManager.GetActiveScene().path);
            set => EditorPrefs.SetString(editorPrefPreviousScene, value);
        }
    }
}
