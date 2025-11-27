
using System;
using System.IO;

public class Logger
{
    private readonly string _logFilePath;
    private readonly object _lockObject = new object();

    public Logger(string logFilePath)
    {
        _logFilePath = logFilePath;
    }

    private void WriteLog(string level, string message)
    {
        lock (_lockObject)
        {
            var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }
    }

    public void LogInfo(string message)
    {
        WriteLog("INFO", message);
    }

    public void LogError(string message, Exception ex = null)
    {
        var fullMessage = ex != null ?
            $"{message}: {ex.Message}\n{ex.StackTrace}" : message;
        WriteLog("ERROR", fullMessage);
    }
}
