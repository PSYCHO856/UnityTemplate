#if YUNBU_APPLE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AnyThinkAds.Api;
using MobileKit;
using UnityEngine.UI;

namespace MobileKit
{
	public class AdsTopOnRewardedVideo : Singleton<AdsTopOnRewardedVideo>, ATRewardedVideoListener
	{

		private string PlacementId = "";
		private static Action adVideoCallback;
        private float lastCheckTime;
        private float lastLoadTime;

        private string sceneId;
		public bool IsRewardVideoReady { get; private set; }

		public void Init(string placementId)
		{
			this.PlacementId = placementId;
			ATRewardedVideo.Instance.setListener(this);
			LoadRewardedVideo();
		}
		
		private void LoadRewardedVideo()
		{
			Debug.Log(nameof(AdsTopOnRewardedVideo) + " " + nameof(LoadRewardedVideo));
			var jsonmap = new Dictionary<string, string>();
			ATRewardedVideo.Instance.loadVideoAd(PlacementId, jsonmap);
			
        #if YUNBU_APPLE
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("request", "video", "default");
        #endif
			lastLoadTime = Time.realtimeSinceStartup;
		}
		
		private void CheckRewardVideoStatus()
		{
			Debug.Log(nameof(AdsTopOnRewardedVideo) + " " + nameof(CheckRewardVideoStatus));
			IsRewardVideoReady = ATRewardedVideo.Instance.hasAdReady(PlacementId);
			lastCheckTime = Time.realtimeSinceStartup;
		}

        private void Update()
        {
            if (!IsRewardVideoReady &&
                Time.realtimeSinceStartup - lastCheckTime > AppConfig.Instance.AdCheckInterval)
            {
                CheckRewardVideoStatus();    
            }

            if (!IsRewardVideoReady &&
                Time.realtimeSinceStartup - lastLoadTime > AppConfig.Instance.AdLoadInterval)
            {
                LoadRewardedVideo();
            }
        }
		
		
		public void ShowRewardedVideo(string sceneId, string adKey, Action callback)
		{
            Debug.Log("ShowRewardVideo apple topon. start");

			this.sceneId = sceneId;
			if (!IsRewardVideoReady) return;
			adVideoCallback = callback;
			var jsonmap = new Dictionary<string, string> {
			{
				ATConst.SCENARIO, adKey
			}};
			ATRewardedVideo.Instance.showAd(PlacementId, jsonmap);

            Debug.Log("ShowRewardVideo apple topon. end");
		}


		public void onRewardedVideoAdLoaded(string placementId)
		{
        #if YUNBU_APPLE
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("filled", "video", "default");
        #endif
			Debug.Log("Developer onRewardedVideoAdLoaded------");
			CheckRewardVideoStatus();
		}

		public void onRewardedVideoAdLoadFail(string placementId, string code, string message)
		{
			Debug.Log("Developer onRewardedVideoAdLoadFail------:code" + code + "--message:" + message);
		}

		public void onRewardedVideoAdPlayStart(string placementId, ATCallbackInfo callbackInfo)
		{
			AudioManager.Volume = 0;
			SendRewardVideoAdShowEvent();
		#if YUNBU_APPLE
			Tracking.Instance.setTrackAdShow(placementId, callbackInfo.adunit_id, true);
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("show", "video", sceneId);
        #endif
			Debug.Log("Developer onRewardedVideoAdPlayStart------" + "->" + placementId);
		}

		public void onRewardedVideoAdPlayEnd(string placementId, ATCallbackInfo callbackInfo)
		{
			Debug.Log("Developer onRewardedVideoAdPlayEnd------" + "->" + placementId);
		}

		public void onRewardedVideoAdPlayFail(string placementId, string code, string message)
		{
			Debug.Log("Developer onRewardedVideoAdPlayFail------code:" + code + "---message:" + message);
		}

		public void onRewardedVideoAdPlayClosed(string placementId, bool isReward, ATCallbackInfo callbackInfo)
		{
			AudioManager.Volume = 1;
			IsRewardVideoReady = false;
			LoadRewardedVideo();
        #if YUNBU_APPLE
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("close", "video", sceneId);
        #endif
			Debug.Log("Developer onRewardedVideoAdPlayClosed------isReward:" + isReward);
		}

		public void onRewardedVideoAdPlayClicked(string placementId, ATCallbackInfo callbackInfo)
		{
		#if YUNBU_APPLE
			Tracking.Instance.setTrackAdClick(placementId, callbackInfo.adunit_id);
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("click", "video", sceneId);
        #endif
			Debug.Log("Developer onRewardVideoAdPlayClicked------" + "->" + placementId);
		}

		public void onReward(string placementId, ATCallbackInfo callbackInfo)
		{
			adVideoCallback?.Invoke();
			adVideoCallback = null;
			
        #if YUNBU_APPLE
            AnalyticsYunbuAppleHandler.Instance.OnUMengAdEvent("reward", "video", sceneId);
        #endif
			LoadRewardedVideo();
			Debug.Log("Developer onReward------" + "->" + placementId);
		}
		
