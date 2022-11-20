using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Watermelon
{
    public static class RuntimeEditorUtils
    {
#if UNITY_EDITOR
        public static T GetAssetByName<T>(string name = "") where T : Object
        {
            var assets =
                AssetDatabase.FindAssets((string.IsNullOrEmpty(name) ? "" : name + " ") + "t:" + typeof(T).Name);
            if (assets.Length > 0)
            {
                string assetPath;
                for (var i = 0; i < assets.Length; i++)
                {
                    assetPath = AssetDatabase.GUIDToAssetPath(assets[i]);
                    if (Path.GetFileNameWithoutExtension(assetPath) == name)
                        return (T) AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));
                }
            }

            return null;
        }
#endif
    }
}