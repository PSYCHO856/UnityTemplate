﻿using System;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    public static class DefineManager
    {
        private static DefinesSettings definesData;

        private static DefinesSettings DefinesData
        {
            get
            {
                if (definesData != null)
                    return definesData;

                definesData = EditorUtils.GetAsset<DefinesSettings>();

                return definesData;
            }
        }

        [MenuItem("Tools/Editor/Define Manager")]
        public static void Init()
        {
            var tempDefinesData = DefinesData;
            if (tempDefinesData != null)
            {
                Selection.activeObject = tempDefinesData;
                EditorGUIUtility.PingObject(tempDefinesData);
            }
            else
            {
                if (EditorUtility.DisplayDialog("Define Manager", "Defines asset can't be found.", "Create", "Ignore"))
                    EditorUtils.CreateAsset<DefinesSettings>(
                        "Assets/" + ApplicationConsts.PROJECT_FOLDER + "/Content/Settings/Editor/Define Settings",
                        true);
                else
                    Debug.LogWarning("[Define Manager]: Defines Settings asset can't be found.");
            }
        }

        public static bool HasDefine(string define)
        {
            var tempDefinesData = DefinesData;
            if (tempDefinesData != null)
            {
                var definesLine =
                    PlayerSettings.GetScriptingDefineSymbolsForGroup(
                        BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget));

                return Array.FindIndex(definesLine.Split(';'), x => x == define) != -1;
            }

            if (EditorUtility.DisplayDialog("Define Manager", "Defines asset can't be found.", "Create", "Ignore"))
                EditorUtils.CreateAsset<DefinesSettings>(
                    "Assets/" + ApplicationConsts.PROJECT_FOLDER + "/Content/Settings/Editor/Define Settings", true);
            else
                Debug.LogWarning("[Define Manager]: Defines Settings asset can't be found.");

            return false;
        }

        public static void EnableDefine(string define)
        {
            var tempDefinesData = DefinesData;
            if (tempDefinesData != null)
            {
                var defineLine =
                    PlayerSettings.GetScriptingDefineSymbolsForGroup(
                        BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget));

                if (Array.FindIndex(defineLine.Split(';'), x => x == define) != -1) return;

                defineLine = defineLine.Insert(0, define + ";");

                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget), defineLine);
            }
            else
            {
                if (EditorUtility.DisplayDialog("Define Manager", "Defines asset can't be found.", "Create", "Ignore"))
                    EditorUtils.CreateAsset<DefinesSettings>(
                        "Assets/" + ApplicationConsts.PROJECT_FOLDER + "/Content/Settings/Editor/Define Settings",
                        true);
                else
                    Debug.LogWarning("[Define Manager]: Defines Settings asset can't be found.");
            }
        }

        public static void DisableDefine(string define)
        {
            var tempDefinesData = DefinesData;
            if (tempDefinesData != null)
            {
                var defineLine =
                    PlayerSettings.GetScriptingDefineSymbolsForGroup(
                        BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget));
                var splitedDefines = defineLine.Split(';');

                var tempDefineIndex = Array.FindIndex(splitedDefines, x => x == define);
                var tempDefineLine = "";
                if (tempDefineIndex != -1)
                    for (var i = 0; i < splitedDefines.Length; i++)
                        if (i != tempDefineIndex)
                            defineLine = defineLine.Insert(0, splitedDefines[i]);

                if (defineLine != tempDefineLine)
                {
                    var serializedObject = new SerializedObject(tempDefinesData);
                    serializedObject.Update();
                    serializedObject.FindProperty("definesLine").stringValue = defineLine;
                    serializedObject.ApplyModifiedProperties();

                    PlayerSettings.SetScriptingDefineSymbolsForGroup(
                        BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget), tempDefineLine);
                }
            }
            else
            {
                if (EditorUtility.DisplayDialog("Define Manager", "Defines asset can't be found.", "Create", "Ignore"))
                    EditorUtils.CreateAsset<DefinesSettings>(
                        "Assets/" + ApplicationConsts.PROJECT_FOLDER + "/Content/Settings/Editor/Define Settings",
                        true);
                else
                    Debug.LogWarning("[Define Manager]: Defines Settings asset can't be found.");
            }
        }
    }
}

// -----------------
// Define Manager v 0.2
// -----------------

// Changelog
// v 0.2
// • Enable define function fix
// v 0.1
// • Added basic version