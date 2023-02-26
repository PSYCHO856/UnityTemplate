
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace MobileKit
{
    public abstract class UIBasePage : MonoBehaviour
    {
        [FormerlySerializedAs("IsTopMost")] public bool IsPopUp = false;

        protected CanvasGroup CanvasGroup { get; private set; }
        protected Canvas Canvas { get; private set; }
        protected Animator Animator { get; private set; }
        
        private static readonly int AnimParamOpen = Animator.StringToHash("Open");
        private static readonly int AnimParamClose = Animator.StringToHash("Close");
        private CanvasGroup popUpBg;
        
        private bool isOpening;
        
        public virtual void OnOpen()
        {
            CanvasGroup ??= GetComponent<CanvasGroup>();
            Canvas ??= GetComponent<Canvas>();
            Animator ??= GetComponent<Animator>();
            
            if (IsPopUp)
            {
                if (popUpBg)
                {
                    popUpBg.gameObject.SetActive(false);
                }
                popUpBg = UIManager.GetPopUpBg();
                popUpBg.GetComponent<Canvas>().sortingOrder = Canvas.sortingOrder - 1;
                popUpBg.transform.SetSiblingIndex(transform.GetSiblingIndex());
                popUpBg.alpha = 0;
                popUpBg.MDOFade(0.87f, 0.1f, true).SetEasing(Ease.Type.Linear);
            }
            
            if (Animator != null)
            {
                Animator.enabled = true;
                Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
                isOpening = true;
                UIPageAnimEventTrigger.onDoneAnimation += OnOpenDone;
                Animator.SetTrigger(AnimParamOpen);
                if (CanvasGroup != null)
                {
                    CanvasGroup.blocksRaycasts = false;
                }
            }

        }

        private void OnOpenDone(AnimatorStateInfo stateInfo)
        {
            isOpening = false;
            if (stateInfo.IsName("In"))
            {
                UIPageAnimEventTrigger.onDoneAnimation -= OnOpenDone;
                if (CanvasGroup)
                {
                    CanvasGroup.blocksRaycasts = true;
                }
            }

        }

        public virtual void OnClose()
        {
            Animator ??= GetComponent<Animator>();
            CanvasGroup ??= GetComponent<CanvasGroup>();
            
            if (Animator != null)
            {
                Animator.enabled = true;
                UIPageAnimEventTrigger.onDoneAnimation += OnCloseDone;
                Animator.SetTrigger(AnimParamClose);
                if (IsPopUp)
                {
                    popUpBg.MDOFade(0f, 0.1f, true).SetEasing(Ease.Type.Linear);
                }
                if (CanvasGroup)
                {
                    CanvasGroup.blocksRaycasts = false;
                }
            }
            else
            {
                gameObject.SetActive(false);
                if (popUpBg)
                {
                    popUpBg.alpha = 0;
                    popUpBg.gameObject.SetActive(false);
                }
            }
        }

        private void OnCloseDone(AnimatorStateInfo stateInfo)
        {
            if (isOpening) return;
            if (stateInfo.IsName("Out"))
            {
                UIPageAnimEventTrigger.onDoneAnimation -= OnCloseDone;
                gameObject.SetActive(false);
                if (popUpBg)
                {
                    popUpBg.gameObject.SetActive(false);
                }
            }
        }
    }
}
