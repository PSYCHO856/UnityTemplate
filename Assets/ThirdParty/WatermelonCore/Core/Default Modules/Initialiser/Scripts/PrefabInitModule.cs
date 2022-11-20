#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    public class PrefabInitModule : InitModule
    {
        [SerializeField] private GameObject[] prefabs;

        public PrefabInitModule()
        {
            moduleName = "Custom Prefab Initialization";
        }

        public override void CreateComponent(Initialiser Initialiser)
        {
            for (var i = 0; i < prefabs.Length; i++)
                if (prefabs[i])
                {
                    var tempPrefab = Instantiate(prefabs[i]);
                    tempPrefab.transform.SetParent(Initialiser.transform);
                }
                else
                {
                    Debug.LogError("[Initialiser]: Custom prefab can't be null!");
                }
        }
    }
}

// -----------------
// Initialiser v 0.3
// -----------------