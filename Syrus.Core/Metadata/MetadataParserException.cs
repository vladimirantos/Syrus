using Syrus.Shared;

namespace Syrus.Core.Metadata
{
    /// <summary>
    /// Vznikne pokud metadata neobsahují povinné hodnoty.
    /// </summary>
    public class MetadataParserException : SyrusException
    {
        public MetadataParserException(string message) : base(message) { }
    }
}
