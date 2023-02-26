#if MOBILEKIT_GOOGLEPLAY
using System;
using UnityEngine;

using Random = UnityEngine.Random;

namespace MobileKit
{
    public class AdsGooglePlayHandler : Singleton<AdsGooglePlayHandler>, IAdsHandler
    {
        private Action adVideoCallback;
        public string RewardVideoId { get; set; }

        private static AndroidJavaClass mainActivity;

        public void Init()
        {
            Debug.Log(nameof(AdsGooglePlayHandler) + " " + nameof(Init));
            mainActivity = new AndroidJavaClass("com.mobilekit.sdk.MainActivity");
        }

        public void ShowSplash(string sceneName)
        {
        }

        public void ShowMREC(string sceneId )
        {
            Debug.Log(nameof(ShowMREC) + " " + sceneId );
            mainActivity.CallStatic("ShowMREC", sceneId );
        }

        public void HideMREC()
        {
            Debug.Log(nameof(HideMREC));
            mainActivity.CallStatic("HideMREC");
        }

        public void ShowBanner(string sceneId, string adKey, BannerPos bannerPos)
        {
            Debug.Log(nameof(ShowBanner) + " " + sceneId + " " + bannerPos);
            mainActivity.CallStatic("ShowBanner", sceneId, bannerPos.ToString());
        }

        public void HideBanner()
        {
            Debug.Log(nameof(HideBanner));
            mainActivity.CallStatic("HideBanner");
        }

        public bool IsInterVideoReady()
        {
            bool isReady = mainActivity.CallStatic<bool>("IsInterstitialReady");
            Debug.Log(nameof(IsInterVideoReady) + " " + isReady);
            return isReady;
        }

        public void ShowInterVideo(string sceneId, string adKey)
        {
            Debug.Log(nameof(AdsGooglePlayHandler) + " " + nameof(ShowInterVideo) + " " + sceneId);
            mainActivity.CallStatic("ShowInterstitial", sceneId);
        }

        public bool IsRewardVideoReady()
        {
            Debug.Log(nameof(AdsGooglePlayHandler) + " " + nameof(IsRewardVideoReady));
            bool isReady = mainActivity.CallStatic<bool>("IsRewardVideoReady");
            return isReady;
        }

        public void ShowRewardVideo(string sceneId, string adKey, Action callback)
        {
            Debug.Log(nameof(ShowRewardVideo) + " " + sceneId);
            RewardVideoId = sceneId;
            adVideoCallback = callback;
            mainActivity.CallStatic("ShowRewardVideo", sceneId);
        }

        public bool IsNativeAdReady()
        {
            Debug.Log(nameof(AdsGooglePlayHandler) + " " + nameof(IsNativeAdReady));
            bool isReady = mainActivity.CallStatic<bool>("IsNativeAdReady");
            return isReady;
        }

        public void ShowNativeAd(string sceneId, string adKey, int x, int y, int width, int height)
        {
            Debug.Log(nameof(AdsGooglePlayHandler) + " " + nameof(ShowNativeAd));
            mainActivity.CallStatic("ShowNativeAd", sceneId, x, y, width, height);
        }

        public void HideNativeAd()
        {
            Debug.Log(nameof(AdsGooglePlayHandler) + " " + nameof(HideNativeAd));
            mainActivity.CallStatic("HideNativeAd");
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