		public static void SendRewardVideoAdShowEvent()
		{
            UMengAdEventConfig umengAdEventConfig = AnalyticsYunbuAppleHandler.UMengAdEventConfig;
            AdjustAdEventConfig adjustAdEventConfig = AnalyticsYunbuAppleHandler.AdjustAdEventConfig;
            TrackingIOAdEventConfig trackingIOAdEventConfig = AnalyticsYunbuAppleHandler.TrackingIOAdEventConfig;
			AdsManager.TodayRewardVideoShowTimes++;
			AnalyticsManager.OnCustomEvent(umengAdEventConfig.ShowRewardVideoSuccess, AnalyticsSDK.UMeng);
			AnalyticsManager.OnCustomEvent(adjustAdEventConfig.ShowRewardVideoSuccess, AnalyticsSDK.Adjust);
			AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.ShowRewardVideoSuccess, AnalyticsSDK.TrackingIO);

			if (DateTime.Today == TimeManager.RegisterDay)
			{
				switch (AdsManager.TodayRewardVideoShowTimes)
				{
					  case 1:
						  AnalyticsManager.OnCustomEvent(umengAdEventConfig.FirstDay_ShowRewardVideoTimes_1, AnalyticsSDK.UMeng);
						  AnalyticsManager.OnCustomEvent(adjustAdEventConfig.FirstDay_ShowRewardVideoTimes_1, AnalyticsSDK.Adjust);
						  AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.FirstDay_ShowRewardVideoTimes_1, AnalyticsSDK.TrackingIO);
						  break;
					  case 2:
						  AnalyticsManager.OnCustomEvent(umengAdEventConfig.FirstDay_ShowRewardVideoTimes_2, AnalyticsSDK.UMeng);
						  AnalyticsManager.OnCustomEvent(adjustAdEventConfig.FirstDay_ShowRewardVideoTimes_2, AnalyticsSDK.Adjust);
						  AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.FirstDay_ShowRewardVideoTimes_2, AnalyticsSDK.TrackingIO);
						  break;
					  case 3:
						  AnalyticsManager.OnCustomEvent(umengAdEventConfig.FirstDay_ShowRewardVideoTimes_3, AnalyticsSDK.UMeng);
						  AnalyticsManager.OnCustomEvent(adjustAdEventConfig.FirstDay_ShowRewardVideoTimes_3, AnalyticsSDK.Adjust);
						  AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.FirstDay_ShowRewardVideoTimes_3, AnalyticsSDK.TrackingIO);
						  break;
					  case 5:
						  AnalyticsManager.OnCustomEvent(umengAdEventConfig.FirstDay_ShowRewardVideoTimes_5, AnalyticsSDK.UMeng);
						  AnalyticsManager.OnCustomEvent(adjustAdEventConfig.FirstDay_ShowRewardVideoTimes_5, AnalyticsSDK.Adjust);
						  AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.FirstDay_ShowRewardVideoTimes_5, AnalyticsSDK.TrackingIO);
						  break;
					  case 10:
						  AnalyticsManager.OnCustomEvent(umengAdEventConfig.FirstDay_ShowRewardVideoTimes_10, AnalyticsSDK.UMeng);
						  AnalyticsManager.OnCustomEvent(adjustAdEventConfig.FirstDay_ShowRewardVideoTimes_10, AnalyticsSDK.Adjust);
						  AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.FirstDay_ShowRewardVideoTimes_10, AnalyticsSDK.TrackingIO);
						  break;
				}
			}
			else
			{
				switch (AdsManager.TodayRewardVideoShowTimes)
				{
					case 1:
						AnalyticsManager.OnCustomEvent(umengAdEventConfig.ShowRewardVideoTimes_1, AnalyticsSDK.UMeng);
						AnalyticsManager.OnCustomEvent(adjustAdEventConfig.ShowRewardVideoTimes_1, AnalyticsSDK.Adjust);
						AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.ShowRewardVideoTimes_1, AnalyticsSDK.TrackingIO);
						break;
					case 2:
						AnalyticsManager.OnCustomEvent(umengAdEventConfig.ShowRewardVideoTimes_2, AnalyticsSDK.UMeng);
						AnalyticsManager.OnCustomEvent(adjustAdEventConfig.ShowRewardVideoTimes_2, AnalyticsSDK.Adjust);
						AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.ShowRewardVideoTimes_2, AnalyticsSDK.TrackingIO);
						break;
					case 3:
						AnalyticsManager.OnCustomEvent(umengAdEventConfig.ShowRewardVideoTimes_3, AnalyticsSDK.UMeng);
						AnalyticsManager.OnCustomEvent(adjustAdEventConfig.ShowRewardVideoTimes_3, AnalyticsSDK.Adjust);
						AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.ShowRewardVideoTimes_3, AnalyticsSDK.TrackingIO);
						break;
					case 5:
						AnalyticsManager.OnCustomEvent(umengAdEventConfig.ShowRewardVideoTimes_5, AnalyticsSDK.UMeng);
						AnalyticsManager.OnCustomEvent(adjustAdEventConfig.ShowRewardVideoTimes_5, AnalyticsSDK.Adjust);
						AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.ShowRewardVideoTimes_5, AnalyticsSDK.TrackingIO);
						break;
					case 10:
						AnalyticsManager.OnCustomEvent(umengAdEventConfig.ShowRewardVideoTimes_10, AnalyticsSDK.UMeng);
						AnalyticsManager.OnCustomEvent(adjustAdEventConfig.ShowRewardVideoTimes_10, AnalyticsSDK.Adjust);
						AnalyticsManager.OnCustomEvent(trackingIOAdEventConfig.ShowRewardVideoTimes_10, AnalyticsSDK.TrackingIO);
						break;
				}
			}
		}
	}
}
#endif