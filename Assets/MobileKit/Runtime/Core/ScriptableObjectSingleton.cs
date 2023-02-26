using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Yzngo copy from: http://wiki.unity3d.com/index.php/Singleton
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// 
/// Usage:
/// public class MySingleton : Singleton<MySingleton>
/// {
///     // (Optional) Prevent non-singleton constructor use.
///     protected MySingleton() { }
///
///    // Then add whatever code to the class you need as you normally would.
///     public string MyTestString = "Hello world!";
/// }
/// </summary>
///

namespace MobileKit
{
    public class ScriptableObjectSingleton<T> : SerializedScriptableObject where T : SerializedScriptableObject
    {
        // Check to see if we're about to be destroyed.
        private static bool m_ShuttingDown = false;
        private readonly static object m_Lock = new object();
        private static T m_Instance;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (m_ShuttingDown)
                {
                    return null;
                }

                lock (m_Lock)
                {
                    if (m_Instance == null)
                    {
                        m_Instance = ResourcesManager.GetAsset<T>();
                    }
                    return m_Instance;
                }
            }
        }
    }
}
