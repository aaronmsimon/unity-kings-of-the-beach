// ScriptableObjectMenuOption.cs - Menu option that writes to ScriptableObjects
using UnityEngine;
using System.Collections.Generic;

namespace KotB.Menus
{
    public class ScriptableObjectMenuOption : ScrollableMenuOption
    {
        private ScriptableObject targetObject;
        private string fieldName;

        public ScriptableObjectMenuOption() : base() { }

        public ScriptableObjectMenuOption(string title, List<string> options, ScriptableObject target, string field) 
            : base(title, options)
        {
            targetObject = target;
            fieldName = field;
            OnValueChanged += WriteToScriptableObject;
        }

        public void SetTarget(ScriptableObject target, string field)
        {
            targetObject = target;
            fieldName = field;
        }

        private void WriteToScriptableObject(string value)
        {
            if (targetObject == null || string.IsNullOrEmpty(fieldName))
                return;

            var field = targetObject.GetType().GetField(fieldName);
            if (field != null)
            {
                try
                {
                    if (field.FieldType == typeof(string))
                    {
                        field.SetValue(targetObject, value);
                    }
                    else if (field.FieldType == typeof(int))
                    {
                        if (int.TryParse(value, out int intValue))
                            field.SetValue(targetObject, intValue);
                        else
                            field.SetValue(targetObject, CurrentIndex);
                    }
                    else if (field.FieldType == typeof(float))
                    {
                        if (float.TryParse(value, out float floatValue))
                            field.SetValue(targetObject, floatValue);
                    }
                    else if (field.FieldType.IsEnum)
                    {
                        if (System.Enum.TryParse(field.FieldType, value, out var enumValue))
                            field.SetValue(targetObject, enumValue);
                    }

#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(targetObject);
#endif
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Failed to write to ScriptableObject field {fieldName}: {e.Message}");
                }
            }
        }
    }
}
