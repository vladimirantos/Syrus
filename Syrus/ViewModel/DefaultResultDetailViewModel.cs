using Syrus.Plugin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syrus.ViewModel
{
    internal class DefaultResultDetailViewModel : BaseViewModel
    {
        public Result Result { get; private set; }
        public DefaultResultDetailViewModel(Result result) => Result = result;
    }
}
