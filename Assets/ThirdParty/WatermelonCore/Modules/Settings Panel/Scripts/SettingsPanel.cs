#pragma warning disable 0414, 0649

using System;
using UnityEngine;

namespace Watermelon
{
    public class SettingsPanel : MonoBehaviour
    {
        private static SettingsPanel instance;

        [DrawReference] [SerializeField] private SettingsAnimation settingsAnimation;

        [Header("Panel Paddings")] [SerializeField]
        private float xPanelPosition;

        [SerializeField] private float yPanelPosition;

        [Header("Element Paddings")] [SerializeField]
        private float elementSpace;

        [SerializeField] private SettingsButtonInfo[] settingsButtonsInfo;

        private bool isActive;
        private bool isAnimationActive;

        public SettingsButtonInfo[] SettingsButtonsInfo => settingsButtonsInfo;

        public Vector2[] ButtonPositions { get; private set; }

        private void Awake()
        {
            instance = this;

            // Disable all buttons
            for (var i = 0; i < settingsButtonsInfo.Length; i++)
                settingsButtonsInfo[i].SettingsButton.gameObject.SetActive(false);

            InitAnimation();
            InitPositions();
        }

        public void OnValidate()
        {
            InitPositions();
        }

        public void InitAnimation()
        {
            settingsAnimation.Init(this);
        }

        public void InitPositions()
        {
            var lastPosition = new Vector2(xPanelPosition, yPanelPosition);

            ButtonPositions = new Vector2[settingsButtonsInfo.Length];
            for (var i = 0; i < ButtonPositions.Length; i++)
                if (settingsButtonsInfo[i].SettingsButton != null)
                {
                    settingsButtonsInfo[i].SettingsButton.Init(i, this);

                    if (settingsButtonsInfo[i].SettingsButton.IsActive())
                    {
#if UNITY_EDITOR
                        if (!Application.isPlaying)
                            settingsButtonsInfo[i].SettingsButton.RectTransform.gameObject.SetActive(true);
#endif

                        var button = settingsButtonsInfo[i].SettingsButton.RectTransform;

                        var buttonPosition = lastPosition;

                        lastPosition -= new Vector2(0, elementSpace);

                        button.anchoredPosition = new Vector2(xPanelPosition, buttonPosition.y);

                        ButtonPositions[i] = buttonPosition;
                    }
                    else
                    {
#if UNITY_EDITOR
                        if (!Application.isPlaying)
                            settingsButtonsInfo[i].SettingsButton.RectTransform.gameObject.SetActive(false);
#endif

                        ButtonPositions[i] = Vector3.zero;
                    }
                }
                else
                {
                    Debug.Log("[Settings Panel]: Button reference is missing!");
                }
        }

        public void SettingsButton()
        {
            if (isAnimationActive) return;

            if (isActive)
            {
                Hide();

                isActive = false;
            }
            else
            {
                Show();

                isActive = true;
            }
        }

        private void Show()
        {
            isAnimationActive = true;

            StartCoroutine(settingsAnimation.Show(delegate { isAnimationActive = false; }));
        }

        private void Hide(bool immediately = false)
        {
            if (!immediately)
            {
                isAnimationActive = true;

                StartCoroutine(settingsAnimation.Hide(delegate { isAnimationActive = false; }));
            }
            else
            {
                for (var i = settingsButtonsInfo.Length - 1; i >= 0; i--)
                    settingsButtonsInfo[i].SettingsButton.gameObject.SetActive(false);

                isActive = false;
            }
        }

        public static void HidePanel(bool immediately = false)
        {
            if (instance != null)
                instance.Hide(immediately);
        }

        public static bool IsPanelActive()
        {
            if (instance != null)
                return instance.isActive;

            return false;
        }

        [Serializable]
        public class SettingsButtonInfo
        {
            [SerializeField] private SettingsButtonBase settingsButton;

            public SettingsButtonBase SettingsButton
            {
                get => settingsButton;
                set => settingsButton = value;
            }
        }
    }
}

// -----------------
// Settings Panel v 0.1
// -----------------

// Changelog
// v 0.1
// • Added basic version
// • Added example prefab