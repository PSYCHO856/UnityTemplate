#if YUNBU_ANDROID
using System;
using UnityEngine;

namespace MobileKit
{
    public class AdsYunbuAndroidHandler : Singleton<AdsYunbuAndroidHandler>, IAdsHandler
    {
        private static AndroidJavaClass mainActivity;
        
        private Action adVideoCallback;
        
        public string RewardVideoId { get; set; }

        public void Init()
        {
            mainActivity = new AndroidJavaClass("com.feamber.MainActivity");
            mainActivity.CallStatic("LoadInterstitial");
            mainActivity.CallStatic("LoadRewardVideo");
            mainActivity.CallStatic("LoadFullScreenVideo");
            mainActivity.CallStatic("LoadNativeAd");
        }

        public void ShowSplash()
        {
        }

        public void ShowBanner(string sceneId, string adKey, BannerPos bannerPos)
        {
            Debug.Log(nameof(AdsYunbuAndroidHandler) + " " + nameof(ShowBanner) + sceneId + " " + bannerPos);
            mainActivity.CallStatic("ShowBanner", sceneId, bannerPos.ToString());
        }

        public void HideBanner()
        {
            Debug.Log(nameof(AdsYunbuAndroidHandler) + " " + nameof(HideBanner));
            mainActivity.CallStatic("HideBanner");
        }

        public bool IsInterVideoReady()
        {
            Debug.Log(nameof(AdsYunbuAndroidHandler) + " " + nameof(IsInterVideoReady));
            bool isReady = mainActivity.CallStatic<bool>("IsFullScreenVideoReady");
            return isReady;

        }

        public void ShowInterVideo(string sceneId, string adKey)
        {
            // Debug.Log("ShowInterVideo "+ "IsInterVideoReady " +IsInterVideoReady() +"sceneId "+sceneId);
            Debug.Log(nameof(AdsYunbuAndroidHandler) + " " + nameof(ShowInterVideo) + " " + sceneId);
            mainActivity.CallStatic("ShowFullScreenVideo", sceneId);
        }

        public bool IsRewardVideoReady()
        {
            bool isReady = mainActivity.CallStatic<bool>("IsRewardVideoReady");
            Debug.Log(nameof(AdsYunbuAndroidHandler) + " " + nameof(IsRewardVideoReady) + " " + isReady);
            return isReady;
        }

        private bool CanDisplayRewardVideo()
        {
            bool isReady = mainActivity.CallStatic<bool>("IsRewardVideoSwitchON");
            return isReady;
        }
        
        public void ShowRewardVideo(string sceneId, string adKey, Action callback)
        {
            if (!CanDisplayRewardVideo()) return;
            Debug.Log(nameof(AdsYunbuAndroidHandler) + " " + nameof(ShowRewardVideo) + " " + sceneId);
            RewardVideoId = sceneId;
            adVideoCallback = callback;
            mainActivity.CallStatic("ShowRewardVideo", sceneId);
        }

        public bool IsNativeAdReady()
        {
            Debug.Log(nameof(AdsYunbuAndroidHandler) + " " + nameof(IsNativeAdReady));
            bool isReady = mainActivity.CallStatic<bool>("IsNativeAdReady");
            return isReady;
        }

        public void ShowNativeAd(string sceneId, string adKey, int x, int y, int width, int height)
        {
            Debug.Log(nameof(AdsYunbuAndroidHandler) + " " + nameof(ShowNativeAd));
            mainActivity.CallStatic("ShowNativeAd", sceneId, x, y, width, height);
        }

        public void HideNativeAd()
        {
            Debug.Log(nameof(AdsYunbuAndroidHandler) + " " + nameof(HideNativeAd));
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