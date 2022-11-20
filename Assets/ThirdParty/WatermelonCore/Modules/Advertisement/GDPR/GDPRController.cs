#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    public class GDPRController : MonoBehaviour
    {
        private const string PREFS_NAME = "GDPR";
        public GameObject panelObject;

        private void Awake()
        {
            if (AdsManager.Settings.gdprContainer.enableGDPR)
                panelObject.SetActive(!IsGDPRStateExist());
            else
                panelObject.SetActive(false);
        }

        public void OpenPrivacyLink()
        {
            Application.OpenURL(AdsManager.Settings.gdprContainer.privacyLink);
        }

        public void SetGDPRState(bool state)
        {
            AdsManager.SetGDPR(state);

            PlayerPrefs.SetInt(PREFS_NAME, state ? 1 : 0);

            Close();
        }

        public void Open()
        {
            panelObject.SetActive(true);
        }

        public void Close()
        {
            panelObject.SetActive(false);
        }

        public static bool GetGDPRState()
        {
            if (PlayerPrefs.HasKey(PREFS_NAME)) return PlayerPrefs.GetInt(PREFS_NAME) == 1 ? true : false;

            return false;
        }

        public static bool IsGDPRStateExist()
        {
            return PlayerPrefs.HasKey(PREFS_NAME);
        }
    }
}

// -----------------
// Advertisement v 0.3
// -----------------