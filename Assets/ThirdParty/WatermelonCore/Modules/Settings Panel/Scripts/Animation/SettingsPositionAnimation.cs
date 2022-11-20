#pragma warning disable 0649

using System.Collections;
using UnityEngine;

namespace Watermelon
{
    [CreateAssetMenu(fileName = "Settings Position Animation", menuName = "Content/Settings Animation/Position")]
    public class SettingsPositionAnimation : SettingsAnimation
    {
        [SerializeField] private float initialDelay;
        [SerializeField] private float elementDelay;
        [SerializeField] private float elementMovementDuration;
        [SerializeField] private float offsetPosition;
        [SerializeField] private Ease.Type showEasing = Ease.Type.BackOut;
        [SerializeField] private Ease.Type hideEasing = Ease.Type.BackIn;

        public override IEnumerator Show(AnimationCallback callback)
        {
            yield return new WaitForSeconds(initialDelay);

            for (var i = 0; i < settingsButtonsInfo.Length; i++)
            {
                if (!settingsButtonsInfo[i].SettingsButton.IsActive()) continue;

                settingsButtonsInfo[i].SettingsButton.RectTransform.anchoredPosition =
                    settingsPanel.ButtonPositions[i].AddToX(offsetPosition);
                settingsButtonsInfo[i].SettingsButton.gameObject.SetActive(true);

                settingsButtonsInfo[i].SettingsButton.RectTransform
                    .DOAnchoredPosition(settingsPanel.ButtonPositions[i], elementMovementDuration)
                    .SetEasing(showEasing);

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
                settingsButtonsInfo[i].SettingsButton.RectTransform
                    .DOAnchoredPosition(settingsPanel.ButtonPositions[i].AddToX(offsetPosition),
                        elementMovementDuration).SetEasing(hideEasing).OnComplete(delegate
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