using System;
using System.Collections.Generic;
using System.Linq;

namespace Syrus.Plugin
{
    public interface IResultConfigurable
    {
        public ResultConfiguration ResultConfiguration { get; set; }
    }

    /**
     * Priorita ResultConfiguration (nejvyšší číslo má nejvyšší prioritu):
     * 1. Plugin
     * 2. SearchingConfiguration
     * 3. ConditionObject
     * 4. Result
     * 
     * Výsledký ResultConfiguration je kombinací všech nastavených dle těchto priorit.
     */
    public class PluginMetadataBase : IResultConfigurable
    {
        /// <summary>
        /// Plugin name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of DLL library
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Current version of this plugin
        /// </summary>
        public string Version { get; set; }

        public string Author { get; set; }

        /// <summary>
        /// If is <c>true</c>, it will be use for searching allways.
        /// </summary>
        public bool Default { get; set; }

        /// <summary>
        /// Results from default plugins are sorted by priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Local path to icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// It will be used when dark mode is activated.
        /// </summary>
        public string NightIcon { get; set; }

        /// <summary>
        /// If its <c>false</c>, help placeholder will be hidden.
        /// </summary>
        public bool EnableHelp { get; set; } = true;

        public bool EnableQueryHistory { get; set; } = false;

        /// <summary>
        /// Určuje jak často se bude volat metoda ISchedulable.UpdateAsync. Výchozí je 1 minuta.
        /// </summary>
        public double UpdateInterval { get; set; } = 60000;

        /// <summary>
        /// Pokud je true, bude se omezovat množství zobrazovaných výsledků. 
        /// Počet výsledků se vezme buď z settings.json
        /// </summary>
        public bool LimitedCountResult { get; set; } = true;

        /// <summary>
        /// Umožní zobrazovat plugin jako výsledek v případě, že nebyl nalezen žádný výsledek.
        /// </summary>
        public bool EnablePluginResult { get; set; } = true;

        public IEnumerable<SearchingConfiguration> SearchingConfigurations { get; set; }

        /// <summary>
        /// Constants, which are configurable
        /// </summary>
        public Dictionary<string, object> Constants { get; set; }

        /// <summary>
        /// Readonly constants, It's not configurable.
        /// </summary>
        public Dictionary<string, object> ReadonlyConstants { get; set; }

        public Caching Caching { get; set; }

        /// <summary>
        /// Konfigurace výsledků vyhledávání - globální nastavení pro celý plugin.
        /// </summary>
        public ResultConfiguration? ResultConfiguration { get; set; }
    }

    public class SearchingConfiguration : IResultConfigurable
    {
        public string Language { get; set; }
        public IEnumerable<ConditionObject> Keywords { get; set; }
        public IEnumerable<ConditionObject> RegularExpressions { get; set; }

        /// <summary>
        /// Konfigurace výsledků vyhledávání - scope SearchingConfiguration
        /// </summary>
        public ResultConfiguration? ResultConfiguration { get; set; }
    }

    public class Caching
    {
        public CacheSettings Queries { get; set; }
        public ResultsCacheSettings Results { get; set; }
    }

    public class CacheSettings
    {
        public int Duration { get; set; }
    }

    public class ResultsCacheSettings : CacheSettings
    {
        public ResultsCacheTypes Type { get; set; }
    }

    public enum ResultsCacheTypes
    {
        Selected, All
    }


    public class ConditionObject : IResultConfigurable
    {
        public int Id { get; set; }
        public string[] Text { get; set; }

        /// <summary>
        /// Nastavení výsledků pro konkrétní conditionObject.
        /// </summary>
        public ResultConfiguration? ResultConfiguration { get; set; }
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

    /// <summary>
    /// Režimy otevírání detailu
    /// </summary>
    public enum OpenDetailMode
    {
        /// <summary>
        /// Otevření na kliknutí. Defaultní hodnota
        /// </summary>
        OnClick,

        /// <summary>
        /// Detail bude otevřen pouze v případě, že plugin vrátil pouze jeden výsledek
        /// </summary>
        ImmediatelyWhenSingle,

        /// <summary>
        /// Otevře detail ihned po načtení, nerozhoduje počet výsledků. Otevřen bude vždy první.
        /// </summary>
        Immediately
    }

    /// <summary>
    /// Řeší jak se bude nastavovat grupování (nadpisy v seznamu výsledků)
    /// </summary>
    public enum GroupingMode
    {
        /// <summary>
        /// Nebude vůbec grupováno
        /// </summary>
        Disabled,

        /// <summary>
        /// Skupiny nastavují výsledky - defaultní hodnota
        /// </summary>
        FromResult,

        /// <summary>
        /// Jako nadpis skupiny bude použit název pluginu.
        /// </summary>
        PluginName,

        /// <summary>
        /// Hodnota group v resultConfiguration v plugin.json
        /// </summary>
        Specified,
    }

    /// <summary>
    /// Objekt pro nastavení výsledku vyhledávání.
    /// </summary>
    public class ResultConfiguration
    {
        /// <summary>
        /// Režim zobrazení výsledků
        /// </summary>
        public ResultViewMode? ViewMode { get; set; }

        public OpenDetailMode? OpenDetailMode { get; set; }

        public GroupingMode GroupingMode { get; set; }

        /// <summary>
        /// Nadpis skupiny výsledků. Aby byl aplikován, musí být nastaven GroupingMode.Specified.
        /// </summary>
        public string Group { get; set; }
    }
}
