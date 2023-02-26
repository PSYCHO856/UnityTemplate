using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using TMPro;
using Cfg.railway;

namespace MobileKit
{
    
    public class UIManager : Singleton<UIManager>
    {
        public static Canvas RootCanvas;
        private static Dictionary<string, UIBasePage> pages { get; } = new Dictionary<string, UIBasePage>();
        
        private static int deltaSortingOrder = 1;

        public void Init()
        {
            InitCanvas();
            InitPopUpBgPool();
            if (BuildConfig.Instance.Debug)
            {
                GMManager.OnDrawInstruct += RegisterGMHelper;
            }
        }

        private static void RegisterGMHelper()
        {
            GUILayout.Space(10);
            if (GUILayout.Button("隐藏/显示UI"))
            {
                RootCanvas.gameObject.SetActive(!Instance.gameObject.activeSelf);
            }
            GUILayout.Space(10);
        }
        
        private static void InitCanvas()
        {
            if (RootCanvas == null)
            {
                pages.Clear();
                // RootCanvas = GameObject.Find("RootCanvas")?.GetComponent<Canvas>();
                RootCanvas = Instance.transform.GetComponent<Canvas>();
                if (RootCanvas != null)
                {
                    // RootCanvas.gameObject.SetActive(true);
                    // RootCanvas.transform.parent = Instance.transform;
                    // RootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    // RootCanvas.transform.position = Vector3.zero;
                    // RootCanvas.worldCamera ??= Camera.main;
                    // RootCanvas.planeDistance = 1f;
                    RootCanvas.GetComponent<CanvasScaler>().referenceResolution = BuildConfig.Instance.ReferenceResolution;
                    var defaultPages = RootCanvas.transform.GetComponentsInChildren<UIBasePage>();
                    foreach (var p in defaultPages)
                    {
                        UpdatePageOrder(p);
                        p.gameObject.SetActive(false);
                        pages.Add(p.name, p);
                    }
                    InitPopUpBgPool();
                    RootCanvas.gameObject.layer = LayerMask.NameToLayer("UI");
                }
            }
            
            // if (RootCanvas == null)
            // {
            //     pages.Clear();
            //     var canvas = Resources.Load<Canvas>("UI/RootCanvas");
            //     RootCanvas = Object.Instantiate(canvas, Instance.transform);
            //     RootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            //     // RootCanvas.worldCamera = Camera.main;
            //     // RootCanvas.planeDistance = 1f;
            //     RootCanvas.transform.position = Vector3.zero;
            //     RootCanvas.GetComponent<CanvasScaler>().referenceResolution = BuildConfig.Instance.ReferenceResolution;
            //     InitPopUpBgPool();
            //     RootCanvas.gameObject.layer = LayerMask.NameToLayer("UI");
            // }
        }

        private static void UpdatePageOrder(UIBasePage page)
        {
            if (page.IsPopUp)
            {
                Canvas canvas = page.GetComponent<Canvas>();
                canvas.overrideSorting = true;
                canvas.sortingLayerID = RootCanvas.sortingLayerID;
                canvas.sortingLayerName = RootCanvas.sortingLayerName;
                deltaSortingOrder += 2;
                canvas.sortingOrder = RootCanvas.sortingOrder + deltaSortingOrder;
            }
        }
        
        private static UIBasePage Instantiate(UIBasePage uiPagePrefab)
        {
            UIBasePage page = Object.Instantiate(uiPagePrefab, RootCanvas.transform);
            page.transform.GetOrAddComponent<Canvas>();
            page.name = uiPagePrefab.name;
            page.gameObject.SetActive(true);
            page.gameObject.layer = LayerMask.NameToLayer("UI");
            pages.Add(page.name, page);
            return page;
        }
        
        public static UIBasePage Open(UIBasePage uiPage, bool hasChildPage = false)
        {
            InitCanvas();
            if (pages.TryGetValue(uiPage.name, out var page))
            {
                page.gameObject.SetActive(true);
            }
            else
            {
                page = Instantiate(uiPage);
            }
            UpdatePageOrder(page);
            page.transform.SetAsLastSibling();
            if (hasChildPage)
            {
                var uiBasePage = page.transform.GetComponentsInChildren<UIBasePage>();
                foreach (var p in uiBasePage)
                {
                    p.OnOpen();
                }
            }
            else
            {
                page.OnOpen();
            }
            return page;
        }

        public static void Close(UIBasePage uiPage, bool hasChildPage = false)
        {
            InitCanvas();
            if (pages.TryGetValue(uiPage.name, out var page))
            {
                if (hasChildPage)
                {
                    var uiBasePage = page.transform.GetComponentsInChildren<UIBasePage>();
                    foreach (var p in uiBasePage)
                    {
                        p.OnClose();
                    }
                }
                else
                {
                    page.OnClose();
                }
            }
        }

        public static UIBasePage GetPage(UIBasePage uiPage)
        {
            if (pages.TryGetValue(uiPage.name, out var page))
            {
                return page;
            }
            return null;
        }

        public static bool IsOpened(UIBasePage uiPage)
        {
            return pages.TryGetValue(uiPage.name, out var page) && page.gameObject.activeSelf;
        }

