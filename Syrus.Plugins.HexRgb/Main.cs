using System.Collections.Generic;
using System.Threading.Tasks;
using Syrus.Plugin;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Linq;

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
                string rgb = HexToRgb(raw);
                string hsv = RgbToHsv(rgb);
                results.Add(new Result
                {
                    Text = $"RGB {rgb}",
                    Group = "Převod do RGB",
                    OnClick = (IAppApi api, Result currentResult) =>
                    {
                        Clipboard.SetText($"rgb({rgb})");
                    }
                });
                results.Add(new Result
                {
                    Text = hsv,
                    Group = "Převod do HSV",
                    OnClick = (IAppApi api, Result currentResult) =>
                    {
                        Clipboard.SetText(hsv);
                    }
                });
            }
            else
            {
                string color = RgbToHex(raw);
                string hsv = RgbToHsv(raw);
                results.Add(new Result
                {
                    Text = color,
                    Group = "Převod do HEX",
                    OnClick = (IAppApi api, Result currentResult) =>
                    {
                        Clipboard.SetText(color);
                    }
                });
                results.Add(new Result
                {
                    Text = hsv,
                    Group = "Převod do HSV",
                    OnClick = (IAppApi api, Result currentResult) =>
                    {
                        Clipboard.SetText(hsv);
                    }
                });
            }

            return Task.FromResult<IEnumerable<Result>>(results);
        }

        private string HexToRgb(string hex)
        {
            var color = ColorTranslator.FromHtml(hex);
            return $"{color.R}, {color.G}, {color.B}";
        }

        private string RgbToHex(string rgb)
        {
            Match match = Regex.Matches(rgb, _rgbRegex.ToString(), RegexOptions.IgnoreCase).First();
            var r = int.Parse(match.Groups[1].Value);
            var g = int.Parse(match.Groups[2].Value);
            var b = int.Parse(match.Groups[3].Value);
            var c = Color.FromArgb(255, r, g, b);
            return ColorTranslator.ToHtml(c);
        }

        private string RgbToHsv(string rgb)
        {
            Match match = Regex.Matches(rgb, _rgbRegex.ToString(), RegexOptions.IgnoreCase).First();
            var r = int.Parse(match.Groups[1].Value);
            var g = int.Parse(match.Groups[2].Value);
            var b = int.Parse(match.Groups[3].Value);
            Color color = Color.FromArgb(r, g, b);
            float hue = color.GetHue();
            float saturation = color.GetSaturation();
            float lightness = color.GetBrightness();
            return $"{hue}, {saturation}, {lightness}";
         }

    }
}
