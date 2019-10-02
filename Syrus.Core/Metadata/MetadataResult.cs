using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syrus.Core.Metadata
{
    /// <summary>
    /// Slouží k odlišení výsledků, kterou jsou vygenerovány z metadat.
    /// </summary>
    public class MetadataResult : Result
    {
        public override ResultConfiguration ResultConfiguration {
            get {
                var config = base.ResultConfiguration;
                config.ViewMode = ResultViewMode.Classic;
                config.OpenDetailMode = OpenDetailMode.OnClick;
                return config;
            }
            set => base.ResultConfiguration = value;
        }
    }
}
