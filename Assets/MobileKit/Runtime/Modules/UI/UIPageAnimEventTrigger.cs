using System;
using UnityEngine;

namespace MobileKit
{
    public class UIPageAnimEventTrigger : StateMachineBehaviour
    {
        public static Action<AnimatorStateInfo> onDoneAnimation;


        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            onDoneAnimation?.Invoke(stateInfo);
        }
    }
}