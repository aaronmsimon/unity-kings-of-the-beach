// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
//
// Adapted by: Aaron Simon
// Date:       5/7/26
// ----------------------------------------------------------------------------

using UnityEngine;

namespace RoboRyanTron.Unite2017.Variables
{
    [CreateAssetMenu(fileName = "New BooleanVariable", menuName = "Variables/Boolean")]
    public class BooleanVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public bool Value;

        public void SetValue(bool value)
        {
            Value = value;
        }

        public void SetValue(BooleanVariable value)
        {
            Value = value.Value;
        }
    }
}