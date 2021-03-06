﻿using System;
using System.Collections.Generic;
using System.Linq;
using Syrus.Plugin;

namespace Syrus.Core.Metadata
{
    /// <summary>
    /// Kontroluje metadata jestli obsahují povinné hodnoty.
    /// </summary>
    internal class MetadataValidator : IValidator<PluginMetadata>
    {
        public IEnumerable<Predicate<PluginMetadata>> Rules { get; private set; }

        public MetadataValidator()
        {
            Rules = new List<Predicate<PluginMetadata>>()
             {
                 (PluginMetadata m) => m.Author != null,
                 (PluginMetadata m) => m.FullName != null,
                 (PluginMetadata m) => m.Name != null,
             };
        }

        public bool IsValid(PluginMetadata metadata) => Rules.All(rule => rule(metadata));
    }
}
