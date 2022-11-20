namespace Watermelon
{
    public class SettingsNoAdsButton : SettingsButtonBase
    {
        public override bool IsActive()
        {
#if MODULE_IAP
            return true;
#else
            return false;
#endif
        }

        public override void OnClick()
        {
            IAPManager.BuyProduct(ProductKeyType.NoAds);
        }
    }
}

// -----------------
// Settings Panel v 0.1
// -----------------