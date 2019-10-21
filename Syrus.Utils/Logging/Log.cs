using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Syrus.Utils.Logging
{
    public static class Log
    {
        private static readonly string DirectoryPath = Path.Combine(Constants.LogDirectory, Constants.AppVersion);
        internal enum Levels { Info, Debug, Error, Exception };

        static Log()
        {
            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);
        }

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
            
            Write($"{message}{Environment.NewLine}{msg}", Levels.Exception, method, lineNumber, filePath);
        }

        public static void Exception(Exception e, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null) 
            => Exception(string.Empty, e, method, lineNumber, filePath);

        public static void Error(string message, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null) 
            => Write(message, Levels.Error, method, lineNumber, filePath);

        public static void Debug(string message, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null) 
            => Write(message, Levels.Debug, method, lineNumber, filePath);

        public static void Info(string message, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null) 
            => Write(message, Levels.Info, method, lineNumber, filePath);

        private static void Write(string message, Levels level, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            string text = $"[{level.ToString().ToUpper()}]: {method} in file {filePath} (at line {lineNumber}){Environment.NewLine}{message}{Environment.NewLine}";
            string path = Path.Combine(DirectoryPath, DateTime.Now.ToString("yyyyMMdd") + ".txt");
            if (!File.Exists(path))
                File.Create(path);
            using (FileStream fsAppend = new FileStream(path, FileMode.Append))
            using(StreamWriter swAppend = new StreamWriter(fsAppend))
            {
                swAppend.WriteLine(text);
            }
        }
    }
}
