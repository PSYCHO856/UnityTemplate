using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MobileKit 
{
    public class MobileKitButton : Button
    {
        [SerializeField] private ButtonClickedEvent m_OnPointerDown = new ButtonClickedEvent();
        [SerializeField] private ButtonClickedEvent m_OnPointerUp = new ButtonClickedEvent();
        [SerializeField] private ButtonClickedEvent m_OnLongPress = new ButtonClickedEvent();
        [SerializeField] private ButtonClickedEvent m_OnDoubleClick = new ButtonClickedEvent();
        
        public ButtonClickedEvent onPointerDown
        {
            get => m_OnPointerDown;
            set => m_OnPointerDown = value;
        }
        
        public ButtonClickedEvent onPointerUp
        {
            get => m_OnPointerUp;
            set => m_OnPointerUp = value;
        }
        
        public ButtonClickedEvent onLongPress
        {
            get => m_OnLongPress;
            set => m_OnLongPress = value;
        }
        
        public ButtonClickedEvent onDoubleClick
        {
            get => m_OnDoubleClick;
            set => m_OnDoubleClick = value;
        }
        
        protected MobileKitButton()
        {
        }

        private bool isStartPress;
        private float curPressDownTime;
        private float longPressThreshold = 1;
        private bool longPressTrigger;


        private void Update()
        {
            if (!interactable) return;
            if (isStartPress && !longPressTrigger)
            {
                if (Time.time > curPressDownTime + longPressThreshold)
                {
                    longPressTrigger = true;
                    isStartPress = false;
                    m_OnLongPress?.Invoke();
                }
            }
        }

        public void SetLongPressThreshold(float value)
        {
            longPressThreshold = value;
        }
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) return;
            base.OnPointerDown(eventData);
            m_OnPointerDown?.Invoke();
            curPressDownTime = Time.time;
            isStartPress = true;
            longPressTrigger = false;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable) return;
            base.OnPointerUp(eventData);
            m_OnPointerUp?.Invoke();
            isStartPress = false;
            longPressTrigger = false;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (!interactable) return;
            base.OnPointerExit(eventData);
            isStartPress = false;
            longPressTrigger = false;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable) return;
            if (!longPressTrigger)
            {
                if (eventData.clickCount == 1)
                {
                    onClick?.Invoke();
                }
                else if (eventData.clickCount == 2)
                {
                    m_OnDoubleClick?.Invoke();
                }
            }
        }
    }
}
