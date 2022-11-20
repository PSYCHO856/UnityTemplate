using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

//Pool module v 1.6.3
namespace Watermelon
{
    /// <summary>
    ///     Basic pool class. Contains pool settings and references to pooled objects.
    /// </summary>
    [Serializable]
    public class Pool
    {
        public enum PoolType
        {
            Single = 0,
            Multi = 1
        }

        [SerializeField] protected string name;

        [SerializeField] protected PoolType type = PoolType.Single;

        [SerializeField] protected GameObject singlePoolPrefab;


        /// <summary>
        ///     List to multiple pool prefabs list.
        /// </summary>
        [SerializeField] protected List<MultiPoolPrefab> multiPoolPrefabsList;

        [SerializeField] private int poolSize = 10;

        [SerializeField] protected bool autoSizeIncrement = true;


        [SerializeField] protected Transform objectsContainer;


        /// <summary>
        ///     True when all default objects spawned.
        /// </summary>
        [SerializeField] protected bool inited;

        /// <summary>
        ///     List of pooled objects for multiple pull.
        /// </summary>
        protected List<List<GameObject>> multiPooledObjects = new();

        /// <summary>
        ///     List of pooled objects for single pull.
        /// </summary>
        protected List<GameObject> pooledObjects = new();

        public Pool(PoolSettings settings)
        {
            name = settings.name;
            type = settings.type;
            singlePoolPrefab = settings.singlePoolPrefab;
            multiPoolPrefabsList = settings.multiPoolPrefabsList;
            poolSize = settings.poolSize;
            autoSizeIncrement = settings.autoSizeIncrement;
            objectsContainer = settings.objectsContainer;
            inited = false;
        }

        /// <summary>
        ///     Pool name, use it get pool reference at PoolManager.
        /// </summary>
        public string Name => name;

        /// <summary>
        ///     Type of pool.
        ///     Single - classic pool with one object. Multiple - pool with multiple objects returned randomly using weights.
        /// </summary>
        public PoolType Type => type;

        /// <summary>
        ///     Reference to single pool prefab.
        /// </summary>
        public GameObject SinglePoolPrefab => singlePoolPrefab;

        /// <summary>
        ///     Amount of prefabs at multi type pool.
        /// </summary>
        public int MultiPoolPrefabsAmount => multiPoolPrefabsList.Count;

        /// <summary>
        ///     Number of objects which be created be default.
        /// </summary>
        public int PoolSize => poolSize;

        /// <summary>
        ///     If enabled pool size will grow automatically if there is no more available objects.
        /// </summary>
        public bool AutoSizeIncrement => autoSizeIncrement;

        /// <summary>
        ///     Custom objects container for pool's objects.
        /// </summary>
        public Transform ObjectsContainer => objectsContainer;

        /// <summary>
        ///     Initializes pool.
        /// </summary>
        public void Initialize()
        {
            if (type == PoolType.Single)
                InitializeAsSingleTypePool();
            else
                InitializeAsMultiTypePool();
        }

        /// <summary>
        ///     Filling pool with spawned by default objects.
        /// </summary>
        protected void InitializeAsSingleTypePool()
        {
            pooledObjects = new List<GameObject>();

            if (singlePoolPrefab != null)
            {
                for (var i = 0; i < poolSize; i++) AddObjectToPoolSingleType(" ");
                inited = true;
            }
            else
            {
                Debug.LogError("[PoolManager] There's no attached prefab at pool: \"" + name + "\"");
            }
        }

        /// <summary>
        ///     Filling pool with spawned by default objects.
        /// </summary>
        protected void InitializeAsMultiTypePool()
        {
            multiPooledObjects = new List<List<GameObject>>();

            for (var i = 0; i < multiPoolPrefabsList.Count; i++)
            {
                multiPooledObjects.Add(new List<GameObject>());

                if (multiPoolPrefabsList[i].prefab != null)
                {
                    for (var j = 0; j < poolSize; j++) AddObjectToPoolMultiType(i, " ");
                    inited = true;
                }
                else
                {
                    Debug.LogError("[PoolManager] There's not attached prefab at pool: \"" + name + "\"");
                }
            }
        }

