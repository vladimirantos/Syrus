using Syrus.Plugin;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Diagnostics;

namespace Syrus.Core
{
    public interface ISearch
    {
        IEnumerable<PluginPair> Plugins { get; set; }
        IEnumerable<Result> Search(string match);
        void Indexing();
    }

    public class SearchEngine : ISearch
    {
        private ICollection<KeyValuePair<string, PluginPair>> _keywordPlugins = new List<KeyValuePair<string, PluginPair>>();
        public IEnumerable<PluginPair> Plugins { get; set; }

        public void Indexing()
        {
            foreach (PluginPair pp in Plugins)
                foreach (string keyword in pp.Metadata.CurrentSearchingConfiguration.Keywords)
                    _keywordPlugins.Add(new KeyValuePair<string, PluginPair>(keyword, pp));
        }

        public IEnumerable<Result> Search(string match)
        {
            match = match.ToLower();
            IEnumerable<PluginPair> plugins = SelectPlugins(match);
            List<Result> results = new List<Result>();
            foreach (var plugin in plugins)
                results.AddRange(plugin.Plugin.Search(match));

            //Task<IEnumerable<Result>>[] tasks = new Task<IEnumerable<Result>>[_searchEngines.Count];
            //for(int i = 0; i < _searchEngines.Count; i++)
            //{
            //    tasks[i] = Task.Factory.StartNew(() => _searchEngines[i].Search(match));
            //}
            ////return (await Task.WhenAll(tasks)).ToList();

           if (results.Count == 0)
                results.AddRange(ResultsFromPlugins(plugins));
            return results;
        }

        private IEnumerable<PluginPair> SelectPlugins(string match)
        {
            List<PluginPair> plugins = new List<PluginPair>();
            //if (_maxCommandLength >= match.Length)
            //    plugins.AddRange(_commandPlugins.Where(kv => kv.Key.StartsWith(match)).Select(kv => kv.Value));
            //var a = _regexPlugins.Where(kv => Regex.IsMatch(match, kv.Key)).ToList(); 
            //plugins.AddRange(a).Select(kv => kv.Value);

            //foreach(var x in _termsPlugins)
            //{
            //    Trace.WriteLine($"{match} x {x.Key} = {DamerauLevenshteinDistance(match, x.Key, match.Length + x.Key.Length)} {x.Value.Metadata.Name}");
            //}

            //plugins.AddRange(_termsPlugins.Where(kv => DamerauLevenshteinDistance(match, kv.Key, match.Length + kv.Key.Length) != int.MaxValue).Select(kv => kv.Value));

            //plugins.AddRange(_termsPlugins.Where(kv => kv.Key.StartsWith(match)).Select(kv => kv.Value));
            return plugins;
        }

        private IEnumerable<Result> ResultsFromPlugins(IEnumerable<PluginPair> plugins)
        {
            List<Result> results = new List<Result>();
            foreach(PluginPair p in plugins)
            {
                results.Add(new Result()
                {
                    Text = p.Metadata.Name,
                    Group = p.Metadata.FullName,
                    Icon = p.Metadata.Icon != null ? Path.Combine(p.Metadata.PluginLocation, p.Metadata.Icon) : ""
                });
            }
            return results;
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
