namespace Syrus.Core.Caching
{
    internal class QueryHistoryCache : Cache<string>
    {
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
