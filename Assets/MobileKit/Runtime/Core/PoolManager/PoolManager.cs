#pragma warning disable 0414

using UnityEngine;
using System.Collections.Generic;
using MobileKit;

// PoolSettings poolSettings = new PoolSettings
// {
//     autoSizeIncrement = true,
//     objectsContainer = null,
//     size = 1,
//     type = Pool.PoolType.Single,
//     singlePoolPrefab = new GameObject(),
//     name = "xxx"
// };
// PoolManager.AddPool(poolSettings);
// PoolManager.GetPoolByName("xxx").GetPooledObject().GetComponent<PoolManager>();
// PoolManager.GetPoolByName("sss").ReturnToPoolEverything();
// PoolManager.DestroyPool(PoolManager.GetPoolByName("dsfs"));

namespace MobileKit
{
    /// <summary>
    /// Class that manages all pool operations.
    /// </summary>
    [DefaultExecutionOrder(-150)]
    public class PoolManager : Singleton<PoolManager>
    {
        
        private List<Pool> poolsList = new List<Pool>();

        /// <summary>
        /// Dictionary which allows to access Pool by name.
        /// </summary>
        private Dictionary<string, Pool> poolsDictionary = new Dictionary<string, Pool>();

        private int spawnedObjectAmount = 0;
        /// <summary>
        /// Amount of spawned objects.
        /// </summary>
        public static int SpawnedObjectsAmount => Instance.spawnedObjectAmount;

        public static GameObject SpawnObject(GameObject prefab, Transform parent)
        {
            if (parent == null)
            {
                parent = Instance.transform;
            }
            Instance.spawnedObjectAmount++;
            return Instantiate(prefab, parent);
        }

        /// <summary>
        /// Returns reference to Pool by it's name.
        /// </summary>
        /// <param name="poolName">Name of Pool which should be returned.</param>
        /// <returns>Reference to Pool.</returns>
        public static Pool GetPoolByName(string poolName)
        {
            if (Instance.poolsDictionary.ContainsKey(poolName))
            {
                return Instance.poolsDictionary[poolName];
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("[PoolManager] Not found pool with name: '" + poolName + "'");
#endif
                return null;
            }
        }

        /// <summary>
        /// Adds new pool at runtime.
        /// </summary>
        /// <param name="poolBuilder">Pool builder settings.</param>
        /// <returns>Newly created pool.</returns>
        public static Pool AddPool(PoolSettings poolBuilder)
        {
            if (Instance.poolsDictionary.ContainsKey(poolBuilder.name))
            {
                Debug.LogError("[Pool manager] Adding a new pool failed. Name \"" + poolBuilder.name + "\" already exists.");
                return null;
            }

            Pool newPool = new Pool(poolBuilder);
            Instance.poolsDictionary.Add(newPool.Name, newPool);
            Instance.poolsList.Add(newPool);

            newPool.Initialize();

            return newPool;
        }

        ///// <summary>
        ///// Destroys all spawned objects. Note, this method is performance heavy.
        ///// </summary>
        public static void DestroyPool(Pool pool)
        {
            pool.Clear();

            Instance.poolsDictionary.Remove(pool.Name);
            Instance.poolsList.Remove(pool);
        }

        public static bool PoolExists(string name)
        {
            return Instance.poolsDictionary.ContainsKey(name);
        }

        // editor methods

        private bool IsAllPrefabsAssignedAtPool(int poolIndex)
        {
            if (poolsList != null && poolIndex < poolsList.Count)
            {
                return poolsList[poolIndex].IsAllPrefabsAssigned();
            }
            else
            {
                return true;
            }
        }

        private void RecalculateWeightsAtPool(int poolIndex)
        {
            poolsList[poolIndex].RecalculateWeights();
        }
    }
}