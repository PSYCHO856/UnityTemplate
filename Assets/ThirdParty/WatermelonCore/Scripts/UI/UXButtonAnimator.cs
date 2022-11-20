using UnityEngine.Events;

namespace Watermelon
{
    public class UXButtonAnimator : BaseButton
    {
        public UnityEvent onClick;

        public override void OnClick(Tween.TweenCallback callback = null)
        {
            base.OnClick(delegate
            {
                if (onClick != null)
                    onClick.Invoke();
            });
        }
    }
}