using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FootballTracker.Database
{
    public class ColorWorker
    {
        private static ColorWorker instance;

        public static ColorWorker GetInstance()
        {
            if (instance == null)
            {
                instance = new ColorWorker();
            }
            return instance;
        }

        public (SolidColorBrush background, SolidColorBrush foreground1, SolidColorBrush foreground2) GetBannerColors(string imgSource)
        {
            SolidColorBrush background = new SolidColorBrush(Color.FromRgb(18, 104, 189)),
                foreground1 = new SolidColorBrush(Colors.White),
                foreground2 = new SolidColorBrush(Colors.White);
            if (imgSource != null)
            {
                try
                {
                    using (MagickImage image = new MagickImage(imgSource))
                    {
                        image.Scale(80, 80);
                        var dict = image.Histogram();
                        var colors = dict.OrderByDescending(x => x.Value).Select(x => x.Key).Take(4).ToArray();
                        background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[1].ToHexString()));
                        var color1 = AverageColor(colors.Where((x, i) => i % 2 == 0).ToArray());
                        var color2 = AverageColor(colors);
                        foreground1 = new SolidColorBrush(color1);
                        foreground2 = new SolidColorBrush(color2);
                        if (BrushesIsEqual(background, foreground1) || BrushesIsEqual(background, foreground2))
                        {
                            if(BrushesColorsIsEqual(background, foreground1) || BrushesColorsIsEqual(background, foreground2))
                            {
                                foreground1 = new SolidColorBrush(InverseColor(background.Color));
                                foreground2 = new SolidColorBrush(InverseColor(background.Color));
                            }
                            else
                            {
                                foreground1 = new SolidColorBrush(InverseColor(foreground1.Color));
                                foreground2 = new SolidColorBrush(InverseColor(foreground2.Color));
                            }
                        }
                    }
                }
                catch { }
            }
            return (background, foreground1, foreground2);
        }

        private Color AverageColor(params IMagickColor<ushort>[] colors)
        {
            var temp = colors.Select(x =>
            {
                int r = Convert.ToInt32(x.R),
                g = Convert.ToInt32(x.G),
                b = Convert.ToInt32(x.B);
                return new { r, g, b };
            });
            int sumR = 0, sumG = 0, sumB = 0, n = temp.Count();
            foreach (var elem in temp)
            {
                sumR += elem.r;
                sumG += elem.g;
                sumB += elem.b;
            }
            byte avgR = (byte)(sumR / n),
                 avgG = (byte)(sumG / n),
                 avgB = (byte)(sumB / n);
            return Color.FromRgb(avgR, avgG, avgB);
        }

        private Color InverseColor(Color color)
        {
            byte r = (byte)(color.R + 128),
                 g = (byte)(color.G + 128),
                 b = (byte)(color.B + 128);
            return Color.FromRgb(r, g, b);
        }

        private bool BrushesIsEqual(SolidColorBrush x, SolidColorBrush y)
        {
            var result = Math.Abs(x.Color.R - y.Color.R) <= 30 && Math.Abs(x.Color.G - y.Color.G) <= 30 && Math.Abs(x.Color.B - y.Color.B) <= 30;
            return result;
        }

        private bool BrushesColorsIsEqual(SolidColorBrush x, SolidColorBrush y)
        {
            var result = x.Color.R == x.Color.G && x.Color.G == x.Color.B && y.Color.R == y.Color.G && y.Color.G == y.Color.B;
            return result;
        }

        private ColorWorker() { }
    }
}
