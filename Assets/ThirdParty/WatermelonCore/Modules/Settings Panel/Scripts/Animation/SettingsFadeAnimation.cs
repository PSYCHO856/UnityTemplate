#pragma warning disable 0649

using System.Collections;
using UnityEngine;

namespace Watermelon
{
    [CreateAssetMenu(fileName = "Settings Fade Animation", menuName = "Content/Settings Animation/Fade")]
    public class SettingsFadeAnimation : SettingsAnimation
    {
        [SerializeField] private float initialDelay;
        [SerializeField] private float elementDelay;
        [SerializeField] private float elementFadeDuration;
        [SerializeField] private Ease.Type showEasing = Ease.Type.BackOut;
        [SerializeField] private Ease.Type hideEasing = Ease.Type.BackIn;

        private CanvasGroup[] canvasGroups;

        protected override void AddExtraComponents()
        {
            canvasGroups = new CanvasGroup[settingsButtonsInfo.Length];
            for (var i = 0; i < settingsButtonsInfo.Length; i++)
            {
                canvasGroups[i] = settingsButtonsInfo[i].SettingsButton.gameObject.GetOrSetComponent<CanvasGroup>();
                canvasGroups[i].alpha = 0;
            }
        }

        public override IEnumerator Show(AnimationCallback callback)
        {
            yield return new WaitForSeconds(initialDelay);

            for (var i = 0; i < settingsButtonsInfo.Length; i++)
            {
                if (!settingsButtonsInfo[i].SettingsButton.IsActive()) continue;

                settingsButtonsInfo[i].SettingsButton.RectTransform.anchoredPosition = settingsPanel.ButtonPositions[i];
                settingsButtonsInfo[i].SettingsButton.gameObject.SetActive(true);

                canvasGroups[i].alpha = 0;
                canvasGroups[i].DOFade(1, elementFadeDuration).SetEasing(showEasing);

                yield return new WaitForSeconds(elementDelay);
            }

            callback.Invoke();
        }

        public override IEnumerator Hide(AnimationCallback callback)
        {
            yield return new WaitForSeconds(initialDelay);

            for (var i = settingsButtonsInfo.Length - 1; i >= 0; i--)
            {
                if (!settingsButtonsInfo[i].SettingsButton.IsActive()) continue;

                var index = i;
                canvasGroups[i].DOFade(0, elementFadeDuration).SetEasing(hideEasing).OnComplete(delegate
                {
                    settingsButtonsInfo[index].SettingsButton.gameObject.SetActive(false);
                });

                yield return new WaitForSeconds(elementDelay);
            }

            callback.Invoke();
        }
    }
}

// -----------------
// Settings Panel v 0.1
// -----------------