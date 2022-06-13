using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomLogger {
  public enum LogLevel {
    DEBUG = 0,
    INFO = 1,
    WARNING = 2,
    ERROR = 3
  }

  public class Logger {
    private static LogLevel LogLevel = LogLevel.DEBUG;

    private string basePath = @"C:\Users\P42674\Documents\HL_Problems\WD";
    private string filePath;

    private string identifier;

    public Logger(string folderName, string fileName, string identifier) {
      if (!Directory.Exists(folderName))
        Directory.CreateDirectory(Path.Join(basePath, folderName));

      filePath = Path.Join(basePath, folderName, fileName);
      var sw = File.AppendText(filePath);
      this.identifier = identifier;
      sw.WriteLine("###################### Next Execution #############################");
      sw.Close();
    }

    public Logger() : this($"log-{DateTime.Now}.txt", "") { }

    public Logger(string fileName, string identifier) {
      this.identifier = identifier;
      StreamWriter sw = File.AppendText(fileName);
      filePath = fileName;
      sw.WriteLine("###################### Next Execution #############################");
      sw.Close();
    }

    public void SetLogLevel(LogLevel level) {
      Logger.LogLevel = level;
    }

    private bool wasNewLine = true;

    public void NewLine() {
      Log("");
    }

    bool isInUse = false;

    private void Log(string text, bool newline = true) {
      while (isInUse)
        Thread.Sleep(10);

      isInUse = true;
      var sw = File.AppendText(filePath);
      if (newline)
        sw.WriteLine(text);
      else
        sw.Write(text);
      sw.Close();
      isInUse = false;

      wasNewLine = newline;
    }

    public void LogDebug(string text, bool newline = true) {
      if (Logger.LogLevel <= LogLevel.DEBUG) {
        if (wasNewLine)
          Log($"DEBUG [{DateTime.Now}] {identifier} - {text}", newline);
        else
          Log($" {text}", newline);
      }
    }

    public void LogInfo(string text, bool newline = true) {
      if (Logger.LogLevel <= LogLevel.INFO) {
        if (wasNewLine)
          Log($"INFO [{DateTime.Now}] {identifier} - {text}", newline);
        else
          Log($" {text}", newline);
      }
    }

    public void LogWarning(string text, bool newline = true) {
      if (Logger.LogLevel <= LogLevel.WARNING) {
        if (wasNewLine)
          Log($"WARNING [{DateTime.Now}] {identifier} - {text}", newline);
        else
          Log($" {text}", newline);
      }
    }

    public void LogError(string text, bool newline = true) {
      if (Logger.LogLevel <= LogLevel.ERROR) {
        if (wasNewLine)
          Log($"ERROR [{DateTime.Now}] {identifier} - {text}", newline);
        else
          Log($" {text}", newline);
      }
    }
  }
}
