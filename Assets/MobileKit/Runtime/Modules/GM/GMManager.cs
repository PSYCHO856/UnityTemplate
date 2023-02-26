using UnityEngine;
using System.Collections.Generic;
using System;
using Tayx.Graphy;
using UnityEngine.UI;

namespace MobileKit 
{
    public class GMManager : Singleton<GMManager>
    {
        public static Action OnDrawInstruct;
        
        [SerializeField] private int _maxLogCount = 200;

        // [SerializeField] private Text gm;
        // [SerializeField] private Text adDebug;
        // [SerializeField] private Text analyticDebug;
        [SerializeField] private Text fps;
        public static int FPS => Instance._fps;

        [SerializeField] private Button gmButton;

        private DebugType _debugType = DebugType.Console;
        private List<LogData> _logInformations = new List<LogData>();
        private int _currentLogIndex = -1;
        private int _infoLogCount = 0;
        int _warningLogCount = 0;
        private int _errorLogCount = 0;
        private int _fatalLogCount = 0;
        private bool _showInfoLog = true;
        private bool _showWarningLog = true;
        private bool _showErrorLog = true;
        private bool _showFatalLog = true;
        private Vector2 _scrollLogView = Vector2.zero;
        private Vector2 _scrollCurrentLogView = Vector2.zero;
        private Vector2 _scrollSystemView = Vector2.zero;
        private Vector2 _scrollInstructView = Vector2.zero;
        private Rect _windowRect;
        private float _shrinkHeight = 60;

        private int _fps;
        private Color _fpsColor = Color.white;
        private int _frameNumber = 0;
        private float _lastShowFPSTime = 0f;
        
        private CanvasGroup bgCanvas;


        private bool isShowDebugText;
        private bool isShowDebugWindow;
        public static void Init()
        {
            if (BuildConfig.Instance.Debug)
            {
                Instantiate((GameObject)Resources.Load("UI/GMCanvas"));
            }
        }
        
        private void Awake() 
        {
            DontDestroyOnLoad(gameObject);
            isShowDebugText = true;
            isShowDebugWindow = false;
            fps.gameObject.SetActive(!BuildConfig.Instance.IsScreenShotMode && BuildConfig.Instance.Debug);
            
            // adDebug.gameObject.SetActive(BuildConfig.Instance.AdDebug);
            // analyticDebug.gameObject.SetActive(BuildConfig.Instance.AnalyticsDebug);
                
            gmButton.onClick.AddListener(OnGMClick);
            gmButton.gameObject.SetActive(BuildConfig.Instance.Debug);
            GetComponent<CanvasScaler>().referenceResolution = BuildConfig.Instance.ReferenceResolution;

        }

        private void UpdateDebugText()
        {
            fps.gameObject.SetActive(isShowDebugText);
        }
        
        private void OnGMClick()
        {
            if (ScenesManager.SceneLoadStatus != SceneLoadStatus.Activated)
            {
                return;
            }
            isShowDebugWindow = !isShowDebugWindow;
            _debugType = DebugType.Instruct;
            if (isShowDebugWindow == false && bgCanvas != null)
            {
                bgCanvas.gameObject.SetActive(false);
                bgCanvas = null;
            }
            else if (isShowDebugWindow && bgCanvas == null)
            {
                bgCanvas = UIManager.GetPopUpBg();
                bgCanvas.GetComponent<Canvas>().sortingOrder = 32700;
                bgCanvas.transform.SetSiblingIndex(transform.GetSiblingIndex());
                bgCanvas.alpha = 0.8f;
            }
        }
        
        private void Start()
        {
            _windowRect = new Rect(60, 10, 100, _shrinkHeight)
            {
                width = 0.4f * BuildConfig.Instance.ReferenceResolution.x,
                height = 0.4f * BuildConfig.Instance.ReferenceResolution.y
            };
            Application.logMessageReceived += LogHandler;
        }
        
