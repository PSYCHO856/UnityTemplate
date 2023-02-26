using System;
using UnityEngine;
using UnityEngine.UI;

namespace MobileKit
{
    public class AdsDummyCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject bannerObject;
        [SerializeField] private GameObject interstitialObject;
        [SerializeField] private GameObject rewardedVideoObject;
        [SerializeField] private GameObject nativeContainer;
        [SerializeField] private GameObject nativeObject;

        [SerializeField] private Button closeInterstitial;
        [SerializeField] private Button closeRewardedVideo;
        [SerializeField] private Button closeNativeAd;
        [SerializeField] private Button getVideoReward;
        
        private RectTransform bannerRectTransform;
        private RectTransform nativeRectTransform;

        private Canvas canvas;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            canvas = GetComponent<Canvas>();
            bannerObject.SetActive(false);
            interstitialObject.SetActive(false);
            rewardedVideoObject.SetActive(false);
            nativeContainer.SetActive(false);
            
            bannerRectTransform = (RectTransform)bannerObject.transform;
            nativeRectTransform = (RectTransform) nativeObject.transform;
            closeInterstitial.onClick.AddListener(OnCloseInter);
            closeRewardedVideo.onClick.AddListener(OnCloseReward);
            closeNativeAd.onClick.AddListener(OnCloseNativeAd);
            getVideoReward.onClick.AddListener(GetVideoReward);
        }

        public void ShowBanner(BannerPos pos)
        {
            SetBannerPosition(pos);
            bannerObject.SetActive(true);
        }

        public void HideBanner()
        {
            bannerObject.SetActive(false);
        }
        
        private void SetBannerPosition(BannerPos bannerPos)
        {
            switch (bannerPos)
            {
                case BannerPos.Bottom:
                    bannerRectTransform.pivot = new Vector2(0.5f, 0.0f);

                    bannerRectTransform.anchorMin = new Vector2(0.0f, 0.0f);
                    bannerRectTransform.anchorMax = new Vector2(1.0f, 0.0f);

                    bannerRectTransform.anchoredPosition = Vector2.zero;
                    break;
                case BannerPos.Top:
                    bannerRectTransform.pivot = new Vector2(0.5f, 1.0f);

                    bannerRectTransform.anchorMin = new Vector2(0.0f, 1.0f);
                    bannerRectTransform.anchorMax = new Vector2(1.0f, 1.0f);

                    bannerRectTransform.anchoredPosition = Vector2.zero;
                    break;
            }
        }

        public void ShowInterstitial()
        {
            AudioListener.volume = 0;
            interstitialObject.SetActive(true);
        }

        public void CloseInterstitial()
        {
            AudioListener.volume = 1;
            interstitialObject.SetActive(false);
        }

        public void ShowRewardedVideo()
        {
            rewardedVideoObject.SetActive(true);
            AudioListener.volume = 0;
            
        }

        public void CloseRewardedVideo()
        {
            rewardedVideoObject.SetActive(false);
            AudioListener.volume = 1;
        }

        private void OnVideoWatched()
        {
            AdsDummyHandler.Instance.OnRewardVideoWatched();
        }

        public void ShowNativeAd(int x, int y, int width, int height)
        {
            x = (int)(x / canvas.scaleFactor);
            y = (int)(-y / canvas.scaleFactor);
            width = (int)(width / canvas.scaleFactor);
            height = (int)(height / canvas.scaleFactor);
            
            nativeRectTransform.anchoredPosition = new Vector2(x, y);
            nativeRectTransform.sizeDelta = new Vector2(width, height);
            nativeContainer.SetActive(true);
        }
        
        public void CloseNativeAd()
        {
            nativeContainer.SetActive(false);
        }

        private void OnCloseInter()
        {
            CloseInterstitial();
        }

        private void OnCloseReward()
        {
            CloseRewardedVideo();
        }

        private void OnCloseNativeAd()
        {
            CloseNativeAd();
        }

        private void GetVideoReward()
        {
            CloseRewardedVideo();
            OnVideoWatched();
        }

    }
}
