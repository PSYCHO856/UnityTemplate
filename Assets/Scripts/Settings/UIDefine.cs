using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "UIDefine", menuName = "Settings/UIDefine")]
public class UIDefine : ScriptableObject
{
    [Header("Logo")] [InspectorShow("真实停车模拟器")]
    public Sprite zstcmn;

    [InspectorShow("老板挪个车")] public Sprite lbngc;

    [Header("通用")] [InspectorShow("星星 暗")] public Sprite starNormal;

    [InspectorShow("星星 亮")] public Sprite starHighlight;

    [InspectorShow("钥匙 暗")] public Sprite keyNormal;

    [InspectorShow("钥匙 亮")] public Sprite keyHighlight;

    [Header("主页")] [InspectorShow("星星出现时的动画曲线")]
    public Ease starInEase = Ease.OutExpo;

    [InspectorShow("星星出现时的动画时间")] public float starInTime = 0.7f;

    [InspectorShow("星星消失时的动画曲线")] public Ease starOutEase = Ease.InElastic;

    [InspectorShow("星星消失时的动画时间")] public float starOutTime = 1.0f;

    [InspectorShow("看广告提示Icon")] public Sprite adTipLevel;

    [InspectorShow("看广告跳关Icon")] public Sprite adJumpLevel;

    [InspectorShow("音量开")] public Sprite soundOn;

    [InspectorShow("音量关")] public Sprite soundOff;


    [Header("车库")] [InspectorShow("车辆底板 普通")]
    public Sprite garageCarBgNormal;

    [InspectorShow("车辆地板 选中")] public Sprite garageCarBgSelect;

    [InspectorShow("车辆地板 高亮")] public Sprite garageCarBgHighlight;

    [InspectorShow("未解锁车辆占位Icon")] public Sprite garageLockedCar;

    [InspectorShow("翻页圆点 暗")] public Sprite garageDotNormal;

    [InspectorShow("翻页圆点 亮")] public Sprite garageDotHighlight;

    [InspectorShow("车库隐藏车辆蒙版颜色")] public Color garageHideColor;

    [Header("关卡选择")] [InspectorShow("关卡底板 已解锁")]
    public Sprite levelItemBgUnlocked;

    [InspectorShow("关卡底板 下一关")] public Sprite levelItemBgCurrent;

    [InspectorShow("关卡底板 锁定")] public Sprite levelItemBgLocked;

    [InspectorShow("里程数 普通颜色")] public Color levelLengthColorNormal;

    [InspectorShow("里程数 高亮颜色")] public Color levelLengthColorHighlight;

    [Header("结算")] [InspectorShow("结束文字出现延时")]
    public float settleCompleteDelay = 0.5f;

    [InspectorShow("结束文字动画时长")] public float settleCompleteDuration = 0.5f;

    [InspectorShow("结束文字动画曲线")] public Ease settleCompleteEase = Ease.Linear;

    [InspectorShow("星星特效播放延时")] public float settleStarDelay = 0.5f;

    [InspectorShow("星星特效播放间隔")] public float settleStarInterval = 1f;

    [InspectorShow("车辆进度出现延时")] public float settleCarProgressDelay = 0.5f;

    [InspectorShow("进入下一关延时")] public float settleNextLevelDelay = 2.0f;

    [Header("额外金币")] [InspectorShow("向外扩散的半径")]
    public float outRadius = 50;

    [InspectorShow("向外扩散的时长")] public float outTimer = 1f;

    [InspectorShow("向外扩散的曲线")] public Ease outEase = Ease.Linear;

    [InspectorShow("飞行时间")] public float flyTimer = 2f;

    [InspectorShow("飞行曲线")] public Ease flyEase = Ease.Linear;
}