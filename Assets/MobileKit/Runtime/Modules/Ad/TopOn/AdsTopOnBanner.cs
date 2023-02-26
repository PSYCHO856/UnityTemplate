#if YUNBU_APPLE

using System;
using System.Collections.Generic;
using AnyThinkAds.Api;
using UnityEngine;

using MobileKit;
namespace MobileKit
{
    public class AdsTopOnBanner : Singleton<AdsTopOnBanner>, ATBannerAdListener
    {
        private string PlacementId = "";
        
        private float lastCheckTime;
        private float lastLoadTime;
        
        private bool isBannerReady;

        private bool isFirstShow;
        private BannerPos bannerPos;

        private string sceneId;
        
        private Dictionary<string, object> bannerParams = new Dictionary<string, object>();
        
        public void Init(string placementId)
        {
            isFirstShow = true;
            PlacementId = placementId;
            ATBannerAd.Instance.setListener(this);
            
            bannerParams = new Dictionary<string,object>();
            ATSize bannerSize = new ATSize(Screen.width, 160, true);
            bannerParams.Add(ATBannerAdLoadingExtra.kATBannerAdLoadingExtraBannerAdSizeStruct, bannerSize);
            bannerParams.Add(ATBannerAdLoadingExtra.kATBannerAdLoadingExtraAdaptiveWidth, bannerSize.width);
            bannerParams.Add(ATBannerAdLoadingExtra.kATBannerAdLoadingExtraAdaptiveOrientation, ATBannerAdLoadingExtra.kATBannerAdLoadingExtraAdaptiveOrientationPortrait);
            LoadBannerAd();
        }
        
        private void LoadBannerAd()
        {
            ATBannerAd.Instance.loadBannerAd(PlacementId, bannerParams);
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("request", "banner", "default");
        }

        public void ShowBannerAd(string sceneId, string adKey, BannerPos pos)
        {
            this.sceneId = sceneId;
            bannerPos = pos;
            if (isFirstShow || pos != bannerPos)
            {
                var jsonmap = new Dictionary<string, string> {
                {
                    ATConst.SCENARIO, adKey
                }};
                
                var position = pos == BannerPos.Bottom ? ATBannerAdLoadingExtra.kATBannerAdShowingPisitionBottom : ATBannerAdLoadingExtra.kATBannerAdShowingPisitionTop;
                ATBannerAd.Instance.showBannerAd(PlacementId, position, jsonmap);
                isFirstShow = false;
            }
            else
            {
                ATBannerAd.Instance.showBannerAd(PlacementId);
            }
        }
        
        public void HideBannerAd()
        {
            ATBannerAd.Instance.hideBannerAd(PlacementId);
        }


        //广告自动刷新成功
        public void onAdAutoRefresh(string placementId, ATCallbackInfo callbackInfo)
        {
            Debug.Log("Developer callback onAdAutoRefresh :" +  placementId);
        }
        
        //广告自动刷新失败
        public void onAdAutoRefreshFail(string placementId, string code, string message)
        {
            Debug.Log("Developer callback onAdAutoRefreshFail : "+ placementId + "--code:" + code + "--msg:" + message);
        }
        
        //广告被点击
        public void onAdClick(string placementId, ATCallbackInfo callbackInfo)
        {
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("click", "banner", sceneId);
            Debug.Log("Developer callback onAdClick :" + placementId);
        }
    
        //v5.5.3之后不再执行该回调，转到onAdCloseButtonTapped方法回调
        public void onAdClose(string placementId) { }
        
        //广告展示成功
        public void onAdImpress(string placementId, ATCallbackInfo callbackInfo)
        {
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("show", "banner", sceneId);
            Debug.Log("Developer callback onAdImpress :" + placementId);
        }
        
        //广告加载成功
        public void onAdLoad(string placementId)
        {
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("filled", "banner", "default");
            Debug.Log("Developer callback onAdLoad :" + placementId);
        }
        
        //广告加载失败
        public void onAdLoadFail(string placementId, string code, string message)
        {
            Debug.Log("Developer callback onAdLoadFail : : " + placementId + "--code:" + code + "--msg:" + message);
            Invoke(nameof(LoadBannerAd), 10f);
        }
        
        //广告关闭按钮被点击
        public void onAdCloseButtonTapped(string placementId, ATCallbackInfo callbackInfo)
        {
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("close", "banner", sceneId);
            Debug.Log("Developer callback onAdCloseButtonTapped :" + placementId);
        }
    }
}
#endif