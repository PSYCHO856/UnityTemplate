using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Watermelon
{
    [InitializeOnLoad]
    public static class EditorStylesExtended
    {
        private const string ICONS_FOLDER_PATH = "/Sprites/Icons";

        private const string GUISKIN_NAME = "EditorStylesExtendedSkin";
        private const string GUISKIN_PRO_NAME = "EditorStylesExtendedProSkin";

        public const string ICON_SPACE = "  ";

        /// <summary>
        ///     Assets\Park Inc\Watermelon Core\Core\Default Scripts\Extended Editor\Editor Style Extended\
        /// </summary>
        public static GUISkin editorSkin;

        public static GUIStyle tab;

        public static GUIStyle box_01;
        public static GUIStyle box_02;
        public static GUIStyle box_03;

        public static GUIStyle labelCentered;

        public static GUIStyle label_small;
        public static GUIStyle label_small_bold;

        public static GUIStyle label_medium;
        public static GUIStyle label_medium_bold;

        public static GUIStyle label_large;
        public static GUIStyle label_large_bold;

        public static GUIStyle button_01;
        public static GUIStyle button_01_large;

        public static GUIStyle button_02;
        public static GUIStyle button_02_large;

        public static GUIStyle button_03;
        public static GUIStyle button_03_large;
        public static GUIStyle button_03_mini;

        public static GUIStyle button_04;
        public static GUIStyle button_04_large;
        public static GUIStyle button_04_mini;

        public static GUIStyle button_05;
        public static GUIStyle button_05_large;

        public static GUIStyle helpbox;

        public static GUIStyle button_tab;

        public static GUIStyle boxHeader;

        public static GUIStyle padding00;
        public static GUIStyle padding05;
        public static GUIStyle padding10;

        public static GUIStyle panelBottom;

        public static GUIStyle boxCompiling;

        private static Dictionary<string, Texture2D> projectIcons = new();

        private static bool isInited;

        private static readonly Color defaultIconColor = Color.black;
        private static readonly Color darkIconColor = Color.white;

        static EditorStylesExtended()
        {
            InitializeStyles();
        }

        public static Color IconColor
        {
            get
            {
                if (EditorGUIUtility.isProSkin)
                    return darkIconColor;

                return defaultIconColor;
            }
        }

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            if (isInited)
                return;

            EditorCoroutines.Execute(TryToInitialize());
        }

        private static IEnumerator TryToInitialize()
        {
            while (EditorApplication.isCompiling) yield return null;

            InitializeStyles();
        }

        public static void InitializeStyles()
        {
            if (isInited)
                return;

            if (!editorSkin)
            {
                if (EditorGUIUtility.isProSkin)
                    editorSkin = EditorUtils.GetAsset<GUISkin>(GUISKIN_PRO_NAME);

                if (!editorSkin)
                    editorSkin = EditorUtils.GetAsset<GUISkin>(GUISKIN_NAME);
            }

            if (editorSkin)
            {
                LoadIcons();

                tab = editorSkin.GetStyle("Tab");

                box_01 = editorSkin.GetStyle("box_01");
                box_02 = editorSkin.GetStyle("box_02");
                box_03 = editorSkin.GetStyle("box_03");

                label_small = editorSkin.GetStyle("label_small");
                label_small_bold = editorSkin.GetStyle("label_small_bold");

                label_medium = editorSkin.GetStyle("label_medium");
                label_medium_bold = editorSkin.GetStyle("label_medium_bold");

                label_large = editorSkin.GetStyle("label_large");
                label_large_bold = editorSkin.GetStyle("label_large_bold");

                button_01 = editorSkin.GetStyle("button_01");
                button_01_large = editorSkin.GetStyle("button_01_large");

                button_02 = editorSkin.GetStyle("button_02");
                button_02_large = editorSkin.GetStyle("button_02_large");

                button_03 = editorSkin.GetStyle("button_03");
                button_03_large = editorSkin.GetStyle("button_03_large");

                button_03_mini = new GUIStyle(button_03_large)
                {
                    padding = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(1, 1, 1, 1),
                    fontStyle = FontStyle.Bold,
                    fontSize = 12
                };

                button_04 = editorSkin.GetStyle("button_04");
                button_04_large = editorSkin.GetStyle("button_04_large");

                button_04_mini = new GUIStyle(button_04_large)
                {
                    padding = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(1, 1, 1, 1),
                    fontStyle = FontStyle.Bold,
                    fontSize = 12
                };

                button_05 = editorSkin.GetStyle("button_05");
                button_05_large = editorSkin.GetStyle("button_05_large");

                helpbox = editorSkin.GetStyle("helpbox");

                button_tab = editorSkin.GetStyle("button_tab");

                boxHeader = editorSkin.GetStyle("boxHeader");

                panelBottom = editorSkin.GetStyle("panelButton");

                labelCentered = new GUIStyle(editorSkin.label);
                labelCentered.alignment = TextAnchor.MiddleCenter;

                padding00 = GetPaddingStyle(new GUIStyle(), new RectOffset(0, 0, 0, 0));
                padding05 = GetPaddingStyle(new GUIStyle(), new RectOffset(0, 0, 5, 5));
                padding10 = GetPaddingStyle(new GUIStyle(), new RectOffset(0, 0, 10, 10));

                boxCompiling = editorSkin.GetStyle("boxCompiling");

                isInited = true;
            }
            else
            {
                Debug.LogWarning("[EditorExtended]: Editor skin can't be found!");
            }
        }

        public class IconAssetPostProcessor : AssetPostprocessor
        {
            private void OnPostprocessTexture(Texture2D texture)
            {
                if (assetPath.Contains("Watermelon")) LoadIcons();
            }
        }

        #region Styles

        public static GUIStyle GetAligmentStyle(GUIStyle style, TextAnchor textAnchor)
        {
            var tempStyle = new GUIStyle(style);
            tempStyle.alignment = textAnchor;

            return tempStyle;
        }

        public static GUIStyle GetTextColorStyle(GUIStyle style, Color color)
        {
            var tempStyle = new GUIStyle(style);
            tempStyle.normal.textColor = color;

            return tempStyle;
        }

        public static GUIStyle GetTextFontSizeStyle(GUIStyle style, int fontSize)
        {
            var tempStyle = new GUIStyle(style);
            tempStyle.fontSize = fontSize;

            return tempStyle;
        }

        public static GUIStyle GetPaddingStyle(GUIStyle style, RectOffset padding)
        {
            var tempStyle = new GUIStyle(style);
            tempStyle.padding = padding;

            return tempStyle;
        }

        public static GUIStyle GetBoxWithColor(Color color)
        {
            var backgroundTexture = new Texture2D(1, 1);
            backgroundTexture.SetPixel(0, 0, color);
            backgroundTexture.Apply();

            var backgroundStyle = new GUIStyle();
            backgroundStyle.normal.background = backgroundTexture;

            return backgroundStyle;
        }

        #endregion

        #region Icons

        private static void LoadIcons()
        {
            projectIcons = new Dictionary<string, Texture2D>();

            if (editorSkin != null)
                if (!string.IsNullOrEmpty(AssetDatabase.GetAssetPath(editorSkin)))
                {
                    var folderPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(editorSkin))?.Replace(@"\", "/") +
                                     ICONS_FOLDER_PATH;

                    var guids = AssetDatabase.FindAssets("t:texture2D", new[] {folderPath});

                    foreach (var guid in guids)
                    {
                        var tempTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(guid));

                        projectIcons.Add(tempTexture.name, tempTexture);
                    }
                }
        }

        public static Texture2D GetTexture(string name)
        {
            if (projectIcons.ContainsKey(name)) return projectIcons[name];

            Debug.LogWarning("Texture " + name + " can't be found!");

            return null;
        }

        public static Texture2D GetTexture(string name, Color color)
        {
            if (projectIcons.ContainsKey(name))
            {
                var tempTexture = new Texture2D(projectIcons[name].width, projectIcons[name].height);
                tempTexture.SetPixels(projectIcons[name].GetPixels());

                for (var x = 0; x < tempTexture.width; x++)
                for (var y = 0; y < tempTexture.height; y++)
                {
                    var tempColor = tempTexture.GetPixel(x, y);
                    if (tempColor.a > 0) tempTexture.SetPixel(x, y, color.SetAlpha(tempColor.a));
                }

                tempTexture.Apply();

                return tempTexture;
            }

            Debug.LogWarning("Texture " + name + " can't be found!");

            return null;
        }

        #endregion
    }
}