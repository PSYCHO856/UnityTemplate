using UnityEngine;

namespace Watermelon
{
    [SetupTab("IAP", texture = "icon_iap")]
    [CreateAssetMenu(fileName = "IAP Settings", menuName = "Settings/IAP Settings")]
    public class IAPSettings : ScriptableObject
    {
        public IAPItem[] storeItems;
    }
}

// -----------------
// IAP Manager v 0.4
// -----------------