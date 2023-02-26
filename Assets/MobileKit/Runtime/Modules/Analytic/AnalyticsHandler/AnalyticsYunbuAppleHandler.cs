#if YUNBU_APPLE
using System.Collections.Generic;
using com.adjust.sdk;
using Newtonsoft.Json;
using UnityEngine;

namespace MobileKit
{
    // UMeng       √ 
    // Adjust
    // TrackingIO
    public class AnalyticsYunbuAppleHandler : Singleton<AnalyticsYunbuAppleHandler>, IAnalyticsHandler
    {
        private static UMengAdEventConfig uMengAdEventConfig;
        
        public static UMengAdEventConfig UMengAdEventConfig
        {
            get
            {
                if (uMengAdEventConfig == null)
                {
                    uMengAdEventConfig = ResourcesManager.GetAsset<UMengAdEventConfig>();
                }
                return uMengAdEventConfig;
            }
        }
        
        
        private static TrackingIOAdEventConfig trackingIOAdEventConfig;
        
        public static TrackingIOAdEventConfig TrackingIOAdEventConfig
        {
            get
            {
                if (trackingIOAdEventConfig == null)
                {
                    trackingIOAdEventConfig = ResourcesManager.GetAsset<TrackingIOAdEventConfig>();
                }
                return trackingIOAdEventConfig;
            }
        }
        
        
        private static AdjustAdEventConfig adjustAdEventConfig;
        
        public static AdjustAdEventConfig AdjustAdEventConfig
        {
            get
            {
                if (adjustAdEventConfig == null)
                {
                    adjustAdEventConfig = ResourcesManager.GetAsset<AdjustAdEventConfig>();
                }
                return adjustAdEventConfig;
            }
        }
        
        private static bool IsAgreePolicy
        {
            get => GameSettingsPrefs.GetBool("IsAgreePolicy", false);
            set => GameSettingsPrefs.SetBool("IsAgreePolicy", value);
        }

        private static bool IsUMengInitialized;
        
        public void Init()
        {
            // Umeng
            if (!IsUMengInitialized)
            {
                if (!IsAgreePolicy)
                {
                    var prefab =  Resources.Load<ProtocolCanvas>("UI/ProtocolCanvas");
                    var protocolCanvas = Object.Instantiate(prefab);
                    protocolCanvas.OnOpen( () =>
                    {
                        IsAgreePolicy = true;
                        InitUmeng();
                    });
                }
                else
                {
                    InitUmeng();
                }
            }
            
            // TrackingIO 热云
            bool isAnalyticsDebug = BuildConfig.Instance.AnalyticsDebug;
            
            Tracking.Instance.setPrintLog(isAnalyticsDebug);
            Tracking.Instance.init(AppConfig.Instance.TrackingIOAppKey, AppConfig.Instance.TrackingIOChannelId);
            
            // Adjust
            string appToken = AppConfig.Instance.AdjustToken;
            AdjustEnvironment environment = isAnalyticsDebug ? AdjustEnvironment.Sandbox : AdjustEnvironment.Production;
            AdjustConfig config = new AdjustConfig(appToken, environment, false);
            config.setLogLevel(isAnalyticsDebug? AdjustLogLevel.Info : AdjustLogLevel.Debug);
            Adjust.start(config);
        }

        private static void InitUmeng()
        {
            Umeng.Analytics.SetLogEnabled(BuildConfig.Instance.AnalyticsDebug);
            Umeng.Analytics.StartWithAppKeyAndChannelId(AppConfig.Instance.UMengAppKey, AppConfig.Instance.UMengChannelId);
            IsUMengInitialized = true;
        }
        
        public void Register(string accountId)
        {
            if (accountId == "")
            {
                accountId = Tracking.Instance.getDeviceId();
            }
            Tracking.Instance.register(accountId);
        }

        public void Login(string accountId)
        {
            if (accountId == "")
            {
                accountId = Tracking.Instance.getDeviceId();
            }
            Tracking.Instance.login(accountId);
        }

