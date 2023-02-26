#if YUNBU_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileKit
{
    public class AppYunbuAndroidHandler : Singleton<AppYunbuAndroidHandler> , IAppHandler
    {
        private static AndroidJavaClass mainActivity;
        public void Init()
        {
            mainActivity = new AndroidJavaClass("com.feamber.MainActivity");
        }

        public void LeisureGameSubject()
        {
            Debug.Log(nameof(AppYunbuAndroidHandler) + " " + nameof(LeisureGameSubject));
            mainActivity.CallStatic("LeisureGameSubject");
        }
        
        public string GetAppName()
        {
            string productName = mainActivity.CallStatic<string>("GetProductName");
            Debug.Log(nameof(AppYunbuAndroidHandler) + " " + nameof(GetAppName) + productName);
            return productName;
        }

        public string GetChannelName()
        {
            string channel = mainActivity.CallStatic<string>("GetChannelName");
            Debug.Log(nameof(AppYunbuAndroidHandler) + " " + nameof(GetChannelName) + " " + channel);
            return channel;
        }

        public void RequestStoreReview()
        {
            if (!GetSwitchState("showMarket")) return;
            var prefab =  Resources.Load<RateCanvas>("UI/RateCanvas");
            var rateCanvas = Instantiate(prefab);
            rateCanvas.OnOpen(() =>
            {
                AppManager.IsStoreReviewed = true;
                Debug.Log(nameof(AppYunbuAndroidHandler) + " " + nameof(RequestStoreReview));
                mainActivity.CallStatic("GotoMarket");
            });
        }

        public void ExitGame()
        {
            Debug.Log(nameof(AppYunbuAndroidHandler) + " " + nameof(ExitGame));
            mainActivity.CallStatic("ExitGame");
        }
        
        public void ShowUserProtocol()
        {
            mainActivity.CallStatic("ShowUserCenter");
        }

        public void ShowPrivacyPolicy()
        {
            mainActivity.CallStatic("ShowUserCenter");
        }

        public bool GetSwitchState(string switchName)
        {
            bool switchState = mainActivity.CallStatic<bool>("GetSwitchState", switchName);
            Debug.Log(nameof(AppYunbuAndroidHandler) + " " + nameof(GetSwitchState) + " " + switchName + " " + switchState);
            return switchState;
        }

 
        public string GetString(string key)
        {
            string value = mainActivity.CallStatic<string>("GetString", key);
            Debug.Log(nameof(AppYunbuAndroidHandler) + " " + nameof(GetString) + " " + key + " " + value);
            return value;
        }

        public int GetInt(string key)
        {
            int value = mainActivity.CallStatic<int>("GetInt", key);
            Debug.Log(nameof(AppYunbuAndroidHandler) + " " + nameof(GetInt) + " " + key + " " + value);
            return value;
        }

        public float GetFloat(string key)
        {
            float value = mainActivity.CallStatic<float>("GetFloat", key);
            Debug.Log(nameof(AppYunbuAndroidHandler) + " " + nameof(GetFloat) + " " + key + " " + value);
            return value;
        }

        public bool GetBool(string key)
        {
            bool value = mainActivity.CallStatic<bool>("GetBool", key);
            Debug.Log(nameof(AppYunbuAndroidHandler) + " " + nameof(GetBool) + " " + key + " " + value);
            return value;
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

        private void OnApplicationQuit()
        {
            Debug.Log(nameof(AppYunbuAndroidHandler) + " " + nameof(OnApplicationQuit));
            mainActivity.CallStatic("ExitGame");
        }
    }
}
#endif