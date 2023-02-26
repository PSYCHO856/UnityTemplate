using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

namespace MobileKit
{
    public class SplashCanvas : MonoBehaviour
    {
        [SerializeField] private Button showAdTestSuite;
        [SerializeField] private Text versionNumber;
        [SerializeField] private AudioClip BGMLoading;
        [SerializeField] private Image imageLoading;

        private float nowProgress;
        private float duration = 3.0f;
        
        private string version;
        
        private void Awake()
        {
            GetComponent<CanvasScaler>().referenceResolution = BuildConfig.Instance.ReferenceResolution;
            MobileKitManager.PreInit();
            MobileKitManager.PostInit();
            AudioManager.PlayMusic(BGMLoading);
// #if MOBILEKIT_GOOGLEPLAY || MOBILEKIT_IOS_US
//             showAdTestSuite.onClick.AddListener(() =>
//             {
//                 AdsUSHandler.Instance.ShowMediationTestSuite();
//             });
// #endif
            
#if UNITY_ANDROID && !UNITY_EDITOR
            version = "V" + AndroidUtils.GetVersionName() + "-" + AndroidUtils.GetVersionCode();
#else
            version = "V" + Application.version;
#endif
        }

        private void Start()
        {
            versionNumber.text = version;
            ScenesManager.LoadScene(BuildConfig.Instance.FirstSceneName);
        }

        private bool isATTChecked;
        private bool isPostInit;
        private int frameCount;
        private void Update()
        {
#if MOBILEKIT_GOOGLEPLAY
            frameCount++;
            if (frameCount == 2)
            {
                AndroidUtils.HideLogo();
            }
#endif

            duration -= Time.deltaTime;
            if (!isATTChecked && duration < 1.3f)
            {
                isATTChecked = true;
                MobileKitManager.CheckATTStatus();
            }

            nowProgress = Mathf.Clamp01((3-duration)*0.33f);
            imageLoading.fillAmount = nowProgress;
            
            if (duration <= 0)
            {
                if (ScenesManager.SceneLoadStatus == SceneLoadStatus.WaitForActivation)
                {
                    ScenesManager.SceneLoadStatus = SceneLoadStatus.Activating;
                    AdsManager.ShowSplash("Start");
                }
            }
        }
    }
}
