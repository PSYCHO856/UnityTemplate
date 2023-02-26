#if YUNBU_GOOGLE_PLAY
using System;
using UnityEngine;

using Random = UnityEngine.Random;

namespace MobileKit
{
    public class AdsYunbuGooglePlayHandler : Singleton<AdsYunbuGooglePlayHandler>, IAdsHandler
    {
        private Action adVideoCallback;
        public string RewardVideoId { get; set; }

        private AdsDummyCanvas adsDummyCanvas;

        public void Init()
        {
            var prefab = Resources.Load<AdsDummyCanvas>("UI/AdsDummyCanvas");
            adsDummyCanvas = Instantiate(prefab);
        }

        public void ShowSplash()
        {
        }

        public void ShowBanner(string sceneId, string adKey, BannerPos bannerPos)
        {
            Debug.Log(nameof(ShowBanner) + " " + sceneId + " " + bannerPos);
            adsDummyCanvas.ShowBanner(bannerPos);
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

        public void ShowInterVideo(string sceneId, string adKey)
        {
            adsDummyCanvas.ShowInterstitial();
        }

        public bool IsRewardVideoReady()
        {
            bool ready = Random.Range(0, 2) > 0;
            Debug.Log(nameof(IsRewardVideoReady) + " " + ready);
            return true;
        }

        public void ShowRewardVideo(string sceneId, string adKey, Action callback)
        {
            Debug.Log(nameof(ShowRewardVideo) + " " + sceneId);
            RewardVideoId = sceneId;
            adVideoCallback = callback;
            adsDummyCanvas.ShowRewardedVideo();
        }

        public bool IsNativeAdReady()
        {
            bool ready = Random.Range(0, 2) > 0;
            Debug.Log(nameof(IsNativeAdReady) + " " + ready);
            return ready;
        }

        public void ShowNativeAd(string sceneId, string adKey, int x, int y, int width, int height)
        {
            Debug.Log(nameof(ShowNativeAd) + " " + sceneId);
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
#endif