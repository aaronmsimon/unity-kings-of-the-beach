using System.IO;
using System;
using UnityEngine;

namespace Cackenballz.Helpers
{
    public static class DebugLogger
    {
        private static string _path;

        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            _path = Path.Combine(Application.persistentDataPath, $"log_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            File.WriteAllText(_path, "Time,Tag,Value\n"); // header
            Debug.Log($"CSV logging to: {_path}");
        }

        public static void Log(string tag, object value)
        {
            var line = $"{Time.time},{tag},{value}\n";
            File.AppendAllText(_path, line);
        }
    }
}
