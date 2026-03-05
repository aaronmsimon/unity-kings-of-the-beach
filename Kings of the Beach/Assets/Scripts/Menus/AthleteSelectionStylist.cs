using UnityEngine;
using KotB.Items;

namespace KotB.Menus.Alt
{
    public class AthleteSelectionStylist : MonoBehaviour
    {
        [SerializeField] private AthleteSelectController controller;
        [SerializeField] private Transform outfitTransform;

        private const int bottomMat = 0;
        private const int topMat = 1;

        private void Awake() {
            controller.OutfitTopChanged += OnTopChanged;
            controller.OutfitBottomChanged += OnBottomChanged;
        }

        private void OnTopChanged(MaterialSO mat) {
            SkinnedMeshRenderer r = outfitTransform.GetComponent<SkinnedMeshRenderer>();
            Material[] mats = r.materials;
            mats[topMat] = mat.Mat;
            r.materials = mats;
        }

        private void OnBottomChanged(MaterialSO mat) {
            SkinnedMeshRenderer r = outfitTransform.GetComponent<SkinnedMeshRenderer>();
            Material[] mats = r.materials;
            mats[bottomMat] = mat.Mat;
            r.materials = mats;
        }
    }
}
