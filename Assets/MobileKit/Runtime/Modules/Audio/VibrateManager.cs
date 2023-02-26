using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace MobileKit
{
    public static class VibrateManager
    {
        public static bool IsVibrateEnabled
        {
            get => GameSettingsPrefs.GetBool("IsVibrateEnabled", true);
            set => GameSettingsPrefs.SetBool("IsVibrateEnabled", value);
        }

#if UNITY_IOS 
        [DllImport ( "__Internal" )]
        private static extern bool _HasVibrator();

        [DllImport ( "__Internal" )]
        private static extern void _Vibrate();

        [DllImport ( "__Internal" )]
        private static extern void _VibratePop();

        [DllImport ( "__Internal" )]
        private static extern void _VibratePeek();

        [DllImport ( "__Internal" )]
        private static extern void _VibrateNope();
#endif
        
#if UNITY_ANDROID && !UNITY_EDITOR
      public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
      public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
      public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
      public static AndroidJavaClass unityPlayer;
      public static AndroidJavaObject currentActivity;
      public static AndroidJavaObject vibrator;  
#endif
        
        public static void WeakVibrate()
        {
            if (!IsOnMobile()) return;
            if (IsVibrateEnabled == false) return;
#if UNITY_IOS
            _VibratePeek();
#elif UNITY_ANDROID
            vibrator.Call("vibrate", 20L);
#endif
        }

        public static void StrongVibrate()
        {
            if (!IsOnMobile()) return;
            if (IsVibrateEnabled == false) return;
#if UNITY_IOS
            _VibratePop();
#elif  UNITY_ANDROID
            vibrator.Call("vibrate", 50L);
#endif
        }

        public static void SeriesVibrate()
        {
            if (!IsOnMobile()) return;
            if (IsVibrateEnabled == false) return;
            
#if UNITY_IOS
            _VibrateNope();
#elif  UNITY_ANDROID
            vibrator.Call("vibrate", 200L);
#endif
        }
        
        private static bool IsOnMobile()
        {
            return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        }
        
        
    }
}
