using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Watermelon
{
    public class SetupGuideWindow : WatermelonWindow
    {
        private const string PREFS_NAME = "ShowSetupGuideOnStartup";

        private static readonly Vector2 WINDOW_SIZE = new(490, 495);
        private static readonly string WINDOW_TITLE = "Setup Guide";

        private static SetupGuideWindow setupWindow;
        private static TabContainer[] tabContainers;
        private static GUIContent[] tabs;

        private int currentTab;

        private Vector2 scrollView;

        protected override void OnEnable()
        {
            base.OnEnable();

            setupWindow = this;

            // Tabs
            var tabsList = new List<TabContainer>();

            var gameTypes = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var tempTypes = assembly.GetTypes().Where(m => m.IsDefined(typeof(SetupTabAttribute), true)).ToArray();
                if (!tempTypes.IsNullOrEmpty())
                    gameTypes.AddRange(tempTypes);
            }

            foreach (var type in gameTypes)
            {
                //Get attribute
                var tabAttributes =
                    (SetupTabAttribute[]) Attribute.GetCustomAttributes(type, typeof(SetupTabAttribute));

                for (var i = 0; i < tabAttributes.Length; i++)
                {
                    var tabObject = EditorUtils.GetAsset(type);
                    if (tabObject != null)
                        tabsList.Add(new TabContainer(tabAttributes[i].tabName, tabAttributes[i].texture, tabObject,
                            tabAttributes[i].showScrollView, tabAttributes[i].priority));
                }
            }

            tabContainers = tabsList.OrderBy(x => x.priority).ToArray();

            ForceInitStyles();
        }

        private void OnDisable()
        {
            for (var i = 0; i < tabContainers.Length; i++) tabContainers[i].Destroy();
        }

        private void OnGUI()
        {
            return;
            InitStyles();

            EditorGUILayout.BeginVertical();

            var tempTab = GUILayout.Toolbar(currentTab, tabs, EditorStylesExtended.tab, GUILayout.Height(30));
            if (tempTab != currentTab)
            {
                currentTab = tempTab;

                scrollView = Vector2.zero;

                GUI.FocusControl(null);
            }

            if (tabContainers[currentTab].showScrollView)
                scrollView = EditorGUILayoutCustom.BeginScrollView(scrollView);

            tabContainers[currentTab].DrawTab();

            if (tabContainers[currentTab].showScrollView)
                EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }

        // todo setup guide window
        // [MenuItem("Tools/Project Setup Guide")]
        // [MenuItem("Window/Project Setup Guide")]
        private static void ShowWindow()
        {
            var tempWindow = (SetupGuideWindow) GetWindow(typeof(SetupGuideWindow), false, WINDOW_TITLE);
            tempWindow.minSize = WINDOW_SIZE;
            tempWindow.titleContent = new GUIContent(WINDOW_TITLE,
                EditorStylesExtended.GetTexture("icon_title", EditorStylesExtended.IconColor));

            setupWindow = tempWindow;

            EditorStylesExtended.InitializeStyles();
        }

        protected override void Styles()
        {
            titleContent = new GUIContent(WINDOW_TITLE,
                EditorStylesExtended.GetTexture("icon_title", EditorStylesExtended.IconColor));

            tabs = new GUIContent[tabContainers.Length];
            for (var i = 0; i < tabs.Length; i++)
            {
                Texture tabTexture = null;

                if (!string.IsNullOrEmpty(tabContainers[i].texture))
                    tabTexture =
                        EditorStylesExtended.GetTexture(tabContainers[i].texture, EditorStylesExtended.IconColor);

                if (!string.IsNullOrEmpty(tabContainers[i].name))
                    tabs[i] = new GUIContent(tabTexture, tabContainers[i].name);
                else
                    tabs[i] = new GUIContent(tabTexture);
            }
        }

        [InitializeOnLoadMethod]
        public static void SetupGuideStartup()
        {
            if (EditorPrefs.HasKey("ShowStartupGuide"))
                return;

            EditorApplication.delayCall += ShowWindow;

            EditorPrefs.SetBool("ShowStartupGuide", true);

            EditorApplication.quitting += delegate { EditorPrefs.DeleteKey("ShowStartupGuide"); };
        }

        public static void RepaintWindow()
        {
            if (setupWindow != null)
                setupWindow.Repaint();
        }

        private class TabContainer
        {
            public delegate void DrawTabDelegate();

            private readonly DrawTabDelegate drawTabFunction;
            public readonly string name;
            public readonly int priority;
            public readonly bool showScrollView;
            private readonly Editor tabEditor;

            private Object tabObject;
            public readonly string texture;

            public TabContainer(string name, string texture, DrawTabDelegate drawTabFunction,
                bool showScrollView = true, int priority = int.MaxValue)
            {
                this.name = name;
                this.texture = texture;
                this.drawTabFunction = drawTabFunction;
                this.priority = priority;
                this.showScrollView = showScrollView;
            }

            public TabContainer(string name, string texture, Object tabObject, bool showScrollView = true,
                int priority = int.MaxValue)
            {
                this.name = name;
                this.texture = texture;
                this.tabObject = tabObject;
                this.priority = priority;
                this.showScrollView = showScrollView;

                Editor.CreateCachedEditor(tabObject, null, ref tabEditor);
            }

            public void DrawTab()
            {
                if (tabEditor)
                {
                    tabEditor.serializedObject.Update();
                    tabEditor.OnInspectorGUI();
                    tabEditor.serializedObject.ApplyModifiedProperties();
                }
                else
                {
                    if (drawTabFunction != null) drawTabFunction.Invoke();
                }
            }

            public void Destroy()
            {
                if (tabEditor != null)
                    DestroyImmediate(tabEditor);
            }
        }
    }
}