        private void Update()
        {
            _frameNumber += 1;
            float time = Time.realtimeSinceStartup - _lastShowFPSTime;
            if (time >= 1)
            {
                _fps = (int)(_frameNumber / time);
                _frameNumber = 0;
                _lastShowFPSTime = Time.realtimeSinceStartup;
                fps.text = "FPS: " +  _fps;
                if (_fps < 20)
                {
                    fps.color = Color.red;
                } else if (_fps < 40)
                {
                    fps.color = Color.yellow;
                }
                else
                {
                    fps.color = Color.green;
                }
            }
        }
        
        private void LogHandler(string condition, string stackTrace, LogType type)
        {
            LogData log = new LogData
            {
                time = DateTime.Now.ToString("HH:mm:ss"), 
                message = condition, 
                stackTrace = stackTrace
            };

            switch (type)
            {
                case LogType.Assert:
                    log.type = "Fatal";
                    _fatalLogCount += 1;
                    _logInformations.Add(log);
                    break;
                case LogType.Exception:
                case LogType.Error:
                    log.type = "Error";
                    _errorLogCount += 1;
                    _logInformations.Add(log);
                    break;
                case LogType.Warning:
                    break;
                case LogType.Log:
                    log.type = "Info";
                    _infoLogCount += 1;
                    _logInformations.Add(log);
                    break;
            }
            if (_logInformations.Count > _maxLogCount) 
            {
                _logInformations.RemoveAt(0);
            }
            if (_errorLogCount > 0)
            {
                _fpsColor = Color.red;
            }
        }

        private static float _guiScaleFactor = -1.0f;

        private static void UIResizing()
        {
            Vector2 nativeSize = BuildConfig.Instance.ReferenceResolution;
            Matrix4x4 m = new Matrix4x4();

            _guiScaleFactor = (Screen.height / nativeSize.y) * 2; // * 0.7f;
            m.SetTRS(Vector3.one, Quaternion.identity, Vector3.one * _guiScaleFactor);
            GUI.matrix *= m;
        }

        private void OnGUI()
        {
            if (!isShowDebugWindow) return;
            UIResizing();
            _windowRect = GUI.Window(0, _windowRect, ExpansionGUIWindow, "DEBUGGER");
        }
        
