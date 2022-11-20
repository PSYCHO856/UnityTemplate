namespace Watermelon
{
    public abstract class AdvertisingHandler
    {
        public delegate void InterstitialCallback(bool isDisplayed);

        public delegate void RewardedVideoCallback(bool hasReward);

        protected AdsData adsSettings;
        protected InterstitialCallback interstitialCallback;

        protected bool isInitialized = false;

        protected RewardedVideoCallback rewardedVideoCallback;

        public AdvertisingHandler(AdvertisingModules moduleType)
        {
            ModuleType = moduleType;
        }

        public AdvertisingModules ModuleType { get; }

        public bool IsInitialized()
        {
            return isInitialized;
        }

        public abstract void Init(AdsData adsSettings);

        public abstract bool IsGDPRRequired();
        public abstract void SetGDPR(bool state);

        public abstract void ShowBanner();
        public abstract void HideBanner();
        public abstract void DestroyBanner();

        public abstract void RequestInterstitial();
        public abstract void ShowInterstitial(InterstitialCallback callback);
        public abstract bool IsInterstitialLoaded();

        public abstract void RequestRewardedVideo();
        public abstract void ShowRewardedVideo(RewardedVideoCallback callback);
        public abstract bool IsRewardedVideoLoaded();

        public abstract string GetBannerID();
        public abstract string GetInterstitialID();
        public abstract string GetRewardedVideoID();
    }
}

// -----------------
// Advertisement v 0.3
// -----------------