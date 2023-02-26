using System;
using UnityEngine;

namespace MobileKit
{ 
    public enum BannerPos
    {
        Bottom,
        Top
    }

    public static class AdsManager
    {
        public static bool IsAdFree
        {
            get => GameSettingsPrefs.GetBool("IsAdFree", false);
            set
            {
                GameSettingsPrefs.SetBool("IsAdFree", value);
                if (value)
                {
                    HideBanner();
                }
            }
        }

        // 不能一开始就弹开屏广告, 防止Apple GuideLine4.0 被拒（不能在还未进行游戏之前显示广告）
        public static bool CanShowSplash
        {
            get => GameSettingsPrefs.GetBool("CanShowSplash", false);
            set => GameSettingsPrefs.SetBool("CanShowSplash", value);
        }


        private static bool hideAllAds;
        public static bool HideAllAds
        {
            get => hideAllAds;
            set
            {
                hideAllAds = value;
                if (value)
                {
                    HideBanner();
                }
            }
        }
        
        public static int InterVideoShowTimes
        {
            get => GameSettingsPrefs.GetInt("InterVideoShowTimes", 0);
            set => GameSettingsPrefs.SetInt("InterVideoShowTimes", value);
        }
        
        public static int RewardVideoShowTimes
        {
            get => GameSettingsPrefs.GetInt("RewardVideoShowTimes", 0);
            set => GameSettingsPrefs.SetInt("RewardVideoShowTimes", value);
        }

        public static int SplashShowTimes
        {
            get => GameSettingsPrefs.GetInt("SplashShowTimes", 0);
            set => GameSettingsPrefs.SetInt("SplashShowTimes", value);
        }
        
        
        public static int DayInterVideoShowTimes
        {
            get => GameSettingsPrefs.GetInt("DayInterVideoShowTimes", 0);
            set => GameSettingsPrefs.SetInt("DayInterVideoShowTimes", value);
        }
        
        public static int DayRewardVideoShowTimes
        {
            get => GameSettingsPrefs.GetInt("DayRewardVideoShowTimes", 0);
            set => GameSettingsPrefs.SetInt("DayRewardVideoShowTimes", value);
        }

        public static int DaySplashShowTimes
        {
            get => GameSettingsPrefs.GetInt("DaySplashShowTimes", 0);
            set => GameSettingsPrefs.SetInt("DaySplashShowTimes", value);
        }
        
        
        private static IAdsHandler adsHandler;

        public static DateTime LastShowAdTime = DateTime.Now;
        
        public static void Init()
        {
            if (DateTime.Today.ToString("yy-MM-dd") != TimeManager.LeaveTime.ToString("yy-MM-dd"))
            {
                DaySplashShowTimes = 0;
                DayInterVideoShowTimes = 0;
                DayRewardVideoShowTimes = 0;
            }
#if UNITY_EDITOR
            adsHandler = AdsDummyHandler.Instance;
#elif MOBILEKIT_GOOGLEPLAY
            adsHandler = AdsGooglePlayHandler.Instance;
            // adsHandler = AdsDummyHandler.Instance;
#elif MOBILEKIT_IOS_US
            adsHandler = AdsDummyHandler.Instance;
#elif MOBILEKIT_IOS_CN
            adsHandler = AdsDummyHandler.Instance;
#else
            adsHandler = AdsDummyHandler.Instance;
#endif
            if (!BuildConfig.Instance.IsScreenShotMode)
            {
                adsHandler.Init();
            }
            if (BuildConfig.Instance.Debug)
            {
                GMManager.OnDrawInstruct += RegisterGMHelper;
            }
            LastShowAdTime = DateTime.Now - TimeSpan.FromSeconds(80f);
        }

        public static void ShowSplash(string sceneName)
        {
            if (HideAllAds || IsAdFree || BuildConfig.Instance.IsScreenShotMode) return;
            if (!CanShowSplash)
            {
                CanShowSplash = true;
                return;
            }
            adsHandler?.ShowSplash(sceneName);
        }
        
        public static void ShowBanner(string sceneName, string sceneID = "", BannerPos bannerPos = BannerPos.Bottom)
        {
            if (HideAllAds || IsAdFree || BuildConfig.Instance.IsScreenShotMode) return;
            adsHandler?.ShowBanner(sceneName, sceneID, bannerPos);
        } 

        public static void HideBanner()
        {
            adsHandler?.HideBanner();
        }

        public static bool IsInterVideoReady()
        {
            if (adsHandler == null) return false;
            if (HideAllAds || IsAdFree || BuildConfig.Instance.IsScreenShotMode) return false;
            if (!BuildConfig.Instance.Debug && (DateTime.Now - LastShowAdTime).TotalSeconds <= 30f) return false;
            return adsHandler.IsInterVideoReady();
        }
        
        /// <summary>
        ///  显示插屏广告
        /// </summary>
        /// <param name="sceneName">方便阅读的场景名，便于统计等</param>
        /// <param name="sceneID">广告平台场景id</param>
        /// <param name="showTips">是否显示5秒可关闭提示</param>
        public static void ShowInterVideo(string sceneName, string sceneID = "", bool showTips = false)
        {
            if (HideAllAds || IsAdFree || BuildConfig.Instance.IsScreenShotMode) return;
            adsHandler?.ShowInterVideo(sceneName, sceneID);
            LastShowAdTime = DateTime.Now;
        }
        
        public static bool IsRewardVideoReady()
        {
            if (adsHandler == null) return false;
            return adsHandler.IsRewardVideoReady();
        }

        public static void ShowRewardVideo(Action adVideoCallback, string sceneName, string sceneID = "")
        {
            if (HideAllAds || BuildConfig.Instance.IsScreenShotMode)
            {
                adVideoCallback?.Invoke();
                return;
            }
            adsHandler?.ShowRewardVideo(sceneName, sceneID, adVideoCallback);
            LastShowAdTime = DateTime.Now;
        }

        /// <summary>
        /// 屏幕左上角为手机屏幕坐标原点(0,0)
        /// 所有单位都是像素
        /// </summary>
        /// <param name="sceneName">场景id</param>
        /// <param name="sceneID">广告id (比如topon Id)</param>
        /// <param name="x">横向坐标，原生广告区域左上角，距离屏幕原点的横向距离</param>
        /// <param name="y">纵向坐标，原生广告区域左上角，距离屏幕原点的纵向距离</param>
        /// <param name="width">原生广告宽度</param>
        /// <param name="height">原生广告高度</param>
        public static void ShowNativeAd(string sceneName, string sceneID, int x, int y, int width, int height)
        {
            if (HideAllAds) return;
            if (IsAdFree) return;
            if (string.IsNullOrEmpty(sceneID)) return;
            adsHandler?.ShowNativeAd(sceneName, sceneID, x, y, width, height);
        }

        public static void HideNativeAd()
        {
            adsHandler?.HideNativeAd();
        }
        
        public static void RegisterGMHelper()
        {
            GUILayout.Space(10);
            GUILayout.Label("去除激励之外的广告:  " + IsAdFree);
            GUILayout.Label("去除所有广告: " + HideAllAds);
            GUILayout.Space(5);

            if (GUILayout.Button("开/关 激励之外的广告"))
            {
                IsAdFree = !IsAdFree;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("开/关 所有广告"))
            {
                HideAllAds = !HideAllAds;
            }
            GUILayout.Space(10);
        }
    }
}
