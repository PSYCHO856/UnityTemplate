using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace MobileKit
{
    public static class AnalyticsManager
    {
        private static IAnalyticsHandler analyticsHandler;

        public static void Init()
        {
#if UNITY_EDITOR
            analyticsHandler = AnalyticsDummyHandler.Instance;
#elif MOBILEKIT_IOS_CN
            analyticsHandler = AnalyticsDummyHandler.Instance;
#elif MOBILEKIT_IOS_US
            analyticsHandler = AnalyticsDummyHandler.Instance;
#elif MOBILEKIT_GOOGLEPLAY
            analyticsHandler = AnalyticsGooglePlayHandler.Instance;
#else
            analyticsHandler = AnalyticsDummyHandler.Instance;
#endif
            analyticsHandler.Init();
        }
        
        public static void Register(string accountId = "")
        {
            analyticsHandler?.Register(accountId);
        }

        public static void Login(string accountId = "")
        {
            analyticsHandler?.Login(accountId);
        }

        public static void Signin(int day)
        {
            analyticsHandler?.Signin(day);
        }
        
        public static void OnUserLv(int lv)
        {
            analyticsHandler?.OnUserLv(lv);
        }
        
        public static void OnLevelStart(int levelId, string sceneName = "")
        {
            if (!IsLevelIndexValid(levelId)) return;
            analyticsHandler?.OnLevelStart(levelId, sceneName);
        }

        public static void OnLevelGiveUp(int levelId, string sceneName = "")
        {
            if (!IsLevelIndexValid(levelId)) return;
            analyticsHandler?.OnLevelSkip(levelId, sceneName);
        }

        public static void OnLevelFailed(int levelId, string sceneName = "")
        {
            if (!IsLevelIndexValid(levelId)) return;
            analyticsHandler?.OnLevelFailed(levelId, sceneName);
        }

        public static void OnLevelWin(int levelId, string sceneName = "")
        {
            if (!IsLevelIndexValid(levelId)) return;
            analyticsHandler?.OnLevelWin(levelId, sceneName);
        }

        private static bool IsLevelIndexValid(int levelId)
        {
            if (levelId < 0) return false;
            if (levelId <= 50) return true;
            if (levelId <= 100 && levelId % 5 == 0) return true;
            if (levelId <= 500 && levelId % 10 == 0) return true;
            if (levelId % 50 == 0) return true;
            return false;
        } 

        public static void OnLevelWatchRV(int levelId, string sceneName)
        {
            if (levelId <= 0) return;
            analyticsHandler?.OnLevelWatchRV(levelId, sceneName);
        }

        /// <summary>
        /// 页面埋点
        /// </summary>
        /// <param name="pageName">Signin, Garage...</param>
        /// <param name="state">Open, Close, Ad...</param>
        public static void OnPageEvent(string pageName, string state)
        {
            analyticsHandler?.OnPageEvent(pageName, state);
        }

        public static void OnCustomEvent(string key)
        {
            analyticsHandler?.OnCustomEvent(key);
        }

        public static void CustomEventWithParams(string eventId, Dictionary<string, string> param)
        {
            analyticsHandler?.CustomEventWithParams(eventId, param);
        }

        public static void AdjustSetEvent(string eventId)
        {
            
            if (string.IsNullOrEmpty(eventId)) return;
            analyticsHandler?.AdjustEvent(eventId);
        }
        
        public static string GetString(string key)
        {
            return analyticsHandler?.GetString(key);
        }

        public static int GetInt(string key)
        {
            if (analyticsHandler != null)
            {
                return analyticsHandler.GetInt(key);
            }
            return default;
        }

        public static float GetFloat(string key)
        {
            if (analyticsHandler != null)
            {
                return analyticsHandler.GetFloat(key);
            }
            return default;
        }

        public static bool GetBool(string key)
        {
            if (analyticsHandler != null)
            {
                return analyticsHandler.GetBool(key);
            }
            return default;
        }
        
    }
}