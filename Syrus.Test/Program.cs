using Syrus.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Syrus.Test
{
    class Program
    {
        static List<KeyValuePair<string[], IEnumerable<string>>> _items = new List<KeyValuePair<string[], IEnumerable<string>>>();
        static Random random = new Random();
        static void Main(string[] args)
        {
            string instalationFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Syrus");
            Core.Syrus factory = new Core.Syrus(instalationFolder);
            factory.LoadPlugins().Initialize();


            string term = "Translate hello to czech";
            string tmp = "";
            for(int i = 0; i < term.Length; i++)
            {
                tmp += term[i];
                factory.Search(tmp);
                Console.WriteLine(tmp + "> " + Compute(tmp, "translate x to y"));
                //Thread.Sleep(1000);
            }

            //_items.Add(new KeyValuePair<string[], IEnumerable<string>>(new[] { "K1", "K2", "K3" }, new List<string>() { "Plugin1" }));
            //_items.Add(new KeyValuePair<string[], IEnumerable<string>>(new[] { "K4", "K1", "K5" }, new List<string>() { "Plugin2" }));
            //_items.Add(new KeyValuePair<string[], IEnumerable<string>>(new[] { "K6", "K7", "K4" }, new List<string>() { "Plugin3" }));

            /**
             * Výsledek
             * K2, K3 -> Plugin1
             * K1 -> Plugin1, Plugin2
             * K5 -> Plugin2
             * K4 -> Plugin2, Plugin3
             * K6, K7 -> Plugin 3
             */


            string input = "translate banik pico to english";

            // B
            // The regular expression we use to match
            Regex r1 = new Regex(@"translate ([A-Za-z\s]+) to ([A-Za-z\s]+)");

            // C
            // Match the input and write results
            Match match = r1.Match(input);
            if (match.Success)
            {
                string v = match.Groups[1].Value;
                Console.WriteLine("Between One and Three: {0}", v);
            }
        }


        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }
    }
}
