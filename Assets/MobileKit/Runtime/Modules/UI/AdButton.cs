using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MobileKit
{
    public class AdButton : Button
    {
        public string AdSceneId = "";
        
        private bool adEnabled = true;
        public bool AdEnabled
        {
            get => adEnabled;
            set
            {
                adEnabled = value;
                adIcon.gameObject.SetActive(adEnabled);
                loadingIcon.gameObject.SetActive(adEnabled);
            }
        }
        
        private Image adIcon;
        private Image loadingIcon;
        private UIShiny uiShiny;
        private const float checkInterval = 3f;
        private bool isVideoReady;
        private float lastCheckTime;
        
        protected override void Awake()
        {
            if (!Application.isEditor)
            {
                adIcon = transform.Find("adIcon").GetComponent<Image>();
                loadingIcon = transform.Find("loadingIcon").GetComponent<Image>();
                uiShiny = GetComponent<UIShiny>();
                onClick.AddListener(OnClick);
                Debug.Assert(adIcon != null);
            }
        }

        protected override void OnEnable()
        {
            if (!Application.isEditor)
            {
                isVideoReady = false;
                if (adEnabled)
                {
                    CheckVideoState();
                }
            }
        }

        public void PlayShiny()
        {
            if (uiShiny != null)
            {
                uiShiny.Play();
            }
        }
        
        private void CheckVideoState()
        {
            lastCheckTime = Time.realtimeSinceStartup;
            isVideoReady = AdsManager.IsRewardVideoReady();
            adIcon.gameObject.SetActive(isVideoReady);
            if (loadingIcon != null)
            {
                loadingIcon.gameObject.SetActive(!isVideoReady);
            }
            interactable = isVideoReady;
            
            if (animator != null)
            {
                animator.enabled = isVideoReady;
            }

            if (uiShiny != null)
            {
                uiShiny.enabled = isVideoReady;
            }
        }

        private void OnClick()
        {
            if (adEnabled)
            {
                lastCheckTime = Time.realtimeSinceStartup;
                adIcon.gameObject.SetActive(false);
                if (loadingIcon != null)
                {
                    loadingIcon.gameObject.SetActive(true);
                }
                interactable = false;
                isVideoReady = false;
            }
        }

        private void Update()
        {
            if (!Application.isEditor && adEnabled && isVideoReady == false && Time.realtimeSinceStartup - lastCheckTime >= checkInterval)
            {
                CheckVideoState();
            }
        }
    }
}
