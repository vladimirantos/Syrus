using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Syrus.Shared.Logging
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

        public static void ExceptionAsync(string message, Exception e, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
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

            WriteAsync($"{message}{Environment.NewLine}{msg}", Levels.Exception, method, lineNumber, filePath).Wait();
        }

        public static void ExceptionAsync(Exception e, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
            => ExceptionAsync(string.Empty, e, method, lineNumber, filePath);

        public static void ErrorAsync(string message, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
           => WriteAsync(message, Levels.Error, method, lineNumber, filePath).Wait();

        public static void DebugAsync(string message, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
            => WriteAsync(message, Levels.Debug, method, lineNumber, filePath).Wait();

        public static void InfoAsync(string message, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
            => WriteAsync(message, Levels.Info, method, lineNumber, filePath).Wait();



        private static void Write(string message, Levels level, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            string text = CreateMessage(message, level, method, lineNumber, filePath);
            string path = CreateFilePath();
            if (!File.Exists(path))
                File.Create(path);
            using (FileStream fsAppend = new FileStream(path, FileMode.Append))
            using(StreamWriter swAppend = new StreamWriter(fsAppend))
            {
                swAppend.WriteLine(text);
            }
        }

        private static async Task WriteAsync(string message, Levels level, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null)
        {
            string text = CreateMessage(message, level, method, lineNumber, filePath);
            string path = CreateFilePath();
            if (!File.Exists(path))
                File.Create(path);

            byte[] encodedText = Encoding.Unicode.GetBytes(text);

            //using (FileStream sourceStream = new FileStream(path,
            //    FileMode.Append, FileAccess.Write, FileShare.None,
            //    bufferSize: 4096, useAsync: true))
            //{
            //    await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            //};

            using(StreamWriter sw = new StreamWriter(path, append:false))
            {
                await sw.WriteAsync(text);
            }
        }

        private static string CreateMessage(string message, Levels level, [CallerMemberName] string method = null, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null) 
            => $"[{level.ToString().ToUpper()}]: {method} in file {filePath} (at line {lineNumber}){Environment.NewLine}{message}{Environment.NewLine}";

        private static string CreateFilePath() => Path.Combine(DirectoryPath, DateTime.Now.ToString("yyyyMMdd") + ".txt");
    }
}
