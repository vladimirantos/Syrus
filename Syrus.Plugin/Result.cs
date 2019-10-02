using System;
using System.Windows;

namespace Syrus.Plugin
{
    public class Result : IResultConfigurable
    {
        private ResultConfiguration _resultConfiguration;
        public string Text { get; set; }
        public string QuickResult { get; set; }
        public string Icon { get; set; }
        public string NightIcon { get; set; }
        public string Group { get; set; }
        public Query FromQuery { get; set; }
        public PluginMetadata FromPlugin { get; set; }
        public View Content { get; set; }
        public Action<IAppApi, Result> OnClick { get; set; }// = (IAppApi api, Result result) => { };
        //public ResultsViewMode? ViewMode { get; set; } = ResultsViewMode.Default;

        /// <summary>
        /// Možnost nastavení výsledku vyhledávání. Vrací první nastavení, které najde v hierarchii.
        /// </summary>
        public virtual ResultConfiguration ResultConfiguration {
            get => CombineConfigurations();
            set => _resultConfiguration = value;
        }

        /// <summary>
        /// Určuje, jestli lze otevřít detail výsledku. Pokud není nastaven OnClick event a zároveň je nastaveno View, může být zobrazeno.
        /// </summary>
        public bool CanOpenDetail => OnClick == null && Content != null;

        /// <summary>
        /// Zkombinuje všechny konfigurace podle priority. Nejvyšší prioritu má konfigurace v třídě Plugin, nejnižší globální konfigurace na pluginu.
        /// </summary>
        /// <returns></returns>
        private ResultConfiguration CombineConfigurations()
        {
            //ResultsViewMode resultsViewMode = _resultConfiguration != null ? _resultConfiguration.ViewMode :
            //    FromPlugin.FromKeyword.ResultConfiguration.ViewMode |
            //    FromPlugin.CurrentSearchingConfiguration.ResultConfiguration.ViewMode |
            //    FromPlugin.ResultConfiguration.ViewMode;
            ResultConfiguration fromPlugin = FromPlugin.ResultConfiguration;
            ResultConfiguration fromCurrentSearchingConfiguration = FromPlugin.CurrentSearchingConfiguration.ResultConfiguration;
            ResultConfiguration fromKeyword = FromPlugin.FromKeyword.ResultConfiguration;

            ResultViewMode viewMode = _resultConfiguration?.ViewMode ??
                fromKeyword?.ViewMode ??
                fromCurrentSearchingConfiguration?.ViewMode ??
                fromPlugin?.ViewMode ??
                ResultViewMode.Classic;

            return new ResultConfiguration()
            {
                ViewMode = viewMode
            };
        }
    }

    public class View
    {
        public BaseViewModel ViewModel { get; set; }
        public ResourceDictionary Template { get; set; }
    }

    /// <summary>
    /// Typy zobrazení výsledků
    /// </summary>
    public enum ResultViewMode
    {
        /// <summary>
        /// Výchozí dvouokení zobrazení. Vlevo seznam výsledků, vpravo detail
        /// </summary>
        Classic,

        /// <summary>
        /// Seznam výsledků přes celou šířku okna, bez detailu.
        /// </summary>
        Fullscreen,

        /// <summary>
        /// Kompletně skryté výsledky. Pro zobrazení výsledků slouží jen QuickResult.
        /// </summary>
        Hidden
    }

    public class BaseViewModel
    {

    }
}
