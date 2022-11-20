#pragma warning disable 0414

using System.Collections.Generic;
using UnityEngine;

//Pool module v 1.6.3
namespace Watermelon
{
    /// <summary>
    ///     Class that manages all pool operations.
    /// </summary>
    [DefaultExecutionOrder(-150)]
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager instance;

        [SerializeField] private Transform objectsContainer;

        /// <summary>
        ///     List of all existing pools.
        /// </summary>
        [SerializeField] private List<Pool> poolsList = new();

        /// <summary>
        ///     True when PoolManager is already initialized.
        /// </summary>
        private bool isInited;

        // [CACHE IS CURRENTLY DISABLED]
        //private bool useCache = false;
        ///// <summary>
        ///// When enabled PoolManager will automaticaly setup amount of spawned by default objects using cashed data.
        ///// </summary>
        //public static bool UseCache
        //{
        //    get { return instance.useCache; }
        //}

        /// <summary>
        ///     Dictionary which allows to acces Pool by name.
        /// </summary>
        private Dictionary<string, Pool> poolsDictionary;

        private int spawnedObjectAmount;

        /// <summary>
        ///     Empty object to store all pooled objects at scene (used only on Editor).
        /// </summary>
        public static Transform ObjectsContainerTransform
        {
            get
            {
#if UNITY_EDITOR
                return instance.objectsContainer.transform;
#else
            return null;
#endif
            }
        }

        /// <summary>
        ///     Amount of spawned objects.
        /// </summary>
        public static int SpawnedObjectsAmount => instance.spawnedObjectAmount;

        private void Awake()
        {
            instance = this;
            Init();
        }

        /// <summary>
        ///     Initialize a single instance of PoolManager.
        /// </summary>
        private static void InitSingletone()
        {
            var poolManager = FindObjectOfType<PoolManager>();

            if (poolManager != null)
            {
                poolManager.Init();

                instance = poolManager;
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError("Please, add PoolManager behaviour at scene.");
            }
#endif
        }

        /// <summary>
        ///     Initialization of PoolManager.
        /// </summary>
        private void Init()
        {
            if (instance == null)
                return;

#if UNITY_EDITOR
            if (objectsContainer == null) objectsContainer = new GameObject("[PooledObjects]").transform;
#endif

            poolsDictionary = new Dictionary<string, Pool>();

            foreach (var pool in poolsList) poolsDictionary.Add(pool.Name, pool);

            foreach (var pool in poolsList) pool.Initialize();
        }

        public static GameObject SpawnObject(GameObject prefab, Transform parrent)
        {
#if UNITY_EDITOR
            if (parrent == null) parrent = ObjectsContainerTransform;
#endif

            instance.spawnedObjectAmount++;
            return Instantiate(prefab, parrent);
        }

        /// <summary>
        ///     Returns reference to Pool by it's name.
        /// </summary>
        /// <param name="poolName">Name of Pool which should be returned.</param>
        /// <returns>Reference to Pool.</returns>
        public static Pool GetPoolByName(string poolName)
        {
            if (instance == null) InitSingletone();

            if (instance.poolsDictionary.ContainsKey(poolName))
            {
                return instance.poolsDictionary[poolName];
            }
#if UNITY_EDITOR
            Debug.LogError("[PoolManager] Not found pool with name: '" + poolName + "'");
#endif
            return null;
        }

        /// <summary>
        ///     Adds new pool at runtime.
        /// </summary>
        /// <param name="poolSettings">Pool builder settings.</param>
        /// <returns>Newly created pool.</returns>
        public static Pool AddPool(PoolSettings poolSettings)
        {
            if (instance.poolsDictionary.ContainsKey(poolSettings.name))
            {
                Debug.LogError("[Pool manager] Adding a new pool failed. Name \"" + poolSettings.name +
                               "\" already exists.");
                return null;
            }

            var newPool = new Pool(poolSettings);
            instance.poolsDictionary.Add(newPool.Name, newPool);
            instance.poolsList.Add(newPool);

            newPool.Initialize();

            return newPool;
        }

        private bool IsAllPrefabsAssignedAtPool(int poolIndex)
        {
            if (poolsList != null && poolIndex < poolsList.Count)
                return poolsList[poolIndex].IsAllPrefabsAssigned();
            return true;
        }

        private void RecalculateWeightsAtPool(int poolIndex)
        {
            poolsList[poolIndex].RecalculateWeights();
        }

#if UNITY_EDITOR

        #region Cache Management

        // //////////////////////////////////////////////////////////////////////////////////
        // New Chace system

        /// <summary>
        ///     Loads cache from disc and initializing current scene's cache id.
        /// </summary>
        private void LoadCache()
        {
            // [CACHE IS CURRENTLY DISABLED]
            // cache system is currently disabled

            //cache = Serializer.DeserializeFromPDP<PoolManagerCache>(CACHE_FILE_NAME, logIfFileNotExists: false);

            //string sceneMetaFile = Serializer.LoadTextFileAtPath(SceneManager.GetActiveScene().path + ".meta");

            //int startIndex = sceneMetaFile.IndexOf("guid: ") + "guid: ".Length;
            //int finalIndex = sceneMetaFile.LastIndexOf("timeCreated");

            //currentCacheId = sceneMetaFile.Substring(startIndex, finalIndex - startIndex);
        }

        /// <summary>
        ///     Updates cache after exit from play mode or creation of new pool.
        /// </summary>
        private void UpdateCache()
        {
            // [CACHE IS CURRENTLY DISABLED]
            // cache system is currently disabled

            //if (currentCacheId != "")
            //{
            //    // true if there is no saved cache
            //    bool init = !cache.ContainsLevel(currentCacheId);

            //    List<PoolCache> newCache = new List<PoolCache>();

            //    for (int i = 0; i < pools.Count; i++)
            //    {
            //        // if we initializing cache we simple adding current pool info
            //        if (init)
            //        {
            //            newCache.Add(new PoolCache(pools[i].Name, pools[i].Size));
            //        }
            //        // if there is a cache, let's update this stuff
            //        else
            //        {
            //            int index = cache.poolsCache[currentCacheId].FindIndex(x => x.poolName == pools[i].Name);
            //            if (index != -1)
            //            {
            //                // do not consider new data if it's (probably pool just was not used at all)
            //                if (pools[i].maxItemsUsedInOneTime != 0)
            //                {
            //                    cache.poolsCache[currentCacheId][index].UpdateSize(pools[i].maxItemsUsedInOneTime);
            //                }

            //                newCache.Add(cache.poolsCache[currentCacheId][index]);
            //            }
            //            else
            //            {
            //                newCache.Add(new PoolCache(pools[i].Name, pools[i].Size));
            //            }
            //        }
            //    }

            //    if (init)
            //    {
            //        cache.poolsCache.Add(currentCacheId, newCache);
            //    }
            //    else
            //    {
            //        cache.UpdateCache(currentCacheId, newCache);
            //    }

            //    Serializer.SerializeToPDP(cache, CACHE_FILE_NAME);
            //}
            //else
            //{
            //    Debug.LogError("[PoolManager] Cache could not be updated. This level was not initialized.");
            //}
        }

        private void OnDisable()
        {
            // [CACHE IS CURRENTLY DISABLED]
            //if (useCache)
            //{
            //    UpdateCache();
            //}
        }

        /// <summary>
        ///     Updates and saves cache.
        /// </summary>
        public void SaveCache()
        {
            LoadCache();
            UpdateCache();
        }

        #endregion

#endif
    }
}