        public void Signin(int day)
        {
            OnCustomEvent("signin", "signin", "sign_" + day, AnalyticsSDK.UMeng);

            switch (day)
            {
                case 1:
                    OnCustomEvent(UMengAdEventConfig.Signin1, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.Signin1, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.Signin1, AnalyticsSDK.TrackingIO);
                    break;
                case 2:
                    OnCustomEvent(UMengAdEventConfig.Signin2, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.Signin2, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.Signin2, AnalyticsSDK.TrackingIO);
                    break;
                case 3:
                    OnCustomEvent(UMengAdEventConfig.Signin3, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.Signin3, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.Signin3, AnalyticsSDK.TrackingIO);
                    break;
                case 4:
                    OnCustomEvent(UMengAdEventConfig.Signin4, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.Signin4, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.Signin4, AnalyticsSDK.TrackingIO);
                    break;
                case 5:
                    OnCustomEvent(UMengAdEventConfig.Signin5, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.Signin5, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.Signin5, AnalyticsSDK.TrackingIO);
                    break;
                case 6:
                    OnCustomEvent(UMengAdEventConfig.Signin6, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.Signin6, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.Signin6, AnalyticsSDK.TrackingIO);
                    break;
                case 7:
                    OnCustomEvent(UMengAdEventConfig.Signin7, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.Signin7, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.Signin7, AnalyticsSDK.TrackingIO);
                    break;
                case 8:
                    OnCustomEvent(UMengAdEventConfig.Signin8, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.Signin8, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.Signin8, AnalyticsSDK.TrackingIO);
                    break;
                case 9:
                    OnCustomEvent(UMengAdEventConfig.Signin9, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.Signin9, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.Signin9, AnalyticsSDK.TrackingIO);
                    break;
                case 10:
                    OnCustomEvent(UMengAdEventConfig.Signin10, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.Signin10, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.Signin10, AnalyticsSDK.TrackingIO);
                    break;
            }
        }

        public void Guide(string stepId)
        {
            OnCustomEvent("guide", "guide", "guide_" + stepId, AnalyticsSDK.UMeng);
        }
        
        public void OnUserLv(int lv)
        {
            if (IsUMengInitialized)
            {
                Umeng.GA.SetUserLevel(lv);
            }
        }

        public void OnUnlockLevel(string levelId, string scene)
        {
        }

        public void OnLevelStart(string levelId, string scene)
        {
            if (IsUMengInitialized)
            {
                Umeng.GA.StartLevel(levelId);
            }
        }

        public void OnLevelGiveUp(string levelId, string scene)
        {
        }

        public void OnLevelFailed(string levelId, string scene)
        {
            if (IsUMengInitialized)
            {
                Umeng.GA.FailLevel(levelId);
            }
        }

        public void OnLevelWin(string levelId, string scene)
        {
            if (int.TryParse(levelId, out var levelIndex))
            {
                SendLevelCustomEvent(levelIndex);
            }
            if (IsUMengInitialized)
            {
                Umeng.GA.FinishLevel(levelId);
            }
        }

        public class AdJsonInfo
        {
            public string adPlatform;
            public string adId;

            public AdJsonInfo(string adPlatform, string adId)
            {
                this.adPlatform = adPlatform;
                this.adId = adId;
            }
        }
        
        public void OnSplashShow(string showInfo)
        {
            var info = JsonConvert.DeserializeObject<AdJsonInfo>(showInfo);
            if (info != default)
            {
                Tracking.Instance.setTrackAdShow(info.adPlatform, info.adId, true);
            }
			OnCustomEvent(UMengAdEventConfig.ShowSplashSuccess, AnalyticsSDK.UMeng);
			OnCustomEvent(AdjustAdEventConfig.ShowSplashSuccess, AnalyticsSDK.Adjust);
			OnCustomEvent(TrackingIOAdEventConfig.ShowSplashSuccess, AnalyticsSDK.TrackingIO);
        }

