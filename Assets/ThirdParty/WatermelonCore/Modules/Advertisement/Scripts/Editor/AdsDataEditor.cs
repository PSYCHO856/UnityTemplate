using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [CustomEditor(typeof(AdsData))]
    public class AdsDataEditor : WatermelonEditor
    {
        private static GUIContent arrowDownContent;
        private static GUIContent arrowUpContent;

        private readonly AdsContainer[] adsContainers =
        {
            new DummyContainer("Dummy", "dummyContainer", string.Empty),
            new AdMobContainer("AdMob", "adMobContainer", "MODULE_ADMOB"),
            new UnityAdsContainer("Unity Ads", "unityAdsContainer", "MODULE_UNITYADS")
        };

        private IEnumerable<SerializedProperty> adsFrequencyContainerProperties;
        private SerializedProperty bannerTypeProperty;

        private IEnumerable<SerializedProperty> gdprContainerProperties;

        private SerializedProperty gdprContainerProperty;
        private SerializedProperty interstitialTypeProperty;
        private SerializedProperty rewardedVideoTypeProperty;
        private SerializedProperty systemLogsProperty;

        private SerializedProperty testModeProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            bannerTypeProperty = serializedObject.FindProperty("bannerType");
            interstitialTypeProperty = serializedObject.FindProperty("interstitialType");
            rewardedVideoTypeProperty = serializedObject.FindProperty("rewardedVideoType");

            testModeProperty = serializedObject.FindProperty("testMode");
            systemLogsProperty = serializedObject.FindProperty("systemLogs");

            gdprContainerProperty = serializedObject.FindProperty("gdprContainer");
            gdprContainerProperties = gdprContainerProperty.GetChildren();

            adsFrequencyContainerProperties = serializedObject.FindProperty("adsFrequency").GetChildren();

            for (var i = 0; i < adsContainers.Length; i++) adsContainers[i].Initialize(serializedObject);

            ForceInitStyles();
        }

        protected override void Styles()
        {
            arrowDownContent =
                new GUIContent(EditorStylesExtended.GetTexture("icon_arrow_down", new Color(0.2f, 0.2f, 0.2f)));
            arrowUpContent =
                new GUIContent(EditorStylesExtended.GetTexture("icon_arrow_up", new Color(0.2f, 0.2f, 0.2f)));
        }

        public override void OnInspectorGUI()
        {
            InitStyles();

            serializedObject.Update();

            EditorGUILayout.BeginVertical(EditorStylesExtended.editorSkin.box);

            EditorGUILayoutCustom.Header("ADVERTISING");

            EditorGUILayout.PropertyField(bannerTypeProperty);
            EditorGUILayout.PropertyField(interstitialTypeProperty);
            EditorGUILayout.PropertyField(rewardedVideoTypeProperty);

            EditorGUILayout.EndVertical();

            GUILayout.Space(8);

            EditorGUILayout.BeginVertical(EditorStylesExtended.editorSkin.box);

            EditorGUILayoutCustom.Header("SETTINGS");

            EditorGUILayout.PropertyField(testModeProperty);
            EditorGUILayout.PropertyField(systemLogsProperty);

            GUILayout.Space(5);

            foreach (var prop in adsFrequencyContainerProperties) EditorGUILayout.PropertyField(prop);

            EditorGUILayout.EndVertical();

            GUILayout.Space(8);

            EditorGUILayout.BeginVertical(EditorStylesExtended.editorSkin.box);

            gdprContainerProperty.isExpanded = EditorGUILayoutCustom.HeaderExpand("GDPR",
                gdprContainerProperty.isExpanded, arrowUpContent, arrowDownContent);

            if (gdprContainerProperty.isExpanded)
                foreach (var prop in gdprContainerProperties)
                    EditorGUILayout.PropertyField(prop);

            EditorGUILayout.EndVertical();

            GUILayout.Space(8);

            for (var i = 0; i < adsContainers.Length; i++) adsContainers[i].DrawContainer();

            serializedObject.ApplyModifiedProperties();
        }

        private abstract class AdsContainer
        {
            private readonly string containerName;
            private IEnumerable<SerializedProperty> containerProperties;
            private SerializedProperty containerProperty;
            private readonly string defineName;

            private bool isDefineEnabled;
            private readonly string propertyName;

            public AdsContainer(string containerName, string propertyName, string defineName)
            {
                this.containerName = containerName;
                this.propertyName = propertyName;
                this.defineName = defineName;

                if (string.IsNullOrEmpty(defineName))
                    isDefineEnabled = true;
            }

            public void Initialize(SerializedObject serializedObject)
            {
                containerProperty = serializedObject.FindProperty(propertyName);
                containerProperties = containerProperty.GetChildren();

                InititalizeDefine();
            }

            public void InititalizeDefine()
            {
                if (!string.IsNullOrEmpty(defineName))
                    isDefineEnabled = DefineManager.HasDefine(defineName);
            }

            public void DrawContainer()
            {
                EditorGUILayout.BeginVertical(EditorStylesExtended.editorSkin.box);

                containerProperty.isExpanded = EditorGUILayoutCustom.HeaderExpand(containerName,
                    containerProperty.isExpanded, arrowUpContent, arrowDownContent);

                if (containerProperty.isExpanded)
                {
                    if (!isDefineEnabled)
                    {
                        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                        EditorGUILayout.LabelField(EditorGUIUtility.IconContent("console.warnicon"),
                            EditorStylesExtended.padding00, GUILayout.Width(16), GUILayout.Height(16));
                        EditorGUILayout.LabelField(containerName + " define isn't enabled!");
                        if (GUILayout.Button("Enable", EditorStyles.miniButton))
                        {
                            DefineManager.EnableDefine(defineName);

                            InititalizeDefine();
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    foreach (var prop in containerProperties) EditorGUILayout.PropertyField(prop);

                    SpecialButtons();
                }

                EditorGUILayout.EndVertical();
            }

            protected abstract void SpecialButtons();
        }

        private class AdMobContainer : AdsContainer
        {
            public AdMobContainer(string containerName, string propertyName, string defineName) : base(containerName,
                propertyName, defineName)
            {
            }

            protected override void SpecialButtons()
            {
                GUILayout.Space(8);

                if (GUILayout.Button("Download AdMob plugin", EditorStylesExtended.button_01))
                    Application.OpenURL(@"https://github.com/googleads/googleads-mobile-unity/releases");

                if (GUILayout.Button("AdMob Dashboard", EditorStylesExtended.button_01))
                    Application.OpenURL(@"https://apps.admob.com/v2/home");

                if (GUILayout.Button("AdMob Quick Start Guide", EditorStylesExtended.button_01))
                    Application.OpenURL(@"https://developers.google.com/admob/unity/start");

                GUILayout.Space(8);

                EditorGUILayout.HelpBox("Tested with AdMob SDK v5.2.0", MessageType.Info);
            }
        }

        private class UnityAdsContainer : AdsContainer
        {
            public UnityAdsContainer(string containerName, string propertyName, string defineName) : base(containerName,
                propertyName, defineName)
            {
            }

            protected override void SpecialButtons()
            {
                GUILayout.Space(8);

                if (GUILayout.Button("Unity Ads Dashboard", EditorStylesExtended.button_01))
                    Application.OpenURL(@"https://operate.dashboard.unity3d.com");

                if (GUILayout.Button("Unity Ads Quick Start Guide", EditorStylesExtended.button_01))
                    Application.OpenURL(@"https://unityads.unity3d.com/help/monetization/getting-started");

                GUILayout.Space(8);

                EditorGUILayout.HelpBox("Tested with Advertisement v3.4.7", MessageType.Info);
            }
        }

        private class DummyContainer : AdsContainer
        {
            public DummyContainer(string containerName, string propertyName, string defineName) : base(containerName,
                propertyName, defineName)
            {
            }

            protected override void SpecialButtons()
            {
            }
        }
    }
}