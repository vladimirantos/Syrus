using Newtonsoft.Json;
using Syrus.Plugin;
using System.IO;

namespace Syrus.Core.Metadata
{
    internal class MetadataParser
    {
        private MetadataValidator _metadataValidator;

        public MetadataParser() => _metadataValidator = new MetadataValidator();
        
        public PluginMetadata ParseFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File {filePath} not found.");

            PluginMetadata metadata = JsonConvert.DeserializeObject<PluginMetadata>(File.ReadAllText(filePath));

            if (!_metadataValidator.IsValid(metadata))
                throw new MetadataParserException($"Metadata file {filePath} is not valid.");
            return metadata;
        }
    }
}
