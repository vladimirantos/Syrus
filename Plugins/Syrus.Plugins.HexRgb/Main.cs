using System.Collections.Generic;
using System.Threading.Tasks;
using Syrus.Plugin;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Linq;
using System;
using System.IO;

namespace Syrus.Plugins.HexRgb
{
    public class Main : IPlugin
    {
        private readonly Regex _rgbRegex = new Regex(@"(\d{1,3}),\s?(\d{1,3}),\s?(\d{1,3})");
        private PluginContext _context;
        private string _iconsLocation;
        public void OnInitialize(PluginContext context)
        {
            _context = context;
            _context.Cache.Clear("icons");
            _context.Cache.CreateFolder("icons");
        }

        public Task<IEnumerable<Result>> SearchAsync(Query query)
        {
            string raw = query.Original;
            var isRgb = _rgbRegex.IsMatch(raw);
            if ((!raw.StartsWith("#") || (raw.Length < 4)) && !isRgb)
                return Task.FromResult<IEnumerable<Result>>(new List<Result>());
            List<Result> results = new List<Result>();
            string iconName = Path.Combine(_context.Cache.Path, "icons", $"{Guid.NewGuid()}.png");
            if (!isRgb)
            {
                Color color = ColorFromHex(raw);
                CreateIcon(color, iconName);
                string rgb = HexToRgb(color);
                string hsv = RgbToHsv(color.R, color.G, color.B);

                results.Add(CreateResult($"RGB {rgb}", "Převod do RGB", iconName, (IAppApi api, Result currentResult) =>
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
                Color color = ColorFromRgb(r, g, b);
                CreateIcon(color, iconName);
                string c = RgbToHex(color);
                string hsv = RgbToHsv(r, g, b);
                results.Add(CreateResult(c, "Převod do HEX", iconName, (IAppApi api, Result currentResult) =>
                {
                    Clipboard.SetText(c);
                }));
                results.Add(CreateResult(hsv, "Převod do HSV", (IAppApi api, Result currentResult) =>
                {
                    Clipboard.SetText(hsv);
                }));
            }

            return Task.FromResult<IEnumerable<Result>>(results);
        }

        private string HexToRgb(Color color) => $"{color.R}, {color.G}, {color.B}";

        private string RgbToHex(Color c) => ColorTranslator.ToHtml(c);

        private string RgbToHsv(int r, int g, int b)
        {
            Color color = Color.FromArgb(r, g, b);
            float hue = color.GetHue();
            float saturation = color.GetSaturation();
            float lightness = color.GetBrightness();
            return $"{hue}, {saturation}, {lightness}";
         }


        private Result CreateResult(string text, string group, string icon, Action<IAppApi, Result> onClick)
            => new Result()
            {
                Text = text,
                Group = group,
                Icon = icon,
                OnClick = onClick
            };

        private Result CreateResult(string text, string group, Action<IAppApi, Result> onClick)
            => new Result()
            {
                Text = text,
                Group = group,
                OnClick = onClick
            };

        private void CreateIcon(Color c, string iconName)
        {
            using(Bitmap bmp = new Bitmap(48, 48))
            {
                using(Graphics g = Graphics.FromImage(bmp))
                {
                    using(Brush b = new SolidBrush(c))
                    {
                        g.FillEllipse(b, 0, 0, 48, 48);
                    }
                }
                bmp.Save(iconName);
            }
        }

        private Color ColorFromHex(string hex) => ColorTranslator.FromHtml(hex);
        private Color ColorFromRgb(int r, int g, int b) => Color.FromArgb(255, r, g, b);
    }
}
