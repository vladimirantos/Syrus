using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Syrus.Helpers
{
    internal static class DrawingHelper
    {

        /// <summary>
        /// Convert pixel to DIP value.
        /// </summary>
        public static Point TransformPixelsToDip(Visual visual, double unitX, double unitY)
        {
            Matrix matrix;
            var source = PresentationSource.FromVisual(visual);
            if (source != null)
            {
                matrix = source.CompositionTarget.TransformFromDevice;
            }
            else
            {
                using (var src = new HwndSource(new HwndSourceParameters()))
                {
                    matrix = src.CompositionTarget.TransformFromDevice;
                }
            }
            return new Point((int)(matrix.M11 * unitX), (int)(matrix.M22 * unitY));
        }
    }
}
