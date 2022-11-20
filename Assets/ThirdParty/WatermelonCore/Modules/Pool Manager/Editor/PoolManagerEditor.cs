#pragma warning disable 0414

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

//Pool module v 1.6.3
namespace Watermelon
{
    [CustomEditor(typeof(PoolManager))]
    internal sealed class PoolManagerEditor : WatermelonEditor
    {
        private const string POOLS_LIST_PROPERTY_NAME = "poolsList";
        private const string RENAMING_EMPTY_STRING = "[PoolManager: empty]";
        private const string EMPTY_POOL_BUILDER_NAME = "[PoolBuilder: empty]";
        private readonly GUIStyle bigHeaderStyle = new();

        private readonly GUIStyle boldStyle = new();
        private readonly GUIStyle centeredTextStyle = new();

        private Color defaultColor;
        private bool dragAndDropActive;
        private GUIStyle dragAndDropBoxStyle = new();
        private Rect dragAndDropRect = new();
        private readonly GUIStyle headerStyle = new();
        private Rect inspectorRect;
        private MethodInfo isAllPrefabsAssignedAtPoolMethodInfo;

        private bool isNameAllowed = true;
        private bool isNameAlreadyExisting;
        private bool isSettingsExpanded;
        private string lastRenamingName = string.Empty;
        private GUIContent lockedIconGUIContent;
        private readonly GUIStyle multiListLablesStyle = new();

        private PoolSettings newPoolBuilder;
        private SerializedProperty poolsListProperty;
        private string prevNewPoolName = string.Empty;
        private string prevSelectedPoolName = string.Empty;
        private MethodInfo recalculateWeightsAtPoolMethodInfo;

        private string searchText = string.Empty;

        private int selectedPoolIndex;
        private bool skipEmptyNameWarning;
        private GUIContent unlockedIconGUIContent;

        private GUIContent warningIconGUIContent;


        protected override void OnEnable()
        {
            base.OnEnable();

            poolsListProperty = serializedObject.FindProperty(POOLS_LIST_PROPERTY_NAME);
            isAllPrefabsAssignedAtPoolMethodInfo = serializedObject.targetObject.GetType()
                .GetMethod("IsAllPrefabsAssignedAtPool", BindingFlags.NonPublic | BindingFlags.Instance);
            recalculateWeightsAtPoolMethodInfo = serializedObject.targetObject.GetType()
                .GetMethod("RecalculateWeightsAtPool", BindingFlags.NonPublic | BindingFlags.Instance);

            lastRenamingName = RENAMING_EMPTY_STRING;
            isNameAllowed = true;
            isNameAlreadyExisting = false;

            selectedPoolIndex = -1;
            newPoolBuilder = new PoolSettings().SetName(EMPTY_POOL_BUILDER_NAME);

            ReloadPoolManager();
            InitStyles();

            Undo.undoRedoPerformed += UndoCallback;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= UndoCallback;
        }

        private void UndoCallback()
        {
            UpdatePools();
        }

        protected override void Styles()
        {
            boldStyle.fontStyle = FontStyle.Bold;

            headerStyle.alignment = TextAnchor.MiddleCenter;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.fontSize = 12;

            bigHeaderStyle.alignment = TextAnchor.MiddleCenter;
            bigHeaderStyle.fontStyle = FontStyle.Bold;
            bigHeaderStyle.fontSize = 14;

            centeredTextStyle.alignment = TextAnchor.MiddleCenter;

            multiListLablesStyle.fontSize = 8;
            multiListLablesStyle.normal.textColor = new Color(0.3f, 0.3f, 0.3f);

            Texture
                warningIconTexture =
                    EditorStylesExtended
                        .GetTexture(
                            "icon_warning"); // AssetDatabase.LoadAssetAtPath<Texture>(@"Assets\Project Name\Watermelon Core\Plugins\Editor\Resources\UI\Sprites\Icons\icon_warning.png");
            warningIconGUIContent = new GUIContent(warningIconTexture);


            Texture
                lockedTexture =
                    EditorStylesExtended
                        .GetTexture(
                            "icon_locked"); // AssetDatabase.LoadAssetAtPath<Texture>(@"Assets\Project Name\Watermelon Core\Plugins\Editor\Resources\UI\Sprites\Icons\icon_locked.png");
            lockedIconGUIContent = new GUIContent(lockedTexture);

            Texture
                unlockedTexture =
                    EditorStylesExtended
                        .GetTexture(
                            "icon_unlocked"); //AssetDatabase.LoadAssetAtPath<Texture>(@"Assets\Project Name\Watermelon Core\Plugins\Editor\Resources\UI\Sprites\Icons\icon_unlocked.png");
            unlockedIconGUIContent = new GUIContent(unlockedTexture);


            defaultColor = GUI.contentColor;
        }

