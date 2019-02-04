﻿using Syrus.Helpers;
using Syrus.Utils.Hotkeys;
using System;
using System.Windows.Forms;

namespace Syrus
{
    public partial class MainWindow
    {
        private HotkeyRegistrator _hotkeyRegistrator;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) => InitializePosition();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _hotkeyRegistrator = new HotkeyRegistrator(this);
            _hotkeyRegistrator.Add(Modifiers.Ctrl, Keys.Space, OpenWindowAction);
            _hotkeyRegistrator.Register();
        }

        private void OpenWindowAction()
        {
            WindowState = System.Windows.WindowState.Normal;
            Show();
        }

        private void InitializePosition()
        {
            Top = WindowTop();
            Left = WindowLeft();
        }

        /// <summary>
        /// Calculate center of screen from top
        /// </summary>
        private double WindowTop()
        {
            var screen = Screen.FromPoint(System.Windows.Forms.Cursor.Position);
            var dip1 = DrawingHelper.TransformPixelsToDip(this, 0, screen.WorkingArea.Y);
            var dip2 = DrawingHelper.TransformPixelsToDip(this, 0, screen.WorkingArea.Height);
            var top = (dip2.Y - ActualHeight) / 4 + dip1.Y;
            return top;
        }

        /// <summary>
        /// Calculate center of screen from left
        /// </summary>
        /// <returns></returns>
        private double WindowLeft()
        {
            var screen = Screen.FromPoint(System.Windows.Forms.Cursor.Position);
            var dip1 = DrawingHelper.TransformPixelsToDip(this, screen.WorkingArea.X, 0);
            var dip2 = DrawingHelper.TransformPixelsToDip(this, screen.WorkingArea.Width, 0);
            var left = (dip2.X - ActualWidth) / 2 + dip1.X;
            return left;
        }

        private void AcrylicWindow_Deactivated(object sender, System.EventArgs e)
            => WindowState = System.Windows.WindowState.Minimized;
    }
}
