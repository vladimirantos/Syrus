using Syrus.Plugin;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Syrus.Core
{
    public interface ISearch
    {
        IEnumerable<PluginPair> Plugins { get; set; }
        void Initialize();
        Task<IEnumerable<Result>> Search(Query query);
    }

    public class SearchEngine : ISearch
    {
        private Regex _regex;
        private Configuration _configuration;
        private ICollection<KeyValuePair<string, PluginPair>> _keywordPlugins = new List<KeyValuePair<string, PluginPair>>();
        private ICollection<KeyValuePair<string, PluginPair>> _regexPlugins = new List<KeyValuePair<string, PluginPair>>();
        private IEnumerable<PluginPair> _defaultPlugins;
        public IEnumerable<PluginPair> Plugins { get; set; }

        public SearchEngine(Configuration configuration) => _configuration = configuration;

        /// <summary>
        /// Select searching configuration by language and prepare keywords.
        /// </summary>
        public void Initialize()
        {
            SelectSearchingConfigurationByLang(_configuration.Language);
            foreach (PluginPair pp in Plugins.Where(p => !p.Metadata.Default))
            {
                if (pp.Metadata.CurrentSearchingConfiguration.Keywords != null)
                    ToKeyValues(ref _keywordPlugins, pp, pp.Metadata.CurrentSearchingConfiguration.Keywords);

                if (pp.Metadata.CurrentSearchingConfiguration.RegularExpressions != null)
                    ToKeyValues(ref _regexPlugins, pp, pp.Metadata.CurrentSearchingConfiguration.RegularExpressions);
            }
             
            _defaultPlugins = Plugins.Where(p => p.Metadata.Default);

            void ToKeyValues(ref ICollection<KeyValuePair<string, PluginPair>> keyValuePairs, PluginPair plugin, IEnumerable<string> col)
            {
                foreach (string item in col)
                    keyValuePairs.Add(new KeyValuePair<string, PluginPair>(item, plugin));
            }
        }

        public async Task<IEnumerable<Result>> Search(Query query)
        {
            List<Result> results;
            
            List<PluginPair> plugins = SelectPluginsByKeyword(query.Command).ToList();
            plugins.AddRange(SelectPluginsByRegex(query.Command));

            Task<IEnumerable<Result>>[] tasks = new Task<IEnumerable<Result>>[plugins.Count];
            int i = 0;
            foreach (PluginPair pluginPair in plugins)
            {
                Trace.WriteLine($"{i}: {pluginPair.Metadata.Name}");
                tasks[i] = Task.Factory.StartNew(() => {
                    List<Result> results = pluginPair.Plugin.Search(query).ToList();
                    foreach (Result result in results) 
                        result.FromPlugin = pluginPair.Metadata;
                    return (IEnumerable<Result>)results;
                });
                i++;
            }
            results = (await Task.WhenAll(tasks)).SelectMany(x => x).ToList();
            if (results.Count == 0)
            {
                results.AddRange(ResultsFromPlugins(plugins));
                results.AddRange(DefaultResults(query.Original));
            }

            foreach(var result in results)
            {
                result.FromQuery = query;
                if (string.IsNullOrEmpty(result.Icon))
                    result.Icon = result.FromPlugin.Icon;
                if (string.IsNullOrEmpty(result.NightIcon))
                    result.NightIcon = result.FromPlugin.NightIcon;
            }
            return results;
        }

        /// <summary>
        /// Find plugins by specified keyword
        /// </summary>
        private IEnumerable<PluginPair> SelectPluginsByKeyword(string match)
            => _keywordPlugins.Where(kv => {
                    bool isKeyword = kv.Key.StartsWith(match, StringComparison.InvariantCultureIgnoreCase);
                    if(isKeyword)
                        kv.Value.Metadata.FromKeyword = kv.Key;
                    return isKeyword;
                })
                .GroupBy(kv => kv.Value.Metadata.Name)
                .Select(kv => kv.First().Value);

        /// <summary>
        /// Select plugins by regex
        /// </summary>
        private IEnumerable<PluginPair> SelectPluginsByRegex(string match)
        {
            return _regexPlugins.Where(kv =>
            {
                var regexMatch = Regex.Match(match, kv.Key, RegexOptions.IgnoreCase);
                if (regexMatch.Success)
                    kv.Value.Metadata.FromKeyword = kv.Key;
                return regexMatch.Success;
            })
            .GroupBy(kv => kv.Value.Metadata.Name)
            .Select(kv => kv.First().Value);
        }

        /// <summary>
        /// Convert plugin to result
        /// </summary>
        private IEnumerable<Result> ResultsFromPlugins(IEnumerable<PluginPair> plugins) 
            => plugins.Select(plugin => (Result)plugin);

        /// <summary>
        /// Searching by default plugins
        /// </summary>
        private IEnumerable<Result> DefaultResults(string match)
        {
            foreach(PluginPair p in _defaultPlugins)
            {
                yield return new Result()
                {
                    Text = p.Metadata.Name,
                    Group = "Vyhledat online",
                    Icon = p.Metadata.Icon != null ? Path.Combine(p.Metadata.PluginLocation, p.Metadata.Icon) : "",
                    FromPlugin = p.Metadata
                };
            }
        }

        private void SelectSearchingConfigurationByLang(string language)
        {
            foreach (PluginPair pluginPair in Plugins)
            {
                pluginPair.Metadata.CurrentSearchingConfiguration
                    = pluginPair.Metadata.SearchingConfigurations.FirstOrDefault(s => s.Language == language);
            }
        }
    }
}
