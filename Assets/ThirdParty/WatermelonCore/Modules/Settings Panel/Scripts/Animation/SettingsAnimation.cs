#pragma warning disable 0649

using System.Collections;
using UnityEngine;

namespace Watermelon
{
    public abstract class SettingsAnimation : ScriptableObject
    {
        public delegate void AnimationCallback();

        protected SettingsPanel.SettingsButtonInfo[] settingsButtonsInfo;
        protected SettingsPanel settingsPanel;

        public void Init(SettingsPanel settingsPanel)
        {
            this.settingsPanel = settingsPanel;

            settingsButtonsInfo = settingsPanel.SettingsButtonsInfo;

            AddExtraComponents();
        }

        protected virtual void AddExtraComponents()
        {
        }

        public abstract IEnumerator Show(AnimationCallback callback);
        public abstract IEnumerator Hide(AnimationCallback callback);
    }
}

// -----------------
// Settings Panel v 0.1
// -----------------