        protected virtual void InitGenericSingleObject(GameObject prefab)
        {
        }

        protected virtual void InitGenericMultiObject(int poolIndex, GameObject prefab)
        {
        }

        protected virtual void OnPoolCleared()
        {
        }

        /// <summary>
        ///     Returns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <returns>Pooled object or null if there is no available objects and new one can not be created.</returns>
        public GameObject GetPooledObject(bool activateObject = true)
        {
            return GetPooledObject(true, activateObject, false, Vector3.zero);
        }

        /// <summary>
        ///     Returns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="position">Sets object to specified position.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <returns>Pooled object or null if there is no available objects and new one can not be created.</returns>
        public GameObject GetPooledObject(Vector3 position, bool activateObject = true)
        {
            return GetPooledObject(true, activateObject, true, position);
        }

        /// <summary>
        ///     Returns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <returns>Pooled object or null if there is no available objects and new one can not be created.</returns>
        public GameObject GetHierarchyPooledObject(bool activateObject = true)
        {
            return GetPooledObject(false, activateObject, false, Vector3.zero);
        }

        /// <summary>
        ///     Returns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="position">Sets object to specified position.</param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <returns>Pooled object or null if there is no available objects and new one can not be created.</returns>
        public GameObject GetHierarchyPooledObject(Vector3 position, bool activateObject = true)
        {
            return GetPooledObject(false, activateObject, true, position);
        }

        /// <summary>
        ///     Rerurns reference to pooled object if it's currently available.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public GameObject GetPooledObject(PooledObjectSettings settings)
        {
            if (type == PoolType.Single)
                return GetPooledObjectSingleType(settings);
            return GetPooledObjectMultiType(settings, -1);
        }

        /// <summary>
        ///     Internal override of GetPooledObject and GetHierarchyPooledObject methods.
        /// </summary>
        /// <param name="checkTypeActiveSelf">
        ///     Which type of checking object's activation state is used: active self or active in
        ///     hierarchy.
        /// </param>
        /// <param name="activateObject">If true object will be set as active.</param>
        /// <param name="setPosition">If true will set position</param>
        /// <param name="position">Sets object to specified position.</param>
        /// <returns></returns>
        private GameObject GetPooledObject(bool checkTypeActiveSelf, bool activateObject, bool setPosition,
            Vector3 position)
        {
            var settings = new PooledObjectSettings(activateObject, !checkTypeActiveSelf);

            if (setPosition) settings = settings.SetPosition(position);

            return type == PoolType.Single
                ? GetPooledObjectSingleType(settings)
                : GetPooledObjectMultiType(settings, -1);
        }

        /// <summary>
        ///     Internal implementation of GetPooledObject and GetHierarchyPooledObject methods for Single type pool.
        /// </summary>
        private GameObject GetPooledObjectSingleType(PooledObjectSettings settings)
        {
            if (!inited) InitializeAsSingleTypePool();

            foreach (var go in pooledObjects)
                if (settings.UseActiveOnHierarchy ? !go.activeInHierarchy : !go.activeSelf)
                {
                    SetupPooledObject(go, settings);
                    return go;
                }

            if (autoSizeIncrement)
            {
                var newObject = AddObjectToPoolSingleType(" e");
                SetupPooledObject(newObject, settings);

                return newObject;
            }

            return null;
        }

