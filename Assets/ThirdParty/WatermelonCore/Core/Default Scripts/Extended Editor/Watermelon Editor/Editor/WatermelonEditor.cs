using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Watermelon
{
    public abstract class WatermelonEditor : Editor
    {
        private static WatermelonEditor watermelonEditor;

        protected bool IsStyleInited { get; private set; }

        protected virtual void OnEnable()
        {
            watermelonEditor = this;

            EditorApplication.playModeStateChanged += LogPlayModeState;
            EditorSceneManager.activeSceneChangedInEditMode += ActiveSceneChanged;
        }

        protected void InitStyles()
        {
            if (IsStyleInited)
                return;

            Styles();

            IsStyleInited = true;
        }

        protected void ForceInitStyles()
        {
            IsStyleInited = false;
        }

        protected virtual void Styles()
        {
        }

        private void ActiveSceneChanged(Scene scene1, Scene scene2)
        {
            ForceInitStyles();
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
                if (watermelonEditor != null)
                    watermelonEditor.ForceInitStyles();
        }
    }
}