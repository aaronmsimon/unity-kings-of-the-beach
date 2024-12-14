using UnityEngine;

namespace KotB.Menus
{
    public class CycleSelections : MonoBehaviour
    {
        [SerializeField] private string folderPath;
        
        private FolderList folderList;

        private void Awake() {
            folderList = GetComponent<FolderList>();
        }
    }
}
