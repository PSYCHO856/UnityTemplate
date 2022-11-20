using UnityEngine;

[CreateAssetMenu(fileName = "LotteryDefine", menuName = "Settings/LotteryDefine")]
public class LotteryDefine : ScriptableObject
{
    [Range(1, 999)] public int[] CoinCount;
    // public LotteryItem[] LotteryItems;

    // [System.Serializable]
    // public struct LotteryItem
    // {
    // public int count;
    // public int weight;
    // }
}