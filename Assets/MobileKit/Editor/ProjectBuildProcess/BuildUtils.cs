using UnityEditor;
using System;
using System.IO;
using MobileKit;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace MobileKit.Editor
{
    public static class BuildUtils
    {
	    // todo 根据版本号规则新增版本号
		public static void IncreaseVersion()
		{
// 			string currentVersion = PlayerSettings.bundleVersion;
// 			string[] versions = currentVersion.Split('.');
// 			int major = Convert.ToInt32(versions[0]);
// 			int minor = Convert.ToInt32(versions[1]);
// 			int build = Convert.ToInt32(versions[2]) + 1;
// 			PlayerSettings.bundleVersion = major + "." + minor + "." + build;
//  #if UNITY_ANDROID
//             int newVersionCode = PlayerSettings.Android.bundleVersionCode;
//             PlayerSettings.Android.bundleVersionCode = ++newVersionCode;
// #endif
		}

		public static void BackupNameTransition()
		{
			string sourcePath = Path.Combine(Application.dataPath, "nameTranslation.txt");
			if (File.Exists(sourcePath))
			{
				string targetName = PlayerSettings.applicationIdentifier + "-" + PlayerSettings.bundleVersion;
				string targetPath = "";
#if MOBILEKIT_GOOGLEPLAY
				targetName += "-" + PlayerSettings.Android.bundleVersionCode + ".txt";
				targetPath = Path.Combine(BuildWindow.Instance.AABPath, targetName);
#elif MOBILEKIT_IOS_CN || MOBILEKIT_IOS_US
				targetName += ".txt";
				targetPath = Path.Combine(BuildWindow.Instance.XCodePath, targetName);
#endif
				if (File.Exists(targetPath))
				{
					File.Delete(targetPath);
				}
				IOUtils.Move(sourcePath, targetPath);
			}
		}
		
		           
		public static void HandlePodFile(BuildReport report)
		{
			string podfile = report.summary.outputPath + "/Podfile"; 
			string podfileBak = report.summary.outputPath + "/Podfile.bak";
			if (IOUtils.IsFileExists(podfileBak))
			{
				File.Delete(podfileBak);
			}

			if (IOUtils.IsFileExists(podfile))
			{
				 File.Move(podfile, podfileBak);
			}
		}
		
        public static void AddFolder(string path)
        {
            if (IOUtils.IsDirectoryExists(path + "~"))
            {
                IOUtils.Move(path + "~", path);
            }
        }

        public static void RemoveFolder(string path)
        {
            if (IOUtils.IsDirectoryExists(path))
            {
	            if (Directory.Exists(path + "~"))
	            {
		            Directory.Delete(path + "~");
	            }
                File.Delete(path + ".meta");
                IOUtils.Move(path, path + "~");
            }
        }
		
    }
}