using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Syrus.Utils
{
    public static class Constants
    {
        public const string Syrus = "Syrus";
        public const string Plugins = "plugins";
        private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
        public static readonly string ProgramDirectory = Directory.GetParent(Assembly.Location).ToString();
        public static readonly string ExecutablePath = Path.Combine(ProgramDirectory, $"{Syrus}.exe");
        public static readonly string DataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Syrus);
        public static readonly string PluginsDirectory = Path.Combine(DataDirectory, Plugins);
        public static readonly string LogDirectory = Path.Combine(DataDirectory, "log");
        public static readonly string CacheDirectory = Path.Combine(DataDirectory, "cache");
        public static readonly string SettingsFile = Path.Combine(DataDirectory, "settings.json");
        public static readonly string AppVersion = FileVersionInfo.GetVersionInfo(Assembly.Location).ProductVersion;
    }
}
