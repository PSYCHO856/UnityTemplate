#if YUNBU_GOOGLE_PLAY

using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MobileKit
{
    public class AnalyticsYunbuGooglePlayHandler : Singleton<AnalyticsYunbuGooglePlayHandler>, IAnalyticsHandler
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

        public void OnUserLv(int lv)
        {
            Debug.Log(nameof(OnUserLv) + " "+ lv);
        }

        public void OnUnlockLevel(string levelId, string scene)
        {
            Debug.Log(nameof(OnUnlockLevel) + " " + levelId);
        }

        public void OnLevelStart(string levelId, string scene)
        {
            Debug.Log(nameof(OnLevelStart) + " " + levelId + " " + scene);
        }

        public void OnLevelGiveUp(string levelId, string scene)
        {
            Debug.Log(nameof(OnLevelGiveUp) + " " + levelId + " " + scene);
        }

        public void OnLevelFailed(string levelId, string scene)
        {
            Debug.Log(nameof(OnLevelFailed) + " " + levelId + " " + scene);
        }

        public void OnLevelWin(string levelId, string scene)
        {
            Debug.Log(nameof(OnLevelWin) + " " + levelId + " " + scene);
        }

        public void OnRVButtonShow(string sceneId)
        {
            Debug.Log(nameof(OnRVButtonShow) + " " + sceneId);
        }

        public void OnRVButtonClick(string sceneId)
        {
            Debug.Log(nameof(OnRVButtonClick) + " " + sceneId);
        }

        public void OnCustomEvent(string eventId, AnalyticsSDK sdk)
        {
            Debug.Log(nameof(OnCustomEvent) + " " + sdk + " " + eventId);
        }

        public void OnCustomEvent(string eventId, string key, string value, AnalyticsSDK sdk)
        {
            Debug.Log(nameof(OnCustomEvent) + " " + sdk + " eventId: " + eventId + "    Key: " + key + "    Value: " + value);
        }
        
        public void OnCustomEvent(string eventId, Dictionary<string, string> param, AnalyticsSDK sdk)
        {
            Debug.Log(nameof(OnCustomEvent) + " " + sdk + " eventId: " + eventId + " " + IOUtils.SerializeObject(param));
        }

    }
}
#endif