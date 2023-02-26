using System;
using UnityEngine;
using UnityEngine.UI;

namespace MobileKit
{
    public class CADPACanvas : MonoBehaviour
    {
        [SerializeField] private Text infoText;
        [SerializeField] private Button btnOK;
        
        private Action OnAgreeProtocol;

        private void Awake()
        {
            btnOK.onClick.AddListener(OnOKClick);
        }

        public void OnOpen(string CADPADesc)
        {
            Debug.Assert(!string.IsNullOrEmpty(CADPADesc), "请在AppConfig中填入适龄提醒的描述信息！");
            infoText.text = CADPADesc;
            TimeManager.PauseGame();
        }

        private void OnOKClick()
        {
            TimeManager.StartGame();
            gameObject.SetActive(false);
            Destroy(gameObject, 2f);
        }
    }
}
