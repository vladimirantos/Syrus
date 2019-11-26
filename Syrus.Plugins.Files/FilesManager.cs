using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Syrus.Plugins.Files
{
    internal class FilesManager
    {
        /// <summary>
        /// Vrací obsah dané složky
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<File> GetFiles(string path)
        {
            if (!System.IO.Directory.Exists(path))
                yield break;
            foreach(string f in System.IO.Directory.GetFiles(path))
            {
                yield return File.FromFileInfo(new FileInfo(f));
            }
        }

        public static IEnumerable<Directory> GetDirectories(string path)
        {
            if (!System.IO.Directory.Exists(path))
                yield break;
            foreach(string d in System.IO.Directory.GetDirectories(path))
            {
                yield return Directory.FromDirectoryInfo(new DirectoryInfo(d));
            }
        }
    }
}
