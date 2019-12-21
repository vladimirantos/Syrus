using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Syrus.Plugin;

namespace Syrus.Plugins.Text
{
    internal enum TextOperations
    {
        Length = 1,
        UrlDecode = 2,
        UrlEncode = 3,
        Reverse = 4,
        Upper = 5,
        Lower = 6
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
                {TextOperations.UrlEncode, Encode },
                {TextOperations.Reverse, Reverse },
                {TextOperations.Upper, Upper },
                {TextOperations.Lower, Lower }
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
            return Task.FromResult<IEnumerable<Result>>(new List<Result>() { result });
        }

        private Result Length(string text)
            => new Result()
            {
                QuickResult = text.Length.ToString(),
                ResultConfiguration = new ResultConfiguration()
                {
                    ViewMode = ResultViewMode.Hidden
                }
            };

        private Result Decode(string text) => GetFullscreenResult(Uri.UnescapeDataString(text));

        private Result Encode(string text) => GetFullscreenResult(Uri.EscapeDataString(text));

        private Result Reverse(string text)
        {
            unsafe String ReverseStr(String s)
            {
                char[] sarr = new char[s.Length];
                fixed (char* c = s)
                fixed (char* d = sarr)
                {
                    char* c1 = c;
                    char* d1 = d + s.Length;
                    while (d1 > d)
                    {
                        *--d1 = *c1++;
                    }
                }

                return new String(sarr);
            }

            return GetFullscreenResult(ReverseStr(text));
        }

        private Result Lower(string text) => GetFullscreenResult(text.ToLower());

        private Result Upper(string text) => GetFullscreenResult(text.ToUpper());


        private Result GetFullscreenResult(string text) => new Result()
        {
            Text = text,
            ResultConfiguration = new ResultConfiguration()
            {
                ViewMode = ResultViewMode.Fullscreen
            },
            OnClick = (IAppApi api, Result result) => Clipboard.SetText(text)
        };
    }
}
