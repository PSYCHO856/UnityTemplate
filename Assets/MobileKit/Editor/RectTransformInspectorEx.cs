using UnityEditor;
using UnityEngine;

namespace MobileKit.Editor
{
    public static class RectTransformInspectorEx
    {
        [MenuItem("CONTEXT/RectTransform/Reset Scale")]
        private static void Operate(MenuCommand cmd) 
        {
            if (cmd.context is RectTransform { } trans)
            {
                trans.sizeDelta = new Vector2(trans.sizeDelta.x * trans.localScale.x,
                                              trans.sizeDelta.y * trans.localScale.y);
                trans.localScale = Vector3.one;
            }
        }
    }
}