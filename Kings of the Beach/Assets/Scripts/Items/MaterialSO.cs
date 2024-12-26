using UnityEngine;

namespace KotB.Items
{
    [CreateAssetMenu(fileName = "MaterialSO", menuName = "Game/Material SO")]
    public class MaterialSO : ScriptableObject
    {
        [SerializeField] private Material mat;

        // --- PROPERTIES ---
        public Material Mat => mat;
    }
}
