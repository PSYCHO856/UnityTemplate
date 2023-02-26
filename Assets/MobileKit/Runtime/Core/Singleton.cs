using UnityEngine;


namespace MobileKit
{
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
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Check to see if we're about to be destroyed.
        private static T instance;
        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    // Search for existing instance.
                    instance = (T)FindObjectOfType(typeof(T), true);

                    // Create new instance if one doesn't already exist.
                    if (instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).Name;

                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return instance;
            }
        }
        private void OnDestroy()
        {
            instance = null;
        }
    }
}
