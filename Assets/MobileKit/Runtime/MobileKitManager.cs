#if MOBILEKIT_IOS_CN || MOBILEKIT_IOS_US
using Unity.Advertisement.IosSupport;
#endif
using Cfg.railway;
using UnityEngine;

namespace MobileKit
{
    public static class MobileKitManager
    { 
        public static bool IsInitialized { get; private set; }
        private const string TAG = nameof(MobileKitManager);
        public static void PreInit()
        {
            Application.targetFrameRate = 60;
            LanguageManager.Init();
            TimeManager.Init();
            AudioManager.Init();
            // NotificationsManager.Init();
            TweenManager.Init(10, 5, 0, true);
            
            TableManager.Init();
            GMManager.Init();
        }

        public static void CheckATTStatus()
        {
#if MOBILEKIT_IOS_CN || MOBILEKIT_IOS_US
            var attStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            if (attStatus == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }
#endif
        }

        public static void PostInit()
        {
            AnalyticsManager.Init();
            AppManager.Init();
            AdsManager.Init();
            IsInitialized = true;
        }
    }
}
