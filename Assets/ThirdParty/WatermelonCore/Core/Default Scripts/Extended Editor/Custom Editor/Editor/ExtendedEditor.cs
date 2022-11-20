using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Watermelon
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true)]
    public class ExtendedEditor : Editor
    {
        private IEnumerable<FieldInfo> fields;
        private HashSet<FieldInfo> groupedFields;
        private Dictionary<string, List<FieldInfo>> groupedFieldsByGroupName;
        private IEnumerable<MethodInfo> methods;
        private IEnumerable<PropertyInfo> nativeProperties;
        private IEnumerable<FieldInfo> nonSerializedFields;
        private SerializedProperty script;

        private Dictionary<string, SerializedProperty> serializedPropertiesByFieldName;

        private bool useDefaultInspector;

        private void OnEnable()
        {
            try
            {
                // Cache serialized fields
                fields = GetFields(f => serializedObject.FindProperty(f.Name) != null);

                // If there are no NaughtyAttributes use default inspector
                if (fields.All(f => f.GetCustomAttributes(typeof(ExtendedEditorAttribute), true).Length == 0))
                {
                    useDefaultInspector = true;
                }
                else
                {
                    useDefaultInspector = false;

                    script = serializedObject.FindProperty("m_Script");

                    // Cache grouped fields
                    groupedFields = new HashSet<FieldInfo>(fields.Where(f =>
                        f.GetCustomAttributes(typeof(GroupAttribute), true).Length > 0));

                    // Cache grouped fields by group name
                    groupedFieldsByGroupName = new Dictionary<string, List<FieldInfo>>();
                    foreach (var groupedField in groupedFields)
                    {
                        var groupName =
                            (groupedField.GetCustomAttributes(typeof(GroupAttribute), true)[0] as GroupAttribute).Name;

                        if (groupedFieldsByGroupName.ContainsKey(groupName))
                            groupedFieldsByGroupName[groupName].Add(groupedField);
                        else
                            groupedFieldsByGroupName[groupName] = new List<FieldInfo>
                            {
                                groupedField
                            };
                    }

                    // Cache serialized properties by field name
                    serializedPropertiesByFieldName = new Dictionary<string, SerializedProperty>();
                    foreach (var field in fields)
                        serializedPropertiesByFieldName[field.Name] = serializedObject.FindProperty(field.Name);
                }

                // Cache non-serialized fields
                nonSerializedFields = GetFields(
                    f => f.GetCustomAttributes(typeof(DrawerAttribute), true).Length > 0 &&
                         serializedObject.FindProperty(f.Name) == null);

                // Cache the native properties
                nativeProperties = GetProperties(
                    p => p.GetCustomAttributes(typeof(DrawerAttribute), true).Length > 0);

                // Cache methods with DrawerAttribute
                methods = GetMethods(m => m.GetCustomAttributes(typeof(DrawerAttribute), true).Length > 0);
            }
            catch
            {
                useDefaultInspector = true;
            }
        }

        private void OnDisable()
        {
            PropertyDrawerDatabase.ClearCache();
        }

        public override void OnInspectorGUI()
        {
            if (useDefaultInspector)
            {
                DrawDefaultInspector();
            }
            else
            {
                serializedObject.Update();

                if (script != null)
                {
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(script);
                    GUI.enabled = true;
                }

                // Draw fields
                var drawnGroups = new HashSet<string>();
                foreach (var field in fields)
                    if (groupedFields.Contains(field))
                    {
                        // Draw grouped fields
                        var groupName = (field.GetCustomAttributes(typeof(GroupAttribute), true)[0] as GroupAttribute)
                            .Name;
                        if (!drawnGroups.Contains(groupName))
                        {
                            drawnGroups.Add(groupName);

                            var grouper = GetPropertyGrouperForField(field);
                            if (grouper != null)
                            {
                                grouper.BeginGroup(groupName);

                                ValidateAndDrawFields(groupedFieldsByGroupName[groupName]);

                                grouper.EndGroup();
                            }
                            else
                            {
                                ValidateAndDrawFields(groupedFieldsByGroupName[groupName]);
                            }
                        }
                    }
                    else
                    {
                        // Draw non-grouped field
                        ValidateAndDrawField(field);
                    }

                serializedObject.ApplyModifiedProperties();
            }

            // Draw non-serialized fields
            foreach (var field in nonSerializedFields)
            {
                var drawerAttribute = (DrawerAttribute) field.GetCustomAttributes(typeof(DrawerAttribute), true)[0];
                var drawer = FieldDrawerDatabase.GetDrawerForAttribute(drawerAttribute.GetType());
                if (drawer != null) drawer.DrawField(target, field);
            }

            // Draw native properties
            foreach (var property in nativeProperties)
            {
                var drawerAttribute = (DrawerAttribute) property.GetCustomAttributes(typeof(DrawerAttribute), true)[0];
                var drawer = NativePropertyDrawerDatabase.GetDrawerForAttribute(drawerAttribute.GetType());
                if (drawer != null) drawer.DrawNativeProperty(target, property);
            }

            // Draw methods
            foreach (var method in methods)
            {
                var drawerAttribute = (DrawerAttribute) method.GetCustomAttributes(typeof(DrawerAttribute), true)[0];
                var methodDrawer = MethodDrawerDatabase.GetDrawerForAttribute(drawerAttribute.GetType());
                if (methodDrawer != null) methodDrawer.DrawMethod(target, method);
            }
        }

        private void ValidateAndDrawFields(IEnumerable<FieldInfo> fields)
        {
            foreach (var field in fields) ValidateAndDrawField(field);
        }

        private void ValidateAndDrawField(FieldInfo field)
        {
            ValidateField(field);
            ApplyFieldMeta(field);
            DrawField(field);
        }

        private void ValidateField(FieldInfo field)
        {
            var validatorAttributes =
                (ValidatorAttribute[]) field.GetCustomAttributes(typeof(ValidatorAttribute), true);

            foreach (var attribute in validatorAttributes)
            {
                var validator = PropertyValidatorDatabase.GetValidatorForAttribute(attribute.GetType());
                if (validator != null) validator.ValidateProperty(serializedPropertiesByFieldName[field.Name]);
            }
        }

        private void DrawField(FieldInfo field)
        {
            // Check if the field has draw conditions
            var drawCondition = GetPropertyDrawConditionForField(field);
            if (drawCondition != null)
            {
                var canDrawProperty = drawCondition.CanDrawProperty(serializedPropertiesByFieldName[field.Name]);
                if (!canDrawProperty) return;
            }

            // Check if the field has HideInInspectorAttribute
            var hideInInspectorAttributes =
                (HideInInspector[]) field.GetCustomAttributes(typeof(HideInInspector), true);
            if (hideInInspectorAttributes.Length > 0) return;

            // Draw the field
            EditorGUI.BeginChangeCheck();
            var drawer = GetPropertyDrawerForField(field);
            if (drawer != null)
                drawer.DrawProperty(serializedPropertiesByFieldName[field.Name]);
            else
                EditorDrawUtility.DrawPropertyField(serializedPropertiesByFieldName[field.Name]);

            if (EditorGUI.EndChangeCheck())
            {
                var onValueChangedAttributes =
                    (OnValueChangedAttribute[]) field.GetCustomAttributes(typeof(OnValueChangedAttribute), true);
                foreach (var onValueChangedAttribute in onValueChangedAttributes)
                {
                    var meta = PropertyMetaDatabase.GetMetaForAttribute(onValueChangedAttribute.GetType());
                    if (meta != null)
                        meta.ApplyPropertyMeta(serializedPropertiesByFieldName[field.Name], onValueChangedAttribute);
                }
            }
        }

        private void ApplyFieldMeta(FieldInfo field)
        {
            // Apply custom meta attributes
            var metaAttributes = field
                .GetCustomAttributes(typeof(MetaAttribute), true)
                .Where(attr => attr.GetType() != typeof(OnValueChangedAttribute))
                .Select(obj => obj as MetaAttribute)
                .ToArray();

            Array.Sort(metaAttributes, (x, y) => { return x.Order - y.Order; });

            foreach (var metaAttribute in metaAttributes)
            {
                var meta = PropertyMetaDatabase.GetMetaForAttribute(metaAttribute.GetType());
                if (meta != null) meta.ApplyPropertyMeta(serializedPropertiesByFieldName[field.Name], metaAttribute);
            }
        }

        private PropertyDrawer GetPropertyDrawerForField(FieldInfo field)
        {
            var drawerAttributes = (DrawerAttribute[]) field.GetCustomAttributes(typeof(DrawerAttribute), true);
            if (drawerAttributes.Length > 0)
            {
                var drawer = PropertyDrawerDatabase.GetDrawerForAttribute(drawerAttributes[0].GetType());
                return drawer;
            }

            return null;
        }

        private PropertyGrouper GetPropertyGrouperForField(FieldInfo field)
        {
            var groupAttributes = (GroupAttribute[]) field.GetCustomAttributes(typeof(GroupAttribute), true);
            if (groupAttributes.Length > 0)
            {
                var grouper = PropertyGrouperDatabase.GetGrouperForAttribute(groupAttributes[0].GetType());
                return grouper;
            }

            return null;
        }

        private PropertyDrawCondition GetPropertyDrawConditionForField(FieldInfo field)
        {
            var drawConditionAttributes =
                (DrawConditionAttribute[]) field.GetCustomAttributes(typeof(DrawConditionAttribute), true);
            if (drawConditionAttributes.Length > 0)
            {
                var drawCondition =
                    PropertyDrawConditionDatabase.GetDrawConditionForAttribute(drawConditionAttributes[0].GetType());
                return drawCondition;
            }

            return null;
        }

        private List<FieldInfo> GetFields(Func<FieldInfo, bool> predicate)
        {
            var types = new List<Type>
            {
                target.GetType()
            };

            while (types.Last().BaseType != null) types.Add(types.Last().BaseType);

            var fields = new List<FieldInfo>();
            for (var i = types.Count - 1; i >= 0; i--)
            {
                var fieldInfos = types[i]
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public |
                               BindingFlags.DeclaredOnly)
                    .Where(predicate);

                fields.AddRange(fieldInfos);
            }

            return fields;
        }

        private List<PropertyInfo> GetProperties(Func<PropertyInfo, bool> predicate)
        {
            var types = new List<Type>
            {
                target.GetType()
            };

            while (types.Last().BaseType != null) types.Add(types.Last().BaseType);

            var properties = new List<PropertyInfo>();
            for (var i = types.Count - 1; i >= 0; i--)
            {
                var propertyInfos = types[i]
                    .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public |
                                   BindingFlags.DeclaredOnly)
                    .Where(predicate);

                properties.AddRange(propertyInfos);
            }

            return properties;
        }

        private List<MethodInfo> GetMethods(Func<MethodInfo, bool> predicate)
        {
            var types = new List<Type>
            {
                target.GetType()
            };

            while (types.Last().BaseType != null) types.Add(types.Last().BaseType);

            var methods = new List<MethodInfo>();
            for (var i = types.Count - 1; i >= 0; i--)
            {
                var methodInfos = types[i]
                    .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public |
                                BindingFlags.DeclaredOnly)
                    .Where(predicate);

                methods.AddRange(methodInfos);
            }

            return methods;
        }
    }
}