        public override void OnInspectorGUI()
        {
            InitStyles();
            serializedObject.Update();

            if (dragAndDropActive)
            {
                dragAndDropBoxStyle = GUI.skin.box;
                dragAndDropBoxStyle.alignment = TextAnchor.MiddleCenter;
                dragAndDropBoxStyle.fontStyle = FontStyle.Bold;
                dragAndDropBoxStyle.fontSize = 12;

                GUILayout.Box("Drag objects here", dragAndDropBoxStyle,
                    GUILayout.Width(EditorGUIUtility.currentViewWidth - 21), GUILayout.Height(inspectorRect.size.y));
            }
            else
            {
                inspectorRect = EditorGUILayout.BeginVertical();


                // Control bar /////////////////////////////////////////////////////////////////////////////
                EditorGUILayout.BeginVertical(GUI.skin.box);

                // if we are not setuping a new pool now - than displaying settings interface
                if (newPoolBuilder.name.Equals(EMPTY_POOL_BUILDER_NAME))
                {
                    EditorGUI.indentLevel++;

                    isSettingsExpanded = EditorGUILayout.Foldout(isSettingsExpanded, "Settings");

                    if (isSettingsExpanded)
                    {
                        EditorGUI.BeginChangeCheck();

                        if (EditorGUI.EndChangeCheck())
                            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                        EditorGUI.BeginChangeCheck();


                        serializedObject.FindProperty("objectsContainer").objectReferenceValue =
                            (Transform) EditorGUILayout.ObjectField("Objects container: ",
                                serializedObject.FindProperty("objectsContainer").objectReferenceValue,
                                typeof(Transform), true);

                        if (EditorGUI.EndChangeCheck())
                            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

                        EditorGUILayout.Space();
                    }

                    EditorGUI.indentLevel--;


                    if (GUILayout.Button("Add pool", GUILayout.Height(30)))
                    {
                        skipEmptyNameWarning = true;
                        AddNewSinglePool();
                    }
                }

                // Pool creation bar //////////////////////////////////////////////////////////////////////////
                if (!newPoolBuilder.name.Equals(EMPTY_POOL_BUILDER_NAME))
                {
                    //EditorGUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.Space(3f);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Pool creation:", headerStyle, GUILayout.Width(100));
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Cancel", GUILayout.Width(60)))
                    {
                        CancelNewPoolCreation();

                        return;
                    }

                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(4f);

                    newPoolBuilder = DrawPool(newPoolBuilder, null, 0);

                    GUILayout.Space(5f);

                    if (GUILayout.Button("Confirm", GUILayout.Height(25)))
                    {
                        GUI.FocusControl(null);
                        ConfirmPoolCreation();

                        return;
                    }

                    GUILayout.Space(5f);
                    //EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();


                // Pools displaying region /////////////////////////////////////////////////////////////////////

                EditorGUILayout.BeginVertical();

                EditorGUILayout.LabelField("Pools list", headerStyle);

                GUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();

                // searching
                searchText = EditorGUILayout.TextField(searchText, GUI.skin.FindStyle("ToolbarSeachTextField"));

                if (!string.IsNullOrEmpty(searchText))
                {
                    if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
                    {
                        // Remove focus if cleared
                        searchText = "";
                        GUI.FocusControl(null);
                    }
                }
                else
                {
                    GUILayout.Button(GUIContent.none, GUI.skin.FindStyle("ToolbarSeachCancelButtonEmpty"));
                }

                if (EditorGUI.EndChangeCheck()) UpdatePools();
                GUILayout.EndHorizontal();

                if (poolsListProperty.arraySize == 0)
                {
                    if (string.IsNullOrEmpty(searchText))
                        EditorGUILayout.HelpBox("There's no pools.", MessageType.Info);
                    else
                        EditorGUILayout.HelpBox("Pool \"" + searchText + "\" is not found.", MessageType.Info);
                }
                else
                {
                    for (var currentPoolIndex = 0; currentPoolIndex < poolsListProperty.arraySize; currentPoolIndex++)
                    {
                        var poolProperty = poolsListProperty.GetArrayElementAtIndex(currentPoolIndex);

                        if (searchText == string.Empty || searchText != string.Empty &&
                            poolProperty.FindPropertyRelative("name").stringValue.Contains(searchText))
                        {
                            var clickRect = EditorGUILayout.BeginVertical(GUI.skin.box);
                            EditorGUI.indentLevel++;

                            if (selectedPoolIndex == -1 || currentPoolIndex != selectedPoolIndex)
                            {
                                if (selectedPoolIndex != -1) CancelNewPoolCreation();


                                if ((bool) isAllPrefabsAssignedAtPoolMethodInfo.Invoke(serializedObject.targetObject,
                                    new object[] {currentPoolIndex}))
                                {
                                    // string runtimeCreatedNameAddition = poolProperty.FindPropertyRelative("isRuntimeCreated").boolValue ? "   [Runtime]" : "";
                                    // EditorGUILayout.LabelField(GetPoolName(currentPoolIndex) + runtimeCreatedNameAddition, centeredTextStyle);
                                }
                                else
                                {
                                    EditorGUILayout.BeginHorizontal();

                                    GUI.contentColor = new Color(0.3f, 0.3f, 0.3f, 1f);
                                    EditorGUILayout.LabelField(warningIconGUIContent, GUILayout.Width(30));
                                    GUI.contentColor = defaultColor;

                                    GUILayout.Space(-35f);
                                    EditorGUILayout.LabelField(GetPoolName(currentPoolIndex), centeredTextStyle);

                                    EditorGUILayout.EndHorizontal();
                                }
                            }
                            else
                            {
                                GUILayout.Space(5);

                                // pool drawing
                                DrawPool(newPoolBuilder, poolProperty, currentPoolIndex);

                                GUILayout.Space(5);

                                if (GUILayout.Button("Delete"))
                                    if (EditorUtility.DisplayDialog("This pool will be removed!", "Are you sure?",
                                        "Remove", "Cancel"))
                                    {
                                        DeletePool(currentPoolIndex);

                                        EditorApplication.delayCall += delegate { EditorUtility.FocusProjectWindow(); };
                                    }

                                GUILayout.Space(5);
                            }

                            EditorGUI.indentLevel--;
                            EditorGUILayout.EndVertical();

                            if (GUI.Button(clickRect, GUIContent.none, GUIStyle.none))
                            {
                                GUI.FocusControl(null);

                                if (selectedPoolIndex == -1 || selectedPoolIndex != currentPoolIndex)
                                {
                                    selectedPoolIndex = currentPoolIndex;
                                    lastRenamingName = RENAMING_EMPTY_STRING;
                                    isNameAlreadyExisting = false;
                                    isNameAllowed = true;
                                    newPoolBuilder = newPoolBuilder.Reset().SetName(EMPTY_POOL_BUILDER_NAME);
                                }
                                else
                                {
                                    selectedPoolIndex = -1;
                                    lastRenamingName = RENAMING_EMPTY_STRING;
                                    isNameAlreadyExisting = false;
                                    isNameAllowed = true;
                                }
                            }
                        }
                    }
                }

                EditorGUILayout.EndVertical();

                if (GUI.changed) EditorUtility.SetDirty(target);
                EditorGUILayout.EndVertical();
            }


            serializedObject.ApplyModifiedProperties();

            // Drag n Drop region /////////////////////////////////////////////////////////////////////

            var currentEvent = Event.current;

            if (inspectorRect.Contains(currentEvent.mousePosition) && selectedPoolIndex == -1 &&
                newPoolBuilder.name.Equals(EMPTY_POOL_BUILDER_NAME) && !isSettingsExpanded)
            {
                if (currentEvent.type == EventType.DragUpdated)
                {
                    dragAndDropActive = true;
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    currentEvent.Use();
                }
                else if (currentEvent.type == EventType.DragPerform)
                {
                    dragAndDropActive = false;
                    var draggedObjects = new List<Pool.MultiPoolPrefab>();

                    foreach (var obj in DragAndDrop.objectReferences)
                        if (obj.GetType() == typeof(GameObject))
                            draggedObjects.Add(new Pool.MultiPoolPrefab(obj as GameObject, 0, false));

                    if (draggedObjects.Count == 1)
                        AddNewSinglePool(draggedObjects[0].prefab);
                    else
                        AddNewMultiPool(draggedObjects);

                    currentEvent.Use();
                }
            }
            else
            {
                if (currentEvent.type == EventType.Repaint) dragAndDropActive = false;
            }
        }


        private PoolSettings DrawPool(PoolSettings poolBuilder, SerializedProperty poolProperty, int poolIndex)
        {
            EditorGUI.BeginChangeCheck();

            // name ///////////
            var poolName = poolProperty != null
                ? poolProperty.FindPropertyRelative("name").stringValue
                : poolBuilder.name;

            GUILayout.BeginHorizontal();

            var newName = EditorGUILayout.TextField("Name: ",
                lastRenamingName != RENAMING_EMPTY_STRING ? lastRenamingName : poolName);

            if (newName == poolName && (!newPoolBuilder.name.Equals(EMPTY_POOL_BUILDER_NAME)
                ? newName.Equals(string.Empty)
                : true))
            {
                lastRenamingName = RENAMING_EMPTY_STRING;
                isNameAllowed = true;
                isNameAlreadyExisting = false;
            }

            if (!isNameAllowed || newName == string.Empty || newName != poolName ||
                lastRenamingName != RENAMING_EMPTY_STRING)
            {
                lastRenamingName = newName;
                isNameAllowed = IsNameAllowed(newName);

                EditorGUI.BeginDisabledGroup(!isNameAllowed);

                // if name is emplty or it's pool creation - do not show rename button

                if (!(!isNameAllowed && !isNameAlreadyExisting || !newPoolBuilder.name.Equals(EMPTY_POOL_BUILDER_NAME)))
                    if (GUILayout.Button("rename"))
                    {
                        RenamePool(poolProperty, poolBuilder, newName);

                        lastRenamingName = RENAMING_EMPTY_STRING;
                    }

                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();

                if (isNameAllowed)
                {
                    // [CACHE IS CURRENTLY DISABLED]
                    //if (poolManagerRef.useCache)
                    //{
                    //    RenameCachedPool(poolName, newName);
                    //}
                }
                else
                {
                    if (isNameAlreadyExisting)
                    {
                        EditorGUILayout.HelpBox("Name already exists", MessageType.Warning);
                    }
                    else
                    {
                        if (!skipEmptyNameWarning) EditorGUILayout.HelpBox("Name can't be empty", MessageType.Warning);
                    }
                }
            }
            else
            {
                EditorGUILayout.EndHorizontal();
            }

            if (!poolBuilder.name.Equals(EMPTY_POOL_BUILDER_NAME)) poolBuilder = poolBuilder.SetName(newName);


            // type ///////////
            var poolType = poolProperty != null
                ? (Pool.PoolType) poolProperty.FindPropertyRelative("type").enumValueIndex
                : poolBuilder.type;
            var currentPoolType = (Pool.PoolType) EditorGUILayout.EnumPopup("Pool type:", poolType);

            if (currentPoolType != poolType)
            {
                if (poolProperty != null)
                    poolProperty.FindPropertyRelative("type").enumValueIndex = (int) currentPoolType;
                else
                    poolBuilder = poolBuilder.SetType(currentPoolType);
            }

            // prefabs field ///////////
            if (currentPoolType == Pool.PoolType.Single)
            {
                // single prefab pool editor
                var currentPrefab = poolProperty != null
                    ? (GameObject) poolProperty.FindPropertyRelative("singlePoolPrefab").objectReferenceValue
                    : poolBuilder.singlePoolPrefab;
                var prefab =
                    (GameObject) EditorGUILayout.ObjectField("Prefab: ", currentPrefab, typeof(GameObject), false);

                if (currentPrefab != prefab)
                {
                    if (poolProperty != null)
                        poolProperty.FindPropertyRelative("singlePoolPrefab").objectReferenceValue = prefab;
                    else
                        poolBuilder = poolBuilder.SetSinglePrefab(prefab);

                    var currentName = poolProperty != null
                        ? poolProperty.FindPropertyRelative("name").stringValue
                        : poolBuilder.name;

                    if (currentName == string.Empty) RenamePool(poolProperty, poolBuilder, prefab.name);
                }
            }
            else
            {
                // multiple prefabs pool editor
                GUILayout.Space(5f);

                var currentPrefabsAmount = poolProperty != null
                    ? poolProperty.FindPropertyRelative("multiPoolPrefabsList").arraySize
                    : poolBuilder.multiPoolPrefabsList.Count;

                EditorGUILayout.BeginHorizontal();

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.IntField("Prefabs amount:", currentPrefabsAmount);
                EditorGUI.EndDisabledGroup();

                var newPrefabsAmount = currentPrefabsAmount;

                if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20)) && newPrefabsAmount > 0)
                {
                    GUI.FocusControl(null);
                    newPrefabsAmount--;
                }

