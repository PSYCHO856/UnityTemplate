using UnityEngine;

namespace MobileKit
{
    public static class MLog
    {
        public static void Log(string TAG, string message, GameObject context = null)
        {
            if (BuildConfig.Instance.Debug)
            {
                Debug.Log(TAG + " " + message, context);
            }
        }

        public static void LogWarning(string TAG, string message, GameObject context)
        {
            if (BuildConfig.Instance.Debug)
            {
                Debug.LogWarning(TAG + " " + message, context);
            }
        }
    }
}