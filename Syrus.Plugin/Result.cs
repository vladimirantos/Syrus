using System;

namespace Syrus.Plugin
{
    public class Result
    {
        public string Text { get; set; }
        public string QuickResult { get; set; }
        public string Icon { get; set; }
        public string Content { get; set; }
        public string Group { get; set; }
        public Action<IAppApi> OnClick { get; set; }
    }
}
