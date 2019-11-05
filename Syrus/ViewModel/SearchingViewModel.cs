using Microsoft.Win32;
using Syrus.Core;
using Syrus.Core.Metadata;
using Syrus.Plugin;
using Syrus.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Syrus.ViewModel
{
    public delegate void PluginSelected();
    class SearchingViewModel : NotifyPropertyChanges, IAppApi
    {
        private string _query;
        private string _placeholder;

        private string _quickResult;
        private string _currentPluginIcon;
        private AppSettings _appSettings;


        private Core.Syrus _syrus;
        private readonly string _defaultPlaceholder = "Search";

        public event PluginSelected OnSelectPlugin;

        public string SearchingQuery {
            get => _query;
            set => SetProperty(ref _query, value, Search);
        }

        public string Placeholder {
            get => _placeholder;
            set => SetProperty(ref _placeholder, value);
        }

        public string QuickResult {
            get => _quickResult;
            set => SetProperty(ref _quickResult, value);
        }

        public string CurrentPluginIcon {
            get => _currentPluginIcon;
            set => SetProperty(ref _currentPluginIcon, value);
        }

        private ObservableCollection<Result> _results;
        public ObservableCollection<Result> Results {
            get => _results;
            set => SetProperty(ref _results, value);
        }

        private BaseViewModel _resultDetail;
        public BaseViewModel ResultDetail {
            get => _resultDetail;
            set => SetProperty(ref _resultDetail, value);
        }

        private ResultViewMode _viewMode;

        /// <summary>
        /// Mod zobrazení výsledků
        /// </summary>
        public ResultViewMode ResultViewMode {
            get => _viewMode;
            set => SetProperty(ref _viewMode, value);
        }

        /// <summary>
        /// Returns true when windows dark mode is enabled.
        /// </summary>
        //public bool IsEnabledDarkMode => _appSettings.Theme == Themes.Dark ||
        //           (_appSettings.Theme == Themes.System &&
        //           (int)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize\", "AppsUseLightTheme", 0) == 0);

        public bool IsEnabledDarkMode => false;


        public ICommand CompleteTextByTabCommand => new Command((object obj) => 
            {
                var result = Results.First();
                string str = result.FromPlugin.FromKeywordString;
                ChangeQuery( str != null ? $"{str} " : result.Text); 
            },
            _ =>
            Results.Count > 0 && CanDisplayHelp(Results.First()));

        public ICommand SelectResultCommand => new Command((object obj) =>
        {
            var result = (Result)obj;
            if (result.OnClick == null)
                DisplayView(result);
            else
                result.OnClick.Invoke(this, obj as Result);
        });

        public SearchingViewModel()
        {
            _appSettings = SettingsLoader.Load(Constants.SettingsFile);

            _syrus = new Core.Syrus(Constants.PluginsDirectory, Constants.CacheDirectory, _appSettings);
            _syrus.LoadPlugins().Initialize();
            Results = new ObservableCollection<Result>();
            Placeholder = _defaultPlaceholder;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int RegGetValue(int nIndex);

        public async void Search(string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                Results = new ObservableCollection<Result>();
                Placeholder = _defaultPlaceholder;
                QuickResult = string.Empty;
                CurrentPluginIcon = string.Empty;
                ResultDetail = null;
                return;
            }

            IEnumerable<Result> results = await _syrus.SearchAsync(newValue);
            Results = new ObservableCollection<Result>(results);
            var mainResult = Results.First();

            CurrentPluginIcon = ResultsFromSinglePlugin(results) ? SelectIcon(mainResult.FromPlugin) : string.Empty;
            ResultViewMode = mainResult.ResultConfiguration.ViewMode.HasValue ? mainResult.ResultConfiguration.ViewMode.Value : ResultViewMode.Classic;

            if (mainResult.FromPlugin.EnableHelp)
                SetHelpPlaceholder(Results.Count > 0 && CanDisplayHelp(mainResult) ? CreateHelp(newValue, mainResult) : string.Empty);
            else Placeholder = null; //disabled default placeholder when searchbox is not empty
            ChangeQuickResult(results.First().QuickResult);

            TryOpenDetail(mainResult, Results.Count);
            if (mainResult is MetadataResult)
                ResultDetail = null;
        }

        /// <summary>
        /// Kontroluje, jestli je možné zobrazit nápovědu (placeholder) a zároveň doplnit command pomocí TAB.
        /// Řeší případy command, command_, command_argument
        /// </summary>
        private bool CanDisplayHelp(Result result) => result.FromQuery.HasCommand
            && !result.FromQuery.HasArguments
            && !result.FromQuery.Original.EndsWith(' ')
            && result.FromPlugin.FromKeyword != null; //pro defaultní pluginy

        /// <summary>
        /// Ze zadaného výsledku hledání vybere zbývající část textu a zobrazí ji v Placeholderu.
        /// </summary>
        /// <param name="text">Text napsaný do vyhledávacího pole</param>
        /// <param name="result">Aktuálně nalezený výsledek hledání</param>
        private string CreateHelp(string text, Result result)
            => Regex.Replace(result.FromPlugin.FromKeywordString, text, string.Empty, RegexOptions.IgnoreCase).ToLower();

        public void ChangeQuery(string query, bool append = false)
        {
            SearchingQuery = append ? SearchingQuery + query : query;
            Placeholder = null;
            OnSelectPlugin.Invoke();
        }

        public void ChangeQuickResult(string text) => QuickResult = text;

        public void SetHelpPlaceholder(string text) => Placeholder = text;

        internal void OnCloseHandler() => _syrus.SaveCache();

        private bool ResultsFromSinglePlugin(IEnumerable<Result> results)
        {
            List<Result> r = results.ToList();
            return r.Count == 1 || r.Select(result => result.FromPlugin.FullName).Distinct().Count() < 2;
        }

        /// <summary>
        /// Vybere ikonu na základě použitého tématu
        /// </summary>
        private string SelectIcon(PluginMetadata plugin)
            => !IsEnabledDarkMode || plugin.NightIcon == null ? plugin.Icon : plugin.NightIcon;

        /// <summary>
        /// Přidá view do Resources a zobrazí jej (pomocí property ResultDetail).
        /// Pokud plugin nemá nastavené žádné view, bude použito výchozí.
        /// </summary>
        private void DisplayView(Result result)
        {
            result.Content = result.Content ?? GetDefaultView(result);
            DisplayView(result.Content);  
        }

        /// <summary>
        /// Nastaví template do ResourceDictionary a zobrazí v okně detailu
        /// </summary>
        private void DisplayView(Plugin.View view)
        {
            Application.Current.Resources.MergedDictionaries.Add(view.Template);
            ResultDetail = view.ViewModel;
        }

        /// <summary>
        /// Vytvoří defaultní view, které zobrazuje informace o pluginu (výsledku)
        /// </summary>
        /// <param name="result"></param>
        private Plugin.View GetDefaultView(Result result) => new Plugin.View()
        {
            Template = new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/Syrus;component/View/DefaultResultDetail.xaml")
            },
            ViewModel = new DefaultResultDetailViewModel(result)
        };

        private void TryOpenDetail(Result result, int countResults)
        {
            if (result.ResultConfiguration.OpenDetailMode.HasValue 
                && result.ResultConfiguration.OpenDetailMode.Value == OpenDetailMode.OnClick || !result.CanOpenDetail)
                return;
            OpenDetailMode openDetailMode = result.ResultConfiguration.OpenDetailMode.Value;
            if (openDetailMode == OpenDetailMode.Immediately 
                || (openDetailMode == OpenDetailMode.ImmediatelyWhenSingle && countResults == 1))
                DisplayView(result.Content);
        }
    }
}
