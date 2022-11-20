#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    public class SettingsLinkButton : SettingsButtonBase
    {
        [SerializeField] private bool isActive = true;
        [SerializeField] private string url;

        public override bool IsActive()
        {
            return isActive;
        }

        public override void OnClick()
        {
            Application.OpenURL(url);
        }
    }
}

// -----------------
// Settings Panel v 0.1
// -----------------