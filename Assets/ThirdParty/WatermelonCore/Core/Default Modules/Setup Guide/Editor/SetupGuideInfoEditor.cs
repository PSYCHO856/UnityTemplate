using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Watermelon
{
    [CustomEditor(typeof(SetupGuideInfo))]
    public class SetupGuideInfoEditor : WatermelonEditor
    {
        private const string SITE_URL = @"https://wmelongames.com";

        private const string PROTOTYPE_URL = @"https://wmelongames.com/prototype/guide.php";
        private const string MAIL_URL = "https://wmelongames.com/contact/";
        private const string DISCORD_URL = "https://discord.gg/xEGUnBg";
        private static SetupGuideInfoEditor instance;

        private static readonly string PROJECT_DESCRIPTION =
            @"Thank you for purchasing {0}.\nBefore you start working with project, read the documentation.\nPlease, leave a review and rate the project.";

        private static SetupButton[] setupButtons;
        private static FinishedProject[] finishedProjects;

        private string description;

        private GUIStyle descriptionStyle;
        private GUIContent discordButtonContent;
        private GUIContent documentationButtonContent;
        private GUIStyle gameButtonStyle;

        private GUIContent logoContent;
        private GUIStyle logoStyle;
        private GUIContent mailButtonContent;
        private GUIStyle projectStyle;
        private GUIStyle setupButtonStyle;

        private SetupGuideInfo setupGuideInfo;
        private GUIStyle textGamesStyle;

        protected override void OnEnable()
        {
            base.OnEnable();

            instance = this;

            setupGuideInfo = target as SetupGuideInfo;

            var tempSetupButtons = new List<SetupButton>();
            if (!setupGuideInfo.windowButtons.IsNullOrEmpty())
                tempSetupButtons.AddRange(setupGuideInfo.windowButtons);
            if (!setupGuideInfo.folderButtons.IsNullOrEmpty())
                tempSetupButtons.AddRange(setupGuideInfo.folderButtons);
            if (!setupGuideInfo.fileButtons.IsNullOrEmpty())
                tempSetupButtons.AddRange(setupGuideInfo.fileButtons);

            setupButtons = tempSetupButtons.ToArray();
        }

        protected override void Styles()
        {
            description = string.Format(PROJECT_DESCRIPTION, setupGuideInfo.gameName).Replace("\\n", "\n");

            logoContent =
                new GUIContent(
                    EditorGUIUtility.isProSkin
                        ? EditorStylesExtended.GetTexture("logo_white")
                        : EditorStylesExtended.GetTexture("logo_black"), SITE_URL);

            textGamesStyle =
                EditorStylesExtended.GetAligmentStyle(EditorStylesExtended.label_small, TextAnchor.MiddleCenter);
            textGamesStyle.alignment = TextAnchor.MiddleCenter;
            textGamesStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;

            gameButtonStyle =
                EditorStylesExtended.GetPaddingStyle(EditorStylesExtended.button_05, new RectOffset(2, 2, 2, 2));

            descriptionStyle = new GUIStyle(EditorStyles.label);
            descriptionStyle.wordWrap = true;

            setupButtonStyle = new GUIStyle(EditorStylesExtended.button_01);
            setupButtonStyle.imagePosition = ImagePosition.ImageAbove;

            mailButtonContent =
                new GUIContent(EditorStylesExtended.GetTexture("icon_mail", EditorStylesExtended.IconColor));
            discordButtonContent =
                new GUIContent(EditorStylesExtended.GetTexture("icon_discord", EditorStylesExtended.IconColor));
            documentationButtonContent = new GUIContent(EditorStylesExtended.ICON_SPACE + "Documentation",
                EditorStylesExtended.GetTexture("icon_documentation", EditorStylesExtended.IconColor));

            logoStyle = new GUIStyle(GUIStyle.none);
            logoStyle.alignment = TextAnchor.MiddleCenter;
            logoStyle.padding = new RectOffset(10, 10, 10, 10);

            projectStyle = new GUIStyle(GUI.skin.label);
            projectStyle.alignment = TextAnchor.MiddleCenter;
            projectStyle.wordWrap = false;
            projectStyle.clipping = TextClipping.Overflow;

            for (var i = 0; i < setupButtons.Length; i++) setupButtons[i].Init();

            if (finishedProjects.IsNullOrEmpty())
                EditorCoroutines.Execute(instance.GetRequest(PROTOTYPE_URL));
            else
                for (var i = 0; i < finishedProjects.Length; i++)
                    finishedProjects[i].LoadTexture();
        }

        public override void OnInspectorGUI()
        {
            InitStyles();

            EditorGUILayout.BeginVertical(EditorStylesExtended.editorSkin.box);

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(logoContent, logoStyle, GUILayout.Width(80), GUILayout.Height(80)))
                Application.OpenURL(SITE_URL);

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal(EditorStylesExtended.padding05, GUILayout.Height(21),
                GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("GREETINGS!", EditorStylesExtended.boxHeader, GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true));

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(discordButtonContent, EditorStylesExtended.button_01, GUILayout.Width(22),
                GUILayout.Height(22))) Application.OpenURL(DISCORD_URL);

            if (GUILayout.Button(mailButtonContent, EditorStylesExtended.button_01, GUILayout.Width(22),
                GUILayout.Height(22))) Application.OpenURL(MAIL_URL);

            if (GUILayout.Button(documentationButtonContent, EditorStylesExtended.button_01, GUILayout.Height(22),
                GUILayout.MinWidth(112))) Application.OpenURL(setupGuideInfo.documentationURL);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(description, descriptionStyle);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
            EditorGUILayout.LabelField(GUIContent.none, EditorStylesExtended.editorSkin.horizontalSlider);
            GUILayout.Space(-15);

            if (setupButtons.Length > 0)
            {
                EditorGUILayoutCustom.Header("LINKS");

                EditorGUILayout.BeginHorizontal();

                for (var i = 0; i < setupButtons.Length; i++) setupButtons[i].Draw(setupButtonStyle);

                EditorGUILayout.EndHorizontal();

                GUILayout.Space(20);
                EditorGUILayout.LabelField(GUIContent.none, EditorStylesExtended.editorSkin.horizontalSlider);
                GUILayout.Space(-10);
            }

            EditorGUILayoutCustom.Header("OUR TEMPLATES");

            EditorGUILayout.BeginHorizontal();

            if (finishedProjects != null)
            {
                GUILayout.FlexibleSpace();
                for (var i = 0; i < finishedProjects.Length; i++)
                {
                    EditorGUILayout.BeginVertical();
                    if (GUILayout.Button(new GUIContent(finishedProjects[i].gameTexture, finishedProjects[i].name),
                        gameButtonStyle, GUILayout.Height(65), GUILayout.Width(65)))
                        Application.OpenURL(finishedProjects[i].url);
                    EditorGUILayout.LabelField(finishedProjects[i].name, projectStyle, GUILayout.Width(70));
                    EditorGUILayout.EndVertical();

                    GUILayout.FlexibleSpace();

                    if (i != 0 && (i + 1) % 4 == 0)
                    {
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(10);
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                    }
                }
            }
            else
            {
                EditorGUILayout.BeginVertical();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("Loading templates..", textGamesStyle);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }

        #region Web

        private IEnumerator GetRequest(string uri)
        {
            var www = UnityWebRequest.Get(uri);
            www.SendWebRequest();

            while (!www.isDone) yield return null;

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("[Setup Guide]: " + www.error);
            }
            else
            {
                // Or retrieve results as binary data
                var results = www.downloadHandler.data;

                // For that you will need to add reference to System.Runtime.Serialization
                var jsonReader = JsonReaderWriterFactory.CreateJsonReader(results, new XmlDictionaryReaderQuotas());

                // For that you will need to add reference to System.Xml and System.Xml.Linq
                var root = XElement.Load(jsonReader);

                var finishedProjectsTemp = new List<FinishedProject>();
                foreach (var element in root.Elements())
                {
                    var projectTemp = new FinishedProject(element.XPathSelectElement("name").Value,
                        element.XPathSelectElement("url").Value, element.XPathSelectElement("image").Value);

                    projectTemp.LoadTexture();

                    finishedProjectsTemp.Add(projectTemp);
                }

                finishedProjects = finishedProjectsTemp.ToArray();
            }
        }

        #endregion

        private static void RepaintEditor()
        {
            if (instance != null)
                instance.Repaint();
        }

        private class FinishedProject
        {
            public Texture2D gameTexture;

            public readonly string imageUrl = "";
            public readonly string name = "";
            public readonly string url = "";

            public FinishedProject(string name, string url, string imageUrl)
            {
                this.name = name;
                this.url = url;
                this.imageUrl = imageUrl;
            }

            public void LoadTexture()
            {
                if (!string.IsNullOrEmpty(url))
                    EditorCoroutines.Execute(GetTexture(imageUrl, texture =>
                    {
                        gameTexture = texture;

                        RepaintEditor();
                        SetupGuideWindow.RepaintWindow();
                    }));
            }

            private IEnumerator GetTexture(string uri, Action<Texture2D> onLoad)
            {
                var www = UnityWebRequestTexture.GetTexture(uri);
                www.SendWebRequest();

                while (!www.isDone) yield return null;

                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    var myTexture = ((DownloadHandlerTexture) www.downloadHandler).texture;
                    if (myTexture != null) onLoad.Invoke(myTexture);
                }
            }
        }
    }
}