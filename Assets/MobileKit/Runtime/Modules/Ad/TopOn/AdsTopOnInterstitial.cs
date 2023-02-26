#if YUNBU_APPLE 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using AnyThinkAds.Api;
using MobileKit;

namespace MobileKit
{

    public class AdsTopOnInterstitial : Singleton<AdsTopOnInterstitial>, ATInterstitialAdListener
    {

        private string PlacementId = "";
        private float lastCheckTime;
        private float lastLoadTime;

        private string sceneId;
        
        public bool IsInterReady { get; private set; }
        
        public void Init(string placementId)
        {
            this.PlacementId = placementId;
            ATInterstitialAd.Instance.setListener(this);
            LoadInterAd();
        }
        
        private void LoadInterAd()
        {
            var jsonmap = new Dictionary<string, object>();
            ATInterstitialAd.Instance.loadInterstitialAd(PlacementId, jsonmap);
            
        #if YUNBU_APPLE
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("request", "ins", "default");
        #endif
            lastLoadTime = Time.realtimeSinceStartup;
        }
        
        private void CheckInterStatus()
        {
            IsInterReady = ATInterstitialAd.Instance.hasInterstitialAdReady(PlacementId);
            lastCheckTime = Time.realtimeSinceStartup;
        }
        
        private void Update()
        {
            if (IsInterReady == false &&
                Time.realtimeSinceStartup - lastCheckTime > AppConfig.Instance.AdCheckInterval)
            {
                CheckInterStatus();    
            }

            if (IsInterReady == false &&
                Time.realtimeSinceStartup - lastLoadTime > AppConfig.Instance.AdLoadInterval)
            {
                LoadInterAd();
            }
        }


        public void ShowInterstitialAd(string sceneId, string adKey)
        {
            this.sceneId = sceneId;
            if (!IsInterReady) return;
            var jsonmap = new Dictionary<string, string> {
            {
                ATConst.SCENARIO, adKey
            }};

            ATInterstitialAd.Instance.showInterstitialAd(PlacementId, jsonmap);
        }

        public void onInterstitialAdClick(string placementId, ATCallbackInfo callbackInfo)
        {
        #if YUNBU_APPLE
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("click", "ins", sceneId);
        #endif
            Debug.Log("Developer callback onInterstitialAdClick :" + placementId);
        }

        public void onInterstitialAdClose(string placementId, ATCallbackInfo callbackInfo)
        {
        #if YUNBU_APPLE
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("close", "ins", sceneId);
        #endif
            AudioManager.Volume = 1;
            IsInterReady = false;
            LoadInterAd();
            Debug.Log("Developer callback onInterstitialAdClose :" + placementId);
        }

        public void onInterstitialAdEndPlayingVideo(string placementId, ATCallbackInfo callbackInfo)
        {
            Debug.Log("Developer callback onInterstitialAdEndPlayingVideo :" + placementId);
        }

        public void onInterstitialAdFailedToPlayVideo(string placementId, string code, string message)
        {
            Debug.Log("Developer callback onInterstitialAdFailedToPlayVideo :" + placementId + "--code:" + code +
                      "--msg:" + message);
        }

        public void onInterstitialAdLoad(string placementId)
        {
        #if YUNBU_APPLE
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("filled", "ins", "default");
        #endif
            Debug.Log("Developer callback onInterstitialAdLoad :" + placementId);
            CheckInterStatus();
        }

        public void onInterstitialAdLoadFail(string placementId, string code, string message)
        {
            Debug.Log("Developer callback onInterstitialAdLoadFail :" + placementId + "--code:" + code + "--msg:" + message);
        }

        public void onInterstitialAdShow(string placementId, ATCallbackInfo callbackInfo)
        {
        #if YUNBU_APPLE
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("show", "ins", sceneId);
        #endif
            AudioManager.Volume = 0;
            SendInterVideoAdShowEvent();
            Debug.Log("Developer callback onInterstitialAdShow :" + placementId);
        }

        public void onInterstitialAdStartPlayingVideo(string placementId, ATCallbackInfo callbackInfo)
        {
            Debug.Log("Developer callback onInterstitialAdStartPlayingVideo :" + placementId);
        }

        public void onInterstitialAdFailedToShow(string placementId)
        {
            Debug.Log("Developer callback onInterstitialAdFailedToShow :" + placementId);
        }

        public static void SendInterVideoAdShowEvent()
        {
#if YUNBU_APPLE
            UMengAdEventConfig umengAdEventConfig = AnalyticsYunbuAppleHandler.UMengAdEventConfig;
            AdjustAdEventConfig adjustAdEventConfig = AnalyticsYunbuAppleHandler.AdjustAdEventConfig;
            TrackingIOAdEventConfig trackingIOAdEventConfig = AnalyticsYunbuAppleHandler.TrackingIOAdEventConfig;
            AdsManager.TodayInterVideoShowTimes++;
            AnalyticsManager.OnCustomEvent(umengAdEventConfig.ShowInterVideoSuccess, AnalyticsSDK.UMeng);
            AnalyticsManager.OnCustomEvent(adjustAdEventConfig.ShowInterVideoSuccess, AnalyticsSDK.Adjust);
            AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.ShowInterVideoSuccess, AnalyticsSDK.TrackingIO);

