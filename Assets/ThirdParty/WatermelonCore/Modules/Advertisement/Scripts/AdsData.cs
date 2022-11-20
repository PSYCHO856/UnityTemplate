using System;
using UnityEngine;

namespace Watermelon
{
    [SetupTab("Advertising", texture = "icon_ads")]
    [CreateAssetMenu(fileName = "Ads Settings", menuName = "Settings/Ads Settings")]
    public class AdsData : ScriptableObject
    {
        public AdvertisingModules bannerType = AdvertisingModules.Dummy; // 横幅广告
        public AdvertisingModules interstitialType = AdvertisingModules.Dummy; // 插屏广告
        public AdvertisingModules rewardedVideoType = AdvertisingModules.Dummy; // 奖励广告

        // Providers
        public AdMobContainer adMobContainer;
        public UnityAdsContainer unityAdsContainer;
        public DummyContainer dummyContainer;

        public GDPRContainer gdprContainer;

        public AdsFrequency adsFrequency;

        public bool testMode;
        public bool systemLogs;

        public bool IsDummyEnabled()
        {
            if (bannerType == AdvertisingModules.Dummy)
                return true;

            if (interstitialType == AdvertisingModules.Dummy)
                return true;

            if (rewardedVideoType == AdvertisingModules.Dummy)
                return true;

            return false;
        }

        [Serializable]
        public class AdsFrequency
        {
            [Tooltip("Delay in seconds before first interstitial appearings.")]
            public float insterstitialFirstDelay = 40f;

            [Tooltip("Delay in seconds between interstitial appearings.")]
            public float interstitialShowingDelay = 30f;
        }

        [Serializable]
        public class GDPRContainer
        {
            public bool enableGDPR;
            public string privacyLink = "";
        }

        [Serializable]
        public class UnityAdsContainer
        {
            public enum BannerPosition
            {
                TOP_LEFT = 0,
                TOP_CENTER = 1,
                TOP_RIGHT = 2,
                BOTTOM_LEFT = 3,
                BOTTOM_CENTER = 4,
                BOTTOM_RIGHT = 5,
                CENTER = 6
            }

            //Application ID
            [Header("Application ID")] public string androidAppID = "1234567";

            public string IOSAppID = "1234567";

            //Banned ID
            [Header("Banner ID")] public string androidBannerID = "banner";

            public string IOSBannerID = "banner";

            //Interstitial ID
            [Header("Interstitial ID")] public string androidInterstitialID = "video";

            public string IOSInterstitialID = "video";

            //Rewarder Video ID
            [Header("Rewarded Video ID")] public string androidRewardedVideoID = "rewardedVideo";

            public string IOSRewardedVideoID = "rewardedVideo";

            [Space] public BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;
        }

        [Serializable]
        public class DummyContainer
        {
            public BannerPosition bannerPosition = BannerPosition.Bottom;
        }

        [Serializable]
        public class AdMobContainer
        {
            public enum BannerType
            {
                Banner = 0,
                MediumRectangle = 1,
                IABBanner = 2,
                Leaderboard = 3,
                SmartBanner = 4
            }

            //Banned ID
            [Header("Banner ID")] public string androidBannerID = "ca-app-pub-3940256099942544/6300978111";

            public string IOSBannerID = "ca-app-pub-3940256099942544/2934735716";

            //Interstitial ID
            [Header("Interstitial ID")] public string androidInterstitialID = "ca-app-pub-3940256099942544/1033173712";

            public string IOSInterstitialID = "ca-app-pub-3940256099942544/4411468910";

            //Rewarder Video ID
            [Header("Rewarded Video ID")]
            public string androidRewardedVideoID = "ca-app-pub-3940256099942544/5224354917";

            public string IOSRewardedVideoID = "ca-app-pub-3940256099942544/1712485313";

            [Space] public BannerType bannerType = BannerType.Banner;

            public BannerPosition bannerPosition = BannerPosition.Bottom;
        }
    }

    public enum BannerPosition
    {
        Bottom = 0,
        Top = 1
    }
}

// -----------------
// Advertisement v 0.3
// -----------------