#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    public class ScriptableObjectInitModule : InitModule
    {
        [SerializeField] private ScriptableObject[] initObjects;

        public ScriptableObjectInitModule()
        {
            moduleName = "Scriptable Object Initialization";
        }

        public override void CreateComponent(Initialiser Initialiser)
        {
            for (var i = 0; i < initObjects.Length; i++)
                if (initObjects[i] != null)
                {
                    var objectInterface = initObjects[i] as IInitialized;
                    if (objectInterface != null)
                        objectInterface.Init();
                    else
                        Debug.LogError("[Initialiser]: Object " + initObjects[i].name +
                                       " should implement IInitialized interface!");
                }
                else
                {
                    Debug.LogError("[Initialiser]: Scriptable object can't be null!");
                }
        }
    }
}

// -----------------
// Initialiser v 0.3
// -----------------