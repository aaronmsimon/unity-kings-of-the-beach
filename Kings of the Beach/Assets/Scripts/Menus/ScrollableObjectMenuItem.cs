using UnityEngine;

namespace KotB.Menus
{
    public class ScriptableObjectMenuItem : ScrollableMenuItem
    {
        [Header("ScriptableObject Settings")]
        [SerializeField] private ScriptableObject targetObject;
        [SerializeField] private string fieldName;

        protected override void OnOptionChanged()
        {
            base.OnOptionChanged();
            WriteToScriptableObject();
        }

        private void WriteToScriptableObject()
        {
            if (targetObject == null || string.IsNullOrEmpty(fieldName))
                return;

            var field = targetObject.GetType().GetField(fieldName);
            if (field != null)
            {
                // Handle different field types
                if (field.FieldType == typeof(string))
                {
                    field.SetValue(targetObject, CurrentOption);
                }
                else if (field.FieldType == typeof(int))
                {
                    if (int.TryParse(CurrentOption, out int intValue))
                    {
                        field.SetValue(targetObject, intValue);
                    }
                    else
                    {
                        field.SetValue(targetObject, CurrentOptionIndex);
                    }
                }
                else if (field.FieldType == typeof(float))
                {
                    if (float.TryParse(CurrentOption, out float floatValue))
                    {
                        field.SetValue(targetObject, floatValue);
                    }
                }
                else if (field.FieldType.IsEnum)
                {
                    if (System.Enum.TryParse(field.FieldType, CurrentOption, out var enumValue))
                    {
                        field.SetValue(targetObject, enumValue);
                    }
                }

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(targetObject);
#endif
            }
        }
    }
}
