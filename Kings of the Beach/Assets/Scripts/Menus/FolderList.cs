using System.IO;
using System.Linq;
using UnityEngine;

namespace KotB.Menus
{
    public class FolderList
    {
        public string[] GetFolderArray(string folderPath) {
            string resourcesPath = Path.Combine(Application.dataPath, "Resources", folderPath);

            if (!Directory.Exists(resourcesPath)) {
                Debug.LogWarning($"Folder path does not exist: {resourcesPath}");
                return new string[0];
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(resourcesPath);
            DirectoryInfo[] subDirectories = directoryInfo.GetDirectories();
            return subDirectories.Select(d => d.Name).ToArray();
        }
    }
}
