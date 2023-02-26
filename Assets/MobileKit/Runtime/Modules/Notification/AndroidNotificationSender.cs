// #if UNITY_ANDROID
//
// namespace GameKit
// {
//     using System;
//     using System.Collections.Generic;
//     using Unity.Notifications.Android;
//     using UnityEngine;
//     public class AndroidNotificationSender:MonoBehaviour
//     {
//         private static bool isInitialized;
//         private static int notificationId = 1;
//         private static List<NotificationInfo> notificationInfos;
//         
//         
//         public static void SendNotification(string title, string text,int day,int hour,int minute,int second)
//         {
//             Init();
//             var notificationInfo = new NotificationInfo()
//             {
//                 title = title,
//                 text=text,
//                 day=day,
//                 hour = hour,
//                 minute = minute,
//                 second = second,
//             };
//             
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
//             AndroidNotificationCenter.CancelAllNotifications();//清除上次注册的通知
//             var channel = new AndroidNotificationChannel()
//             {
//                 Id = "channel_id",
//                 Name = "Default Channel",
//                 Importance = Importance.Default,
//                 Description = "Generic notifications",
//                 CanShowBadge = true,
//                 EnableLights=true,
//                 LockScreenVisibility=LockScreenVisibility.Public
//             };
//             
//             AndroidNotificationCenter.RegisterNotificationChannel(channel);
//
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
//          
//         }
//         
//         
//         private static void SendNotification(NotificationInfo notificationInfo)//string title, string text,DateTime time,string smallIconId=null,string largeIconId=null)
//         {
//             var time = NotificationSender.GetNotificationTime(notificationInfo);
//             var notification = new AndroidNotification(){
//                 Title = notificationInfo.title,
//                 Text = notificationInfo.text,
//                 FireTime = time,
//                 Number = notificationId
//             };
//             notificationId++;
//             AndroidNotificationCenter.SendNotification(notification, "channel_id");
//         }
//
//      
//     }
// }
// #endif