        private Color btnHighlightedColor = Color.white;
        private Color btnNormalColor = new Color(0.8f, 0.8f, 0.8f);
        private void ExpansionGUIWindow(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            #region title
            GUILayout.BeginHorizontal();
            GUI.contentColor = _fpsColor;
            GUI.contentColor = (_debugType == DebugType.Instruct ? btnHighlightedColor : btnNormalColor);
            if (GUILayout.Button("Instruct", GUILayout.Height(30)))
            {
                _debugType = DebugType.Instruct;
            }
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUI.contentColor = (_debugType == DebugType.Console ? btnHighlightedColor : btnNormalColor);
            if (GUILayout.Button("Console", GUILayout.Height(30)))
            {
                _debugType = DebugType.Console;
            }
            
            GUI.contentColor = (_debugType == DebugType.System ? btnHighlightedColor : btnNormalColor);
            if (GUILayout.Button("System", GUILayout.Height(30)))
            {
                _debugType = DebugType.System;
            }
            GUI.contentColor = (_debugType == DebugType.Screen ? btnHighlightedColor : btnNormalColor);
            if (GUILayout.Button("Screen", GUILayout.Height(30)))
            {
                _debugType = DebugType.Screen;
            }
            GUI.contentColor = (_debugType == DebugType.Quality ? btnHighlightedColor : btnNormalColor);
            if (GUILayout.Button("Quality", GUILayout.Height(30)))
            {
                _debugType = DebugType.Quality;
            }
            GUI.contentColor = (_debugType == DebugType.Environment ? btnHighlightedColor : btnNormalColor);
            if (GUILayout.Button("Environment", GUILayout.Height(30)))
            {
                _debugType = DebugType.Environment;
            }
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(10);
            
            #region console
            if (_debugType == DebugType.Console)
            {
                GUILayout.BeginHorizontal();
                GUI.contentColor = (_showInfoLog ? btnHighlightedColor : btnNormalColor);
                _showInfoLog = GUILayout.Toggle(_showInfoLog, "Info [" + _infoLogCount + "]");
                GUI.contentColor = (_showWarningLog ? btnHighlightedColor : btnNormalColor);
                _showWarningLog = GUILayout.Toggle(_showWarningLog, "Warning [" + _warningLogCount + "]");
                GUI.contentColor = (_showErrorLog ? btnHighlightedColor : btnNormalColor);
                _showErrorLog = GUILayout.Toggle(_showErrorLog, "Error [" + _errorLogCount + "]");
                GUI.contentColor = (_showFatalLog ? btnHighlightedColor : btnNormalColor);
                _showFatalLog = GUILayout.Toggle(_showFatalLog, "Fatal [" + _fatalLogCount + "]");
                GUI.contentColor = Color.white;
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
                GUILayout.EndHorizontal();
                GUILayout.Space(10);

                float logViewHeight = BuildConfig.Instance.ReferenceResolution.y * 0.25f;
                _scrollLogView = GUILayout.BeginScrollView(_scrollLogView, "Box", GUILayout.Height(logViewHeight));
                for (int i = 0; i < _logInformations.Count; i++)
                {
                    bool show = false;
                    Color color = Color.white;
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
                        if (GUILayout.Toggle(_currentLogIndex == i, ""))
                        {
                            _currentLogIndex = i;
                        }
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

                float currentLogViewHeight = BuildConfig.Instance.ReferenceResolution.y * 0.2f;
                _scrollCurrentLogView = GUILayout.BeginScrollView(_scrollCurrentLogView, "Box", GUILayout.Height(currentLogViewHeight));
                if (_currentLogIndex != -1)
                {
                    GUILayout.Label(_logInformations[_currentLogIndex].message + "\r\n\r\n" + _logInformations[_currentLogIndex].stackTrace);
                }
                GUILayout.EndScrollView();
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
                GUILayout.Label("分辨率：" + Screen.currentResolution.ToString());
                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("全屏"))
                {
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, !Screen.fullScreen);
                }
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
                string value = "";
                if (QualitySettings.GetQualityLevel() == 0)
                {
                    value = " [最低]";
                }
                else if (QualitySettings.GetQualityLevel() == QualitySettings.names.Length - 1)
                {
                    value = " [最高]";
                }

                GUILayout.Label("图形质量：" + QualitySettings.names[QualitySettings.GetQualityLevel()] + value);
                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("降低一级图形质量"))
                {
                    QualitySettings.DecreaseLevel();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("提升一级图形质量"))
                {
                    QualitySettings.IncreaseLevel();
                }
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
                if (GUILayout.Button("退出程序"))
                {
                    Application.Quit();
                }
                GUILayout.EndHorizontal();
            }
            #endregion

            #region Instruct
            else if (_debugType == DebugType.Instruct)
            {
                _scrollInstructView = GUILayout.BeginScrollView(_scrollInstructView, "Box");
                
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("显示/隐藏 统计Monitor"))
                {
                    if (GraphyManager.Instance == null)
                    {
                        Instantiate((GameObject)Resources.Load("UI/Graphy"));
                        GraphyManager.Instance.Enable();
                    }
                    else
                    {
                        GraphyManager.Instance.gameObject.SetActive(!GraphyManager.Instance.gameObject.activeSelf);
                    }
                }

                GUILayout.Space(10);

                if (GUILayout.Button("显示/隐藏FPS"))
                {
                    isShowDebugText = !isShowDebugText;
                    UpdateDebugText();
                }
                GUILayout.EndHorizontal();
                
                OnDrawInstruct?.Invoke();
                
                GUILayout.EndScrollView();
            }
            #endregion
        }

        private void ShrinkGUIWindow(int windowId)
        {
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
        System,
        Screen,
        Quality,
        Environment,
    }
}
