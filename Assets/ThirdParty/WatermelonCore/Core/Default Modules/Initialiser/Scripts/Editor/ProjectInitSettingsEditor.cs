using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [CustomEditor(typeof(ProjectInitSettings))]
    public class ProjectInitSettingsEditor : WatermelonEditor
    {
        private readonly string INIT_MODULES_PROPERTY_NAME = "initModules";
        private readonly string MODULE_NAME_PROPERTY_NAME = "moduleName";

        private GUIContent addButton;

        private Type[] allowedTypes;

        private GUIStyle arrowButtonStyle;
        private GUIContent arrowDownContent;
        private GUIContent arrowUpContent;
        private InitModuleContainer[] initModulesEditors;

        private SerializedProperty initModulesProperty;

        private GenericMenu modulesGenericMenu;

        protected override void OnEnable()
        {
            base.OnEnable();

            initModulesProperty = serializedObject.FindProperty(INIT_MODULES_PROPERTY_NAME);

            //Load all modules
            allowedTypes = Assembly.GetAssembly(typeof(InitModule)).GetTypes().Where(type =>
                type.IsClass && !type.IsAbstract &&
                (type.IsSubclassOf(typeof(InitModule)) || type.Equals(typeof(InitModule)))).ToArray();
            modulesGenericMenu = new GenericMenu();

            for (var i = 0; i < allowedTypes.Length; i++)
            {
                var tempType = allowedTypes[i];

                modulesGenericMenu.AddItem(
                    new GUIContent(Regex
                        .Replace(tempType.Name.Replace("Manager", " Manager"), "([a-z]) ?([A-Z])", "$1 $2")
                        .Replace("Init Module", "")), false, delegate { AddModule(tempType); });
            }

            LoadEditorsList();

            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= LogPlayModeState;
        }

        private void OnDestroy()
        {
            if (initModulesEditors != null)
            {
                // Destroy old editors
                for (var i = 0; i < initModulesEditors.Length; i++)
                    if (initModulesEditors[i] != null && initModulesEditors[i].editor != null)
                        DestroyImmediate(initModulesEditors[i].editor);

                initModulesEditors = null;
            }
        }

        private void LogPlayModeState(PlayModeStateChange obj)
        {
            if (Selection.activeObject == target)
                Selection.activeObject = null;
        }

        protected override void Styles()
        {
            addButton = new GUIContent("", EditorStylesExtended.GetTexture("icon_add", EditorStylesExtended.IconColor));

            arrowDownContent =
                new GUIContent(EditorStylesExtended.GetTexture("icon_arrow_down", new Color(0.2f, 0.2f, 0.2f)));
            arrowUpContent =
                new GUIContent(EditorStylesExtended.GetTexture("icon_arrow_up", new Color(0.2f, 0.2f, 0.2f)));

            arrowButtonStyle = new GUIStyle(EditorStylesExtended.padding00);
        }

        private void LoadEditorsList()
        {
            if (initModulesEditors != null)
                // Destroy old editors
                for (var i = 0; i < initModulesEditors.Length; i++)
                    if (initModulesEditors[i] != null && initModulesEditors[i].editor != null)
                        DestroyImmediate(initModulesEditors[i].editor);

            var initModulesArraySize = initModulesProperty.arraySize;
            initModulesEditors = new InitModuleContainer[initModulesArraySize];
            for (var i = 0; i < initModulesArraySize; i++)
            {
                var initModule = initModulesProperty.GetArrayElementAtIndex(i);

                if (initModule.objectReferenceValue != null)
                {
                    var initModuleSerializedObject = new SerializedObject(initModule.objectReferenceValue);

                    initModulesEditors[i] = new InitModuleContainer(initModuleSerializedObject,
                        CreateEditor(initModuleSerializedObject.targetObject));
                }
            }
        }

        public override void OnInspectorGUI()
        {
            InitStyles();

            var projectModulesRect = EditorGUILayout.BeginVertical(EditorStylesExtended.editorSkin.box);

            EditorGUILayoutCustom.Header("PROJECT MODULES");

            var initModulesArraySize = initModulesProperty.arraySize;
            for (var i = 0; i < initModulesArraySize; i++)
            {
                var index = i;
                var initModule = initModulesProperty.GetArrayElementAtIndex(i);

                if (initModule.objectReferenceValue != null)
                {
                    var moduleSeializedObject = new SerializedObject(initModule.objectReferenceValue);

                    moduleSeializedObject.Update();

                    var moduleNameProperty = moduleSeializedObject.FindProperty(MODULE_NAME_PROPERTY_NAME);

                    EditorGUILayout.BeginVertical(EditorStylesExtended.editorSkin.box);

                    var moduleRect = EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(EditorStylesExtended.ICON_SPACE + moduleNameProperty.stringValue,
                        EditorStylesExtended.label_medium);
                    EditorGUILayout.EndHorizontal();

                    if (initModule.isExpanded)
                    {
                        if (initModulesEditors[index] != null && initModulesEditors[index].editor != null)
                            initModulesEditors[index].editor.OnInspectorGUI();

                        GUILayout.Space(10);

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();

                        if (initModulesEditors[index].isModuleInitEditor)
                            initModulesEditors[index].initModuleEditor.Buttons();

                        if (GUILayout.Button("Remove", GUILayout.Width(90)))
                        {
                            if (EditorUtility.DisplayDialog("This object will be removed!", "Are you sure?", "Remove",
                                "Cancel"))
                            {
                                var removedObject = initModule.objectReferenceValue;

                                initModulesProperty.RemoveFromObjectArrayAt(index);

                                AssetDatabase.RemoveObjectFromAsset(removedObject);
                                DestroyImmediate(removedObject, true);
                            }

                            GUIUtility.ExitGUI();
                            Event.current.Use();

                            return;
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndVertical();

                    if (GUI.Button(new Rect(moduleRect.x + moduleRect.width - 15, moduleRect.y, 12, 12), arrowUpContent,
                        arrowButtonStyle))
                        if (i > 0)
                        {
                            var expandState = initModulesProperty.GetArrayElementAtIndex(index - 1).isExpanded;

                            var tempInitModuleContainer = initModulesEditors[index];
                            initModulesEditors[index] = initModulesEditors[index - 1];
                            initModulesEditors[index - 1] = tempInitModuleContainer;

                            initModulesProperty.MoveArrayElement(index, index - 1);
                            serializedObject.ApplyModifiedProperties();

                            initModulesProperty.GetArrayElementAtIndex(index - 1).isExpanded = initModule.isExpanded;
                            initModulesProperty.GetArrayElementAtIndex(index).isExpanded = expandState;
                        }

                    if (GUI.Button(new Rect(moduleRect.x + moduleRect.width - 15, moduleRect.y + 12, 12, 12),
                        arrowDownContent, arrowButtonStyle))
                        if (i + 1 < initModulesArraySize)
                        {
                            var expandState = initModulesProperty.GetArrayElementAtIndex(index + 1).isExpanded;

                            var tempInitModuleContainer = initModulesEditors[index];
                            initModulesEditors[index] = initModulesEditors[index + 1];
                            initModulesEditors[index + 1] = tempInitModuleContainer;

                            initModulesProperty.MoveArrayElement(index, index + 1);
                            serializedObject.ApplyModifiedProperties();

                            initModulesProperty.GetArrayElementAtIndex(index + 1).isExpanded = initModule.isExpanded;
                            initModulesProperty.GetArrayElementAtIndex(index).isExpanded = expandState;
                        }

                    if (GUI.Button(moduleRect, GUIContent.none, GUIStyle.none))
                        initModule.isExpanded = !initModule.isExpanded;

                    moduleSeializedObject.ApplyModifiedProperties();
                }
                else
                {
                    EditorGUILayout.BeginHorizontal(EditorStylesExtended.editorSkin.box);
                    EditorGUILayout.BeginHorizontal(EditorStylesExtended.padding00);
                    EditorGUILayout.LabelField(EditorGUIUtility.IconContent("console.warnicon"),
                        EditorStylesExtended.padding00, GUILayout.Width(16), GUILayout.Height(16));
                    EditorGUILayout.LabelField("Object referenct is null");
                    if (GUILayout.Button("Remove", EditorStyles.miniButton))
                    {
                        initModulesProperty.RemoveFromObjectArrayAt(index);

                        GUIUtility.ExitGUI();
                        Event.current.Use();

                        return;
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndHorizontal();
                }
            }

            // Buttons panel
            var buttonsPanelRect = new Rect(projectModulesRect.x + projectModulesRect.width - 40,
                projectModulesRect.y + projectModulesRect.height, 30, 20);
            var addButtonRect = new Rect(buttonsPanelRect.x + 5, buttonsPanelRect.y, 20, 20);

            GUI.Box(buttonsPanelRect, "", EditorStylesExtended.panelBottom);
            GUI.Label(addButtonRect, addButton, EditorStylesExtended.labelCentered);

            if (GUI.Button(buttonsPanelRect, GUIContent.none, GUIStyle.none)) modulesGenericMenu.ShowAsContext();

            EditorGUILayout.EndVertical();

            GUILayout.Space(90);
        }

        public void AddModule(Type moduleType)
        {
            if (!moduleType.IsSubclassOf(typeof(InitModule)))
            {
                Debug.LogError("[Initialiser]: Module type should be subclass of InitModule class!");

                return;
            }

            serializedObject.Update();

            initModulesProperty.arraySize++;

            var testInitModule = (InitModule) CreateInstance(moduleType);
            testInitModule.name = moduleType.ToString();
            testInitModule.hideFlags = HideFlags.HideInHierarchy;

            AssetDatabase.AddObjectToAsset(testInitModule, target);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(testInitModule));
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(target));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            initModulesProperty.GetArrayElementAtIndex(initModulesProperty.arraySize - 1).objectReferenceValue =
                testInitModule;

            serializedObject.ApplyModifiedProperties();

            LoadEditorsList();
        }

        private class InitModuleContainer
        {
            public readonly Editor editor;
            public readonly InitModuleEditor initModuleEditor;

            public readonly bool isModuleInitEditor;
            public SerializedObject serializedObject;

            public InitModuleContainer(SerializedObject serializedObject, Editor editor)
            {
                this.serializedObject = serializedObject;
                this.editor = editor;

                initModuleEditor = editor as InitModuleEditor;
                isModuleInitEditor = initModuleEditor != null;
            }
        }
    }
}

// -----------------
// Initialiser v 0.3
// -----------------