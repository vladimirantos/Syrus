﻿using Syrus.Plugin;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

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
        private Configuration _configuration;
        private ICollection<KeyValuePair<string, PluginPair>> _keywordPlugins = new List<KeyValuePair<string, PluginPair>>();
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
                foreach (string keyword in pp.Metadata.CurrentSearchingConfiguration.Keywords)
                    _keywordPlugins.Add(new KeyValuePair<string, PluginPair>(keyword, pp));
            _defaultPlugins = Plugins.Where(p => p.Metadata.Default);
        }

        public async Task<IEnumerable<Result>> Search(Query query)
        {
            List<Result> results;
            
            List<PluginPair> plugins = SelectPlugins(query.Command).ToList();
            //plugins.AddRange(_defaultPlugins);

            Task<IEnumerable<Result>>[] tasks = new Task<IEnumerable<Result>>[plugins.Count];
            int i = 0;
            foreach (PluginPair pluginPair in plugins)
            {
                Trace.WriteLine($"{i}: {pluginPair.Metadata.Name}");
                tasks[i] = Task.Factory.StartNew(() => {
                    List<Result> results = pluginPair.Plugin.Search(query).ToList();
                    //foreach (Result result in results) //todo: je nutné? možná se nevyužije
                    //    result.FromPlugin = pluginPair.Metadata;
                    return (IEnumerable<Result>)results;
                });
                i++;
            }
            results = (await Task.WhenAll(tasks)).SelectMany(x => x).ToList();
            if (results.Count == 0)
                results.AddRange(ResultsFromPlugins(plugins));

            foreach(var result in results)
            {
                result.FromQuery = query;
            }
            return results;
        }

        private IEnumerable<PluginPair> SelectPlugins(string match)
            => _keywordPlugins.Where(kv => {
                    bool isKeyword = kv.Key.StartsWith(match, StringComparison.InvariantCultureIgnoreCase);
                    if(isKeyword)
                        kv.Value.Metadata.FromKeyword = kv.Key;
                    return isKeyword;
                })
                .GroupBy(kv => kv.Value.Metadata.Name)
                .Select(kv => kv.First().Value);



        private IEnumerable<Result> ResultsFromPlugins(IEnumerable<PluginPair> plugins)
        {
            List<Result> results = new List<Result>();
            foreach (PluginPair p in plugins)
            {
                results.Add(new Result()
                {
                    Text = p.Metadata.Name,
                    Group = "Možnosti vyhledávání",
                    Icon = p.Metadata.Icon != null ? Path.Combine(p.Metadata.PluginLocation, p.Metadata.Icon) : "",
                    FromPlugin = p.Metadata,
                    OnClick = (IAppApi api, Result currentResult) => api.ChangeQuery(currentResult.FromPlugin.FromKeyword)
                });
            }
            return results;
        }

        private void SelectSearchingConfigurationByLang(string language)
        {
            foreach (PluginPair pluginPair in Plugins)
            {
                pluginPair.Metadata.CurrentSearchingConfiguration
                    = pluginPair.Metadata.SearchingConfigurations.FirstOrDefault(s => s.Language == language);
            }
        }

        public static int DamerauLevenshteinDistance(string source, string target, int threshold)
        {
            void Swap<T>(ref T arg1, ref T arg2)
            {
                T temp = arg1;
                arg1 = arg2;
                arg2 = temp;
            }


            int length1 = source.Length;
            int length2 = target.Length;

            // Return trivial case - difference in string lengths exceeds threshhold
            if (Math.Abs(length1 - length2) > threshold) { return int.MaxValue; }

            // Ensure arrays [i] / length1 use shorter length 
            if (length1 > length2)
            {
                Swap(ref target, ref source);
                Swap(ref length1, ref length2);
            }

            int maxi = length1;
            int maxj = length2;

            int[] dCurrent = new int[maxi + 1];
            int[] dMinus1 = new int[maxi + 1];
            int[] dMinus2 = new int[maxi + 1];
            int[] dSwap;

            for (int i = 0; i <= maxi; i++) { dCurrent[i] = i; }

            int jm1 = 0, im1 = 0, im2 = -1;

            for (int j = 1; j <= maxj; j++)
            {

                // Rotate
                dSwap = dMinus2;
                dMinus2 = dMinus1;
                dMinus1 = dCurrent;
                dCurrent = dSwap;

                // Initialize
                int minDistance = int.MaxValue;
                dCurrent[0] = j;
                im1 = 0;
                im2 = -1;

                for (int i = 1; i <= maxi; i++)
                {

                    int cost = source[im1] == target[jm1] ? 0 : 1;

                    int del = dCurrent[im1] + 1;
                    int ins = dMinus1[i] + 1;
                    int sub = dMinus1[im1] + cost;

                    //Fastest execution for min value of 3 integers
                    int min = (del > ins) ? (ins > sub ? sub : ins) : (del > sub ? sub : del);

                    if (i > 1 && j > 1 && source[im2] == target[jm1] && source[im1] == target[j - 2])
                        min = Math.Min(min, dMinus2[im2] + cost);

                    dCurrent[i] = min;
                    if (min < minDistance) { minDistance = min; }
                    im1++;
                    im2++;
                }
                jm1++;
                if (minDistance > threshold)
                {
                    return int.MaxValue;
                }
            }

            int result = dCurrent[maxi];
            return (result > threshold) ? int.MaxValue : result;
        }
    }
}
