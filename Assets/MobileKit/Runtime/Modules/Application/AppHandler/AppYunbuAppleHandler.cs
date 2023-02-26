#if YUNBU_APPLE

using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.Networking;

namespace MobileKit
{
    public class AppYunbuAppleHandler : Singleton<AppYunbuAppleHandler> , IAppHandler
    {
        private static JToken WebParamsJson { get; set; }
        
        public void Init()
        {
            StartCoroutine(GetWebParams(AppConfig.Instance.YunbuWebConfigKey));
        }

        public string GetAppName()
        {
            return Application.productName;
        }

        public string GetChannelName()
        {
            return "AppleStore";
        }

        public void ExitGame()
        {
        }

        public void RequestStoreReview()
        {
            AppManager.IsStoreReviewed = true;
            Device.RequestStoreReview();
        }

        public void LeisureGameSubject()
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
            if (WebParamsJson != null)
            {
                string param = WebParamsJson?[switchName]?.ToString();
                if (!string.IsNullOrEmpty(param))
                {
                    bool.TryParse(param, out var value);
                    return value;
                }
            }
            return false;

        }

        public string GetString(string key)
        {
            if (WebParamsJson != null)
            {
                return WebParamsJson?[key]?.ToString();
            }
            return "";
        }

        public int GetInt(string key)
        {
            if (WebParamsJson != null)
            {
                string param = WebParamsJson?[key]?.ToString();
                if (!string.IsNullOrEmpty(param))
                {
                    int.TryParse(param, out var value);
                    return value;
                }
            }
            return 0;
        }

        public float GetFloat(string key)
        {
            if (WebParamsJson != null)
            {
                string param = WebParamsJson?[key]?.ToString();
                if (!string.IsNullOrEmpty(param))
                {
                    float.TryParse(param, out var value);
                    return value;
                }
            }
            return 0;
        }

        public bool GetBool(string key)
        {
            if (WebParamsJson != null)
            {
                string param = WebParamsJson?[key]?.ToString();
                if (!string.IsNullOrEmpty(param))
                {
                    bool.TryParse(param, out var value);
                    return value;
                }
            }

            if (key == "real_name_certification_switch")
            {
                return true;
            }
            return false;
        }

        public void InitIAP(List<string> iapKeys)
        {
        }

        public void PurchaseInApp( string skuId, Action<string> succCallback, Action<string> failCallback )
        {
        }

        public string GetPriceById( string skuId )
        {
            return "$2.99";
        }
        
        
        private static IEnumerator GetWebParams(string appKey)
        {
            string uri =
                $"http://z.yunbu.me/yunsdk/api/v5/ios/config/finalparams/?appKey={appKey}&channel=ios&etag=1";
            using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.downloadHandler.text);
                if (JsonConvert.DeserializeObject(webRequest.downloadHandler.text) is JObject jobject)
                {
                    WebParamsJson = jobject["data"];
                    // string end = jobject["data"]?["end"]?.ToString();
                    // if (end != null)
                    // {
                    //     int.TryParse(end, out endInterLevelThreshold);
                    // }
                }
            }
        }
    }
}
#endif