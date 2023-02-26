#if YUNBU_ANDROID
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MobileKit
{
    // YunbuSDK
    public class AnalyticsYunbuAndroidHandler : Singleton<AnalyticsYunbuAndroidHandler>, IAnalyticsHandler
    {
        private static AndroidJavaClass mainActivity;
        public void Init()
        {
            mainActivity = new AndroidJavaClass("com.feamber.MainActivity");
        }

        public void Register(string accountId)
        {
        }

        public void Login(string accountId)
        {
        }

        public void Signin(int day)
        {
            OnCustomEvent("signin", "signin", "sign_" + day, AnalyticsSDK.UMeng);
        }

        public void Guide(string stepId)
        {
            OnCustomEvent("guide", "guide", "guide_" + stepId, AnalyticsSDK.UMeng);
        }

        private bool IsAgreePolicy()
        {
            Debug.Log(nameof(AnalyticsYunbuAndroidHandler) + " " + nameof(IsAgreePolicy));
            return mainActivity.CallStatic<bool>("IsAgreePolicy");
        }
        
        public void OnUserLv(int lv)
        {
            Debug.Log(nameof(OnUserLv) + " "+ lv);
        }

        public void OnUnlockLevel(string levelId, string scene)
        {
            Debug.Log(nameof(OnUnlockLevel) + " " + levelId);
            mainActivity.CallStatic("OnUnlockLevel", levelId);
        }

        public void OnLevelStart(string levelId, string scene)
        {
            Debug.Log(nameof(OnLevelStart) + " " + levelId + " " + scene);
            mainActivity.CallStatic("OnLevelStart", levelId, scene);
        }

        public void OnLevelGiveUp(string levelId, string scene)
        {
            Debug.Log(nameof(OnLevelGiveUp) + " " + levelId + " " + scene);
            mainActivity.CallStatic("OnLevelGiveUp", levelId);
        }

        public void OnLevelFailed(string levelId, string scene)
        {
            Debug.Log(nameof(OnLevelFailed) + " " + levelId + " " + scene);
            mainActivity.CallStatic("OnLevelFailed", levelId);
        }

        public void OnLevelWin(string levelId, string scene)
        {
            Debug.Log(nameof(OnLevelWin) + " " + levelId + " " + scene);
            mainActivity.CallStatic("OnLevelFinish", levelId);
        }

        public void OnUMengLevelEvent(string levelId, string levelStatus, bool rePlay)
        {
            string userStatus = rePlay ? "0" : "1";
            OnCustomEvent("level_Analytics", "level", "level_" + levelId + "_" + levelStatus + "_" + userStatus, AnalyticsSDK.UMeng);

            OnCustomEvent("ui","ui","ui_ui_"+levelId,AnalyticsSDK.UMeng);
        }

        public void OnRVButtonShow(string sceneName)
        {
            OnCustomEvent("ui", "ui",  "ui_" + sceneName + "_show", AnalyticsSDK.UMeng);

        }

        public void OnRVButtonClick(string sceneName)
        {
            OnCustomEvent("ui", "ui",  "ui_" + sceneName + "_click", AnalyticsSDK.UMeng);
        }

        public void OnUMengGamePageEvent(string pageName, bool passive)
        {
            string pageType = passive ? "game" : "user";
            OnCustomEvent("gamePage_Analytics", "gamePage_Analytics", pageName + "_" + pageType, AnalyticsSDK.UMeng);
        }

        public void OnUMengAdEvent(string adStatus, string adType, string sceneId)
        {
            OnCustomEvent("ad_Analytics", "ad", adStatus + "_" + adType + "_" + sceneId, AnalyticsSDK.UMeng);
        }

        void IAnalyticsHandler.OnUMengSkinUnlockEvent(string skinType)
        {
            OnUMengSkinUnlockEvent(skinType);
        }



        public void OnUMengSkinUnlockEvent(string skinType)
        {
            OnCustomEvent("skin", "skin", "skin_" + skinType + "_" + "click", AnalyticsSDK.UMeng);
        }

        public void OnCustomEvent(string eventId, AnalyticsSDK sdk)
        {
            Debug.Log(nameof(OnCustomEvent) + " " + sdk + " " + eventId);
            mainActivity.CallStatic("CustomEvent", eventId);
        }

        public void OnCustomEvent(string eventId, string key, string value, AnalyticsSDK sdk)
        {
            if (sdk == AnalyticsSDK.Default || sdk == AnalyticsSDK.UMeng)
            {
                Debug.Log(nameof(OnCustomEvent) + " " + sdk + " eventId: " + eventId + "    Key: " + key + "    Value: " + value);
                AndroidJavaObject map = new Dictionary<string, string> {[key] = value}.ToAndroidMap();
                mainActivity.CallStatic("CustomEventWithParams", eventId, map);
            }
        }
        
        public void OnCustomEvent(string eventId, Dictionary<string, string> param, AnalyticsSDK sdk)
        {
            if (sdk == AnalyticsSDK.Default || sdk == AnalyticsSDK.UMeng)
            {
                Debug.Log(nameof(OnCustomEvent) + " " + sdk + " eventId: " + eventId + " " + IOUtils.SerializeObject(param));
                mainActivity.CallStatic("CustomEventWithParams", eventId, param.ToAndroidMap());
            }
        }

    }
}
#endif
