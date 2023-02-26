using System;
using UnityEngine;

namespace MobileKit
{
    public  interface IAdsHandler
    {
        public string RewardVideoId { get; set; }
        
        public void Init();

        public void ShowSplash(string sceneName);
        
        public void ShowBanner(string sceneName, string sceneID, BannerPos bannerPos);
        public void HideBanner();
        
        public bool IsInterVideoReady();
        public void ShowInterVideo(string sceneName, string sceneID);
        
        public bool IsRewardVideoReady();
        public void ShowRewardVideo(string sceneName, string sceneID, Action callback);

        public bool IsNativeAdReady();
        public void ShowNativeAd(string sceneName, string sceneID, int x, int y, int width, int height);
        public void HideNativeAd();
        
    }
}
