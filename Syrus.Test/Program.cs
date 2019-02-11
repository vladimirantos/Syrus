using Syrus.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Syrus.Test
{
    class Program
    {
        static List<KeyValuePair<string[], IEnumerable<string>>> _items = new List<KeyValuePair<string[], IEnumerable<string>>>();
        static Random random = new Random();
        static void Main(string[] args)
        {
            //string instalationFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Syrus");
            //Core.Syrus factory = new Core.Syrus(instalationFolder);
            //factory.LoadPlugins().Initialize();

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

        }
    }
}
