// ScriptableObjectOptionHandler.cs - Handler for ScriptableObject options
using UnityEngine;
using System.Reflection;

namespace KotB.Menus
{
    public class ScriptableObjectOptionHandler : ScrollableOptionHandler
    {
        private ScriptableObject targetObject;
        private string fieldName;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Get configuration from MenuItemComponent
            var menuItemComponent = FindMenuItemComponent();
            if (menuItemComponent != null)
            {
                var config = menuItemComponent.GetScriptableObjectConfig();
                targetObject = config.targetObject;
                fieldName = config.fieldName;
            }
        }

        private MenuItemComponent FindMenuItemComponent()
        {
            // Try to find MenuItemComponent in the scene that references this element
            var menuItemComponents = Object.FindObjectsOfType<MenuItemComponent>();
            foreach (var component in menuItemComponents)
            {
                if (component.GetTargetElement() == element)
                {
                    return component;
                }
            }
            return null;
        }

        protected override void OnValueChanged()
        {
            base.OnValueChanged();
            WriteToScriptableObject();
        }

        private void WriteToScriptableObject()
        {
            if (targetObject == null || string.IsNullOrEmpty(fieldName))
                return;

            var field = targetObject.GetType().GetField(fieldName);
            if (field != null)
            {
                try
                {
                    var value = CurrentValue;
                    
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

        public void SetTarget(ScriptableObject target, string field)
        {
            targetObject = target;
            fieldName = field;
        }
    }
}
