using System;
using System.Collections;
using MobileKit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MobileKit
{
    public class RateCanvas : MonoBehaviour/*, IPointerDownHandler*/
    {
        [FormerlySerializedAs("clickBtn")] [SerializeField] private Button marketBtn;
        [FormerlySerializedAs("returnBtn")] [SerializeField] private Button closeBtn;

        private Action GoToMarket;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            GetComponent<CanvasScaler>().referenceResolution = BuildConfig.Instance.ReferenceResolution;
            marketBtn.onClick.AddListener(OnMarketClick);
            closeBtn.onClick.AddListener(OnCloseClick);
        }

        public void OnOpen(Action gotoMarket)
        {
            TimeManager.PauseGame();
            GoToMarket = gotoMarket;
        }

        private void OnMarketClick()
        {
            GoToMarket?.Invoke();
            GoToMarket = null;
            Close();
        }
        
        private void OnCloseClick()
        {
            GoToMarket = null;
            Close();
        }

        private void Close()
        {
            TimeManager.StartGame();
            gameObject.SetActive(false);
            Destroy(gameObject, 1f);
        }
    }
}