        /// <summary>
        ///     Internal implementation of GetPooledObject and GetHierarchyPooledObject methods for Multi type pool.
        /// </summary>
        private GameObject GetPooledObjectMultiType(PooledObjectSettings settings, int poolIndex)
        {
            if (!inited)
                InitializeAsMultiTypePool();

            int chosenPoolIndex;

            if (poolIndex != -1)
            {
                chosenPoolIndex = poolIndex;
            }
            else
            {
                var randomPoolIndex = 0;
                var randomValueWasInRange = false;
                var randomValue = Random.Range(1, 101);
                var currentValue = 0;

                for (var i = 0; i < multiPoolPrefabsList.Count; i++)
                {
                    currentValue += multiPoolPrefabsList[i].weight;

                    if (randomValue <= currentValue)
                    {
                        randomPoolIndex = i;
                        randomValueWasInRange = true;
                        break;
                    }
                }

                if (!randomValueWasInRange)
                    Debug.LogError("[Pool Manager] Random value(" + randomValue +
                                   ") is out of weights sum range at pool: \"" + name + "\"");

                chosenPoolIndex = randomPoolIndex;
            }

            var objectsList = multiPooledObjects[chosenPoolIndex];


            foreach (var go in objectsList)
                if (settings.UseActiveOnHierarchy ? !go.activeInHierarchy : !go.activeSelf)
                {
                    SetupPooledObject(go, settings);
                    return go;
                }

            if (autoSizeIncrement)
            {
                var newObject = AddObjectToPoolMultiType(chosenPoolIndex, " e");
                SetupPooledObject(newObject, settings);
                return newObject;
            }

            return null;
        }

        /// <summary>
        ///     Applies pooled object settings to object.
        /// </summary>
        /// <param name="gameObject">Game object to apply settings.</param>
        /// <param name="settings">Settings to apply.</param>
        protected void SetupPooledObject(GameObject gameObject, PooledObjectSettings settings)
        {
            var objectTransform = gameObject.transform;

            if (settings.ApplyParrent) objectTransform.SetParent(settings.Parrent);

            if (settings.ApplyPosition) objectTransform.position = settings.Position;

            if (settings.ApplyLocalPosition) objectTransform.localPosition = settings.LocalPosition;

            if (settings.ApplyEulerRotation) objectTransform.eulerAngles = settings.EulerRotation;

            if (settings.ApplyLocalScale) objectTransform.localScale = settings.LocalScale;

            gameObject.SetActive(settings.Activate);
        }

        /// <summary>
        ///     Adds one more object to a single type pool.
        /// </summary>
        protected GameObject AddObjectToPoolSingleType(string nameAddition)
        {
            var newObject =
                PoolManager.SpawnObject(singlePoolPrefab, objectsContainer != null ? objectsContainer : null);

            newObject.name += nameAddition + PoolManager.SpawnedObjectsAmount;
            newObject.SetActive(false);

            pooledObjects.Add(newObject);
            InitGenericSingleObject(newObject);
            return newObject;
        }

        /// <summary>
        ///     Adds one more object to multi type Pool.
        /// </summary>
        protected GameObject AddObjectToPoolMultiType(int PoolIndex, string nameAddition)
        {
            var newObject = PoolManager.SpawnObject(multiPoolPrefabsList[PoolIndex].prefab,
                objectsContainer != null ? objectsContainer : null);

            newObject.name += nameAddition + PoolManager.SpawnedObjectsAmount;
            newObject.SetActive(false);
            multiPooledObjects[PoolIndex].Add(newObject);
            InitGenericMultiObject(PoolIndex, newObject);

            return newObject;
        }

        /// <summary>
        ///     Sets initial parents to all objects.
        /// </summary>
        public void ResetParents()
        {
            if (type == PoolType.Single)
                foreach (var go in pooledObjects)
                    go.transform.SetParent(objectsContainer != null
                        ? objectsContainer
                        : PoolManager.ObjectsContainerTransform);
            else
                foreach (var goList in multiPooledObjects)
                foreach (var go in goList)
                    go.transform.SetParent(objectsContainer != null
                        ? objectsContainer
                        : PoolManager.ObjectsContainerTransform);
        }

