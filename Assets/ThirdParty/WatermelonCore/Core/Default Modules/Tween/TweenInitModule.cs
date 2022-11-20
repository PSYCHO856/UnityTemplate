#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    public class TweenInitModule : InitModule
    {
        [SerializeField] private int tweensUpdateCount = 300;
        [SerializeField] private int tweensFixedUpdateCount = 30;
        [SerializeField] private int tweensLateUpdateCount;

        [Space] [SerializeField] private bool enableSystemLogs;

        public TweenInitModule()
        {
            moduleName = "Tween";
        }

        public override void CreateComponent(Initialiser Initialiser)
        {
            var tween = Initialiser.gameObject.AddComponent<Tween>();
            tween.Init(tweensUpdateCount, tweensFixedUpdateCount, tweensLateUpdateCount, enableSystemLogs);
        }
    }
}