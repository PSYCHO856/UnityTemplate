#if MOBILEKIT_IOS_US
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Xml;
using MobileKit;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

namespace MobileKit.Editor.BuildProcessor
{
	public class BuildProcessor_iOS_CN : IPreprocessBuildWithReport, IPostprocessBuildWithReport, IProcessSceneWithReport
	{
		public int callbackOrder { get; } = 10000;
		
		public void OnPreprocessBuild(BuildReport report)
		{
			PlayerSettings.applicationIdentifier = AppConfig.Instance.BundleId;
		}

		public void OnPostprocessBuild(BuildReport report)
		{
			SetXCodeTarget(report.summary.outputPath);
			SetPlist(report.summary.outputPath);
			BuildUtils.HandlePodFile(report);
			BuildUtils.BackupNameTransition();
            BuildWindow.Instance.SetDebugFlag(true);
		}



		private static void SetXCodeTarget(string path)
		{
				string projectPath = PBXProject.GetPBXProjectPath(path);
				PBXProject pbxProject = new PBXProject();
				pbxProject.ReadFromString(File.ReadAllText(projectPath));
				
				// Set Unity Framework
				string target = pbxProject.GetUnityFrameworkTargetGuid();
				// Build Setting
				pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
				pbxProject.SetBuildProperty(target, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
				pbxProject.SetBuildProperty(target, "GCC_C_LANGUAGE_STANDARD", "gnu99");
				
				pbxProject.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
				// pbxProject.AddBuildProperty(target, "OTHER_LDFLAGS", "-fobjc-arc");
				pbxProject.AddBuildProperty(target, "OTHER_LDFLAGS", "-l\"c++\"");
				pbxProject.AddBuildProperty(target, "OTHER_LDFLAGS", "-l\"c++abi\"");
				pbxProject.AddBuildProperty(target, "OTHER_LDFLAGS", "-l\"sqlite3\"");
				pbxProject.AddBuildProperty(target, "OTHER_LDFLAGS", "-l\"z\"");
				// Add Files
				pbxProject.AddFileToBuild(target, pbxProject.AddFile("usr/lib/libxml2.tbd", "Libraries/libxml2.tbd", PBXSourceTree.Sdk));
				pbxProject.AddFileToBuild(target, pbxProject.AddFile("usr/lib/libresolv.9.tbd", "Libraries/libresolv.9.tbd", PBXSourceTree.Sdk));
				// pbxProject.AddFileToBuild(target, pbxProject.AddFile("Frameworks/MobileKit/SDK_US/Plugins/iOS/GoogleMobileAdsMediationTestSuite.bundle", "Frameworks/MobileKit/SDK_US/Plugins/iOS/GoogleMobileAdsMediationTestSuite.bundle", PBXSourceTree.Sdk));

				// Add Frameworks 
				pbxProject.AddFrameworkToProject(target, "VideoToolbox.framework", false);
				pbxProject.AddFrameworkToProject(target, "Security.framework", false);
				pbxProject.AddFrameworkToProject(target, "CoreTelephony.framework", false);
				pbxProject.AddFrameworkToProject(target, "AdSupport.framework", false);
				pbxProject.AddFrameworkToProject(target, "libsqlite3.tbd", false);
				pbxProject.AddFrameworkToProject(target, "iAd.framework", false);
				pbxProject.AddFrameworkToProject(target, "AVFoundation.framework", false);
				pbxProject.AddFrameworkToProject(target, "libz.tbd", false);
				pbxProject.AddFrameworkToProject(target, "libresolv.tbd", false);
				pbxProject.AddFrameworkToProject(target, "CFNetwork.framework", false);
				pbxProject.AddFrameworkToProject(target, "WebKit.framework", false);
				pbxProject.AddFrameworkToProject(target, "StoreKit.framework", false);
				pbxProject.AddFrameworkToProject(target, "SystemConfiguration.framework", false);
				pbxProject.AddFrameworkToProject(target, "AVKit.framework", false);
				pbxProject.AddFrameworkToProject(target, "CoreMedia.framework", false);
				pbxProject.AddFrameworkToProject(target, "JavaScriptCore.framework", false);
				pbxProject.AddFrameworkToProject(target, "libbz2.1.0.tbd", false);
				pbxProject.AddFrameworkToProject(target, "AppTrackingTransparency.framework", false);
				pbxProject.AddFrameworkToProject(target, "libc++.tbd", false);
				pbxProject.AddFrameworkToProject(target, "Foundation.framework", false);
				pbxProject.AddFrameworkToProject(target, "UIKit.framework", false);
				pbxProject.AddFrameworkToProject(target, "CoreFoundation.framework", false);
				pbxProject.AddFrameworkToProject(target, "CoreGraphics.framework", false);
				
				// pbxProject.AddFrameworkToProject(target, "CoreMotion.framework", false);
				// pbxProject.AddFrameworkToProject(target, "MediaPlayer.framework", false);
				// pbxProject.AddFrameworkToProject(target, "CoreLocation.framework", false);
				// pbxProject.AddFrameworkToProject(target, "SafariServices.framework", false);
				// pbxProject.AddFrameworkToProject(target, "MobileCoreServices.framework", false);
				// pbxProject.AddFrameworkToProject(target, "ImageIO.framework", false);
				
				// Set Main Target
				string mainTarget = pbxProject.GetUnityMainTargetGuid();
				pbxProject.SetBuildProperty(mainTarget, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
				// Write.
				File.WriteAllText(projectPath, pbxProject.WriteToString());
		}


		private static void SetPlist(string path)
		{
			// Set Plist
			string pListPath = path + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(pListPath));
			
			PlistElementDict rootDict = plist.root;
			// This identifier will be used to deliver personalized ads to you.
			// By pressing "Allow" we will be able to provide you personalized ads.
			// 该标识符将用于向您投放个性化广告。 
			rootDict.SetString("NSUserTrackingUsageDescription", "By pressing \"Allow\" we will be able to provide you personalized ads.");
			
			PlistElementDict dictTmp = rootDict.CreateDict("NSAppTransportSecurity");
			dictTmp.SetBoolean( "NSAllowsArbitraryLoads", true);
			plist.WriteToFile(pListPath);
		}
		public void OnProcessScene(Scene scene, BuildReport report)
		{
		}


	}
}
#endif
