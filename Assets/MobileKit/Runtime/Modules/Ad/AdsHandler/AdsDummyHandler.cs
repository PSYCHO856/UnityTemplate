using System;
using UnityEngine;

using MobileKit;
using Random = UnityEngine.Random;

namespace MobileKit
{
    public class AdsDummyHandler : Singleton<AdsDummyHandler>, IAdsHandler
    {
        private Action adVideoCallback;
        public string RewardVideoId { get; set; }

        private AdsDummyCanvas adsDummyCanvas;
        

        public void Init()
        {
            var prefab = Resources.Load<AdsDummyCanvas>("UI/AdsDummyCanvas");
            adsDummyCanvas = Instantiate(prefab);
        }

        public void ShowSplash(string sceneName)
        {
            Debug.Log(nameof(AdsDummyHandler) + " " + nameof(ShowSplash));
        }

        public void ShowBanner(string sceneName, string sceneID, BannerPos bannerPos)
        {
            if (!BuildConfig.Instance.IsScreenShotMode)
            {
                adsDummyCanvas.ShowBanner(bannerPos);
            }
        }

        public void HideBanner()
        {
            Debug.Log(nameof(HideBanner));
            adsDummyCanvas.HideBanner();
        }

        public bool IsInterVideoReady()
        {
            bool ready = Random.Range(0, 2) > 0;
            Debug.Log(nameof(IsInterVideoReady) + " " + ready);
            return true;
        }

        public void ShowInterVideo(string sceneName, string sceneID)
        {
            adsDummyCanvas.ShowInterstitial();
            AdsManager.InterVideoShowTimes++;
            Debug.Log("Ad Inter Show Count: " + AdsManager.InterVideoShowTimes);
        }

        public bool IsRewardVideoReady()
        {
            bool ready = Random.Range(0, 2) > 0;
            Debug.Log(nameof(IsRewardVideoReady) + " " + ready);
            return true;
        }

        public void ShowRewardVideo(string sceneName, string sceneID, Action callback)
        {
            Debug.Log(nameof(ShowRewardVideo) + " " + sceneName);
            RewardVideoId = sceneName;
            adVideoCallback = callback;
            adsDummyCanvas.ShowRewardedVideo();
            AdsManager.RewardVideoShowTimes++;
            Debug.Log("Ad RV Show Count: " + AdsManager.RewardVideoShowTimes);
        }

        public bool IsNativeAdReady()
        {
            bool ready = Random.Range(0, 2) > 0;
            Debug.Log(nameof(IsNativeAdReady) + " " + ready);
            return ready;
        }

        public void ShowNativeAd(string sceneName, string sceneID, int x, int y, int width, int height)
        {
            Debug.Log(nameof(ShowNativeAd) + " " + sceneName);
            adsDummyCanvas.ShowNativeAd(x, y, width, height);
        }

        public void HideNativeAd()
        {
            Debug.Log(nameof(HideNativeAd));
            adsDummyCanvas.CloseNativeAd();
        }

        
        public void OnRewardVideoWatched()
        {
            Debug.Log(nameof(OnRewardVideoWatched));
            adVideoCallback?.Invoke();
            RewardVideoId = "";
            adVideoCallback = null;
        }

    }
}