                if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    GUI.FocusControl(null);
                    newPrefabsAmount++;
                }

                EditorGUILayout.EndHorizontal();

                if (newPrefabsAmount != currentPrefabsAmount)
                {
                    if (poolProperty != null)
                    {
                        poolProperty.FindPropertyRelative("multiPoolPrefabsList").arraySize = newPrefabsAmount;
                    }
                    else
                    {
                        if (newPrefabsAmount == 0)
                        {
                            poolBuilder.multiPoolPrefabsList.Clear();
                        }
                        else if (newPrefabsAmount < poolBuilder.multiPoolPrefabsList.Count)
                        {
                            var itemsToRemove = poolBuilder.multiPoolPrefabsList.Count - newPrefabsAmount;
                            poolBuilder.multiPoolPrefabsList.RemoveRange(
                                poolBuilder.multiPoolPrefabsList.Count - itemsToRemove - 1, itemsToRemove);
                        }
                        else
                        {
                            var itemsToAdd = newPrefabsAmount - poolBuilder.multiPoolPrefabsList.Count;
                            for (var j = 0; j < itemsToAdd; j++)
                                poolBuilder.multiPoolPrefabsList.Add(new Pool.MultiPoolPrefab());
                        }
                    }

                    if (poolProperty != null)
                    {
                        if (newPrefabsAmount > currentPrefabsAmount)
                            for (var i = 0; i < newPrefabsAmount - currentPrefabsAmount; i++)
                            {
                                poolProperty.FindPropertyRelative("multiPoolPrefabsList")
                                    .GetArrayElementAtIndex(currentPrefabsAmount + i).FindPropertyRelative("prefab")
                                    .objectReferenceValue = null;
                                poolProperty.FindPropertyRelative("multiPoolPrefabsList")
                                    .GetArrayElementAtIndex(currentPrefabsAmount + i).FindPropertyRelative("weight")
                                    .intValue = 0;
                                poolProperty.FindPropertyRelative("multiPoolPrefabsList")
                                    .GetArrayElementAtIndex(currentPrefabsAmount + i)
                                    .FindPropertyRelative("isWeightLocked").boolValue = false;
                            }

                        serializedObject.ApplyModifiedProperties();
                        recalculateWeightsAtPoolMethodInfo.Invoke(serializedObject.targetObject,
                            new object[] {poolIndex});
                    }
                    else
                    {
                        poolBuilder.RecalculateWeights();
                    }

                    currentPrefabsAmount = newPrefabsAmount;
                }

