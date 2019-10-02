namespace Syrus.Core
{
    public class Configuration
    {

        /// <summary>
        /// Jazyk vyhledávání. Podle něj se vybírají searchingConfigurations.
        /// </summary>
        public string SearchingLanguages { get; set; }

        /// <summary>
        /// Maximum of searched results.
        /// </summary>
        public int MaxResults { get; set; }
    }
}
