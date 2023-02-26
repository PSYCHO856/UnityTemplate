#if MOBILEKIT_GOOGLEPLAY
using System.Collections.Generic;
using UnityEngine;

namespace MobileKit
{
    public static class AndroidUtils
    {
        public static AndroidJavaObject ToAndroidMap(this Dictionary<string, string> dictionary)
        {
            if(dictionary == null)
            {
                return null;
            }
            AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");
            foreach(KeyValuePair<string, string> pair in dictionary)
            {
                map.Call<string>("put", pair.Key, pair.Value);
            }
            return map;
        }
        
        /*
         https://developer.android.com/distribute/marketing-tools/linking-to-google-play?hl=zh-cn#android-app
         目标结果	            链接格式
         显示特定应用的商品详情	    https://play.google.com/store/apps/details?id=<package_name>
         显示特定发布商的开发者页面	https://play.google.com/store/apps/dev?id=<developer_id>
         显示搜索查询的结果	    https://play.google.com/store/search?q=<query>
         显示某个应用合集	        https://play.google.com/store/apps/collection/<collection_name>
        */
        
        /// <summary>
        /// 跳转到应用商店页面
        /// <param name="uri">http uri</param>
        /// <param name="marketPackage">商店Package Name</param>
        /// </summary>
        public static void JumpMarket(string uri, string marketPackage = "com.android.vending")
        {
            if (!Application.isEditor)
            {
                  AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
                  AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
                  intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_VIEW"));
                  AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri"); // 对应的安卓调用函数是Uri.parse()
                  AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", uri);
                  intentObject.Call<AndroidJavaObject>("setData", uriObject);
                  intentObject.Call<AndroidJavaObject>("setPackage", marketPackage); // 记得要set package他才知道在哪里运行url sheme，否则url会从默认的浏览器里打开

                  AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                  AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
                  currentActivity.Call("startActivity", intentObject);
            }
        }
        
          //int vesioncode =  context().getPackageManager().getPackageInfo(context().getPackageName(), 0).versionCode;
          public static int GetVersionCode() {
              AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
              AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity"); 
              AndroidJavaObject packageManager = context.Call<AndroidJavaObject>("getPackageManager");
              string packageName = context.Call<string>("getPackageName");
              AndroidJavaObject packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
              return packageInfo.Get<int>("versionCode");
          }
          
          //int versionName =  context().getPackageManager().getPackageInfo(context().getPackageName(), 0).versionName;
          public static string GetVersionName() {
              AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
              AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity"); 
              AndroidJavaObject packageManager = context.Call<AndroidJavaObject>("getPackageManager");
              string packageName = context.Call<string>("getPackageName");
              AndroidJavaObject packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
              return packageInfo.Get<string>("versionName");
          }


          public static void HideLogo()
          {
              Debug.Log(" HideLogo ");
#if MOBILEKIT_GOOGLEPLAY
                var activity = new AndroidJavaClass("com.mobilekit.sdk.MainActivity");
                activity.CallStatic("HideLogo");
#endif
          }

    }
}
#endif
