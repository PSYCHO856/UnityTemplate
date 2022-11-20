#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    public class SettingsGDPRButton : SettingsButtonBase
    {
        [SerializeField] private GDPRController GDPRController;

        public override bool IsActive()
        {
            AdsData adsSettings;
#if UNITY_EDITOR
            adsSettings = RuntimeEditorUtils.GetAssetByName<AdsData>("Ads Settings");
#else
            adsSettings = AdsManager.Settings;
#endif

            return adsSettings.gdprContainer.enableGDPR;
        }

        public override void OnClick()
        {
            GDPRController.Open();
        }
    }
}

// -----------------
// Settings Panel v 0.1
// -----------------