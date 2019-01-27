using Syrus.Plugin;
using System.Collections.Generic;

namespace Syrus.Core
{
    class SyrusFactory
    {
        public void Initialize()
        {

        }

        public IEnumerable<Result> Search(string term) => new List<Result>();

        private IEnumerable<IPlugin> LoadPlugins() => new List<IPlugin>();
    }
}
