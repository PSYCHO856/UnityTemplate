#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class SettingsSoundToggleButton : SettingsButtonBase
    {
        [SerializeField] private Image imageRef;

        [Space] [SerializeField] private Sprite activeSprite;

        [SerializeField] private Sprite disableSprite;

        private bool isActive = true;

        private void Start()
        {
            isActive = AudioController.GetVolume() != 0;

            if (isActive)
                imageRef.sprite = activeSprite;
            else
                imageRef.sprite = disableSprite;
        }

        public override bool IsActive()
        {
            return AudioController.IsAudioModuleEnabled();
        }

        public override void OnClick()
        {
            isActive = !isActive;

            if (isActive)
            {
                imageRef.sprite = activeSprite;
                AudioController.SetVolume(1f);
            }
            else
            {
                imageRef.sprite = disableSprite;
                AudioController.SetVolume(0f);
            }
        }
    }
}

// -----------------
// Settings Panel v 0.1
// -----------------