            switch (AdsManager.TodayInterVideoShowTimes)
            {
                case 1:
                    AnalyticsManager.OnCustomEvent(umengAdEventConfig.ShowInterVideoTimes_1, AnalyticsSDK.UMeng);
                    AnalyticsManager.OnCustomEvent(adjustAdEventConfig.ShowInterVideoTimes_1, AnalyticsSDK.Adjust);
                    AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.ShowInterVideoTimes_1, AnalyticsSDK.TrackingIO);
                    break;
                case 2:
                    AnalyticsManager.OnCustomEvent(umengAdEventConfig.ShowInterVideoTimes_2, AnalyticsSDK.UMeng);
                    AnalyticsManager.OnCustomEvent(adjustAdEventConfig.ShowInterVideoTimes_2, AnalyticsSDK.Adjust);
                    AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.ShowInterVideoTimes_2, AnalyticsSDK.TrackingIO);
                    break;
                case 3:
                    AnalyticsManager.OnCustomEvent(umengAdEventConfig.ShowInterVideoTimes_3, AnalyticsSDK.UMeng);
                    AnalyticsManager.OnCustomEvent(adjustAdEventConfig.ShowInterVideoTimes_3, AnalyticsSDK.Adjust);
                    AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.ShowInterVideoTimes_3, AnalyticsSDK.TrackingIO);
                    break;
                case 5:
                    AnalyticsManager.OnCustomEvent(umengAdEventConfig.ShowInterVideoTimes_5, AnalyticsSDK.UMeng);
                    AnalyticsManager.OnCustomEvent(adjustAdEventConfig.ShowInterVideoTimes_5, AnalyticsSDK.Adjust);
                    AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.ShowInterVideoTimes_5, AnalyticsSDK.TrackingIO);
                    break;
                case 10:
                    AnalyticsManager.OnCustomEvent(umengAdEventConfig.ShowInterVideoTimes_10, AnalyticsSDK.UMeng);
                    AnalyticsManager.OnCustomEvent(adjustAdEventConfig.ShowInterVideoTimes_10, AnalyticsSDK.Adjust);
                    AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.ShowInterVideoTimes_10, AnalyticsSDK.TrackingIO);
                    break;
            }

            if (DateTime.Today == TimeManager.RegisterDay)
            {
                switch (AdsManager.TodayInterVideoShowTimes)
                {
                      case 1:
                          AnalyticsManager.OnCustomEvent(umengAdEventConfig.FirstDay_ShowInterVideoTimes_1, AnalyticsSDK.UMeng);
                          AnalyticsManager.OnCustomEvent(adjustAdEventConfig.FirstDay_ShowInterVideoTimes_1, AnalyticsSDK.Adjust);
                          AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.FirstDay_ShowInterVideoTimes_1, AnalyticsSDK.TrackingIO);
                          break;
                      case 2:
                          AnalyticsManager.OnCustomEvent(umengAdEventConfig.FirstDay_ShowInterVideoTimes_2, AnalyticsSDK.UMeng);
                          AnalyticsManager.OnCustomEvent(adjustAdEventConfig.FirstDay_ShowInterVideoTimes_2, AnalyticsSDK.Adjust);
                          AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.FirstDay_ShowInterVideoTimes_2, AnalyticsSDK.TrackingIO);
                          break;
                      case 3:
                          AnalyticsManager.OnCustomEvent(umengAdEventConfig.FirstDay_ShowInterVideoTimes_3, AnalyticsSDK.UMeng);
                          AnalyticsManager.OnCustomEvent(adjustAdEventConfig.FirstDay_ShowInterVideoTimes_3, AnalyticsSDK.Adjust);
                          AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.FirstDay_ShowInterVideoTimes_3, AnalyticsSDK.TrackingIO);
                          break;
                      case 5:
                          AnalyticsManager.OnCustomEvent(umengAdEventConfig.FirstDay_ShowInterVideoTimes_5, AnalyticsSDK.UMeng);
                          AnalyticsManager.OnCustomEvent(adjustAdEventConfig.FirstDay_ShowInterVideoTimes_5, AnalyticsSDK.Adjust);
                          AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.FirstDay_ShowInterVideoTimes_5, AnalyticsSDK.TrackingIO);
                          break;
                      case 10:
                          AnalyticsManager.OnCustomEvent(umengAdEventConfig.FirstDay_ShowInterVideoTimes_10, AnalyticsSDK.UMeng);
                          AnalyticsManager.OnCustomEvent(adjustAdEventConfig.FirstDay_ShowInterVideoTimes_10, AnalyticsSDK.Adjust);
                          AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.FirstDay_ShowInterVideoTimes_10, AnalyticsSDK.TrackingIO);
                          break;
                }
            }
#endif
        }
    }
}
#endif
