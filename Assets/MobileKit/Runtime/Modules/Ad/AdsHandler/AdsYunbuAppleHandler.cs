#if YUNBU_APPLE
using System;
using System.Runtime.InteropServices;
using AnyThinkAds.Api;
using Newtonsoft.Json;
using UnityEngine;

namespace MobileKit
{
    /// <summary>
    /// TopOn(AnyThink)
    /// </summary>
    public class AdsYunbuAppleHandler : Singleton<AdsYunbuAppleHandler>, IAdsHandler
    {
        public string RewardVideoId { get; set; }
        private bool IsInitialized;
        public void Init()
        {

            ATSDKAPI.setChannel("AppStore");
            ATSDKAPI.setSubChannel("Yunbu");
            // ATSDKAPI.setLogDebug(AppConfig.AdDebug);
            ATSDKAPI.getUserLocation(new GetLocationListener());
            // ATSDKAPI.initSDK(AppConfig.TopOnAppId, AppConfig.TopOnAppKey, this);
        }

        public void OnTopOnInitted()
        {
            InitTopOnAds();
        }

        // public void initSuccess()
        // {
        //     Debug.Log("Developer Develop callback SDK initSuccess");
        //     InitTopOnAds();
        // }

        // public void initFail(string message)
        // {
        //     Debug.Log("Developer callback SDK initFail:" + message);
        // }

        private void InitTopOnAds()
        {
            AdsTopOnBanner.Instance.Init(AppConfig.Instance.TopOnBannerPlacementId);
            AdsTopOnInterstitial.Instance.Init(AppConfig.Instance.TopOnInterPlacementId);
            AdsTopOnRewardedVideo.Instance.Init(AppConfig.Instance.TopOnRewardVideoPlacementId);
            IsInitialized = true;
        }

        [DllImport ( "__Internal" )]
        private static extern bool _ShowSplashAd();


        public void ShowSplash()
        {
            _ShowSplashAd();
        }
        
        public void ShowBanner(string sceneId, string adKey, BannerPos bannerPos)
        {
            if (!IsInitialized) return;
            AdsTopOnBanner.Instance.ShowBannerAd(sceneId, adKey, bannerPos);
        }

        public void HideBanner()
        {
            if (!IsInitialized) return;
            AdsTopOnBanner.Instance.HideBannerAd();
        }

        public bool IsInterVideoReady()
        {
            if (!IsInitialized) return false;
            return AdsTopOnInterstitial.Instance.IsInterReady;
        }

        public void ShowInterVideo(string sceneId, string adKey)
        {
            if (!IsInitialized) return;
            if (AdsTopOnInterstitial.Instance.IsInterReady == false) return;
            AdsTopOnInterstitial.Instance.ShowInterstitialAd(sceneId, adKey);
        }

        public bool IsRewardVideoReady()
        {
            if (!IsInitialized) return false;
            return AdsTopOnRewardedVideo.Instance.IsRewardVideoReady;
        }

        public void ShowRewardVideo(string sceneId, string adKey, Action callback)
        {
            Debug.Log("ShowRewardVideo apple handler. start");

            if (!IsInitialized) return;
            AdsTopOnRewardedVideo.Instance.ShowRewardedVideo(sceneId, adKey, callback);

            Debug.Log("ShowRewardVideo apple handler. end");
        }

        public bool IsNativeAdReady()
        {
            return false;
        }

        public void ShowNativeAd(string sceneId, string adKey, int x, int y, int width, int height)
        {
        }

        public void HideNativeAd()
        {
        }


        private class GetLocationListener : ATGetUserLocationListener
        {
            public void didGetUserLocation(int location)
            {
                Debug.Log("Developer callback didGetUserLocation(): " + location);
                // if (location == ATSDKAPI.kATUserLocationInEU && ATSDKAPI.getGDPRLevel() == ATSDKAPI.UNKNOWN)
                // {
                //     ATSDKAPI.showGDPRAuth();
                // }
            }
        }

        public void OnSplashShow(string jsonInfo)
        {
            var adInfo = JsonConvert.DeserializeObject<AdJsonInfo>(jsonInfo);
            if (adInfo != default)
            {
                Tracking.Instance.setTrackAdShow(adInfo.adPlatform, adInfo.adId, true);
            }
        }

        public void OnSplashClick(string jsonInfo)
        {
            var adInfo = JsonConvert.DeserializeObject<AdJsonInfo>(jsonInfo);
            if (adInfo != default)
            {
                Tracking.Instance.setTrackAdClick(adInfo.adPlatform, adInfo.adId);
            }
        }
        
        public class AdJsonInfo
        {
            public string adPlatform;
            public string adId;
        }
    }
}
#endif
