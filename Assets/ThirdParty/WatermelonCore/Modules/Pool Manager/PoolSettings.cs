using System.Collections.Generic;
using UnityEngine;

//Pool module v 1.6.3
namespace Watermelon
{
    public struct PoolSettings
    {
        public string name;
        public Pool.PoolType type;
        public GameObject singlePoolPrefab;
        public List<Pool.MultiPoolPrefab> multiPoolPrefabsList;
        public int poolSize;
        public bool autoSizeIncrement;
        public Transform objectsContainer;

        public PoolSettings(string name, GameObject singlePoolPrefab, int poolSize, bool willGrow,
            Transform objectsContainer = null)
        {
            type = Pool.PoolType.Single;
            multiPoolPrefabsList = new List<Pool.MultiPoolPrefab>();

            this.name = name;
            this.singlePoolPrefab = singlePoolPrefab;
            this.poolSize = poolSize;
            autoSizeIncrement = willGrow;
            this.objectsContainer = objectsContainer;
        }

        public PoolSettings(string name, List<Pool.MultiPoolPrefab> multiPoolPrefabs, int poolSize, bool willGrow,
            Transform objectsContainer = null)
        {
            type = Pool.PoolType.Multi;
            singlePoolPrefab = null;

            this.name = name;
            multiPoolPrefabsList = multiPoolPrefabs;
            this.poolSize = poolSize;
            autoSizeIncrement = willGrow;
            this.objectsContainer = objectsContainer;
        }

        public PoolSettings(Pool origin)
        {
            name = origin.Name;
            type = origin.Type;
            singlePoolPrefab = origin.SinglePoolPrefab;
            multiPoolPrefabsList = new List<Pool.MultiPoolPrefab>();

            for (var i = 0; i < origin.MultiPoolPrefabsAmount; i++)
                multiPoolPrefabsList.Add(origin.MultiPoolPrefabByIndex(i));

            poolSize = origin.PoolSize;
            autoSizeIncrement = origin.AutoSizeIncrement;
            objectsContainer = origin.ObjectsContainer;
        }

        public PoolSettings SetName(string name)
        {
            this.name = name;
            return this;
        }

        public PoolSettings SetType(Pool.PoolType type)
        {
            this.type = type;
            return this;
        }

        public PoolSettings SetSinglePrefab(GameObject prefab)
        {
            singlePoolPrefab = prefab;
            return this;
        }

        public PoolSettings SetMultiPrefabsList(List<Pool.MultiPoolPrefab> prefabsList)
        {
            multiPoolPrefabsList = prefabsList;
            return this;
        }

        public PoolSettings SetSize(int size)
        {
            poolSize = size;
            return this;
        }

        public PoolSettings SetAutoSizeIncrement(bool autoSizeIncrement)
        {
            this.autoSizeIncrement = autoSizeIncrement;
            return this;
        }

        public PoolSettings SetObjectsContainer(Transform objectsContainer)
        {
            this.objectsContainer = objectsContainer;
            return this;
        }

        public PoolSettings Reset()
        {
            name = string.Empty;
            type = Pool.PoolType.Single;
            singlePoolPrefab = null;
            multiPoolPrefabsList = new List<Pool.MultiPoolPrefab>();
            poolSize = 10;
            autoSizeIncrement = true;
            objectsContainer = null;

            return this;
        }

        public void RecalculateWeights()
        {
            var oldPrefabsList = new List<Pool.MultiPoolPrefab>(multiPoolPrefabsList);
            multiPoolPrefabsList = new List<Pool.MultiPoolPrefab>();

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
                            multiPoolPrefabsList.Add(new Pool.MultiPoolPrefab(oldPrefabsList[j].prefab,
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
    }
}