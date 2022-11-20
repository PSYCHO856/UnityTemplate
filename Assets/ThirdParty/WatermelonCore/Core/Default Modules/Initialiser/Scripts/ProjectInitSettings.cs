#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    [SetupTab("Init Settings", priority = 1, texture = "icon_puzzle")]
    [CreateAssetMenu(fileName = "Project Init Settings", menuName = "Settings/Project Init Settings")]
    public class ProjectInitSettings : ScriptableObject
    {
        [SerializeField] private InitModule[] initModules;

        public bool IsInititalized { get; private set; }

        public void Init(Initialiser Initialiser)
        {
            if (IsInititalized)
            {
                Debug.LogError("[Initialiser]: Project is already initialized!");

                return;
            }

            foreach (var t in initModules) t.CreateComponent(Initialiser);

            IsInititalized = true;
        }

        public void Destroy()
        {
            IsInititalized = false;
        }
    }
}

// -----------------
// Initialiser v 0.3
// -----------------