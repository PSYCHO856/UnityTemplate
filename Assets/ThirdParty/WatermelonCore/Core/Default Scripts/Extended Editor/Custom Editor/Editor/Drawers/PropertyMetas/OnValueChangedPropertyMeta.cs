using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [PropertyMeta(typeof(OnValueChangedAttribute))]
    public class OnValueChangedPropertyMeta : PropertyMeta
    {
        public override void ApplyPropertyMeta(SerializedProperty property, MetaAttribute metaAttribute)
        {
            var onValueChangedAttribute = (OnValueChangedAttribute) metaAttribute;
            var target = PropertyUtility.GetTargetObject(property);

            var callbackMethod = target.GetType().GetMethod(onValueChangedAttribute.CallbackName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (callbackMethod != null &&
                callbackMethod.ReturnType == typeof(void) &&
                callbackMethod.GetParameters().Length == 0)
            {
                property.serializedObject
                    .ApplyModifiedProperties(); // We must apply modifications so that the callback can be invoked with up-to-date data

                callbackMethod.Invoke(target, null);
            }
            else
            {
                var warning = onValueChangedAttribute.GetType().Name +
                              " can invoke only action methods - with void return type and no parameters";
                Debug.LogWarning(warning, target);
            }
        }
    }
}