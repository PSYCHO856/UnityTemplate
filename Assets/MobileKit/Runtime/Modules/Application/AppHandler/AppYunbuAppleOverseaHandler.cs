#if YUNBU_APPLE_OVERSEA

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileKit
{
    public class AppYunbuAppleOverseaHandler : Singleton<AppYunbuAppleOverseaHandler> , IAppHandler
    {
        public void Init()
        {
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
            Application.OpenURL("http://www.yunbu.me/service/mingya/contract.html");
        }

        public void ShowPrivacyPolicy()
        {
            Application.OpenURL("http://www.yunbu.me/service/mingya/privacy.html");
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
            succCallback?.Invoke();
        }

        public string GetPriceById( string skuId )
        {
            return "$2.99";
        }
    }
}
#endif