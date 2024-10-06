using UnityEngine;
using System.IO;

public class LogToFile : MonoBehaviour
{
    private string logFilePath;

    void OnEnable()
    {
        // Define the path for the log file (in the persistentDataPath folder)
        logFilePath = Path.Combine(Application.persistentDataPath, "GameLog.txt");

        // Subscribe to the logMessageReceived event to capture all log messages
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        // Unsubscribe from the event when the object is disabled
        Application.logMessageReceived -= HandleLog;
    }

    // This method is called whenever Debug.Log, Debug.LogWarning, or Debug.LogError is called
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Combine the log message and stack trace into a single string
        // string logMessage = $"[{type}] {logString}\n{stackTrace}";
        string logMessage = $"{logString}";

        // Append the log message to the log file
        File.AppendAllText(logFilePath, logMessage + "\n");
    }

    // Optionally, you can call this function to clear the log file (e.g., on game start)
    public void ClearLogFile()
    {
        if (File.Exists(logFilePath))
        {
            File.Delete(logFilePath);
        }
    }
}
