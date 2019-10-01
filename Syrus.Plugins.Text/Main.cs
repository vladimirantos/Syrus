using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syrus.Plugin;

namespace Syrus.Plugins.Text
{
    internal enum TextOperations
    {
        Length = 1,
        UrlDecode = 2,
        UrlEncode = 3
    }

    public class Main : IPlugin
    {
        private PluginContext _context;

        private Dictionary<TextOperations, Func<string, Result>> _availableFunctions;

        public Main()
        {
            _availableFunctions = new Dictionary<TextOperations, Func<string, Result>>()
            {
                {TextOperations.Length, Length },
                {TextOperations.UrlDecode, Decode },
                {TextOperations.UrlEncode, Encode }
            };
        }

        public void OnInitialize(PluginContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            if (!query.HasArguments)
                return Task.FromResult<IEnumerable<Result>>(new List<Result>());
            Result result = _availableFunctions[(TextOperations)_context.Metadata.FromKeyword.Id].Invoke(query.Arguments);
            return Task.FromResult<IEnumerable<Result>>(new List<Result>() { result, result });
        }

        private Result Length(string text)
            => new Result()
            {
                QuickResult = text.Length.ToString(),
                ResultConfiguration = new ResultConfiguration()
                {
                    ViewMode = ResultsViewMode.Hide
                }
            };

        private Result Decode(string text) => new Result()
        {
            Text = Uri.UnescapeDataString(text),
            ResultConfiguration = new ResultConfiguration()
            {
                ViewMode = ResultsViewMode.Fullscreen
            }
        };

        private Result Encode(string text) => new Result()
        {
            Text = Uri.EscapeDataString(text),
            ResultConfiguration = new ResultConfiguration()
            {
                ViewMode = ResultsViewMode.Default
            }
        };
    }
}
