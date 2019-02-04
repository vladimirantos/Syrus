using Syrus.Helpers;
using System;
using System.Windows;
using System.Windows.Forms;
namespace Syrus
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) => InitializePosition();
        }

        private void InitializePosition()
        {
            Top = WindowTop();
            Left = WindowLeft();
        }

        private double WindowTop()
        {
            var screen = Screen.FromPoint(System.Windows.Forms.Cursor.Position);
            var dip1 = DrawingHelper.TransformPixelsToDip(this, 0, screen.WorkingArea.Y);
            var dip2 = DrawingHelper.TransformPixelsToDip(this, 0, screen.WorkingArea.Height);
            var top = (dip2.Y - ActualHeight) / 4 + dip1.Y;
            return top;
        }

        private double WindowLeft()
        {
            var screen = Screen.FromPoint(System.Windows.Forms.Cursor.Position);
            var dip1 = DrawingHelper.TransformPixelsToDip(this, screen.WorkingArea.X, 0);
            var dip2 = DrawingHelper.TransformPixelsToDip(this, screen.WorkingArea.Width, 0);
            var left = (dip2.X - ActualWidth) / 2 + dip1.X;
            return left;
        }
    }
}
