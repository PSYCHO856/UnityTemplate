using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class CMDCommand
{
    private static string SVNProjectPath => Directory.GetParent(Application.dataPath).ToString();

    [MenuItem("Assets/SVN/上传此项目所有改动 %#C", false, 0)]
    private static void SVNCommit()
    {
        ProcessCommand("TortoiseProc.exe", "/command:commit /path:" + "\"" + GetSelectSVNPaths() + "\"");
    }

    [MenuItem("Assets/SVN/从项目根目录更新 %#U", false, 0)]
    private static void SVNUpdate()
    {
        ProcessCommand("TortoiseProc.exe",
            "/command:update /path:" + "\"" + GetSelectSVNPaths() + "\"" + " /closeonend:0");
    }

    // [MenuItem("Assets/SVN/CleanUp", false, 3)]
    private static void SVNCleanUp()
    {
        ProcessCommand("TortoiseProc.exe", "/command:cleanup /path:" + "\"" + GetSelectSVNPaths() + "\"");
    }

    // [MenuItem("Assets/SVN/Log",     false, 4)]
    private static void SVNLog()
    {
        ProcessCommand("TortoiseProc.exe", "/command:log /path:" + "\"" + GetSelectSVNPaths() + "\"");
    }

    private static string GetSelectSVNPaths()
    {
        var pathList = new List<string>();
        var GUIDs = Selection.assetGUIDs;
        foreach (var guid in GUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            pathList.Add(path);
            if (path == "Assets")
            {
                pathList.Add(SVNProjectPath + "/ProjectSettings");
                pathList.Add(SVNProjectPath + "/Packages");
            }
        }

        return string.Join("*", pathList.ToArray());
    }

//--------------------------------------------------------------------------------------------------------------

    [MenuItem("Tools/CMD/Delete Save File")]
    private static void DeleteSaveFile()
    {
        var path = Path.Combine(Application.persistentDataPath, "*");
        var command = "del \"" + path + "\"" + "/S /Q";
        Debug.Log(command);
        StartCmd(command);
        PlayerPrefs.DeleteAll();
    }

    public static void ProcessCommand(string command, string argument)
    {
        var info = new ProcessStartInfo(command);
        info.Arguments = argument;
        info.CreateNoWindow = false;
        info.ErrorDialog = true;
        info.UseShellExecute = true;
        if (info.UseShellExecute)
        {
            info.RedirectStandardOutput = false;
            info.RedirectStandardError = false;
            info.RedirectStandardInput = false;
        }
        else
        {
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.RedirectStandardInput = true;
            info.StandardOutputEncoding = Encoding.UTF8;
            info.StandardErrorEncoding = Encoding.UTF8;
        }

        var process = Process.Start(info);

        if (!info.UseShellExecute)
        {
            Debug.Log(process.StandardOutput);
            Debug.Log(process.StandardError);
        }

        // process.WaitForExit();
        process.Close();
    }

    public static void StartCmd(string command)
    {
        var p = new Process();
        p.StartInfo.FileName = @"C:\Windows\system32\cmd.exe";
        p.StartInfo.Arguments = "/c" + command;
        p.StartInfo.UseShellExecute = true;
        p.StartInfo.RedirectStandardInput = false;
        p.StartInfo.RedirectStandardOutput = false;
        p.Start();
        p.WaitForExit();
        p.Close();
    }
}