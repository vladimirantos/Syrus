using System.Collections.Generic;
using System.Threading.Tasks;
using Syrus.Plugin;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;

namespace Syrus.Plugins.HexRgb
{
    public class Main : IPlugin
    {
        private readonly Regex _rgbRegex = new Regex(@"(\d{1,3}),\s?(\d{1,3}),\s?(\d{1,3})");

        public void OnInitialize(PluginContext context)
        {
        }

        public Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            string raw = query.Original;
            var isRgb = _rgbRegex.IsMatch(raw);
            if ((!raw.StartsWith("#") || (raw.Length < 4)) && !isRgb)
                return Task.FromResult<IEnumerable<Result>>(new List<Result>());
            return Task.FromResult<IEnumerable<Result>>(!isRgb ? HexToRgb(raw) : RgbToHex(raw));
        }

        private List<Result> HexToRgb(string hex)
        {
            var color = ColorTranslator.FromHtml(hex);
            var colorString = $"rgb({color.R}, {color.G}, {color.B})";
            //CreateIcon(color);
            return new List<Result>
            {
                new Result
                {
                    Text = colorString,
                    Group = "Převod do RGB",
                    OnClick = (IAppApi api, Result currentResult) =>
                    {
                        Clipboard.SetText(colorString);
                    }
                    //Title = colorString,
                    //IcoPath = Path.Combine("images", _resultIcon),
                    //Action = ctx =>
                    //{
                    //    Clipboard.SetText(colorString);
                    //    return true;
                    //}
                }
            };
        }

        private List<Result> RgbToHex(string rgb)
        {
            foreach (Match match in Regex.Matches(rgb, _rgbRegex.ToString(), RegexOptions.IgnoreCase))
            {
                var r = int.Parse(match.Groups[1].Value);
                var g = int.Parse(match.Groups[2].Value);
                var b = int.Parse(match.Groups[3].Value);
                var c = Color.FromArgb(255, r, g, b);
                var color = ColorTranslator.ToHtml(c);
                //CreateIcon(c);

                return new List<Result>
                {
                    new Result
                    {
                        Text = color,
                        Group = "Převod do HEX",
                        OnClick = (IAppApi api, Result currentResult) =>
                        {
                            Clipboard.SetText(color);
                        }
                    }
                };
            }
            return new List<Result>();
        }

    }
}