        public static float GetUIScaleFactor() => RootCanvas.scaleFactor;
        
        
        private static Pool popUpBgPool;


        private static void InitPopUpBgPool()
        {
            if (popUpBgPool != null)
            {
                PoolManager.DestroyPool(popUpBgPool);
            }
            var bgPrefab = Resources.Load<GameObject>("UI/PopUpBg");
            var poolSettings = new PoolSettings
            {
                autoSizeIncrement = true,
                objectsContainer = RootCanvas.transform,
                size = 1,
                type = Pool.PoolType.Single,
                singlePoolPrefab = bgPrefab,
                name = "PopUpBg"
            };
            popUpBgPool = PoolManager.AddPool(poolSettings);
        }
        
        public static CanvasGroup GetPopUpBg()
        {
            InitCanvas();
            return popUpBgPool.GetPooledObject().GetComponent<CanvasGroup>();
        }

        public static void SetModelTextTMPro<T>(TextMeshProUGUI text, T param)
        {
            var loc = text.GetComponent<I2.Loc.Localize>();
            if ( text != default && loc != default )
            {
                text.text = Loc.GetModelText( loc, param );
            }
        }

        public static void SetModelTextTMPro<T, U>(TextMeshProUGUI text, T param0, U param1)
        {
            var loc = text.GetComponent<I2.Loc.Localize>();
            if ( text != default && loc != default )
            {
                text.text = Loc.GetModelText( loc, param0, param1 );
            }
        }

        public static void SetModelTextTMPro<T, U, V>(TextMeshProUGUI text, T param0, U param1, V param2)
        {
            var loc = text.GetComponent<I2.Loc.Localize>();
            if ( text != default && loc != default )
            {
                text.text = Loc.GetModelText( loc, param0, param1, param2 );
            }
        }
        
        public static void SetModelTextTMPro<T, U, V, W>(TextMeshProUGUI text, T param0, U param1, V param2, W param3)
        {
            var loc = text.GetComponent<I2.Loc.Localize>();
            if ( text != default && loc != default )
            {
                text.text = Loc.GetModelText( loc, param0, param1, param2, param3 );
            }
        }

        public static string GetCNLangugeNum(int num)
        {
            return GetCNLangugeNum(""+num);
        }

        public static string GetCNLangugeNum(string num)
        {
            if ( LanguageManager.SelectedLanguage == DefaultLanguage.Chinese_Traditional || LanguageManager.SelectedLanguage == DefaultLanguage.Chinese_Simplified || LanguageManager.SelectedLanguage == DefaultLanguage.Japanese )
            {
                return $"<style=\"SizeNumInCN\">{num}</style>";
            }
            return num;
        }

        // /// <summary>
        // /// 根据 稀有度 赋予 列车名字 颜色
        // /// </summary>
        // public static string GetColorStrByQuality( string idtName, QualityType quality )
        // {
        //     if ( quality == QualityType.Green )
        //     {
        //         idtName = $"<style=\"ColorGreenTrain\">{idtName}</style>";
        //     }
        //     else if ( quality == QualityType.Blue )
        //     {
        //         idtName = $"<style=\"ColorBlueTrain\">{idtName}</style>";
        //     }
        //     else if ( quality == QualityType.Purple )
        //     {
        //         idtName = $"<style=\"ColorPurpleTrain\">{idtName}</style>";
        //     }
        //     else if ( quality == QualityType.Orange )
        //     {
        //         idtName = $"<style=\"ColorGold\">{idtName}</style>";
        //     }
        //     return idtName;
        // }

#if ESSENTIAL_KIT
        /// <summary>
        /// 切换场景时会销毁,导致回调报错!!!!!
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="okBtn"></param>
        /// <param name="okCallback"></param>
        /// <param name="cancelBtn"></param>
        /// <param name="cancelCallback"></param>
        public static void ShowDialog(string title, string message, string okBtn, Action okCallback = null, string cancelBtn = "", Action cancelCallback = null)
        {
            AlertDialog dialog = AlertDialog.CreateInstance();
            dialog.Title = title;
            dialog.Message = message;
            dialog.AddButton(okBtn, () =>
            {
                TimeManager.StartGame();
                okCallback?.Invoke();
            });
            if (cancelBtn != "")
            {
                dialog.AddButton(cancelBtn, () =>
                {
                    TimeManager.StartGame();
                    cancelCallback?.Invoke();
                });
            }
            dialog.Show(); //Show the dialog
            TimeManager.PauseGame();
        }
        
        private static Canvas networkWaiting;
        public static void ShowNetworkWaiting()
        {
            return;
            if (networkWaiting == null)
            {
                var networkWaitingPrefab = Resources.Load<Canvas>("UI/NetworkWaitingCanvas");
                networkWaiting = UnityEngine.Object.Instantiate(networkWaitingPrefab, Instance.transform);
            }
            networkWaiting.gameObject.SetActive(true);
        }

        public static void HideNetworkWaiting()
        {
            return;
            if (networkWaiting != null)
            {
                networkWaiting.gameObject.SetActive(false);
            }
        }
        
#endif
    }
}
