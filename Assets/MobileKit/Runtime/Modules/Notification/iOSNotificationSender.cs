// #if  UNITY_IOS
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace GameKit
// {
//     public class iOSNotificationSender : MonoBehaviour
//     {
//          private static bool isInitialized;
//          private static int notificationId = 1;
//          private static List<NotificationInfo> notificationInfos;
//          
//         public static void SendNotification(string title, string text, int day, int hour, int minute, int second)
//         {
//             Init();
//             var notificationInfo = new NotificationInfo()
//             {
//                 title = title,
//                 text = text,
//                 day = day,
//                 hour = hour,
//                 minute = minute,
//                 second = second,
//             };
//             notificationInfos.Add(notificationInfo);
//             SendNotification(notificationInfo);
//         }
//         
//         private static void Init()
//         {
//             if(isInitialized)
//                 return;
//             notificationInfos = new List<NotificationInfo>();
//             ResetNotificationChannel();
//             var notificationGo= new GameObject("NotificationBehaviour").AddComponent<NotificationSender>();
//             DontDestroyOnLoad(notificationGo);
//             isInitialized = true;
//         }
//         
//         private static void ResetNotificationChannel()
//         {
//             notificationId = 1;
//             iOSNotificationCenter.ApplicationBadge=0;
//             iOSNotificationCenter.RemoveAllDeliveredNotifications();
//             iOSNotificationCenter.RemoveAllScheduledNotifications();
//         }
//
//         protected static void ReSendNotification()
//         {
//             if (isInitialized&&notificationInfos!=null && notificationInfos.Count > 0)
//             {
//                 ResetNotificationChannel();
//                 foreach (var t in notificationInfos)
//                 {
//                     SendNotification(t);
//                 }
//             }
//         }
//         
//         private static void SendNotification(NotificationInfo notificationInfo)
//         {
//             var time = NotificationSender.GetNotificationTime(notificationInfo);
//             var timeInterval =time.Subtract(DateTime.Now);
//             if (timeInterval.Seconds <= 0)
//             {
//                 return;
//             }
//             var timeTrigger = new iOSNotificationTimeIntervalTrigger()
//             {
//                 TimeInterval = new TimeSpan(timeInterval.Days, timeInterval.Hours, timeInterval.Minutes, timeInterval.Seconds),// timeInterval,
//                 Repeats = false
//             };
//
//             var notification = new iOSNotification()
//             {
//                 Identifier = "_notification_"+ notificationId,
//                 Title = notificationInfo.title,
//                 Body = notificationInfo.text,
//                 Badge = notificationId,
//                 ShowInForeground = false,
//                 ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound | PresentationOption.Badge),
//                 CategoryIdentifier = "category_a",
//                 ThreadIdentifier = "thread1",
//                 Trigger = timeTrigger,
//             };
//             notificationId++;
//             iOSNotificationCenter.ScheduleNotification(notification);
//         }
//     }
// }
// #endif