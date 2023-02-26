#if MOBILEKIT_GOOGLEPLAY
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileKit
{
    public class AppGooglePlayHandler : Singleton<AppGooglePlayHandler> , IAppHandler
    {
        private static AndroidJavaClass mainActivity;
        private Action<string> succCallback = default;
        private Action<string> failCallback = default;

        public void Init()
        {
            Debug.Log(nameof(AppGooglePlayHandler) + " " + nameof(Init));
            mainActivity = new AndroidJavaClass("com.mobilekit.sdk.MainActivity");
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
        }

        public void ShowUserProtocol()
        {
            Application.OpenURL("https://sites.google.com/view/candy-madness-privacy-policy");
        }

        public void ShowPrivacyPolicy()
        {
            Application.OpenURL("https://sites.google.com/view/candy-madness-privacy-policy");
        }

        public bool GetSwitchState(string switchName)
        {
            return true;
        }

        public string GetString(string key)
        {
            return "";
        }

        public int GetInt(string key)
        {
            return 0;
        }

        public float GetFloat(string key)
        {
            return 0;
        }

        public bool GetBool(string key)
        {
            return true;
        }

        public void InitIAP(List<string> iapKeys)
        {
        }

        public void PurchaseInApp( string skuId, Action<string> succCallback, Action<string> failCallback )
        {
            Debug.Log(nameof(PurchaseInApp) + " " + skuId);
            this.succCallback = succCallback;
            this.failCallback = failCallback;
            mainActivity.CallStatic( "PurchaseInApp", skuId );
        }

        public string GetPriceById( string skuId )
        {
            return mainActivity.CallStatic<string>("GetPriceById", skuId);
        }

        private void OnPurchaseCompleted( string skuId )
        {
            Debug.Log(nameof(OnPurchaseCompleted));
            succCallback?.Invoke( skuId );
            succCallback = null;
            failCallback = null;
        }

        private void OnPurchaseFailed( string stateStr )
        {
            Debug.Log(nameof(OnPurchaseFailed));
            failCallback?.Invoke( stateStr );
            succCallback = null;
            failCallback = null;
            // int state = 0;
            // if ( int.TryParse( stateStr, out state ) )
            // {
                    //state 1 onPurchaseCanceled, 2 onPurchaseFailed, 3 onPurchaseError
            // }
        }

        public void RestoredPurchase( Action<string[]> succCallback, Action<string> failCallback )
        {
            Debug.Log(nameof(RestoredPurchase));
            // mainActivity.CallStatic( "RestoredPurchase" );
            // succCallbackRestore = succCallback;
            // failCallbackRestore = failCallback;
        }
    }
}

#endif