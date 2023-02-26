using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MobileKit
{
    public class AdsInterVideoTipCanvas : MonoBehaviour
    {
        private Action OnFullScreenVideoTipShowed;

        private float openTime;
        private void Awake()
        {
            GetComponent<CanvasScaler>().referenceResolution = BuildConfig.Instance.ReferenceResolution;
            DontDestroyOnLoad(gameObject);
            gameObject.SetActive(false);
        }

        public void Show(Action onFullScreenVideoTipShowed)
        {
            OnFullScreenVideoTipShowed = onFullScreenVideoTipShowed;
            openTime = Time.realtimeSinceStartup;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (Time.realtimeSinceStartup - openTime > 1.0f)
            {
                OnFullScreenVideoTipShowed?.Invoke();
                OnFullScreenVideoTipShowed = null;
            }

            if (Time.realtimeSinceStartup - openTime > 1.5f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
