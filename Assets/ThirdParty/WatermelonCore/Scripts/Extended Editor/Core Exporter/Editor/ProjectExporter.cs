using System;
using System.Collections.Generic;
using UnityEditor;

namespace Watermelon
{
    public static class ProjectExporter
    {
        private static readonly Dictionary<string, string[]> exportFolder = new()
        {
            {
                "Project", new[]
                {
                    "Assets"
                }
            },
            {
                "Core", new[]
                {
                    "Assets/" + ApplicationConsts.PROJECT_FOLDER + "/Watermelon Core",
                    "Assets/" + ApplicationConsts.PROJECT_FOLDER + "/Game",
                    "Assets/" + ApplicationConsts.PROJECT_FOLDER + "/Resources",
                    "Assets/" + ApplicationConsts.PROJECT_FOLDER + "/Content"
                }
            },
            {
                "Extras", new[]
                {
                    "Assets/" + ApplicationConsts.PROJECT_FOLDER + "/Project Extras"
                }
            }
        };

        [MenuItem("Tools/Export/Core")]
        public static void ExportCore()
        {
            Export("Core", exportFolder["Core"]);
        }

        [MenuItem("Tools/Export/Extras")]
        public static void ExportExtras()
        {
            Export("Extras", exportFolder["Extras"]);
        }

        [MenuItem("Tools/Export/Project")]
        public static void ExportProject()
        {
            Export("Project", exportFolder["Project"]);
        }

        private static void Export(string name, string[] folders)
        {
            var currentDate = DateTime.Now.Date;

            var path = EditorUtility.SaveFilePanel(name, "",
                name + "_" + currentDate.ToString("yyyy-MM-dd") + ".unitypackage", "unitypackage");

            if (!string.IsNullOrEmpty(path))
            {
                EditorUtility.DisplayProgressBar("Hold on!", "Exporting assets..", 0);
                AssetDatabase.ExportPackage(folders, path, ExportPackageOptions.Recurse);
                EditorUtility.ClearProgressBar();
            }
        }
    }
}