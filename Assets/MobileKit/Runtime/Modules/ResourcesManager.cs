using UnityEngine;

namespace MobileKit
{
    public static class ResourcesManager
    {
#if MOBILEKIT_IOS_CN
     public const string PLATFORM_FOLDER = "iOS_CN/";
#elif MOBILEKIT_IOS_US
     public const string PLATFORM_FOLDER = "iOS_US/";
#elif MOBILEKIT_GOOGLEPLAY
     public const string PLATFORM_FOLDER = "GooglePlay/";
#else
     public const string PLATFORM_FOLDER = "GooglePlay/";
#endif
        
        public static T GetAsset<T>() where T : Object
        {
            T asset = Resources.Load<T>(PLATFORM_FOLDER + typeof(T).Name);
            if (asset == null)
            {
                asset = Resources.Load<T>(typeof(T).Name);
            }
            Debug.Assert(asset != null, $"配置文件 ({PLATFORM_FOLDER}) {typeof(T).Name} 不存在！");
            return asset;
        }
    }
}