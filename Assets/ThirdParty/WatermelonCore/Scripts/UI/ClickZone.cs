using UnityEngine;
using UnityEngine.EventSystems;

namespace Watermelon
{
    public class ClickZone : MonoBehaviour, IPointerClickHandler
    {
        public delegate void ClickCallback(bool state);

        private static ClickZone instance;

        private bool isOpened;

        public ClickCallback onZoneClick;

        public static ClickCallback OnZoneClick
        {
            get => instance.onZoneClick;
            set => instance.onZoneClick = value;
        }

        private void Awake()
        {
            instance = this;

            gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Close();
        }

        public static void Close()
        {
            if (instance.isOpened)
            {
                instance.gameObject.SetActive(false);

                OnZoneClick(false);

                instance.isOpened = false;
            }
        }

        public static void Open()
        {
            if (!instance.isOpened)
            {
                instance.gameObject.SetActive(true);

                OnZoneClick(true);

                instance.isOpened = true;
            }
            else
            {
                OnZoneClick(false);
            }
        }
    }
}