        public void OnSplashClick(string clickInfo)
        {
            var info = JsonConvert.DeserializeObject<AdJsonInfo>(clickInfo);
            if (info != default)
            {
                Tracking.Instance.setTrackAdClick(info.adPlatform, info.adId);
            }
        }
        

        public void OnUMengLevelEvent(string levelId, string levelStatus, bool rePlay)
        {
            string userStatus = rePlay ? "0" : "1";
            OnCustomEvent("level_Analytics", "level", "level_" + levelId + "_" + levelStatus + "_" + userStatus, AnalyticsSDK.UMeng);
        }
        

        public void OnRVButtonShow(string sceneName)
        {
            OnCustomEvent("ui", "ui",  "ui_" + sceneName + "_show", AnalyticsSDK.UMeng);
        }


        public void OnRVButtonClick(string sceneId)
        {
            OnCustomEvent("ui", "ui",  "ui_" + sceneId + "_click", AnalyticsSDK.UMeng);
        }
        

        public void OnUMengGamePageEvent(string pageName, bool passive)
        {
            string pageType = passive ? "game" : "user";
            OnCustomEvent("gamePage_Analytics", "gamePage_Analytics", pageName + "_" + pageType, AnalyticsSDK.UMeng);
        }
        

        /// <summary>
        /// 友盟自定义广告相关统计
        /// **不需要上层游戏逻辑接入,SDK已接**
        /// </summary>
        /// <param name="adStatus">request, filled, show, click, reward, close</param>
        /// <param name="adType">ins, splash, video, banner, native</param>
        /// <param name="sceneId">无场景填 default</param>
        public void OnUMengAdEvent(string adStatus, string adType, string sceneId)
        {
            OnCustomEvent("ad_Analytics", "ad", adStatus + "_" + adType + "_" + sceneId, AnalyticsSDK.UMeng);
        }


        #region parkJamADD

        public void OnUMengSkinUnlockEvent(string skinType)
        {
            OnCustomEvent("skin", "skin", "skin_" + skinType + "_" + "click", AnalyticsSDK.UMeng);
        }

        #endregion
        
        
        private void SendLevelCustomEvent(int levelId)
        {
            OnCustomEvent(UMengAdEventConfig.LevelPass, AnalyticsSDK.UMeng);
            switch (levelId)
            {
                case 1:
                    OnCustomEvent(UMengAdEventConfig.LevelPass1, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass1, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass1, AnalyticsSDK.TrackingIO);
                    break;
                case 2:
                    OnCustomEvent(UMengAdEventConfig.LevelPass2, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass2, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass2, AnalyticsSDK.TrackingIO);
                    break;
                case 3:
                    OnCustomEvent(UMengAdEventConfig.LevelPass3, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass3, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass3, AnalyticsSDK.TrackingIO);
                    break;
                case 4:
                    OnCustomEvent(UMengAdEventConfig.LevelPass4, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass4, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass4, AnalyticsSDK.TrackingIO);
                    break;
                case 5:
                    OnCustomEvent(UMengAdEventConfig.LevelPass5, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass5, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass5, AnalyticsSDK.TrackingIO);
                    break;
                case 6:
                    OnCustomEvent(UMengAdEventConfig.LevelPass6, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass6, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass6, AnalyticsSDK.TrackingIO);
                    break;
                case 7:
                    OnCustomEvent(UMengAdEventConfig.LevelPass7, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass7, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass7, AnalyticsSDK.TrackingIO);
                    break;
                case 8:
                    OnCustomEvent(UMengAdEventConfig.LevelPass8, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass8, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass8, AnalyticsSDK.TrackingIO);
                    break;
                case 9:
                    OnCustomEvent(UMengAdEventConfig.LevelPass9, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass9, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass9, AnalyticsSDK.TrackingIO);
                    break;
                case 10:
                    OnCustomEvent(UMengAdEventConfig.LevelPass10, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass10, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass10, AnalyticsSDK.TrackingIO);
                    break;
                case 11:
                    OnCustomEvent(UMengAdEventConfig.LevelPass11, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass11, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass11, AnalyticsSDK.TrackingIO);
                    break;
                case 12:
                    OnCustomEvent(UMengAdEventConfig.LevelPass12, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass12, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass12, AnalyticsSDK.TrackingIO);
                    break;
                case 13:
                    OnCustomEvent(UMengAdEventConfig.LevelPass13, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass13, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass13, AnalyticsSDK.TrackingIO);
                    break;
                case 14:
                    OnCustomEvent(UMengAdEventConfig.LevelPass14, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass14, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass14, AnalyticsSDK.TrackingIO);
                    break;
                case 15:
                    OnCustomEvent(UMengAdEventConfig.LevelPass15, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass15, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass15, AnalyticsSDK.TrackingIO);
                    break;
                case 30:
                    OnCustomEvent(UMengAdEventConfig.LevelPass30, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass30, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass30, AnalyticsSDK.TrackingIO);
                    break;
                case 50:
                    OnCustomEvent(UMengAdEventConfig.LevelPass50, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass50, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass50, AnalyticsSDK.TrackingIO);
                    break;
                case 100:
                    OnCustomEvent(UMengAdEventConfig.LevelPass100, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass100, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass100, AnalyticsSDK.TrackingIO);
                    break;
                case 300:
                    OnCustomEvent(UMengAdEventConfig.LevelPass300, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass300, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass300, AnalyticsSDK.TrackingIO);
                    break;
                case 500:
                    OnCustomEvent(UMengAdEventConfig.LevelPass500, AnalyticsSDK.UMeng);
                    OnCustomEvent(AdjustAdEventConfig.LevelPass500, AnalyticsSDK.Adjust);
                    OnCustomEvent(TrackingIOAdEventConfig.LevelPass500, AnalyticsSDK.TrackingIO);
                    break;
            }
        }



