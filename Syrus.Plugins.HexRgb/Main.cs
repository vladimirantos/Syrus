using System.Collections.Generic;
using System.Threading.Tasks;
using Syrus.Plugin;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Linq;
using System;

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
            List<Result> results = new List<Result>();
            if (!isRgb)
            {
                var color = ColorTranslator.FromHtml(raw);
                string rgb = HexToRgb(color);
                string hsv = RgbToHsv(color.R, color.G, color.B);

                results.Add(CreateResult($"RGB {rgb}", "Převod do RGB", (IAppApi api, Result currentResult) =>
                {
                    Clipboard.SetText($"rgb({rgb})");
                }));

                results.Add(CreateResult(hsv, "Převod do HSV", (IAppApi api, Result currentResult) =>
                {
                    Clipboard.SetText(hsv);
                }));
            }
            else
            {
                Match match = Regex.Matches(raw, _rgbRegex.ToString(), RegexOptions.IgnoreCase).First();
                var r = int.Parse(match.Groups[1].Value);
                var g = int.Parse(match.Groups[2].Value);
                var b = int.Parse(match.Groups[3].Value);
                string color = RgbToHex(r, g, b);
                string hsv = RgbToHsv(r, g, b);
                results.Add(CreateResult(color, "Převod do HEX", (IAppApi api, Result currentResult) =>
                {
                    Clipboard.SetText(color);
                }));
                results.Add(CreateResult(hsv, "Převod do HSV", (IAppApi api, Result currentResult) =>
                {
                    Clipboard.SetText(hsv);
                }));
            }

            return Task.FromResult<IEnumerable<Result>>(results);
        }

        private string HexToRgb(Color color) => $"{color.R}, {color.G}, {color.B}";

        private string RgbToHex(int r, int g, int b)
        {
            var c = Color.FromArgb(255, r, g, b);
            return ColorTranslator.ToHtml(c);
        }

        private string RgbToHsv(int r, int g, int b)
        {
            Color color = Color.FromArgb(r, g, b);
            float hue = color.GetHue();
            float saturation = color.GetSaturation();
            float lightness = color.GetBrightness();
            return $"{hue}, {saturation}, {lightness}";
         }


        private Result CreateResult(string text, string group, Action<IAppApi, Result> onClick)
            => new Result()
            {
                Text = text,
                Group = group,
                OnClick = onClick
            };
    }
}
