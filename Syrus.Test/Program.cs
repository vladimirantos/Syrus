using Syrus.Core;
using System;
using System.IO;

namespace Syrus.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string instalationFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Syrus");
            Core.Syrus factory = new Core.Syrus(instalationFolder);
            factory.LoadPlugins().Initialize();
        }
    }
}
