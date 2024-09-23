// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
//
// Adapted by: Aaron Simon
// Date:       9/23/24
// ----------------------------------------------------------------------------

using UnityEngine;

namespace RoboRyanTron.Unite2017.Variables
{
    [CreateAssetMenu(fileName = "New Vector3Variable", menuName = "Variables/Vector3")]
    public class Vector3Variable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public Vector3 Value;

        public void SetValue(Vector3 value)
        {
            Value = value;
        }

        public void SetValue(Vector3Variable value)
        {
            Value = value.Value;
        }
    }
}