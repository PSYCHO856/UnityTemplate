using System;
using System.Collections.Generic;
using UnityEngine; // using Feamber.Tools;

public class DebuggerWindow : MonoBehaviour
{
    public static Vector2 NativeResolution = new(720, 720);
    private static float _guiScaleFactor = -1.0f;
    private static Vector3 _offset = Vector3.zero;

    /// <summary>
    ///     是否允许调试
    /// </summary>
    [SerializeField] private bool _allowDebugging = true;

    [SerializeField] private int _maxLogCount = 50;
    private int _currentLogIndex = -1;

    private DebugType _debugType = DebugType.Console;
    private int _errorLogCount;
    private bool _expansion;
    private int _fatalLogCount;

    private int _fps;
    private Color _fpsColor = Color.white;
    private int _frameNumber;
    private int _infoLogCount;
    private float _lastShowFPSTime;
    private readonly List<LogData> _logInformations = new();
    private Vector2 _scrollAttrView = Vector2.zero;
    private Vector2 _scrollCurrentLogView = Vector2.zero;
    private Vector2 _scrollLogView = Vector2.zero;
    private Vector2 _scrollSystemView = Vector2.zero;
    private bool _showErrorLog = true;
    private bool _showFatalLog = true;
    private bool _showInfoLog = true;
    private bool _showWarningLog = true;
    private readonly float _shrinkHeight = 60;
    private int _warningLogCount;
    private Rect _windowRect;

    private void Start()
    {
        // enabled = GameController.GameDefine.Debug;
        _windowRect = new Rect(100, 10, 100, _shrinkHeight);
        if (_allowDebugging) Application.logMessageReceived += LogHandler;
    }