        /// <summary>
        ///     Disables all active objects from this pool.
        /// </summary>
        /// <param name="resetParent">Sets default parent if checked.</param>
        public void ReturnToPoolEverything(bool resetParent = false)
        {
            if (type == PoolType.Single)
                foreach (var go in pooledObjects)
                {
                    if (resetParent)
                        go.transform.SetParent(objectsContainer != null
                            ? objectsContainer
                            : PoolManager.ObjectsContainerTransform);

                    go.SetActive(false);
                }
            else
                foreach (var goList in multiPooledObjects)
                foreach (var go in goList)
                {
                    if (resetParent)
                        go.transform.SetParent(objectsContainer != null
                            ? objectsContainer
                            : PoolManager.ObjectsContainerTransform);
                    go.SetActive(false);
                }
        }

        /// <summary>
        ///     Destroys all spawned objects. Note, this method is performance heavy.
        /// </summary>
        public void Clear()
        {
            if (type == PoolType.Single)
            {
                foreach (var go in pooledObjects) Object.Destroy(go);
                pooledObjects.Clear();
            }
            else
            {
                foreach (var goList in multiPooledObjects)
                {
                    foreach (var go in goList) Object.Destroy(go);
                    goList.Clear();
                }

                multiPooledObjects.Clear();
            }

            OnPoolCleared();
        }

        /// <summary>
        ///     Returns object from multi type pool by it's index on prefabs list.
        /// </summary>
        public GameObject GetMultiPooledObjectByIndex(int index, PooledObjectSettings setting)
        {
            return GetPooledObjectMultiType(setting, index);
        }

        /// <summary>
        ///     Returns prefab from multi type pool by it's index.
        /// </summary>
        public MultiPoolPrefab MultiPoolPrefabByIndex(int index)
        {
            return multiPoolPrefabsList[index];
        }

        /// <summary>
        ///     Evenly distributes the weight between multi pooled objects, leaving locked weights as is.
        /// </summary>
        public void RecalculateWeights()
        {
            var oldPrefabsList = new List<MultiPoolPrefab>(multiPoolPrefabsList);
            multiPoolPrefabsList = new List<MultiPoolPrefab>();

            if (oldPrefabsList.Count > 0)
            {
                var totalUnlockedPoints = 100;
                var unlockedPrefabsAmount = oldPrefabsList.Count;

                for (var i = 0; i < oldPrefabsList.Count; i++)
                    if (oldPrefabsList[i].isWeightLocked)
                    {
                        totalUnlockedPoints -= oldPrefabsList[i].weight;
                        unlockedPrefabsAmount--;
                    }

                if (unlockedPrefabsAmount > 0)
                {
                    var averagePoints = totalUnlockedPoints / unlockedPrefabsAmount;
                    var additionalPoints = totalUnlockedPoints - averagePoints * unlockedPrefabsAmount;

                    for (var j = 0; j < oldPrefabsList.Count; j++)
                        if (oldPrefabsList[j].isWeightLocked)
                        {
                            multiPoolPrefabsList.Add(oldPrefabsList[j]);
                        }
                        else
                        {
                            multiPoolPrefabsList.Add(new MultiPoolPrefab(oldPrefabsList[j].prefab,
                                averagePoints + (additionalPoints > 0 ? 1 : 0), false));
                            additionalPoints--;
                        }
                }
                else
                {
                    multiPoolPrefabsList = oldPrefabsList;
                }
            }
        }

        /// <summary>
        ///     Checks are all prefabs references assigned.
        /// </summary>
        public bool IsAllPrefabsAssigned()
        {
            if (type == PoolType.Single) return singlePoolPrefab != null;

            if (multiPoolPrefabsList.Count == 0)
                return false;

            for (var i = 0; i < multiPoolPrefabsList.Count; i++)
                if (multiPoolPrefabsList[i].prefab == null)
                    return false;

            return true;
        }

        [Serializable]
        public struct MultiPoolPrefab
        {
            public GameObject prefab;
            public int weight;
            public bool isWeightLocked;

            public MultiPoolPrefab(GameObject prefab, int weight, bool isWeightLocked)
            {
                this.prefab = prefab;
                this.weight = weight;
                this.isWeightLocked = isWeightLocked;
            }
        }
    }
}