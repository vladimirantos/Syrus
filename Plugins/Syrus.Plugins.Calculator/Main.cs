using System.Collections.Generic;
using System.Threading.Tasks;
using NCalc;
using Syrus.Plugin;

namespace Syrus.Plugins.Calculator
{
    public class Main : IPlugin
    {
        public void OnInitialize(PluginContext context)
        {
        }

        public Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            //if (query.Original.Length < 3)
            //    return Task.FromResult<IEnumerable<Result>>(new List<Result>());
            //Expression e = new Expression(query.Original);
            //var x = e.Evaluate();
                return Task.FromResult<IEnumerable<Result>>(new List<Result>());

        }
    }
}
