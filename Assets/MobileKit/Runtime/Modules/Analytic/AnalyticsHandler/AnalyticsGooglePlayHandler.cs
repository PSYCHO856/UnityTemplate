#if MOBILEKIT_GOOGLEPLAY

using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace MobileKit
{
    public class AnalyticsGooglePlayHandler : Singleton<AnalyticsGooglePlayHandler>, IAnalyticsHandler
    {
        private static AndroidJavaClass mainActivity;
        // [NonSerialized] public static UnityEvent onAppQuit = new UnityEvent();

        public void Init()
        {
            Debug.Log(nameof(AnalyticsGooglePlayHandler) + " " + nameof(Init));
            mainActivity = new AndroidJavaClass("com.mobilekit.sdk.MainActivity");
        }

        public void Register(string accountId)
        {
        }

        public void Login(string accountId)
        {
        }

        public void Signin(int day)
        {
        }

        public void OnUserLv(int lv)
        {
            Debug.Log(nameof(OnUserLv) + " "+ lv);
        }

        public void OnLevelStart(int levelId, string sceneName)
        {
            Debug.Log(nameof(OnLevelStart) + " " + levelId + " " + sceneName);
        }

        public void OnLevelSkip(int levelId, string sceneName)
        {
            Debug.Log(nameof(OnLevelSkip) + " " + levelId + " " + sceneName);
        }

        public void OnLevelFailed(int levelId, string sceneName)
        {
            Debug.Log(nameof(OnLevelFailed) + " " + levelId + " " + sceneName);
        }

        public void OnLevelWin(int levelId, string sceneName)
        {
            Debug.Log(nameof(OnLevelWin) + " " + levelId + " " + sceneName);
        }

        public void OnLevelWatchRV(int levelId, string sceneName)
        {
        }

        public void OnPageEvent(string pageName, string state)
        {
        }

        public void OnCustomEvent(string eventId)
        {
            Debug.Log(nameof(OnCustomEvent) + " " + " eventId: " + eventId);
        }

        public void OnCustomEvent(string eventId, string key, string value)
        {
            Debug.Log(nameof(OnCustomEvent) + " " + " eventId: " + eventId + "    Key: " + key + "    Value: " + value);
        }
        
        public void OnCustomEvent(string eventId, Dictionary<string, string> param)
        {
            Debug.Log(nameof(OnCustomEvent) + " " + " eventId: " + eventId + " " + IOUtils.SerializeObject(param));
        }
        public string GetString(string key)
        {
            return "";
        }

        public int GetInt(string key)
        {
            return 0;
        }

        public float GetFloat(string key)
        {
            return 0;
        }

        public bool GetBool(string key)
        {
            return false;
        }

        public void CustomEventWithParams(string eventId, Dictionary<string, string> param)
        {
            AndroidJavaObject map = DicToMap(param);
            mainActivity?.CallStatic("CustomEventWithParams", eventId, map);
        }


        //Adjust
        public void AdjustEvent(string eventId)
        {
            Debug.Log(nameof(AdjustEvent) + " " + " eventId: " + eventId);
            mainActivity?.CallStatic("AdjustEvent", eventId);
        }

        //EAS
        public void EASEvent(string eventId, Dictionary<string, string> param)
        {
            Debug.Log(nameof(EASEvent) + " " + " eventId: " + eventId);
            AndroidJavaObject map = DicToMap(param);
            mainActivity?.CallStatic("EASEvent", eventId, map);
        }

        public void EASUserSetOnce(Dictionary<string, string> param)
        {
            AndroidJavaObject map = DicToMap(param);
            mainActivity?.CallStatic("EASUserSetOnce", map);
        }

        public void EASUserSet(Dictionary<string, string> param)
        {
            AndroidJavaObject map = DicToMap(param);
            mainActivity?.CallStatic("EASUserSet", map);
        }

        public void EASUserAdd(Dictionary<string, string> param)
        {
            AndroidJavaObject map = DicToMap(param);
            mainActivity?.CallStatic("EASUserAdd", map);
        }

        public static AndroidJavaObject DicToMap(Dictionary<string, string> dictionary)
        {
            if(dictionary == null)
            {
                return null;
            }
            AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");
            foreach(KeyValuePair<string, string> pair in dictionary)
            {
                map.Call<string>("put", pair.Key, pair.Value);
            }
            return map;
        }

    }
}
#endif