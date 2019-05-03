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
        //public List<AppInfo> AppInfos { get; private set; } = new List<AppInfo>();

        public Trie<AppInfo> AppInfos { get; private set; } = new Trie<AppInfo>(new List<AppInfo>(), KeySelector, new AppInfoComparer());

        private static string KeySelector(AppInfo arg) => arg.Name.ToLower();

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
            //AppInfos.Sort((x, y));
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

    public class Trie<TItem>
    {
        #region Constructors

        public Trie(
            IEnumerable<TItem> items,
            Func<TItem, string> keySelector,
            IComparer<TItem> comparer)
        {
            this.KeySelector = keySelector;
            this.Comparer = comparer;
            this.Items = (from item in items
                          from i in Enumerable.Range(1, this.KeySelector(item).Length)
                          let key = this.KeySelector(item).Substring(0, i)
                          group item by key)
                         .ToDictionary(group => group.Key, group => group.ToList());
        }

        #endregion

        #region Properties

        protected Dictionary<string, List<TItem>> Items { get; set; }

        protected Func<TItem, string> KeySelector { get; set; }

        protected IComparer<TItem> Comparer { get; set; }

        #endregion

        #region Methods

        public List<TItem> Retrieve(string prefix)
        {
            return this.Items.ContainsKey(prefix)
                ? this.Items[prefix]
                : new List<TItem>();
        }

        public void Add(TItem item)
        {
            var keys = (from i in Enumerable.Range(1, this.KeySelector(item).Length)
                        let key = this.KeySelector(item).Substring(0, i)
                        select key).ToList();
            keys.ForEach(key =>
            {
                if (!this.Items.ContainsKey(key))
                {
                    this.Items.Add(key, new List<TItem> { item });
                }
                else if (this.Items[key].All(x => this.Comparer.Compare(x, item) != 0))
                {
                    this.Items[key].Add(item);
                }
            });
        }

        public void Remove(TItem item)
        {
            this.Items.Keys.ToList().ForEach(key =>
            {
                if (this.Items[key].Any(x => this.Comparer.Compare(x, item) == 0))
                {
                    this.Items[key].RemoveAll(x => this.Comparer.Compare(x, item) == 0);
                    if (this.Items[key].Count == 0)
                    {
                        this.Items.Remove(key);
                    }
                }
            });
        }

        #endregion
    }
}