                // prefabs list
                GUILayout.Space(-2f);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("objects", multiListLablesStyle, GUILayout.MaxHeight(10f));
                GUILayout.Space(-25);
                EditorGUILayout.LabelField("weights", multiListLablesStyle, GUILayout.Width(75),
                    GUILayout.MaxHeight(10f));
                EditorGUILayout.EndHorizontal();
                var weightsSum = 0f;

                for (var j = 0; j < currentPrefabsAmount; j++)
                {
                    EditorGUILayout.BeginHorizontal();

                    // object 
                    var currentPrefab = poolProperty != null
                        ? (GameObject) poolProperty.FindPropertyRelative("multiPoolPrefabsList")
                            .GetArrayElementAtIndex(j).FindPropertyRelative("prefab").objectReferenceValue
                        : poolBuilder.multiPoolPrefabsList[j].prefab;
                    var newPrefab = (GameObject) EditorGUILayout.ObjectField(currentPrefab, typeof(GameObject), true);

                    if (newPrefab != currentPrefab)
                    {
                        if (poolProperty != null)
                            poolProperty.FindPropertyRelative("multiPoolPrefabsList").GetArrayElementAtIndex(j)
                                .FindPropertyRelative("prefab").objectReferenceValue = newPrefab;
                        else
                            poolBuilder.multiPoolPrefabsList[j] = new Pool.MultiPoolPrefab(newPrefab,
                                poolBuilder.multiPoolPrefabsList[j].weight,
                                poolBuilder.multiPoolPrefabsList[j].isWeightLocked);
                    }

                    // weight
                    var isWeightLocked = poolProperty != null
                        ? poolProperty.FindPropertyRelative("multiPoolPrefabsList").GetArrayElementAtIndex(j)
                            .FindPropertyRelative("isWeightLocked").boolValue
                        : poolBuilder.multiPoolPrefabsList[j].isWeightLocked;
                    EditorGUI.BeginDisabledGroup(isWeightLocked);

                    var currentWeight = poolProperty != null
                        ? poolProperty.FindPropertyRelative("multiPoolPrefabsList").GetArrayElementAtIndex(j)
                            .FindPropertyRelative("weight").intValue
                        : poolBuilder.multiPoolPrefabsList[j].weight;


                    var newWeight = EditorGUILayout.DelayedIntField(Math.Abs(currentWeight), GUILayout.Width(75));
                    if (newWeight != currentWeight)
                    {
                        if (poolProperty != null)
                            poolProperty.FindPropertyRelative("multiPoolPrefabsList").GetArrayElementAtIndex(j)
                                .FindPropertyRelative("weight").intValue = newWeight;
                        else
                            poolBuilder.multiPoolPrefabsList[j] = new Pool.MultiPoolPrefab(newPrefab, newWeight,
                                poolBuilder.multiPoolPrefabsList[j].isWeightLocked);
                    }

                    EditorGUI.EndDisabledGroup();

                    weightsSum += newWeight;

                    // lock
                    GUI.contentColor = new Color(0.3f, 0.3f, 0.3f, 1f);
                    if (GUILayout.Button(isWeightLocked ? lockedIconGUIContent : unlockedIconGUIContent,
                        centeredTextStyle, GUILayout.Height(13f), GUILayout.Width(13f)))
                    {
                        GUI.FocusControl(null);

                        if (poolProperty != null)
                            poolProperty.FindPropertyRelative("multiPoolPrefabsList").GetArrayElementAtIndex(j)
                                .FindPropertyRelative("isWeightLocked").boolValue = !isWeightLocked;
                        else
                            poolBuilder.multiPoolPrefabsList[j] = new Pool.MultiPoolPrefab(newPrefab,
                                poolBuilder.multiPoolPrefabsList[j].weight,
                                !poolBuilder.multiPoolPrefabsList[j].isWeightLocked);
                    }

                    GUI.contentColor = defaultColor;

                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(5f);

                if (currentPrefabsAmount != 0 && weightsSum != 100)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.HelpBox("Weights sum should be 100 (current " + weightsSum + ").",
                        MessageType.Warning);

                    if (GUILayout.Button("Recalculate", GUILayout.Height(40f), GUILayout.Width(76)))
                    {
                        GUI.FocusControl(null);

                        recalculateWeightsAtPoolMethodInfo.Invoke(serializedObject.targetObject,
                            new object[] {poolIndex});

                        // pool.RecalculateWeights();
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            if (!(bool) isAllPrefabsAssignedAtPoolMethodInfo.Invoke(serializedObject.targetObject,
                    new object[] {poolIndex}) &&
                newPoolBuilder.name.Equals(EMPTY_POOL_BUILDER_NAME))
                EditorGUILayout.HelpBox("Please assign all prefabs references.", MessageType.Warning);


            // pool size ///////////
            var currentSize = poolProperty != null
                ? poolProperty.FindPropertyRelative("size").intValue
                : poolBuilder.poolSize;

            if (currentPoolType == Pool.PoolType.Single)
            {
                var newSize = EditorGUILayout.IntField("Pool size: ", currentSize);

                if (poolProperty != null)
                    poolProperty.FindPropertyRelative("size").intValue = newSize;
                else
                    poolBuilder.poolSize = newSize;
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                var newSize = EditorGUILayout.IntField("Pool size: ", currentSize);

                newSize = newSize >= 0 ? newSize : 0;

                if (poolProperty != null)
                    poolProperty.FindPropertyRelative("size").intValue = newSize;
                else
                    poolBuilder.poolSize = newSize;

                GUILayout.FlexibleSpace();

                var multiPrefabsAmount = poolProperty != null
                    ?
                    poolProperty.FindPropertyRelative("multiPoolPrefabsList").arraySize
                    : poolBuilder.multiPoolPrefabsList != null
                        ? poolBuilder.multiPoolPrefabsList.Count
                        : 0;
                var lableString = " x " + multiPrefabsAmount + " = " + newSize * multiPrefabsAmount;
                GUILayout.Space(-18);
                EditorGUILayout.LabelField(lableString);

                EditorGUILayout.EndHorizontal();
            }

            // [CACHE IS CURRENTLY DISABLED]
            //if (poolManagerRef.useCache && currentSize != poolBuilder.size)
            //{
            //    UpdateCacheStateList();
            //}

            // auto size increment toggle ///////////
            var currentAutoSizeIncrementState = poolProperty != null
                ? poolProperty.FindPropertyRelative("autoSizeIncrement").boolValue
                : poolBuilder.autoSizeIncrement;
            var newAutoSizeIncrementState = EditorGUILayout.Toggle("Will grow: ", currentAutoSizeIncrementState);

            if (poolProperty != null)
                poolProperty.FindPropertyRelative("autoSizeIncrement").boolValue = newAutoSizeIncrementState;
            else
                poolBuilder.autoSizeIncrement = newAutoSizeIncrementState;

            // objects parrent ///////////
            var currentContainer = poolProperty != null
                ? (Transform) poolProperty.FindPropertyRelative("objectsContainer").objectReferenceValue
                : poolBuilder.objectsContainer;
            var newContainer =
                (Transform) EditorGUILayout.ObjectField("Objects parrent", currentContainer, typeof(Transform), true);

            if (poolProperty != null)
                poolProperty.FindPropertyRelative("objectsContainer").objectReferenceValue = newContainer;
            else
                poolBuilder.objectsContainer = newContainer;


            if (EditorGUI.EndChangeCheck()) EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            return poolBuilder;
        }

        private void RenamePool(SerializedProperty poolProperty, PoolSettings poolBuilder, string newName)
        {
            if (poolProperty != null)
            {
                poolProperty.FindPropertyRelative("name").stringValue = newName;
                serializedObject.ApplyModifiedProperties();

                // sorting pools list

                var newIndex = -1;
                var oldIndex = -1;

                for (var i = 0; i < poolsListProperty.arraySize; i++)
                {
                    var comparingResult = newName.CompareTo(poolsListProperty.GetArrayElementAtIndex(i)
                        .FindPropertyRelative("name").stringValue);

                    if (newIndex == -1 && comparingResult == -1)
                    {
                        newIndex = i;

                        if (oldIndex != -1)
                            break;
                    }

                    if (comparingResult == 0)
                    {
                        oldIndex = i;

                        if (newIndex != -1)
                            break;
                    }
                }

                if (newIndex == -1) newIndex = poolsListProperty.arraySize - 1;

                selectedPoolIndex = newIndex;
                poolsListProperty.MoveArrayElement(oldIndex, newIndex);
                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                poolBuilder = poolBuilder.SetName(newName);
            }
        }

        private void CancelNewPoolCreation()
        {
            if (newPoolBuilder.name.Equals(EMPTY_POOL_BUILDER_NAME))
                return;

            newPoolBuilder = newPoolBuilder.Reset().SetName(EMPTY_POOL_BUILDER_NAME);
            lastRenamingName = RENAMING_EMPTY_STRING;
            isNameAllowed = true;
            isNameAlreadyExisting = false;
            skipEmptyNameWarning = false;
        }

        private string GetPoolName(int poolIndex)
        {
            var poolName = poolsListProperty.GetArrayElementAtIndex(poolIndex).FindPropertyRelative("name").stringValue;
            //poolsList[poolIndex].Name;

            // [CACHE IS CURRENTLY DISABLED]
            //if (poolManagerRef.useCache)
            //{
            //    if (poolsCacheList.IsNullOrEmpty() || poolsCacheDeltaList.IsNullOrEmpty() || poolIndex > poolsCacheDeltaList.Count || poolIndex > poolsCacheList.Count)
            //    {
            //        UpdateCacheStateList();
            //    }

            //    // there is not cache for current scene returning
            //    if (poolsCacheList.IsNullOrEmpty())
            //    {
            //        return poolName;
            //    }

            //    int delta = poolsCacheDeltaList[poolIndex];

            //    if (poolsCacheList[poolIndex] != null && poolsCacheList[poolIndex].ignoreCache)
            //    {
            //        poolName += "   [cache ignored]";
            //    }
            //    else if (delta != 0)
            //    {
            //        poolName += "   " + CacheDeltaToState(delta);
            //    }
            //}

            return poolName;
        }

        private void AddNewSinglePool(GameObject prefab = null)
        {
            selectedPoolIndex = -1;

            newPoolBuilder = new PoolSettings(prefab != null ? prefab.name : string.Empty, prefab, 10, true);
            IsNameAllowed(newPoolBuilder.name);
        }

        private void AddNewMultiPool(List<Pool.MultiPoolPrefab> prefabs = null)
        {
            selectedPoolIndex = -1;

            var name = prefabs != null && prefabs.Count != 0 ? prefabs[0].prefab.name : string.Empty;
            newPoolBuilder = new PoolSettings(name, prefabs, 10, true);

            IsNameAllowed(newPoolBuilder.name);
        }

        private void ConfirmPoolCreation()
        {
            skipEmptyNameWarning = false;

            if (IsNameAllowed(newPoolBuilder.name))
            {
                Undo.RecordObject(target, "New pool added");

                var poolsAmount = serializedObject.FindProperty("poolsList").arraySize;
                poolsAmount++;
                serializedObject.FindProperty("poolsList").arraySize = poolsAmount;

                var newPoolProperty =
                    serializedObject.FindProperty("poolsList").GetArrayElementAtIndex(poolsAmount - 1);

                newPoolProperty.FindPropertyRelative("name").stringValue = newPoolBuilder.name;
                newPoolProperty.FindPropertyRelative("type").enumValueIndex = (int) newPoolBuilder.type;
                newPoolProperty.FindPropertyRelative("singlePoolPrefab").objectReferenceValue =
                    newPoolBuilder.singlePoolPrefab;
                newPoolProperty.FindPropertyRelative("multiPoolPrefabsList").arraySize =
                    newPoolBuilder.multiPoolPrefabsList.Count;

                for (var i = 0; i < newPoolBuilder.multiPoolPrefabsList.Count; i++)
                {
                    newPoolProperty.FindPropertyRelative("multiPoolPrefabsList").GetArrayElementAtIndex(i)
                            .FindPropertyRelative("prefab").objectReferenceValue =
                        newPoolBuilder.multiPoolPrefabsList[i].prefab;
                    newPoolProperty.FindPropertyRelative("multiPoolPrefabsList").GetArrayElementAtIndex(i)
                        .FindPropertyRelative("weight").intValue = newPoolBuilder.multiPoolPrefabsList[i].weight;
                    newPoolProperty.FindPropertyRelative("multiPoolPrefabsList").GetArrayElementAtIndex(i)
                            .FindPropertyRelative("isWeightLocked").boolValue =
                        newPoolBuilder.multiPoolPrefabsList[i].isWeightLocked;
                }

                newPoolProperty.FindPropertyRelative("size").intValue = newPoolBuilder.poolSize;
                newPoolProperty.FindPropertyRelative("autoSizeIncrement").boolValue = newPoolBuilder.autoSizeIncrement;
                newPoolProperty.FindPropertyRelative("objectsContainer").objectReferenceValue =
                    newPoolBuilder.objectsContainer;

                serializedObject.ApplyModifiedProperties();

                recalculateWeightsAtPoolMethodInfo.Invoke(serializedObject.targetObject,
                    new object[] {poolsAmount - 1});

                for (var i = 0; i < poolsListProperty.arraySize; i++)
                    if (poolsListProperty.GetArrayElementAtIndex(poolsAmount - 1).FindPropertyRelative("name")
                        .stringValue.CompareTo(poolsListProperty.GetArrayElementAtIndex(i).FindPropertyRelative("name")
                            .stringValue) == -1)
                    {
                        poolsListProperty.MoveArrayElement(poolsAmount - 1, i);
                        break;
                    }

                serializedObject.ApplyModifiedProperties();

                // [CACHE IS CURRENTLY DISABLED]
                //if (poolManagerRef.useCache)
                //{
                //    poolManagerRef.SaveCache();
                //}

                ReloadPoolManager(true);
                newPoolBuilder = newPoolBuilder.Reset().SetName(EMPTY_POOL_BUILDER_NAME);
                prevNewPoolName = string.Empty;

                lastRenamingName = RENAMING_EMPTY_STRING;
                isNameAllowed = true;
                isNameAlreadyExisting = false;

                searchText = "";
            }
        }

        private void DeletePool(int indexOfPoolToRemove)
        {
            Undo.RecordObject(target, "Pool deleted");

            serializedObject.FindProperty("poolsList").RemoveFromVariableArrayAt(indexOfPoolToRemove);

            selectedPoolIndex = -1;
            lastRenamingName = RENAMING_EMPTY_STRING;
            isNameAllowed = true;
            isNameAlreadyExisting = false;

            ReloadPoolManager();
        }

        private bool IsNameAllowed(string nameToCheck)
        {
            if (nameToCheck.Equals(string.Empty))
            {
                isNameAllowed = false;
                isNameAlreadyExisting = false;
                return false;
            }

            if (serializedObject.FindProperty("poolsList").arraySize == 0)
            {
                isNameAllowed = true;
                isNameAlreadyExisting = false;
                return true;
            }

            if (IsNameAlreadyExisting(nameToCheck))
            {
                isNameAllowed = false;
                isNameAlreadyExisting = true;
                return false;
            }

            isNameAllowed = true;
            isNameAlreadyExisting = false;
            return true;
        }

        private bool IsNameAlreadyExisting(string nameToCheck)
        {
            for (var i = 0; i < poolsListProperty.arraySize; i++)
                if (poolsListProperty.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue
                    .Equals(nameToCheck))
                    return true;

            return false;
        }

        private void ReloadPoolManager(bool sortPool = false)
        {
            //poolsList.Clear();

            UpdatePools(sortPool);

            UpdateCacheStateList();
        }

        private void UpdatePools(bool needToSort = false)
        {
            if (needToSort)
            {
                //poolManagerRef.poolsList.Sort((x, y) => x.Name.CompareTo(y.Name));
            }

            //if (poolManagerRef.poolsList != null)
            //{
            //    poolsList = poolManagerRef.poolsList.FindAll(x => x.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
            //}
        }

        #region Cache management

        private void ApplyCache()
        {
            // [CACHE IS CURRENTLY DISABLED]
            //List<PoolCache> currentLevelCache = LoadCurrentCache();

            //if (!currentLevelCache.IsNullOrEmpty())
            //{
            //    Undo.RecordObject(target, "Apply cache");

            //    for (int i = 0; i < poolManagerRef.poolsList.Count; i++)
            //    {
            //        int index = currentLevelCache.FindIndex(x => x.poolName == poolManagerRef.poolsList[i].Name);
            //        if (index != -1 && !currentLevelCache[index].ignoreCache)
            //        {
            //            //poolManagerRef.pools[i].Size = currentLevelCache[index].poolSize;
            //        }
            //    }

            //    ClearObsoleteCache();
            //    UpdateCacheStateList();
            //}
            //else
            //{
            //    Debug.Log("[PoolManager] There's no saved cache for current scene.");
            //}
        }

        private void DisplayCacheState()
        {
            // [CACHE IS CURRENTLY DISABLED]
            //List<PoolCache> currentLevelCache = LoadCurrentCache();

            //if (!currentLevelCache.IsNullOrEmpty())
            //{
            //    List<PoolCache> cacheToDelete = new List<PoolCache>();

            //    string cacheInfo = string.Empty;
            //    foreach (PoolCache poolCache in currentLevelCache)
            //    {
            //        // if pool not exists - delete it's cache
            //        int index = poolManagerRef.poolsList.FindIndex(x => x.Name == poolCache.poolName);
            //        if (index == -1)
            //        {
            //            cacheToDelete.Add(poolCache);
            //        }
            //        // otherwise adding pool and cache stats to log
            //        else
            //        {
            //            cacheInfo += poolCache.poolName + "\tcurrent size: " + poolManagerRef.poolsList[index].Size + "\tcached size: " + poolCache.poolSize + "\t(updates count: " + poolCache.updatesCount + ")\n";
            //        }
            //    }

            //    // deleting all obsolete cache
            //    if (cacheToDelete.Count > 0)
            //    {
            //        if (cacheInfo != string.Empty)
            //        {
            //            cacheInfo += "\n";
            //        }

            //        foreach (PoolCache currentCache in cacheToDelete)
            //        {
            //            currentLevelCache.Remove(currentCache);
            //            cacheInfo += "deleted cache for unexisting pool: \"" + currentCache.poolName + "\"\n";
            //        }

            //        PoolManagerCache allCache = LoadAllCache();

            //        allCache.UpdateCache(GetCurrentCacheId(), currentLevelCache);
            //        Serializer.SerializeToPDP(allCache, PoolManager.CACHE_FILE_NAME);
            //    }

            //    Debug.Log(cacheInfo);
            //}
            //else
            //{
            //    Debug.Log("[PoolManager] There's no saved cache for current scene.");
            //}
        }

        private void ClearObsoleteCache()
        {
            // [CACHE IS CURRENTLY DISABLED]
            //List<PoolCache> currentLevelCache = LoadCurrentCache();

            //if (currentLevelCache != null)
            //{
            //    List<PoolCache> cacheToDelete = new List<PoolCache>();

            //    foreach (PoolCache poolCache in currentLevelCache)
            //    {
            //        // if pool not exists - delete it's cache
            //        int index = poolManagerRef.poolsList.FindIndex(x => x.Name == poolCache.poolName);
            //        if (index == -1)
            //        {
            //            cacheToDelete.Add(poolCache);
            //        }
            //    }

            //    // deleting all obsolete cache
            //    if (cacheToDelete.Count > 0)
            //    {
            //        string updateLog = "";

            //        foreach (PoolCache currentCache in cacheToDelete)
            //        {
            //            currentLevelCache.Remove(currentCache);
            //            updateLog += "deleted cache for unexisting pool: \"" + currentCache.poolName + "\"\n";
            //        }

            //        Debug.Log(updateLog);
            //        PoolManagerCache allCache = LoadAllCache();

            //        allCache.UpdateCache(GetCurrentCacheId(), currentLevelCache);
            //        Serializer.SerializeToPDP(allCache, PoolManager.CACHE_FILE_NAME);
            //    }
            //}
        }

        public void ClearCurrentChache()
        {
            // [CACHE IS CURRENTLY DISABLED]
            //if (EditorUtility.DisplayDialog("Delete all cache", "All cache for current scene will be cleared", "Delete", "Cancel"))
            //{
            //    PoolManagerCache allCache = LoadAllCache();

            //    allCache.DeleteCache(GetCurrentCacheId());
            //    Serializer.SerializeToPDP(allCache, PoolManager.CACHE_FILE_NAME);

            //    Debug.Log("Cache for current scene cleared");
            //}
        }

        // [CACHE IS CURRENTLY DISABLED]
        //private PoolManagerCache LoadAllCache()
        //{
        //    return Serializer.DeserializeFromPDP<PoolManagerCache>(PoolManager.CACHE_FILE_NAME, logIfFileNotExists: false);
        //}

        //private List<PoolCache> LoadCurrentCache()
        //{
        //    PoolManagerCache allCache = LoadAllCache();

        //    string currentCacheId = GetCurrentCacheId();

        //    return allCache.GetPoolCache(currentCacheId);
        //}

        private string GetCurrentCacheId()
        {
            return null;
            //string sceneMetaFile = Serializer.LoadTextFileAtPath(SceneManager.GetActiveScene().path + ".meta");

            //int startIndex = sceneMetaFile.IndexOf("guid: ") + "guid: ".Length;
            //int finalIndex = sceneMetaFile.LastIndexOf("DefaultImporter:");

            //return sceneMetaFile.Substring(startIndex, finalIndex - startIndex);
        }

        private void UpdateCacheStateList()
        {
            // [CACHE IS CURRENTLY DISABLED]
            //poolsCacheDeltaList = new List<int>();
            //poolsCacheList = new List<PoolCache>();

            //for (int i = 0; i < poolManagerRef.poolsList.Count; i++)
            //{
            //    poolsCacheDeltaList.Add(0);
            //    poolsCacheList.Add(null);
            //}

            //if (!poolManagerRef.useCache)
            //return;

            //List<PoolCache> cache = LoadCurrentCache();

            //if (!cache.IsNullOrEmpty())
            //{
            //    for (int i = 0; i < poolManagerRef.poolsList.Count; i++)
            //    {
            //        int index = cache.FindIndex(x => x.poolName == poolManagerRef.poolsList[i].Name);
            //        if (index != -1)
            //        {
            //            int delta = cache[index].poolSize - poolManagerRef.poolsList[i].Size;

            //            poolsCacheDeltaList[i] = delta;
            //            poolsCacheList[i] = cache[index];
            //        }
            //    }
            //}
        }

        private string CacheDeltaToState(int delta)
        {
            var state = string.Empty;

            if (delta > 0)
                state = "+" + delta;
            else if (delta < 0) state = delta.ToString();

            return state;
        }

        private void RenameCachedPool(string oldName, string newName)
        {
            // [CACHE IS CURRENTLY DISABLED]
            //List<PoolCache> poolCacheList = LoadCurrentCache();

            //int index = poolCacheList.FindIndex(x => x.poolName == oldName);
            //if (index != -1)
            //{
            //    poolCacheList[index].poolName = newName;
            //}

            //PoolManagerCache allCache = LoadAllCache();

            //allCache.UpdateCache(GetCurrentCacheId(), poolCacheList);
            //Serializer.SerializeToPDP(allCache, PoolManager.CACHE_FILE_NAME);
        }

        private void UpdateIgnoreCacheStateOfPool(string poolName, bool newState)
        {
            // [CACHE IS CURRENTLY DISABLED]
            //List<PoolCache> poolCacheList = LoadCurrentCache();

            //int index = poolCacheList.FindIndex(x => x.poolName == poolName);
            //if (index != -1)
            //{
            //    poolCacheList[index].ignoreCache = newState;
            //}

            //PoolManagerCache allCache = LoadAllCache();

            //allCache.UpdateCache(GetCurrentCacheId(), poolCacheList);
            //Serializer.SerializeToPDP(allCache, PoolManager.CACHE_FILE_NAME);
        }

        #endregion
    }
}