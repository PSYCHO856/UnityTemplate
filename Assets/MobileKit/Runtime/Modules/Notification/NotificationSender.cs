// #if UNITY_ANDROID || UNITY_IOS
// using System;
//
// namespace GameKit
// {
//     public class NotificationSender 
// #if UNITY_ANDROID
//         : AndroidNotificationSender
// #elif UNITY_IOS
//         : iOSNotificationSender
// #endif
//     {
//         private void OnApplicationFocus(bool hasFocus)
//         {
//             if(hasFocus)
//                 ReSendNotification();
//         }
//         
//         /// <summary>
//         /// 得到注册通知的时间
//         /// </summary>
//         /// <returns></returns>
//         public static DateTime GetNotificationTime(NotificationInfo notificationInfo)
//         {
//             var daySpan = new TimeSpan(notificationInfo.day,0,0,0);
//             var dateTime=new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, notificationInfo.hour, notificationInfo.minute, notificationInfo.second) ;
//             dateTime += daySpan;
//             return dateTime;
//         }
//     }
//     
//     public class NotificationInfo
//     {
//         public string title;
//         public string text;
//         public int day;
//         public int hour;
//         public int minute;
//         public int second;
//     }
// }
// #endif