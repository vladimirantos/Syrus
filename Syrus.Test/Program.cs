using Syrus.Core;
using System;
using System.IO;

namespace Syrus.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string instalationFolder = Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.ApplicationData), "Syrus");
            SyrusFactory factory = new SyrusFactory(instalationFolder);
            factory.Initialize();
        }
    }
}
