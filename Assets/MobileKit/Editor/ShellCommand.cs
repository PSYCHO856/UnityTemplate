using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MobileKit.Editor
{
    public static class ShellCommand
    {
       #if UNITY_EDITOR_WIN
        private static string SVNProjectPath    => Directory.GetParent(Application.dataPath).ToString();

        [MenuItem("Assets/SVN/上传此项目所有改动",  false, 0)]
        private static void SVNCommit()         => ProcessCommand("TortoiseProc.exe", "/command:commit /path:" + "\"" + GetSelectSVNPaths() + "\"");
        
        [MenuItem("Assets/SVN/从项目根目录更新",  false, 0)]
        private static void SVNUpdate()         => ProcessCommand("TortoiseProc.exe", "/command:update /path:" + "\"" + GetSelectSVNPaths() + "\""+ " /closeonend:0");
        
        
        private static string GetSelectSVNPaths()
        {
            List<string> pathList = new List<string>();
            string[] GUIDs = Selection.assetGUIDs;
            foreach(var guid in GUIDs) {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                pathList.Add(path);
                if (path == "Assets") {
                    pathList.Add(SVNProjectPath + "/ProjectSettings");
                    pathList.Add(SVNProjectPath + "/Packages");
                }
            }
            return string.Join("*", pathList.ToArray());
        }
        
        #endif
        
        
        
        [MenuItem("Tools/MobileKit/Save File/Delete Save File")]
        private static void DeleteSaveFile() {
            PlayerPrefs.DeleteAll();
            
            var path = Path.Combine(Application.persistentDataPath, "*");
        #if UNITY_EDITOR_WIN
            WinCommand("del \"" + path + "\"" + "/S /Q");
        #else
            path = path.Replace(" ", "\\ ");
            MacCommand("rm -rf " + path);
        #endif
        }

        [MenuItem("Tools/MobileKit/Save File/Open Save Directory")]
        private static void OpenSaveDir()
        {
            var path = Application.persistentDataPath;
            Debug.Log(path);
            path = path.Replace("/", "\\");
        #if UNITY_EDITOR_WIN
            WinCommand("start " + path);
        #else
            path = path.Replace("\\", "/");
            path = path.Replace(" ", "\\ ");
            MacCommand("open " + path);
        #endif
        }

        private static void WinCommand(string command)
        {
            command = "/c " + command;
            ProcessCommand(@"C:\Windows\system32\cmd.exe", command);
            UnityEngine.Debug.Log(command);
        }
        
        private static void MacCommand(string command)
        {
            command = "-c \"" + command + "\"";
            ProcessCommand(@"/bin/bash", command);
            UnityEngine.Debug.Log(command);
        }


        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="command">命令名</param>
        /// <param name="argument">参数</param>
        /// <param name="useShellExecute">Sets a value indicating whether to use the operating system shell to start the process</param>
        private static void ProcessCommand(string command, string argument, bool useShellExecute = true) {
            ProcessStartInfo info = new ProcessStartInfo(command)
            {
                Arguments = argument, 
                CreateNoWindow = false, 
                ErrorDialog = true, 
                UseShellExecute = useShellExecute
            };
            if (info.UseShellExecute) {
                info.RedirectStandardOutput = false;
                info.RedirectStandardError = false;
                info.RedirectStandardInput = false;
            } else {
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.RedirectStandardInput = true;
                info.StandardOutputEncoding = System.Text.Encoding.UTF8;
                info.StandardErrorEncoding = System.Text.Encoding.UTF8;
            }
            
            Process process = Process.Start(info);
            if (!info.UseShellExecute) {
                if (process != null)
                {
                    Debug.Log(process.StandardOutput);
                    Debug.Log(process.StandardError);
                }
            }
            process?.WaitForExit();
            process?.Close();
        }
    }
}
