using Newtonsoft.Json;
using Syrus.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Syrus
{
    internal enum Themes
    {
        Light, Dark, System
    }

    internal class AppSettings : Configuration
    {
        public Themes Theme { get; set; }
        public string AppLanguage { get; set; }
    }

    internal static class SettingsLoader
    {
        public static AppSettings Load(string path) => JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(path));
    }
}
