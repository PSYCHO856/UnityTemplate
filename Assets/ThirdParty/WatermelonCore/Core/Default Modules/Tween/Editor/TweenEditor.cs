using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [CustomEditor(typeof(Tween))]
    public class TweenEditor : Editor
    {
        private readonly string[] tweenTypes = {"Update", "Fixed", "Late"};
        private Tween refTarget;

        private MonoScript script;

        private bool showTweens;
        private int tabID;

        private void OnEnable()
        {
            refTarget = (Tween) target;

            script = MonoScript.FromMonoBehaviour(refTarget);
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script:", script, typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            showTweens = EditorGUILayout.Foldout(showTweens, "Show Active Tweens");

            if (showTweens)
            {
                tabID = GUILayout.Toolbar(tabID, tweenTypes);

                EditorGUILayout.BeginVertical();
                switch (tabID)
                {
                    case 0:
                        for (var i = 0; i < refTarget.UpdateTweens.Length; i++)
                            if (refTarget.UpdateTweens[i] != null)
                                if (GUILayout.Button(refTarget.UpdateTweens[i].activeId + ": " +
                                                     refTarget.UpdateTweens[i].GetType()))
                                {
                                    refTarget.UpdateTweens[i].Kill();

                                    break;
                                }

                        break;
                    case 1:
                        for (var i = 0; i < refTarget.FixedTweens.Length; i++)
                            if (refTarget.FixedTweens[i] != null)
                                if (GUILayout.Button(refTarget.FixedTweens[i].activeId + ": " +
                                                     refTarget.FixedTweens[i].GetType()))
                                {
                                    refTarget.FixedTweens[i].Kill();

                                    break;
                                }

                        break;
                    case 2:
                        for (var i = 0; i < refTarget.LateTweens.Length; i++)
                            if (refTarget.LateTweens[i] != null)
                                if (GUILayout.Button(refTarget.LateTweens[i].activeId + ": " +
                                                     refTarget.LateTweens[i].GetType()))
                                {
                                    refTarget.LateTweens[i].Kill();

                                    break;
                                }

                        break;
                }

                EditorGUILayout.EndVertical();
            }
        }
    }
}