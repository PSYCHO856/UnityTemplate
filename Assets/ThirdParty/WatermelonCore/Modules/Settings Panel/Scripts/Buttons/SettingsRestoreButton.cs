namespace Watermelon
{
    public class SettingsRestoreButton : SettingsButtonBase
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
            IAPManager.RestorePurchases();
        }
    }
}

// -----------------
// Settings Panel v 0.1
// -----------------