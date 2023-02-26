using System.Collections.Generic;
using UnityEngine;

namespace MobileKit
{
    public class AnalyticsDummyHandler : Singleton<AnalyticsDummyHandler>, IAnalyticsHandler
    {
        
        public void Init()
        {
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


        public void CustomEventWithParams(string eventId, Dictionary<string, string> param)
        {
        }


        //Adjust
        public void AdjustEvent(string eventId)
        {
            Debug.Log(nameof(AdjustEvent) + " " + " eventId: " + eventId);
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


    }
}
