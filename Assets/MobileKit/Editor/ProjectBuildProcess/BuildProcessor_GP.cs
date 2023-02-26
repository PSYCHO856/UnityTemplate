#if MOBILEKIT_GOOGLEPLAY
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;
using System.IO;
using System.Xml;
using MobileKit;
using UnityEditor;

namespace MobileKit.Editor.BuildProcessor
{
    public class BuildProcessor_GP : IPreprocessBuildWithReport, IPostprocessBuildWithReport, IProcessSceneWithReport
    {
        public int callbackOrder => 10000;

        public void OnPreprocessBuild(BuildReport report)
        {
            PlayerSettings.Android.keystorePass = "idletrainempire1234";
            PlayerSettings.Android.keyaliasName = "com.idle.train.empire.sim.tycoon";
            PlayerSettings.Android.keyaliasPass = "idletrainempire1234";
        }
 
        public void OnPostprocessBuild(BuildReport report)
        {
            if (EditorUserBuildSettings.buildAppBundle)
            {
                BuildUtils.BackupNameTransition();
            }
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            EditorUserBuildSettings.buildAppBundle = false;
            EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Disabled;
            EditorUserBuildSettings.development = false;
            BuildWindow.Instance.SetDebugFlag(true);
        }

        public void OnProcessScene(Scene scene,BuildReport report)
        {
            Debug.Log("OnProcessScene: " + scene.name);
        }
    }
}
#endif