using System;
using UnityEngine;
using UnityEngine.UI;

namespace MobileKit
{
    public class ProtocolCanvas : MonoBehaviour
    {
        [SerializeField] private Button btnProtocol;
        [SerializeField] private Button btnPrivacy;
        [SerializeField] private Button btnAgree;
        
        private Action OnAgreeProtocol;

        private void Awake()
        {
            GetComponent<CanvasScaler>().referenceResolution = BuildConfig.Instance.ReferenceResolution;
            btnProtocol.onClick.AddListener(OnProtocolClick);
            btnPrivacy.onClick.AddListener(OnPrivacyClick);
            btnAgree.onClick.AddListener(OnAgreeClick);
        }

        public void OnOpen(Action onAgreeProtocol)
        {
            OnAgreeProtocol = onAgreeProtocol;
            TimeManager.PauseGame();
        }

        private void OnProtocolClick()
        {
            Application.OpenURL(AppConfig.Instance.ProtocolURL);
        }

        private void OnPrivacyClick()
        {
            Application.OpenURL(AppConfig.Instance.PrivacyURL);
        }

        private void OnAgreeClick()
        {
            OnAgreeProtocol?.Invoke();
            OnAgreeProtocol = null;
            TimeManager.StartGame();
            gameObject.SetActive(false);
            Destroy(gameObject, 2f);
        }
    }
}
