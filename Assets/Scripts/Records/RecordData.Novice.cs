using System.Collections.Generic;
// using UnityEditor.Searcher;

namespace MobileKit
{
    public partial class RecordData
    {
        // public Dictionary<string, bool> NoviceStates = new Dictionary<string, bool>();
        //
        // public bool IsComplete(string phase)
        // {
        //     // 兼容老的新手引导存档
        //     if (GameSettingsPrefs.GetBool("IsComplete" + phase, false))
        //     {
        //         SetComplete(phase, true);
        //         return true;
        //     }
        //     // 是否跳过新手引导
        //     if (!GameConfig.Instance.EnableNovice)
        //     {
        //         SetComplete(phase, true);
        //         return true;
        //     }
        //
        //     if (!NoviceStates.TryGetValue(phase, out var value))
        //     {
        //         value = NoviceStates[phase] = false;
        //     }
        //     return value;
        // }
        //
        // public void SetComplete(string phase, bool state)
        // {
        //     NoviceStates[phase] = state;
        // }
    }
}