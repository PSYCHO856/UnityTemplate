using UnityEngine;

[CreateAssetMenu(fileName = "GameDefine", menuName = "Settings/GameDefine")]
public class GameDefine : ScriptableObject
{
    [Header("Debug")] public bool Debug;

    [InspectorShow("购买背景花费的金币")] public int bgCoinCost = 500;

    [InspectorShow("购买瓶子皮肤花费的金币")] public int bottleCoinCost = 500;

    [InspectorShow("商店页面观看广告获得的金币")] public int watchCoinGainInShop = 500;

    [InspectorShow("正常过关获得的金币")] public int normalCoinGain = 500;

    [InspectorShow("观看广告过关获得的金币")] public int watchCoinGain = 500;

    [InspectorShow("无广告的新手关卡")] public int noRedEnvelopeLevels = 5;

    [InspectorShow("中场跳红包开始关卡下标")] public int insertJumpLevel = 6;

    [InspectorShow("中场跳红包时瓶子完成数")] public int bottleFinishInInsertJump = 3;

    [InspectorShow("中场跳红包瓶子总数")] public int bottleMinNum = 5;

    [InspectorShow("加速时间")] public float baseSpeedUpTime = 10f;

    /*[InspectorShow("回退次数上限")]
    public int undoLimit=5;*/
    [InspectorShow("空瓶上限")] public int emptyBottleLimit = 5;

    [InspectorShow("单行瓶子屏幕位置")] public float bottlePosOneRow = 0.5f;

    [InspectorShow("单行瓶子屏幕位置")] public float bottlePosTwoRow1 = 0.66f;

    [InspectorShow("单行瓶子屏幕位置")] public float bottlePosTowRow2 = 0.38f;
}