using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace MobileKit
{
    [CreateAssetMenu(fileName = "DebugConfig", menuName = "MobileKit/Debug Config", order = 1)]
    public class BuildConfig : ScriptableObjectSingleton<BuildConfig>
    {
        public bool Debug = true;

        [LabelText("默认语言")] 
        public DefaultLanguage Language = DefaultLanguage.FollowSystem;

        [LabelText("初始场景名字")]
        public string FirstSceneName = "Game";

        [LabelText("参考分辨率")] 
        public Vector2 ReferenceResolution = new Vector2(720, 1280);
        
        [LabelText("截图模式开关")]
        public bool IsScreenShotMode;
    }
}
