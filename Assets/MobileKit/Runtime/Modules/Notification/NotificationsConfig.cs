using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MobileKit
{
    [Serializable]
    public struct NotificationItem
    {
        public string Title;
        public string Desc;
    }
    
    [CreateAssetMenu(fileName = "NotificationsConfig", menuName = "MobileKit/Notifications Config", order = 1)]
    public class NotificationsConfig : ScriptableObject
    {
        public NotificationItem[] EnglishItems;
        public NotificationItem[] ChineseItems;
        public NotificationItem[] ChineseTraditionalItems;
    }

}