        public void OnCustomEvent(string eventId, AnalyticsSDK sdk)
        {
            if (string.IsNullOrEmpty(eventId)) return;
            if (sdk == AnalyticsSDK.TrackingIO || sdk == AnalyticsSDK.Default)
            {
                Tracking.Instance.setEvent(eventId);
            }

            if (IsUMengInitialized && (sdk == AnalyticsSDK.UMeng || sdk == AnalyticsSDK.Default))
            {
                Umeng.Analytics.Event(eventId);
            }
            if (sdk == AnalyticsSDK.Adjust || sdk == AnalyticsSDK.Default)
            {
                AdjustEvent adjustEvent = new AdjustEvent(eventId);
                Adjust.trackEvent(adjustEvent);
            }
        }
        
        public void OnCustomEvent(string eventId, Dictionary<string, string> param, AnalyticsSDK sdk)
        {
            if (sdk == AnalyticsSDK.TrackingIO || sdk == AnalyticsSDK.Default)
            {
            }

            if (IsUMengInitialized && (sdk == AnalyticsSDK.UMeng || sdk == AnalyticsSDK.Default))
            {
                Umeng.Analytics.Event(eventId, param);
            }

            if (sdk == AnalyticsSDK.Adjust || sdk == AnalyticsSDK.Default)
            {
                
            }
            
        }

        
        public void OnCustomEvent(string eventId, string key, string value, AnalyticsSDK sdk)
        {
            
            if (sdk == AnalyticsSDK.TrackingIO || sdk == AnalyticsSDK.Default)
            {
                Tracking.Instance.setEvent(eventId);
            }

            if (IsUMengInitialized && (sdk == AnalyticsSDK.UMeng || sdk == AnalyticsSDK.Default))
            {
                Umeng.Analytics.Event(eventId, new Dictionary<string, string> { [key] = value });
            }

            if (sdk == AnalyticsSDK.Adjust || sdk == AnalyticsSDK.Default)
            {
                AdjustEvent adjustEvent = new AdjustEvent(eventId);
                Adjust.trackEvent(adjustEvent);
            }
        }
        
    }
}
#endif