    private void Update()
    {
        if (_allowDebugging)
        {
            _frameNumber += 1;
            var time = Time.realtimeSinceStartup - _lastShowFPSTime;
            if (time >= 1)
            {
                _fps = (int) (_frameNumber / time);
                _frameNumber = 0;
                _lastShowFPSTime = Time.realtimeSinceStartup;
            }
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            Time.timeScale *= 2;
            if (Time.timeScale > 4) Time.timeScale = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Minus))
        {
            Time.timeScale /= 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Time.timeScale = 1;
        }
    }

    private void OnGUI()
    {
        if (_allowDebugging)
        {
            UIResizing();
            if (_expansion)
                _windowRect = GUI.Window(0, _windowRect, ExpansionGUIWindow, "DEBUGGER");
            else
                _windowRect = GUI.Window(0, _windowRect, ShrinkGUIWindow, "DEBUGGER");
        }
    }

    private void OnDestory()
    {
        if (_allowDebugging) Application.logMessageReceived -= LogHandler;
    }

    private void LogHandler(string condition, string stackTrace, LogType type)
    {
        var log = new LogData();
        log.time = DateTime.Now.ToString("HH:mm:ss");
        log.message = condition;
        log.stackTrace = stackTrace;

        if (type == LogType.Assert)
        {
            log.type = "Fatal";
            _fatalLogCount += 1;
        }
        else if (type == LogType.Exception || type == LogType.Error)
        {
            log.type = "Error";
            _errorLogCount += 1;
        }
        else if (type == LogType.Warning)
        {
            log.type = "Warning";
            _warningLogCount += 1;
        }
        else if (type == LogType.Log)
        {
            log.type = "Info";
            _infoLogCount += 1;
        }

        _logInformations.Add(log);
        if (_logInformations.Count > _maxLogCount) _logInformations.RemoveAt(0);
        if (_warningLogCount > 0) _fpsColor = Color.yellow;
        if (_errorLogCount > 0) _fpsColor = Color.red;
    }

    private void UIResizing()
    {
        var nativeSize = NativeResolution;
        var m = new Matrix4x4();

        var w = (float) Screen.width;
        var h = (float) Screen.height;
        _guiScaleFactor = Screen.height / nativeSize.y * 0.7f;
        m.SetTRS(Vector3.one, Quaternion.identity, Vector3.one * _guiScaleFactor);
        GUI.matrix *= m;
    }

    private void ExpansionGUIWindow(int windowId)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 20));

        #region title

        GUILayout.BeginHorizontal();
        GUI.contentColor = _fpsColor;
        if (GUILayout.Button("FPS:" + _fps, GUILayout.Height(30)))
        {
            _expansion = false;
            _windowRect.width = 100;
            _windowRect.height = _shrinkHeight;
        }

        GUI.contentColor = _debugType == DebugType.Instruct ? Color.white : Color.gray;
        if (GUILayout.Button("Instruct", GUILayout.Height(30))) _debugType = DebugType.Instruct;
        GUI.contentColor = _debugType == DebugType.Console ? Color.white : Color.gray;
        if (GUILayout.Button("Console", GUILayout.Height(30))) _debugType = DebugType.Console;
        GUI.contentColor = _debugType == DebugType.Memory ? Color.white : Color.gray;
        if (GUILayout.Button("Memory", GUILayout.Height(30))) _debugType = DebugType.Memory;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUI.contentColor = _debugType == DebugType.System ? Color.white : Color.gray;
        if (GUILayout.Button("System", GUILayout.Height(30))) _debugType = DebugType.System;
        GUI.contentColor = _debugType == DebugType.Screen ? Color.white : Color.gray;
        if (GUILayout.Button("Screen", GUILayout.Height(30))) _debugType = DebugType.Screen;
        GUI.contentColor = _debugType == DebugType.Quality ? Color.white : Color.gray;
        if (GUILayout.Button("Quality", GUILayout.Height(30))) _debugType = DebugType.Quality;
        GUI.contentColor = _debugType == DebugType.Environment ? Color.white : Color.gray;
        if (GUILayout.Button("Environment", GUILayout.Height(30))) _debugType = DebugType.Environment;
        GUI.contentColor = Color.white;
        GUILayout.EndHorizontal();

        #endregion

        #region console

        if (_debugType == DebugType.Console)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear"))
            {
                _logInformations.Clear();
                _fatalLogCount = 0;
                _warningLogCount = 0;
                _errorLogCount = 0;
                _infoLogCount = 0;
                _currentLogIndex = -1;
                _fpsColor = Color.white;
            }

            GUI.contentColor = _showInfoLog ? Color.white : Color.gray;
            _showInfoLog = GUILayout.Toggle(_showInfoLog, "Info [" + _infoLogCount + "]");
            GUI.contentColor = _showWarningLog ? Color.white : Color.gray;
            _showWarningLog = GUILayout.Toggle(_showWarningLog, "Warning [" + _warningLogCount + "]");
            GUI.contentColor = _showErrorLog ? Color.white : Color.gray;
            _showErrorLog = GUILayout.Toggle(_showErrorLog, "Error [" + _errorLogCount + "]");
            GUI.contentColor = _showFatalLog ? Color.white : Color.gray;
            _showFatalLog = GUILayout.Toggle(_showFatalLog, "Fatal [" + _fatalLogCount + "]");
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();

            _scrollLogView = GUILayout.BeginScrollView(_scrollLogView, "Box", GUILayout.Height(165));
            for (var i = 0; i < _logInformations.Count; i++)
            {
                var show = false;
                var color = Color.white;
                switch (_logInformations[i].type)
                {
                    case "Fatal":
                        show = _showFatalLog;
                        color = Color.red;
                        break;
                    case "Error":
                        show = _showErrorLog;
                        color = Color.red;
                        break;
                    case "Info":
                        show = _showInfoLog;
                        color = Color.white;
                        break;
                    case "Warning":
                        show = _showWarningLog;
                        color = Color.yellow;
                        break;
                }

                if (show)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Toggle(_currentLogIndex == i, "")) _currentLogIndex = i;
                    GUI.contentColor = color;
                    GUILayout.Label("[" + _logInformations[i].type + "] ");
                    GUILayout.Label("[" + _logInformations[i].time + "] ");
                    GUILayout.Label(_logInformations[i].message);
                    GUILayout.FlexibleSpace();
                    GUI.contentColor = Color.white;
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.EndScrollView();

            _scrollCurrentLogView = GUILayout.BeginScrollView(_scrollCurrentLogView, "Box", GUILayout.Height(100));
            if (_currentLogIndex != -1)
                GUILayout.Label(_logInformations[_currentLogIndex].message + "\r\n\r\n" +
                                _logInformations[_currentLogIndex].stackTrace);
            GUILayout.EndScrollView();
        }

        #endregion

        #region memory

        else if (_debugType == DebugType.Memory)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Memory Information");
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("卸载未使用的资源")) Resources.UnloadUnusedAssets();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("使用GC垃圾回收")) GC.Collect();
            GUILayout.EndHorizontal();
        }

        #endregion

        #region system

        else if (_debugType == DebugType.System)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("System Information");
            GUILayout.EndHorizontal();
            _scrollSystemView = GUILayout.BeginScrollView(_scrollSystemView, "Box");
            GUILayout.Label("操作系统：" + SystemInfo.operatingSystem);
            GUILayout.Label("语言：" + Application.systemLanguage);
            GUILayout.Label("系统内存：" + SystemInfo.systemMemorySize + "MB");
            GUILayout.Label("处理器：" + SystemInfo.processorType);
            GUILayout.Label("处理器数量：" + SystemInfo.processorCount);
            GUILayout.Label("显卡：" + SystemInfo.graphicsDeviceName);
            GUILayout.Label("显卡类型：" + SystemInfo.graphicsDeviceType);
            GUILayout.Label("API版本：" + SystemInfo.graphicsDeviceVersion);
            GUILayout.Label("显存：" + SystemInfo.graphicsMemorySize + "MB");
            GUILayout.Label("显卡标识：" + SystemInfo.graphicsDeviceID);
            GUILayout.Label("显卡供应商：" + SystemInfo.graphicsDeviceVendor);
            GUILayout.Label("显卡供应商标识码：" + SystemInfo.graphicsDeviceVendorID);
            GUILayout.Label("设备模式：" + SystemInfo.deviceModel);
            GUILayout.Label("设备名称：" + SystemInfo.deviceName);
            GUILayout.Label("设备类型：" + SystemInfo.deviceType);
            GUILayout.Label("设备标识：" + SystemInfo.deviceUniqueIdentifier);
            GUILayout.EndScrollView();
        }

        #endregion

        #region screen

        else if (_debugType == DebugType.Screen)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Screen Information");
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");
            GUILayout.Label("DPI：" + Screen.dpi);
            GUILayout.Label("分辨率：" + Screen.currentResolution);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("全屏"))
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height,
                    !Screen.fullScreen);
            GUILayout.EndHorizontal();
        }

        #endregion

        #region Quality

        else if (_debugType == DebugType.Quality)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Quality Information");
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");
            var value = "";
            if (QualitySettings.GetQualityLevel() == 0)
                value = " [最低]";
            else if (QualitySettings.GetQualityLevel() == QualitySettings.names.Length - 1) value = " [最高]";

            GUILayout.Label("图形质量：" + QualitySettings.names[QualitySettings.GetQualityLevel()] + value);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("降低一级图形质量")) QualitySettings.DecreaseLevel();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("提升一级图形质量")) QualitySettings.IncreaseLevel();
            GUILayout.EndHorizontal();
        }

        #endregion

        #region Environment

        else if (_debugType == DebugType.Environment)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Environment Information");
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");
            GUILayout.Label("项目名称：" + Application.productName);
            GUILayout.Label("项目版本：" + Application.version);
            GUILayout.Label("Unity版本：" + Application.unityVersion);
            GUILayout.Label("公司名称：" + Application.companyName);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("退出程序")) Application.Quit();
            GUILayout.EndHorizontal();
        }

        #endregion

        #region Instruct

        else if (_debugType == DebugType.Instruct)
        {
            GUILayout.BeginVertical();

            GUILayout.Space(10);
            // if (GUILayout.Button("打开广告"))
            // {
            //     AdsManager.HideAds = false;
            // }
            //
            // GUILayout.Space(10);
            // if (GUILayout.Button("关闭广告"))
            // {
            //     AdsManager.HideAds = true;
            // }
            // GUILayout.EndVertical();
            //
            // GUILayout.Space(10);
            // if (GUILayout.Button("上传数据"))
            // {
            //     AppManager.UpdatePlayerData(100, 10.0f);
            //     RecordManager.RecordData.CashPool = 10.0f;
            //     RecordManager.RecordData.totalADNum++;
            // }
            //
            // GUILayout.Space(10);
            // if (GUILayout.Button("更新数据"))
            // {
            //     AppManager.GetPlayerData((success, info) => {});
            // }
        }

        #endregion
    }

    private void ShrinkGUIWindow(int windowId)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 20));

        GUI.contentColor = _fpsColor;
        if (GUILayout.Button("FPS:" + _fps, GUILayout.Width(80), GUILayout.Height(30)))
        {
            _expansion = true;
            _windowRect.width = 360;
            _windowRect.height = 760;
        }

        GUI.contentColor = Color.white;
    }
}

public struct LogData
{
    public string time;
    public string type;
    public string message;
    public string stackTrace;
}

public enum DebugType
{
    Instruct,
    Console,
    Memory,
    System,
    Screen,
    Quality,
    Environment
}