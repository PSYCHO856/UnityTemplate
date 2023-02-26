using System;
using System.Collections.Generic;

namespace MobileKit
{
    public static class AppManager
    {
        public static bool IsStoreReviewed
        {
            get => GameSettingsPrefs.GetBool(GameSettingsKey.IsStoreReviewed, false);
            set => GameSettingsPrefs.SetBool(GameSettingsKey.IsStoreReviewed, value);
        }

        private static IAppHandler appHandler;
        public static void Init()
        {
#if UNITY_EDITOR
            appHandler = AppDummyHandler.Instance;
#elif MOBILEKIT_IOS_CN
            appHandler = AppDummyHandler.Instance;
#elif MOBILEKIT_IOS_US
            appHandler = AppDummyHandler.Instance;
#elif MOBILEKIT_GOOGLEPLAY
            appHandler = AppGooglePlayHandler.Instance;
            // appHandler = AppDummyHandler.Instance;
#else
            appHandler = AppDummyHandler.Instance;
#endif
            appHandler.Init();
        }

        public static string GetAppName() => appHandler.GetAppName();
        public static string GetChannelName() => appHandler.GetChannelName();
        
        public static void RequestStoreReview()
        {
            if (IsStoreReviewed) return;
            appHandler.RequestStoreReview();
        }

        public static void ShowUserProtocol()
        {
            appHandler.ShowUserProtocol();
        }

        public static void ShowPrivacyPolicy()
        {
            appHandler.ShowPrivacyPolicy();
        }

        /// <summary>
        /// 购买
        /// </summary>
        public static void PurchaseInApp( string skuId, Action<string> succCallback, Action<string> failCallback ) => appHandler.PurchaseInApp(skuId, succCallback, failCallback);

        /// <summary>
        /// 含货币符号
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static string GetPriceById( string skuId ) => appHandler.GetPriceById(skuId);

        /// <summary>
        /// 恢复购买
        /// </summary>
        public static void RestoredPurchase( Action<string[]> succCallback, Action<string> failCallback ) => appHandler.RestoredPurchase(succCallback, failCallback);
    }
}