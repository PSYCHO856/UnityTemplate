using System;
using UnityEngine;

namespace MobileKit
{
    public class TimeManager : Singleton<TimeManager>
    {
        private static DateTime timeStampStartTime = new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc);
        public static DateTime RegisterDay => GameSettingsPrefs.GetDateTime(GameSettingsKey.RegisterDay, DateTime.Today);

        //private set => GameSettingsPrefs.SetDateTime(GameSettingsKey.RegisterDay, value);
        public static DateTime LeaveTime
        {
            get => GameSettingsPrefs.GetDateTime(GameSettingsKey.LeaveTime, DateTime.Now);
            private set => GameSettingsPrefs.SetDateTime(GameSettingsKey.LeaveTime, value);
        }

        /// <summary>
        /// 距离上次离开的时间间隔
        /// </summary>
        public static TimeSpan LeaveTimeSpan => DateTime.Now - LeaveTime;
        
        
        private static float TimeScale = 1;

        public static void Init()
        {
            var instance = Instance;
            var registerDay = RegisterDay;

            TimeScale = 1;
            
            if (BuildConfig.Instance.Debug)
            {
                GMManager.OnDrawInstruct += RegisterGMHelper;
            }
        }

        private void OnApplicationQuit()
        {
            LeaveTime = DateTime.Now;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                LeaveTime = DateTime.Now;
            }
			if (pauseStatus == false)
			{
				if (LeaveTimeSpan.TotalSeconds >= 60)
				{
					AdsManager.ShowSplash("Return");
				}
			}
        }

        public static void StartGame()
        {
            Time.timeScale = TimeScale;
        }
        
        public static void PauseGame()
        {
            Time.timeScale = 0;
        }

        public static void SetSpeed(float scale)
        {
            Time.timeScale = scale;
        }
        
        public static void SpeedUp(float scale = 1.25f)
        {
            if (scale <= 1) return;
            TimeScale *= scale;
            Time.timeScale = TimeScale;
        }

        public static void SlowDown(float scale = 0.8f)
        {
            if (scale >= 1) return;
            TimeScale *= scale;
            Time.timeScale = TimeScale;
        }

        public static void ResetSpeed()
        {
            TimeScale = 1;
            Time.timeScale = TimeScale;
        }

        private static void RegisterGMHelper()
        {
            GUILayout.Space(10);
            GUILayout.Label("TimeScale:  " + Time.timeScale);
            GUILayout.Space(5);
            if (GUILayout.Button("加速 x1.25"))
            {
                SpeedUp();
            }

            GUILayout.Space(5);
            if (GUILayout.Button("减速 /1.25"))
            {
                SlowDown();
            }

            GUILayout.Space(5);
            if (GUILayout.Button("重置速度"))
            {
                ResetSpeed();
            }
            GUILayout.Space(10);
        }

        /// <summary>
        /// 获取当前10位时间戳, 单位:秒
        /// </summary>
        public static long TimeStampNow()
        {
            return (long)(DateTime.UtcNow-timeStampStartTime).TotalSeconds;
        }

        /// <summary>
        /// 获取当前13位时间戳, 单位:毫秒
        /// </summary>
        public static long LongTimeStampNow()
        {
            return (long)(DateTime.UtcNow-timeStampStartTime).TotalMilliseconds;
        }

        /// <summary>
        /// DateTime转10位时间戳, 单位:秒
        /// </summary>
        public static long DateTimeToTimeStamp( DateTime dateTime )
        {
            return (long)(dateTime.ToUniversalTime()-timeStampStartTime).TotalSeconds;
        }

        /// <summary>
        /// DateTime转13位时间戳, 单位:毫秒
        /// </summary>
        public static long DateTimeToLongTimeStamp( DateTime dateTime )
        {
            return (long)(dateTime.ToUniversalTime()-timeStampStartTime).TotalMilliseconds;
        }

        /// <summary>
        /// 获取今日日期字符串，格式yyyy-MM-dd
        /// </summary>
        public static string GetDateStr()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取UTC今日日期字符串，格式yyyy-MM-dd
        /// </summary>
        public static string GetDateStrUTC()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Datetime转string，格式yyyy-MM-dd
        /// </summary>
        public static string DateToStr( DateTime dt )
        {
            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// string转Datetime，参考格式yyyy-MM-dd
        /// </summary>
        public static DateTime StringToDateTime( string dateString )
        {
            // DateTime dt = DateTime.ParseExact(dateString, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
            return Convert.ToDateTime(dateString);
        }

        /// <summary>
        /// 计算经过天数 日期格式yyyy-MM-dd
        /// </summary>
        public static int GetDaysPass( string dateString, string dateStringNext )
        {
            DateTime dt = StringToDateTime(dateString);
            DateTime dt2 = StringToDateTime(dateStringNext);
            TimeSpan ts = dt2.Subtract(dt);
            return ts.Days;
        }

        /// <summary>
        /// 判断是否是第二天 日期格式yyyy-MM-dd
        /// </summary>
        public static bool IsNextDay( string dateString, string dateStringNext )
        {
            DateTime dt = StringToDateTime(dateString);
            DateTime dt2 = StringToDateTime(dateStringNext);
            TimeSpan ts = dt2.Subtract(dt);
            return ts.Days == 1;
        }
        
    }
}