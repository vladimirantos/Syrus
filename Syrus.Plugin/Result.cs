using System;
using System.Windows;

namespace Syrus.Plugin
{
    public class Result
    {
        public string Text { get; set; }
        public string QuickResult { get; set; }
        public string Icon { get; set; }
        public string NightIcon { get; set; }
        public string Group { get; set; }
        public Query FromQuery { get; set; }
        public PluginMetadata FromPlugin { get; set; }
        public View Content { get; set; }
        public Action<IAppApi, Result> OnClick { get; set; }
    }

    public class View
    {
        public BaseViewModel ViewModel { get; set; }
        public ResourceDictionary Template { get; set; }
    }

    public class BaseViewModel
    {

    }
}
