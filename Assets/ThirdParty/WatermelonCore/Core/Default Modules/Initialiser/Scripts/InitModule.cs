using UnityEngine;

namespace Watermelon
{
    public abstract class InitModule : ScriptableObject
    {
        [HideInInspector] [SerializeField] protected string moduleName;

        protected InitModule()
        {
            moduleName = "Default Module";
        }

        public abstract void CreateComponent(Initialiser Initialiser);
    }
}

// -----------------
// Initialiser v 0.3
// -----------------