using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syrus.Core
{
    internal class MetadataValidator
    {
        private IEnumerable<Predicate<PluginMetadata>> _validationRules;

        public MetadataValidator()
        {
            _validationRules = new List<Predicate<PluginMetadata>>()
             {
                 (PluginMetadata m) => m.Author != null,
                 (PluginMetadata m) => m.FullName != null,
                 (PluginMetadata m) => m.Name != null,
                 (PluginMetadata m) => m.IconPath != null,
             };
        }

        public bool IsValid(PluginMetadata metadata) => _validationRules.All(rule => rule(metadata));
    }
}
