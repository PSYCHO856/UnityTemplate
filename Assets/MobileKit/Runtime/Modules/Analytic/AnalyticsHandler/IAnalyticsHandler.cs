using System.Collections.Generic;

namespace MobileKit
{
    public interface IAnalyticsHandler
    {
        public void Init();
        
        void Register(string accountId);
        void Login(string accountId);

        void Signin(int day);
        
        public void OnUserLv(int lv);

        public void OnLevelStart(int levelId, string sceneName);

        public void OnLevelSkip(int levelId, string sceneName);
        
        public void OnLevelFailed(int levelId, string sceneName);
        
        public void OnLevelWin(int levelId, string sceneName);

        public void OnLevelWatchRV(int levelId, string sceneName);

        public void OnPageEvent(string pageName, string state);

        public void OnCustomEvent(string eventId);
        public void OnCustomEvent(string eventId, string key, string value);
        public void OnCustomEvent(string eventId, Dictionary<string, string> param);

        public void CustomEventWithParams(string eventId, Dictionary<string, string> param);
        public void AdjustEvent(string eventId);

        public string GetString(string key);
        public int GetInt(string key);
        public float GetFloat(string key);
        public bool GetBool(string key);
    }
}
