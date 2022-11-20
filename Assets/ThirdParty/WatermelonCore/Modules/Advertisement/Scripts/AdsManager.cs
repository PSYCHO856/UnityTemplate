#pragma warning disable 0649
#pragma warning disable 0162

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Watermelon
{
    [Define("MODULE_ADMOB")]
    [Define("MODULE_UNITYADS")]
    public class AdsManager
    {
        private static AdsManager instance;

        private AdsData settings;
        public static AdsData Settings => instance.settings;

        private GameObject dummyCanvasPrefab;

        public static bool IsInititalized { get; set; }

        // Fired when module is initialized
        public static OnAdsModuleInitializedCallback OnAdsModuleInitializedEvent;

        // Fired when a banner is loaded
        public static OnAdsCallback OnBannerAdLoadedEvent;

        // Fired when a banner has failed to load
        public static OnAdsCallback OnBannerAdLoadFailedEvent;

        // Fired when a banner ad is clicked
        public static OnAdsCallback OnBannerAdClickedEvent;

        // Fired when an interstitial ad is loaded and ready to be shown
        public static OnAdsCallback OnInterstitialLoadedEvent;

        // Fired when an interstitial ad fails to load
        public static OnAdsCallback OnInterstitialLoadFailedEvent;

        // Fired when an interstitial ad is dismissed
        public static OnAdsCallback OnInterstitialHiddenEvent;

        // Fired when an interstitial ad is displayed (may not be received by Unity until the interstitial closes)
        public static OnAdsCallback OnInterstitialDisplayedEvent;

        // Fired when an interstitial ad is clicked (may not be received by Unity until the interstitial closes)
        public static OnAdsCallback OnInterstitialClickedEvent;

        // Fired when a rewarded ad finishes loading and is ready to be displayed
        public static OnAdsCallback OnRewardedAdLoadedEvent;

        // Fired when a rewarded ad fails to load. Includes the error message.
        public static OnAdsCallback OnRewardedAdLoadFailedEvent;

        // Fired when an rewarded ad is displayed (may not be received by Unity until the rewarded ad closes)
        public static OnAdsCallback OnRewardedAdDisplayedEvent;

        // Fired when an rewarded ad is hidden
        public static OnAdsCallback OnRewardedAdHiddenEvent;

        // Fired when an rewarded video is clicked (may not be received by Unity until the rewarded ad closes)
        public static OnAdsCallback OnRewardedAdClickedEvent;

        // Fired when a rewarded video completes. Includes information about the reward
        public static OnAdsCallback OnRewardedAdReceivedRewardEvent;

        public static IsAdsReadyCallback ExtraInterstitialReadyConditions;
        public static IsAdsReadyCallback ExtraBannerReadyConditions;

        private static double lastInterstitialTime;

        private readonly AdvertisingHandler[] advertisingModules =
        {
            new DummyHandler(AdvertisingModules.Dummy), // Dummy

#if MODULE_ADMOB
            new AdMobHandler(AdvertisingModules.AdMob), // AdMob module
#endif

#if MODULE_UNITYADS
            new UnityAdsHandler(AdvertisingModules.UnityAds), // Unity Ads module
#endif
        };

        private static Dictionary<AdvertisingModules, AdvertisingHandler> advertisingLink = new();

        #region Init

        public void Init(AdsManagerInitModule adsManagerInitModule)
        {
            if (instance != null)
            {
                Debug.LogWarning("[AdsManager]: Module already exists!");

                return;
            }

            instance = this;

            settings = adsManagerInitModule.settings;
            dummyCanvasPrefab = adsManagerInitModule.dummyCanvasPrefab;

            if (settings == null)
            {
                Debug.LogError("[AdsManager]: Settings don't exist!");

                return;
            }

            InitDummy();

            lastInterstitialTime = TimeUtils.GetCurrentUnixTimestamp() + settings.adsFrequency.insterstitialFirstDelay;

            // Callbacks
            OnInterstitialDisplayedEvent += OnInterstitialDisplayed;
            OnInterstitialHiddenEvent += OnInterstitialHidden;
            OnRewardedAdHiddenEvent += OnRewardedAdHidden;
            OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedReward;

            // Validate
            ExtraInterstitialReadyConditions += CheckInterstitialTime;

#if MODULE_IAP
            IAPManager.OnPurchaseComplete += OnPurchaseComplete;
#endif

            if (settings.gdprContainer.enableGDPR)
            {
                if (GDPRController.IsGDPRStateExist()) InitModules(GDPRController.GetGDPRState());
            }
            else
            {
                InitModules(false);
            }
        }

        public void InitModules(bool gdprState)
        {
            if (settings.systemLogs)
                Debug.Log("[AdsManager]: Init modules");

            // Initialize all advertising modules
            advertisingLink = new Dictionary<AdvertisingModules, AdvertisingHandler>();
            for (var i = 0; i < instance.advertisingModules.Length; i++)
            {
                instance.advertisingModules[i].Init(instance.settings);
                instance.advertisingModules[i].SetGDPR(gdprState);

                advertisingLink.Add(instance.advertisingModules[i].ModuleType, instance.advertisingModules[i]);
            }

            IsInititalized = true;
        }

        private void InitDummy()
        {
            // Inititalize dummy controller
            if (settings.IsDummyEnabled())
            {
                if (dummyCanvasPrefab != null)
                {
                    var dummyCanvas = Object.Instantiate(dummyCanvasPrefab);
                    dummyCanvas.transform.position = Vector3.zero;
                    dummyCanvas.transform.localScale = Vector3.one;
                    dummyCanvas.transform.rotation = Quaternion.identity;

                    var adsDummyController = dummyCanvas.GetComponent<AdsDummyController>();
                    adsDummyController.Init(settings);

                    var dummyHandler = (DummyHandler) Array.Find(advertisingModules,
                        x => x.ModuleType == AdvertisingModules.Dummy);
                    if (dummyHandler != null)
                        dummyHandler.SetDummyController(adsDummyController);
                }
                else
                {
                    Debug.LogError("[AdsManager]: Dummy controller can't be null!");
                }
            }
        }

        #endregion

        public static bool IsModuleActive(AdvertisingModules advertisingModules)
        {
            if (!IsInititalized)
                return false;

            if (advertisingModules == AdvertisingModules.Disable)
                return false;

            return advertisingLink.ContainsKey(advertisingModules);
        }

        public static void SetGDPR(bool state)
        {
            if (!IsInititalized)
                instance.InitModules(state);
            else
                for (var i = 0; i < instance.advertisingModules.Length; i++)
                    instance.advertisingModules[i].SetGDPR(state);
        }

        #region Interstitial

        public static bool IsInterstitialLoaded()
        {
            return IsInterstitialLoaded(instance.settings.interstitialType);
        }

        public static bool IsInterstitialLoaded(AdvertisingModules advertisingModules)
        {
            if (!IsModuleActive(advertisingModules))
                return false;

            return advertisingLink[advertisingModules].IsInterstitialLoaded();
        }

        public static void RequestInterstitial()
        {
            RequestInterstitial(instance.settings.interstitialType);
        }

        public static void RequestInterstitial(AdvertisingModules advertisingModules)
        {
            if (!IsModuleActive(advertisingModules))
                return;

            if (advertisingLink[advertisingModules].IsInterstitialLoaded())
                return;

            advertisingLink[advertisingModules].RequestInterstitial();
        }

        public static void ShowInterstitial(AdvertisingHandler.InterstitialCallback callback)
        {
            ShowInterstitial(instance.settings.interstitialType, callback);
        }

        public static void ShowInterstitial(AdvertisingModules advertisingModules,
            AdvertisingHandler.InterstitialCallback callback)
        {
            if (!IsModuleActive(advertisingModules))
            {
                if (callback != null)
                    callback.Invoke(false);

                return;
            }

            if (!CheckExtraInterstitialCondition())
            {
                if (callback != null)
                    callback.Invoke(false);

                return;
            }

            if (!advertisingLink[advertisingModules].IsInterstitialLoaded())
            {
                if (callback != null)
                    callback.Invoke(false);

#if MODULE_GA
                GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.FailedShow, GameAnalyticsSDK.GAAdType.Interstitial, advertisingModules.ToString(), advertisingLink[advertisingModules].GetInterstitialID());
#endif

                return;
            }

            advertisingLink[advertisingModules].ShowInterstitial(callback);
        }

        public static void ResetInterstitialDelayTime()
        {
            lastInterstitialTime = TimeUtils.GetCurrentUnixTimestamp() +
                                   instance.settings.adsFrequency.interstitialShowingDelay;
        }

        private bool CheckInterstitialTime()
        {
            if (settings.systemLogs)
                Debug.Log("[AdsManager]: Interstitial Time: " + lastInterstitialTime + "; Time: " +
                          TimeUtils.GetCurrentUnixTimestamp());

            return lastInterstitialTime < TimeUtils.GetCurrentUnixTimestamp();
        }

        public static bool CheckExtraInterstitialCondition()
        {
            if (ExtraInterstitialReadyConditions != null)
            {
                var state = true;

                var listDelegates = ExtraInterstitialReadyConditions.GetInvocationList();
                for (var i = 0; i < listDelegates.Length; i++)
                    if (!(bool) listDelegates[i].DynamicInvoke())
                    {
                        state = false;

                        break;
                    }

                if (instance.settings.systemLogs)
                    Debug.Log("[AdsManager]: Extra condition interstitial state: " + state);

                return state;
            }

            return true;
        }

        #endregion

        #region Rewarded Video

        public static bool IsRewardBasedVideoLoaded()
        {
            return IsRewardBasedVideoLoaded(instance.settings.rewardedVideoType);
        }

        public static bool IsRewardBasedVideoLoaded(AdvertisingModules advertisingModules)
        {
            if (!IsModuleActive(advertisingModules))
                return false;

            return advertisingLink[advertisingModules].IsRewardedVideoLoaded();
        }

        public static void RequestRewardBasedVideo()
        {
            RequestRewardBasedVideo(instance.settings.rewardedVideoType);
        }

        public static void RequestRewardBasedVideo(AdvertisingModules advertisingModules)
        {
            if (!IsModuleActive(advertisingModules))
                return;

            if (advertisingLink[advertisingModules].IsRewardedVideoLoaded())
                return;

            advertisingLink[advertisingModules].RequestRewardedVideo();
        }

        public static bool ShowRewardBasedVideo(AdvertisingHandler.RewardedVideoCallback callback)
        {
            return ShowRewardBasedVideo(instance.settings.rewardedVideoType, callback);
        }

        public static bool ShowRewardBasedVideo(AdvertisingModules advertisingModules,
            AdvertisingHandler.RewardedVideoCallback callback)
        {
            if (!IsModuleActive(advertisingModules))
                return false;

            if (!advertisingLink[advertisingModules].IsRewardedVideoLoaded())
            {
#if MODULE_GA
                GameAnalyticsSDK.GameAnalytics.NewAdEvent(GameAnalyticsSDK.GAAdAction.FailedShow, GameAnalyticsSDK.GAAdType.RewardedVideo, advertisingModules.ToString(), advertisingLink[advertisingModules].GetRewardedVideoID());
#endif
                return false;
            }

            advertisingLink[advertisingModules].ShowRewardedVideo(callback);

            return true;
        }

        #endregion

        #region Banner

        public static void ShowBanner()
        {
            ShowBanner(instance.settings.bannerType);
        }

        public static void ShowBanner(AdvertisingModules advertisingModules)
        {
            if (!IsModuleActive(advertisingModules))
                return;

            if (!CheckExtraBannerCondition())
                return;

            advertisingLink[advertisingModules].ShowBanner();
        }

        public static void DestroyBanner()
        {
            DestroyBanner(instance.settings.bannerType);
        }

        public static void DestroyBanner(AdvertisingModules advertisingModules)
        {
            if (!IsModuleActive(advertisingModules))
                return;

            advertisingLink[advertisingModules].DestroyBanner();
        }

        public static void HideBanner()
        {
            HideBanner(instance.settings.bannerType);
        }

        public static void HideBanner(AdvertisingModules advertisingModules)
        {
            if (!IsModuleActive(advertisingModules))
                return;

            advertisingLink[advertisingModules].HideBanner();
        }

        public static bool CheckExtraBannerCondition()
        {
            if (ExtraBannerReadyConditions != null)
            {
                var state = true;

                var listDelegates = ExtraBannerReadyConditions.GetInvocationList();
                for (var i = 0; i < listDelegates.Length; i++)
                    if (!(bool) listDelegates[i].DynamicInvoke())
                    {
                        state = false;

                        break;
                    }

                if (instance.settings.systemLogs)
                    Debug.Log("[AdsManager]: Extra condition banner state: " + state);

                return state;
            }

            return true;
        }

        #endregion

        #region Callbacks

        private void OnInterstitialDisplayed()
        {
            ResetInterstitialDelayTime();
        }

        private void OnRewardedAdReceivedReward()
        {
            ResetInterstitialDelayTime();
        }

        private void OnRewardedAdHidden()
        {
            ResetInterstitialDelayTime();
        }

        private void OnInterstitialHidden()
        {
            ResetInterstitialDelayTime();
        }

#if MODULE_IAP
        private void OnPurchaseComplete(ProductKeyType productKeyType)
        {
            if (productKeyType == ProductKeyType.NoAds)
            {
                Debug.Log("[IAP Manager]: Banners and interstitials are disabled!");

                GameSettingsPrefs.Set("no_ads", true);

                AdsManager.DestroyBanner(AdsManager.Settings.bannerType);
            }
        }
#endif

        #endregion

        public delegate void OnAdsCallback();

        public delegate void OnAdsModuleInitializedCallback(AdvertisingModules advertisingModules);

        public delegate bool IsAdsReadyCallback();
    }
}

// -----------------
// Advertisement v 0.3
// -----------------

// Changelog
// v 0.3
// • Unity Ads fixed
// v 0.2
// • Bug fix
// v 0.1
// • Added basic version