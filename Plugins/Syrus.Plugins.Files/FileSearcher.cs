using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Syrus.Plugins.Files
{
    internal class FileSearcher
    {
        public static List<string> GetFiles(string location, List<string> exclude)
        {
            string[] files = System.IO.Directory.GetFiles(location);
            List<string> result = new List<string>();
            IEnumerable<string> regexExclude = exclude.Where(IsRegex);
            IEnumerable<string> fileExclude = exclude.Where(x => !IsRegex(x));
            foreach(string file in files)
            {
                string fileStr = file.Replace("\\\\", "\\");
                if (fileExclude.Contains(fileStr))
                    continue;
                if (!regexExclude.Any(e => Regex.IsMatch(fileStr, e)))
                    continue;
                result.Add(fileStr);
            }
            return result;
        }

        private static bool IsRegex(string pattern) => pattern.StartsWith("^") && pattern.EndsWith("$");
    }

}
