using Microsoft.Win32;
using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Syrus.Plugins.Applications
{
    internal class ApplicationSearcher
    {
        private PluginContext pluginContext;
        public ICollection<AppInfo> AppInfos { get; private set; } = new List<AppInfo>();

        public ApplicationSearcher(PluginContext pluginContext) => this.pluginContext = pluginContext;

        public void Initialize(string registryKey)
        {
            //string registry_key = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        string helpLink = subkey.GetValue("HelpLink")?.ToString();
                        string urlInfoAbout = subkey.GetValue("UrlInfoAbout")?.ToString();
                        string installLocation = GetInstallLocation(subkey);
                        string name = subkey.GetValue("DisplayName")?.ToString();
                        
                        if (string.IsNullOrEmpty(name))
                            continue;
                        string iconLocation = Path.Combine(pluginContext.Cache.Path, 
                            $"{Regex.Replace(name, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.0))}.png");
                        var displayIcon = subkey.GetValue("DisplayIcon")?.ToString();
                        if (!string.IsNullOrEmpty(displayIcon))
                            SaveIcon(iconLocation, displayIcon);
                        AppInfos.Add(new AppInfo()
                        {
                            Name = name,
                            Icon = iconLocation,
                            UninstallPath = subkey.GetValue("UninstallString")?.ToString(),
                            AppVersion = subkey.GetValue("DisplayVersion")?.ToString(),
                            Publisher = subkey.GetValue("Publisher")?.ToString(),
                            Web = helpLink != null ? helpLink : urlInfoAbout,
                            InstallLocation = installLocation
                        });
                    }
                }
            }
        }

        private static void SaveIcon(string iconLocation, string displayIcon)
        {
                var icon = displayIcon.Split(',');
            try
            {
                Icon.ExtractAssociatedIcon(icon[0]).ToBitmap().Save(iconLocation);
            }catch(FileNotFoundException e)
            {

            }
        }

        private string GetInstallLocation(RegistryKey key)
        {
            string installLocation = key.GetValue("InstallLocation")?.ToString();
            if (installLocation != null)
                return installLocation;
            string uninstallString = key.GetValue("UninstallString")?.ToString();
            if (uninstallString != null)
                return System.IO.Path.GetDirectoryName(uninstallString);
            string icon = key.GetValue("DisplayIcon")?.ToString();
            if (icon != null)
                return System.IO.Path.GetDirectoryName(icon);
            return null;
        }
    }

    internal class AppInfo
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string UninstallPath { get; set; }
        public string AppVersion { get; set; }
        public string Web { get; set; }
        public string InstallLocation { get; set; }
        public string Publisher { get; set; }

        public override string ToString() => Name;
    }
}
