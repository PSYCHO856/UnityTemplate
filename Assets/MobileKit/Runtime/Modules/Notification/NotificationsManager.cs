#if ESSENTIAL_KIT
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
using NotificationServices = VoxelBusters.EssentialKit.NotificationServices;

namespace MobileKit
{
    public static class NotificationsManager
    {
        private const string TAG = nameof(NotificationsManager);
        private static NotificationSettings settings;

        public static void Init()
        {
            MLog.Log(TAG, nameof(Init));
            NotificationServices.GetSettings((result) =>
            {
                settings = result.Settings;
                MLog.Log(TAG, "" + settings.PermissionStatus);
                if (settings.PermissionStatus == NotificationPermissionStatus.Authorized)
                {
                    NotificationServices.RemoveAllDeliveredNotifications();
                    NotificationServices.CancelAllScheduledNotifications();
                    ScheduleNotification();
                    NotificationServices.OnNotificationReceived += OnNotificationReceived;
                }
            });
        }

        private static void OnNotificationReceived(NotificationServicesNotificationReceivedResult result)
        {
            if (result.Notification.IsLaunchNotification)
            {
                MLog.Log(TAG, nameof(OnNotificationReceived));
                AnalyticsManager.OnPageEvent("Notification", "Launcher");
            }
        }

        public static void RequestPermission()
        {
            MLog.Log(TAG, nameof(RequestPermission));
            if (settings is { PermissionStatus: NotificationPermissionStatus.NotDetermined }) 
            {
                NotificationServices.RequestPermission(NotificationPermissionOptions.Alert | NotificationPermissionOptions.Sound | NotificationPermissionOptions.Badge, callback: (result, error) =>
                {
                    AnalyticsManager.OnPageEvent("Notification", result.PermissionStatus.ToString());
                    if (result.PermissionStatus == NotificationPermissionStatus.Authorized)
                    {
                        ScheduleNotification();
                        NotificationServices.GetSettings((r) =>
                        {
                            settings = r.Settings;
                        });
                    }
                });
            }
        }
        
        private static void ScheduleNotification()
        {
            MLog.Log(TAG, nameof(ScheduleNotification));
                var data = ResourcesManager.GetAsset<NotificationsConfig>();
                var items = Application.systemLanguage switch
                {
                    SystemLanguage.ChineseSimplified => data.ChineseItems,
                    SystemLanguage.ChineseTraditional => data.ChineseTraditionalItems,
                    SystemLanguage.Chinese => data.ChineseItems,
                    _ => data.EnglishItems
                };
                
                if (items.Length > 0)
                {
                    var index = UnityEngine.Random.Range(0, items.Length - 1);
                    var item = items[index];

                    DateTime nextTime = DateTime.Now;
                    if (DateTime.Now.Hour <= 9)
                    {
                        nextTime = new DateTime(nextTime.Year, nextTime.Month, nextTime.Day, 12, 0, 0);
                    }
                    else if (DateTime.Now.Hour < 16)
                    {
                        nextTime = new DateTime(nextTime.Year, nextTime.Month, nextTime.Day, 19, 0, 0);
                    }
                    else
                    {
                        nextTime = new DateTime(nextTime.Year, nextTime.Month, nextTime.Day, 12, 0, 0) +
                                   TimeSpan.FromDays(1);
                    }
                    double timeSpan = (nextTime - DateTime.Now).TotalSeconds;
                    INotification notification = NotificationBuilder.CreateNotification("notificationId")
                        .SetTitle(item.Title).SetBody(item.Desc)
                        .SetTimeIntervalNotificationTrigger(timeSpan, true)
                        // .SetCalendarNotificationTrigger(dateComponents, true) 
                        .Create();
                    
                    NotificationServices.ScheduleNotification(notification);

                }
        }

        
        
        
        
        
        
        //
        // private static void SendNotificationAtHour( string title, string text, int hour )
        // {
        //     DateTime dateTimeNow = DateTime.Now;
        //     DateTime dateTime = new DateTime(dateTimeNow.Year,dateTimeNow.Month,dateTimeNow.Day,hour,0,0);
        //
        //     TimeSpan timeSpan = dateTime - dateTimeNow;
        //
        //     int dayDelay = timeSpan.Seconds> 0 ? 0 : 1;
        //
        //     LogManager.Log($"SendNotificationAtHour : title:{title}, text:{text}, dayDelay:{dayDelay}, hour:{hour}, ");
        //
        //     //注册离线1天后12点30分10秒通知，title+text
        //     #if UNITY_ANDROID || UNITY_IOS
        //     NotificationSender.SendNotification( title, text, dayDelay, hour, 0, 0 );
        //     #endif
        // }
    }
}

#endif