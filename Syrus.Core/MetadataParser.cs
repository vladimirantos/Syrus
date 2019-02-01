using Newtonsoft.Json;
using Syrus.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Syrus.Core
{
    internal class MetadataParser
    {
        private MetadataValidator _metadataValidator;

        public MetadataParser() => _metadataValidator = new MetadataValidator();

        public PluginMetadata ParseFromFile(string filePath)
        {
            PluginMetadata metadata = JsonConvert.DeserializeObject<PluginMetadata>(File.ReadAllText(filePath));
            if (!_metadataValidator.IsValid(metadata))
                throw new MetadataParserException($"Metadata file {filePath} is not valid.");
            return metadata;
        }
    }

    public class MetadataParserException : SystemException
    {
        public MetadataParserException(string message) : base(message)
        {

        }
    }
}
