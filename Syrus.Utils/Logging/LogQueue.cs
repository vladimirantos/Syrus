using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Syrus.Shared.Logging.Log;

namespace Syrus.Shared.Logging
{
    public class LogQueue
    {
        private static readonly string DirectoryPath = Path.Combine(Constants.LogDirectory, Constants.AppVersion);
        private static readonly Queue<LogItem> messages = new Queue<LogItem>();

        public static bool IsEmpty => messages.Any();

        public static void Exception(string message, Exception e, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            string msg = string.Empty;
            do
            {
                message += $"{e.GetType().FullName}: {e.Message}{Environment.NewLine}{e.StackTrace}{Environment.NewLine}" +
                    $"Source: {e.Source}{Environment.NewLine}Target site:{e.TargetSite}{Environment.NewLine}HResult{e.HResult}";
                if (e.InnerException != null)
                    message += $"{Environment.NewLine}------------INNER EXCEPTION------------{Environment.NewLine}";
                e = e.InnerException;
            } while (e != null);

            messages.Enqueue(new LogItem($"{message}{Environment.NewLine}{msg}", Levels.Exception, method, lineNumber, filePath));
        }

        public static void Exception(Exception e, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
            => Exception(string.Empty, e, method, lineNumber, filePath);

        public static void Error(string message, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
            => messages.Enqueue(new LogItem(message, Levels.Error, method, lineNumber, filePath));

        public static void Debug(string message, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
            => messages.Enqueue(new LogItem(message, Levels.Debug, method, lineNumber, filePath));

        public static void Info(string message, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
            => messages.Enqueue(new LogItem(message, Levels.Info, method, lineNumber, filePath));

        public static async Task WriteAsync()
        {
            LogItem item = messages.Dequeue();
            string text = item.ToString();
            string path = CreateFilePath();
            if (!File.Exists(path))
                File.Create(path);

            byte[] encodedText = Encoding.Unicode.GetBytes(text);

            using (FileStream sourceStream = new FileStream(path,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }

        private static string CreateFilePath() => Path.Combine(DirectoryPath, DateTime.Now.ToString("yyyyMMdd") + ".txt");
    }

    internal class LogItem
    {
        public string Text { get; private set; }
        public Levels Level { get; private set; }
        public string Method { get; private set; }
        public int LineNumber { get; private set; }
        public string FilePath { get; private set; }
        public DateTime CreationDate { get; private set; }

        public LogItem(string text, Levels level, string method, int lineNumber, string filePath)
        {
            Text = text;
            Level = level;
            Method = method;
            LineNumber = lineNumber;
            FilePath = filePath;
            CreationDate = DateTime.Now;
        }

        public override string ToString() 
            => $"[{Level.ToString().ToUpper()} {CreationDate}]: {Method} in file {FilePath} (at line {LineNumber}){Environment.NewLine}\t{Text}{Environment.NewLine}";
    }
}
