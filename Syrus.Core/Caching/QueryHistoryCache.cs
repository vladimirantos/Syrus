using System.IO;

namespace Syrus.Core.Caching
{
    internal class QueryHistoryCache : CacheFacade<string>
    {
        public QueryHistoryCache(string location) : base(Path.Combine(location, "searching-history.json"))
        {
        }

        /// <summary>
        /// Add when not exist
        /// </summary>
        public override void Add(string value)
        {
            if(!Exists(value.TrimEnd()))
                base.Add(value);
        }

        public override void Save() => base.Save();
    }
}
