using System;
using System.Collections.Generic;
using UnityEngine;
// using VoxelBusters.EssentialKit;

namespace MobileKit
{
    public class AppDummyHandler : Singleton<AppDummyHandler> , IAppHandler
    {
        public void Init()
        {
            // PurchaseManager.Init();
        }

        public string GetAppName()
        {
            return Application.productName;
        }

        public string GetChannelName()
        {
            return Application.platform.ToString();
        }

        public void RequestStoreReview()
        {
            // string appName = AppConfig.Instance.AppName;
            // string title = "Enjoying " + appName + "?";
            // string message =
            //     $"If you enjoy playing {appName} would you mind taking a moment to rate it? It wont take more than a minute. Thanks for your support.";
            // string ok = "Ok";
            // string cancel = "Cancel";
            // UIManager.ShowDialog(title, message,
            //     ok, () =>
            //     {
            //         Utilities.RequestStoreReview();
            //         AppManager.IsStoreReviewed = true;
            //     },
            //     cancel, () =>
            //     {
            //     });
        }

        public void OpenStore_WriteReview(string appId)
        {
            if (string.IsNullOrEmpty(appId)) return;
			 var url = $"https://itunes.apple.com/cn/app/id{appId}?mt=8&action=write-review";
			 Application.OpenURL(url);
        }

        public void ShowUserProtocol()
        {
            Application.OpenURL(AppConfig.Instance.ProtocolURL);
        }

        public void ShowPrivacyPolicy()
        {
            Application.OpenURL(AppConfig.Instance.PrivacyURL);
        }

        public void PurchaseInApp( string skuId, Action<string> succCallback, Action<string> failCallback )
        {
            Debug.Log(nameof(PurchaseInApp) + " " + skuId);
            succCallback.Invoke(skuId);
        }

        public string GetPriceById( string skuId )
        {
            return "$2.99";
        }

        public void RestoredPurchase( Action<string[]> succCallback, Action<string> failCallback )
        {
            Debug.Log(nameof(RestoredPurchase));
        }
    }
}