using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Sirenix.OdinInspector;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.Xml;
using Newtonsoft.Json;

namespace MobileKit.Editor
{
    public enum Platform
    {
        GP,
        AppleUS,
        AppleCN,
    }
    public class RemoveUnusedFolder : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder { get; }
        public void OnPreprocessBuild(BuildReport report)
        {
            string fontsPath = Path.Combine(Application.dataPath, "MobileKit/Fonts/Fonts");
            BuildUtils.RemoveFolder(fontsPath);
        }
        
        
        public void OnPostprocessBuild(BuildReport report)
        {
            string fontsPath = Path.Combine(Application.dataPath, "MobileKit/Fonts/Fonts");
            BuildUtils.AddFolder(fontsPath);
        }
    
    }
    
    
    
    
    
    [CreateAssetMenu(fileName = "BuildWindow", menuName = "MobileKit/BuildWindow", order = 1)]
    public class BuildWindow : ScriptableObjectSingleton<BuildWindow>, IActiveBuildTargetChanged
    {
        [ShowInInspector]
        public bool DebugFlag => BuildConfig.Instance.Debug;
        
        [Space]
        public string AABPath = "D:\\AndroidAAB";
        public string XCodePath = "D:\\iOS";

        public string SDKPath_CN = "MobileKit/SDK_CN";
        public string SDKPath_US = "MobileKit/SDK_US";
        public string StringXMLPath = "Plugins/Android/com.mobilekit.androidlib/src/main/res/values/strings.xml";
        
        [PropertySpace(spaceBefore:10, spaceAfter:10), InlineButton("SwitchPlatform", "Switch")]
        public Platform Platform;

        
        public void SetDebugFlag(bool enable)
        {
            BuildConfig.Instance.Debug = enable;
            EditorUtility.SetDirty(BuildConfig.Instance);
            AssetDatabase.SaveAssetIfDirty(BuildConfig.Instance);
        }
        
        public void SwitchPlatform()
        {
            bool isSwitchBuildTarget = SwitchActiveBuildTarget();
            if (!isSwitchBuildTarget)
            {
                SwitchPlatformInternal();
            }else{
                UpdateXML();
            }
        }

        private void SwitchPlatformInternal()
        {
            UpdateSymbols();
            ChangePlatformFolder();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UpdateXML();
        }

        public bool SwitchActiveBuildTarget()
        {
            if (EditorUtility.DisplayDialog("Warning", "确定要切换平台吗?", "ok", "cancel"))
            {
                if (Platform == Platform.GP)
                {
                    if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
                    {
                        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
                        return true;
                    }
                } else if (Platform == Platform.AppleCN || Platform == Platform.AppleUS)
                {
                    if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
                    {
                        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
                        return true;
                    }
                }
            }
            return false;
        }

        public int callbackOrder { get; } = 0;

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            Debug.Log("切换到: " + newTarget);
            SwitchPlatformInternal();
        }

        private void UpdateSymbols()
        {
            if (Platform == Platform.GP)
            {
                SymbolsManager.AddToPlatform("MOBILEKIT_GOOGLEPLAY", BuildTargetGroup.Android);
            } else if (Platform == Platform.AppleCN)
            {
                SymbolsManager.RemoveFromPlatform("MOBILEKIT_IOS_US", BuildTargetGroup.iOS);
                SymbolsManager.AddToPlatform("MOBILEKIT_IOS_CN", BuildTargetGroup.iOS);
            } else if (Platform == Platform.AppleUS)
            {
                SymbolsManager.RemoveFromPlatform("MOBILEKIT_IOS_CN", BuildTargetGroup.iOS);
                SymbolsManager.AddToPlatform("MOBILEKIT_IOS_US", BuildTargetGroup.iOS);
            }
        }

        private void ChangePlatformFolder()
        {
            string sdkPathCN = Path.Combine(Application.dataPath, SDKPath_CN);
            string sdkPathUS = Path.Combine(Application.dataPath, SDKPath_US);
            if (Platform == Platform.GP || Platform == Platform.AppleUS)
            {
                BuildUtils.RemoveFolder(sdkPathCN);
                BuildUtils.AddFolder(sdkPathUS);
            } else if (Platform == Platform.AppleCN)
            {
                BuildUtils.RemoveFolder(sdkPathUS);
                BuildUtils.AddFolder(sdkPathCN);
            }
        }

        private void UpdateXML()
        {
#if MOBILEKIT_GOOGLEPLAY
            if ( Platform == Platform.GP )
            {
                Debug.Log("UpdateXML");

                var localPath = UnityEngine.Application.dataPath+"/"+StringXMLPath;

                Dictionary<string,string> keyValues =  new Dictionary<string, string>();
                var serializedString = JsonConvert.SerializeObject(AppConfig.Instance.IAPInfos, Newtonsoft.Json.Formatting.None);
                keyValues.Add("inAppSKUSJson",serializedString);
                keyValues.Add("REWARDVIDEO_AD_ID",AppConfig.Instance.MAXRewardId);
                UpdateStringXmlBatch( localPath, keyValues );
            }
#endif
        }

        public static void UpdateStringXmlBatch( string localPath, Dictionary<string,string> keyValues )
        {
            if ( File.Exists(localPath) )
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(localPath);
                XmlNodeList nodeList = xmlDoc.SelectSingleNode("resources").ChildNodes;
                // cannot use <!-- --> in xml
                foreach ( XmlElement xe in nodeList )
                {
                    var key = xe.GetAttribute("name");
                    if ( keyValues.ContainsKey( key ) && !string.IsNullOrWhiteSpace(keyValues[key]) )
                    {
                        xe.InnerText = keyValues[key];       
                        Debug.Log($"更新XML成功！{key}-{keyValues[key]}");
                    }
                }
                xmlDoc.Save(localPath);
            }
        }

        
        
        [HorizontalGroup("build")]
        [VerticalGroup("build/left")]
        [Button(ButtonSizes.Large)]
        public void Development()
        {
            SetDebugFlag(true);
            EditorUserBuildSettings.development = true;
#if MOBILEKIT_GOOGLEPLAY
            BuildTarget buildTarget = BuildTarget.Android;
			PlayerSettings.applicationIdentifier = AppConfig.Instance.BundleId;
            string path = AABPath + "\\" + PlayerSettings.applicationIdentifier+ ".apk";
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            EditorUserBuildSettings.buildAppBundle = false;
            EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Disabled;
            BuildPipeline.BuildPlayer(GetBuildScenes(), path, buildTarget, BuildOptions.None);
            
#elif MOBILEKIT_IOS_CN || MOBILEKIT_IOS_US
            BuildTarget buildTarget = BuildTarget.iOS;
            string path = XCodePath + "\\" + PlayerSettings.applicationIdentifier;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            BuildPipeline.BuildPlayer(GetBuildScenes(), path, buildTarget, BuildOptions.None);
#endif
        }
        
        
        
        [HorizontalGroup("build")]
        [VerticalGroup("build/right")]
        [Button(ButtonSizes.Large)]
        public void Publish()
        {
            SetDebugFlag(false);
            BuildUtils.IncreaseVersion();
            EditorUserBuildSettings.development = false;
#if MOBILEKIT_GOOGLEPLAY
            BuildTarget buildTarget = BuildTarget.Android;
            string path = AABPath + "\\" + PlayerSettings.applicationIdentifier + "-" + PlayerSettings.bundleVersion + "-" + PlayerSettings.Android.bundleVersionCode +  ".aab";
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            EditorUserBuildSettings.buildAppBundle = true;
            EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Debugging;
            BuildPipeline.BuildPlayer(GetBuildScenes(), path, buildTarget, BuildOptions.None);
            
#elif MOBILEKIT_IOS_CN || MOBILEKIT_IOS_US
            BuildTarget buildTarget = BuildTarget.iOS;
            string path = XCodePath + "\\" + PlayerSettings.applicationIdentifier;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            BuildPipeline.BuildPlayer(GetBuildScenes(), path, buildTarget, BuildOptions.None);
#endif
        }
        
        private string[] GetBuildScenes()
        {
            var scenes = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene == null) continue;
                if (scene.enabled)
                {
                    scenes.Add(scene.path);
                }
            }
            return scenes.ToArray();
        }
    }
}