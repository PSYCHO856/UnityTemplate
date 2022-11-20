using UnityEngine;

namespace Watermelon
{
    public abstract class SettingsButtonBase : MonoBehaviour
    {
        private int index;

        private SettingsPanel settingsPanel;
        public RectTransform RectTransform { get; private set; }

        public void Init(int index, SettingsPanel settingsPanel)
        {
            this.index = index;
            this.settingsPanel = settingsPanel;

            RectTransform = GetComponent<RectTransform>();
        }

        public abstract bool IsActive();
        public abstract void OnClick();
    }
}

// -----------------
// Settings Panel v 0.1
// -----------------