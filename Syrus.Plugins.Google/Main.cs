using Syrus.Plugin;
using System.Collections.Generic;

namespace Syrus.Plugins.Google
{
    public class Main : IPlugin
    {
        public void OnInitialize(PluginContext context)
        {
            System.Console.WriteLine("GOOGLE");
            System.Console.WriteLine(context.Metadata.Constants["x"]);
            System.Console.WriteLine(context.Metadata.Constants["y"]);
            System.Console.WriteLine(context.Metadata.Constants["z"]);
            System.Console.WriteLine(context.Deserialize<Dictionary<string, string>>(context.Metadata.Constants["z"].ToString())["a"] + "*");
        }

        public void Search(string searchTerm)
        {
            throw new System.NotImplementedException();
        }
    }
}
