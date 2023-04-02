using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;
using MobileKit;

public enum UIPageId
{
    Splash,
    Main,
    Settle,
    Rate,
    Info,
    Tips,
    Shop,
    Start,
    NormalChallengeLevelSettle,
    FirstNormalChallengeLevelSettle,
    Wallet,
    RedEnvelope,
    Record,
    ExtraBottle,
    SpeedUp,
    Challenge,
    ChallengeLevelSettle,
    ChallengeLevelFailed,
    ChallengeLevelExit,
    NormalChallenge,
    FirstNormalChallenge
}

public class UIController : Singleton<UIController>
{
    private static SpriteAtlas SpriteAtlas;

    private static SpriteAtlas CarIconAtlas;
    private static Canvas uiCanvas;
    [SerializeField] private UIBasePage mainPage;
    [SerializeField] private UIBasePage infoPage;

    [SerializeField] private SpriteAtlas spriteAtlas;
    [SerializeField] private UIDefine uiDefine;


    public static UIDefine UIDefine { get; set; }

    private static Dictionary<UIPageId, UIBasePage> pages { get; } = new();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        uiCanvas = GetComponent<Canvas>();
        pages.Add(UIPageId.Main, Instantiate(mainPage, transform));
        pages.Add(UIPageId.Info, Instantiate(infoPage, transform));

        SpriteAtlas = spriteAtlas;
        foreach (var page in pages) page.Value.gameObject.SetActive(false);

        UIDefine = Instance.uiDefine;
    }

    public static UIBasePage Open(UIPageId id)
    {
        if (pages.TryGetValue(id, out var page))
        {
            page.gameObject.SetActive(true);
            page.OnOpen();
            return page;
        }

        return null;
    }

    public static void Close(UIPageId id)
    {
        if (pages.TryGetValue(id, out var page))
        {
            page.OnClose();
        }
    }

    public static bool IsOpened(UIPageId id)
    {
        return pages.TryGetValue(id, out var page) && page.gameObject.activeSelf;
    }

    public static Sprite GetUISprite(string name)
    {
        return string.IsNullOrEmpty(name) ? null : SpriteAtlas.GetSprite(name);
    }

    public static Sprite GetCarIcon(string name)
    {
        return string.IsNullOrEmpty(name) ? null : CarIconAtlas.GetSprite(name);
    }

    public static Sprite GetLevelScreenShot(int level)
    {
        var path = Application.persistentDataPath + "/ScreenShot" + level + ".png";

        if (File.Exists(path))
        {
            var bytes = File.ReadAllBytes(path);
            var texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }

        return null;
    }

    public static float GetUIScaleFactor()
    {
        return uiCanvas.scaleFactor;
    }
}