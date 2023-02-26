using System;
using System.Collections;
using System.Collections.Generic;
using Beebyte.Obfuscator;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace MobileKit
{
    [CreateAssetMenu(fileName = "AppConfig", menuName = "MobileKit/App Config", order = 1)]
    public class AppConfig : ScriptableObjectSingleton<AppConfig>
    {
        [FoldoutGroup("App Info")]
        public string StoreAppId = "";
        [FoldoutGroup("App Info")]
        public string BundleId = "com.mobilekit.xxx";
        [FoldoutGroup("App Info")]
        public string AppName = "MyGame";

        [FoldoutGroup("Links")] public string ProtocolURL;
        [FoldoutGroup("Links")] public string PrivacyURL;

        [FoldoutGroup("AdMob")] public string AdMobAppId;
        [FoldoutGroup("AdMob")] public string AdMobSplashId;
        [FoldoutGroup("AdMob")] public string AdMobInterId;
        [FoldoutGroup("AdMob")] public string AdMobRewardId;
        [FoldoutGroup("AdMob")] public string AdMobBannerId;

        // 检查控制台或 logcat 输出，查找类似以下内容的消息：
        // Android: UseRequestConfiguration.Builder.setTestDeviceIds(Arrays.asList("33BE2250B43518CCDA7DE426D04EE231"))
        // iOS:     set:GADMobileAds.sharedInstance.requestConfiguration.testDeviceIdentifiers =@[ @"2077ef9a63d2b398840261c8221a0c9b" ];
        [FoldoutGroup("AdMob")] public List<string> testDevices;

        [Header("MAX")]
        [FoldoutGroup("MAX")] public string MAXRewardId;
        [FoldoutGroup("MAX")] public string MAXInterId;
        [FoldoutGroup("MAX")] public string MAXBannerId;
        [FoldoutGroup("MAX")] public string MAXSplashId;
        [FoldoutGroup("MAX")] public string MAXNativeId;

        [Header("IAP")]
        [ListDrawerSettings(ShowIndexLabels = true, HideRemoveButton = true)]
        [FoldoutGroup("IAP")] public List<IAPInfo> IAPInfos;
        // [FoldoutGroup("IAP")] public List<string> IAPIds;
        // [FoldoutGroup("IAP")] public List<string> inAppSKUS;

        [Serializable]
        public class IAPInfo
        {
            public string IAPId;
            public string inAppSKU;
            public float price;     //默认的美元价格，用于EAS统计
            public string desc;
        }

        public string GetSKUbyId( string IAPId ){
            foreach (var info in IAPInfos)
            {
                if (info.IAPId.Equals(IAPId))
                {
                    return info.inAppSKU;
                }
            }
            return "";
        }

        public string GetIdbySKU( string inAppSKU ){
            foreach (var info in IAPInfos)
            {
                if (info.inAppSKU.Equals(inAppSKU))
                {
                    return info.IAPId;
                }
            }
            return "";
        }

        public float GetIAPDefaultPrice( string IAPId ){
            foreach (var info in IAPInfos)
            {
                if (info.IAPId.Equals(IAPId))
                {
                    return info.price;
                }
            }
            return 0;
        }

        public string GetIAPDesc( string IAPId ){
            foreach (var info in IAPInfos)
            {
                if (info.IAPId.Equals(IAPId))
                {
                    return info.desc;
                }
            }
            return "";
        }
    }
}
