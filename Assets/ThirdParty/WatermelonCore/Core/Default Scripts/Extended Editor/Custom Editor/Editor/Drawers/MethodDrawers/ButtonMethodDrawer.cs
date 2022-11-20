using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [MethodDrawer(typeof(ButtonAttribute))]
    public class ButtonMethodDrawer : MethodDrawer
    {
        public override void DrawMethod(Object target, MethodInfo methodInfo)
        {
            if (methodInfo.GetParameters().Length == 0)
            {
                var buttonAttribute =
                    (ButtonAttribute) methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
                var buttonText = string.IsNullOrEmpty(buttonAttribute.Text) ? methodInfo.Name : buttonAttribute.Text;

                if (GUILayout.Button(buttonText)) methodInfo.Invoke(target, null);
            }
            else
            {
                var warning = typeof(ButtonAttribute).Name + " works only on methods with no parameters";
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, target);
            }
        }
    }
}