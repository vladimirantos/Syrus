using Syrus.Plugin;
using System.Collections.Generic;

namespace Syrus.Core
{
    public interface ILoader
    {
        IEnumerable<IPlugin> Load(string pluginsLocation);
    }
}
