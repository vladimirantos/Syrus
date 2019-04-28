using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Syrus.Plugin
{
    public class Cache
    {
        public string Path { get; private set; }

        /// <param name="location">Path to cache directory</param>
        /// <param name="name">File name</param>
        public Cache(string location, string name)
        {
            Path = System.IO.Path.Combine(location, name);
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        public void CreateFolder(string name)
        {
            string path = System.IO.Path.Combine(Path, name);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public void Clear(string folderName)
        {
            string path = System.IO.Path.Combine(Path, folderName);
            if (!Directory.Exists(path))
                return;
            var files = Directory.GetFiles(path);
            foreach (var t in files)
                File.Delete(t);
        }
    }
}
