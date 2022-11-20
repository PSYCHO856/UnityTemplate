using UnityEngine;

namespace Watermelon
{
    [CreateAssetMenu(fileName = "Define Settings", menuName = "Settings/Editor/Define Settings")]
    public class DefinesSettings : ScriptableObject
    {
        public static readonly string[] STATIC_DEFINES =
        {
            "UNITY_POST_PROCESSING_STACK_V2",
            "PHOTON_UNITY_NETWORKING",
            "PUN_2_0_OR_NEWER",
            "PUN_2_OR_NEWER"
        };

        public string[] customDefines;
    }
}

// -----------------
// Define Manager v 0.2
// -----------------