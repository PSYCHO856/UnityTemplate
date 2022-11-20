using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    public abstract class HelpBoxDrawer : DecoratorDrawer
    {
        private HelpBoxAttribute warning => (HelpBoxAttribute) attribute;

        private float BoxHeight(string content, float width)
        {
            return EditorStyles.helpBox.CalcHeight(new GUIContent(content), width) + 14;
        }

        public override void OnGUI(Rect position)
        {
            var warning = this.warning;

            var oldGuiColor = GUI.color;

            var height = BoxHeight(warning.message, position.width);
            EditorGUI.HelpBox(new Rect(position.x, position.y, position.width, height), warning.message,
                GetMessageType());
            GUI.color = oldGuiColor;
        }

        public override float GetHeight()
        {
            return BoxHeight(warning.message, EditorGUIUtility.currentViewWidth) +
                   EditorGUIUtility.standardVerticalSpacing;
        }

        protected virtual MessageType GetMessageType()
        {
            return MessageType.Info;
        }
    }
}