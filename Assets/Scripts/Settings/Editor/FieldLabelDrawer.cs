using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


//将属性名显示成中文
[CustomPropertyDrawer(typeof(InspectorShow))]
public class FieldLabelDrawer : PropertyDrawer
{
    private InspectorShow FLAttribute => (InspectorShow) attribute;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //在这里重新绘制
        EditorGUI.PropertyField(position, property, new GUIContent(FLAttribute.label), true);
    }
}


//显示颜色
[CustomPropertyDrawer(typeof(TitleAttribute))]
public class TitleAttributeDrawer : DecoratorDrawer
{
    // 文本样式
    private readonly GUIStyle style = new();

    public override void OnGUI(Rect position)
    {
        // 获取Attribute
        var attr = (TitleAttribute) attribute;

        // 转换颜色
        var color = htmlToColor(attr.htmlColor);

        // 重绘GUI
        position = EditorGUI.IndentedRect(position);
        style.normal.textColor = color;
        GUI.Label(position, attr.title, style);
    }

    public override float GetHeight()
    {
        return base.GetHeight() - 4;
    }

    /// <summary> Html颜色转换为Color </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    private Color htmlToColor(string hex)
    {
        // 默认黑色
        if (string.IsNullOrEmpty(hex)) return Color.black;

        // 转换颜色
        hex = hex.ToLower();
        if (hex.IndexOf("#") == 0 && hex.Length == 7)
        {
            var r = Convert.ToInt32(hex.Substring(1, 2), 16);
            var g = Convert.ToInt32(hex.Substring(3, 2), 16);
            var b = Convert.ToInt32(hex.Substring(5, 2), 16);
            return new Color(r / 255f, g / 255f, b / 255f);
        }

        if (hex == "red") return Color.red;
        if (hex == "green") return Color.green;
        if (hex == "blue") return Color.blue;
        if (hex == "yellow") return Color.yellow;
        if (hex == "black") return Color.black;
        if (hex == "white") return Color.white;
        if (hex == "cyan") return Color.cyan;
        if (hex == "gray") return Color.gray;
        if (hex == "grey") return Color.grey;
        if (hex == "magenta") return Color.magenta;
        return Color.black;
    }
}

//显示中文枚举
[CustomPropertyDrawer(typeof(EnumNameAttribute))]
public class EnumNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 替换属性名称
        var rename = (EnumNameAttribute) attribute;
        label.text = rename.name;

        // 重绘GUI
        var isElement = Regex.IsMatch(property.displayName, "Element \\d+");
        if (isElement) label.text = property.displayName;
        if (property.propertyType == SerializedPropertyType.Enum)
            drawEnum(position, property, label);
        else
            EditorGUI.PropertyField(position, property, label, true);
    }

    // 绘制枚举类型
    private void drawEnum(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();

        // 获取枚举相关属性
        var type = fieldInfo.FieldType;
        var names = property.enumNames;
        var values = new string[names.Length];
        while (type.IsArray) type = type.GetElementType();

        // 获取枚举所对应的名称
        for (var i = 0; i < names.Length; i++)
        {
            var info = type.GetField(names[i]);
            var atts = (EnumNameAttribute[]) info.GetCustomAttributes(typeof(EnumNameAttribute), false);
            values[i] = atts.Length == 0 ? names[i] : atts[0].name;
        }

        // 重绘GUI
        var index = EditorGUI.Popup(position, label.text, property.enumValueIndex, values);
        if (EditorGUI.EndChangeCheck() && index != -1) property.enumValueIndex = index;
    }
}