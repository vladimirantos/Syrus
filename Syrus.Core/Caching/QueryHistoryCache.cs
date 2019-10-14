namespace Syrus.Core.Caching
{
    internal class QueryHistoryCache : CacheFacade<string>
    {
        public QueryHistoryCache(string location) : base(location)
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
    }
}
