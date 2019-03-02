using Newtonsoft.Json;
using Syrus.Plugin;
using System.Collections.Generic;
using System.IO;

namespace Syrus.Core.Metadata
{
    /// <summary>
    /// Převede konfigurační json soubor pluginu na objekt metadata a provádí validaci povinných hodnot.
    /// </summary>
    internal class MetadataParser
    {
        private MetadataValidator _metadataValidator;

        public MetadataParser() => _metadataValidator = new MetadataValidator();
        
        /// <summary>
        /// Načte json soubor a převede ho na PluginMetadata
        /// </summary>
        /// <exception cref="FileNotFoundException">Pokud soubor neexistuje</exception>
        /// <exception cref="MetadataParserException">Pokud soubor není validní (nejsou definovány poviné hodnoty)</exception>
        /// <param name="filePath">Absolutní cesta k json souboru</param>
        public PluginMetadata ParseFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File {filePath} not found.");

            PluginMetadata metadata = JsonConvert.DeserializeObject<PluginMetadata>(File.ReadAllText(filePath));
            if (metadata.Constants == null)
                metadata.Constants = new Dictionary<string, object>();
            if(metadata.ReadonlyConstants == null)
                metadata.ReadonlyConstants = new Dictionary<string, object>();
            if (metadata.SearchingConfigurations == null)
                metadata.SearchingConfigurations = new List<SearchingConfiguration>();
            if (!_metadataValidator.IsValid(metadata))
                throw new MetadataParserException($"Metadata file {filePath} is not valid.");
            return metadata;
        }
    }
}
