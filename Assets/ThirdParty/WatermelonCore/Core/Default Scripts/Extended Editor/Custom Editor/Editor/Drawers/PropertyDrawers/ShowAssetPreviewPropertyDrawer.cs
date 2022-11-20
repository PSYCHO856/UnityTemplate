using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [PropertyDrawer(typeof(ShowAssetPreviewAttribute))]
    public class ShowAssetPreviewPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawPropertyField(property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue != null)
                {
                    var previewTexture = AssetPreview.GetAssetPreview(property.objectReferenceValue);
                    if (previewTexture != null)
                    {
                        var showAssetPreviewAttribute =
                            PropertyUtility.GetAttribute<ShowAssetPreviewAttribute>(property);
                        var width = Mathf.Clamp(showAssetPreviewAttribute.Width, 0, previewTexture.width);
                        var height = Mathf.Clamp(showAssetPreviewAttribute.Height, 0, previewTexture.height);

                        GUILayout.Label(previewTexture, GUILayout.MaxWidth(width), GUILayout.MaxHeight(height));
                    }
                    else
                    {
                        DrawWarningBox(property.name + " doesn't have an asset preview", property);
                    }
                }
            }
            else
            {
                DrawWarningBox(property.name + " doesn't have an asset preview", property);
            }
        }

        private void DrawWarningBox(string warningText, SerializedProperty property)
        {
            EditorGUILayout.HelpBox(warningText, MessageType.Warning);
            Debug.LogWarning(warningText, PropertyUtility.GetTargetObject(property));
        }
    }
}