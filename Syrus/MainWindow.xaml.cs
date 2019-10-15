using Syrus.Helpers;
using Syrus.Utils.Hotkeys;
using Syrus.ViewModel;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Syrus
{
    public partial class MainWindow
    {
        private HotkeyRegistrator _hotkeyRegistrator;

        private SearchingViewModel _searchingViewModel;

        /// <summary>
        /// Get true when window is opened and activated.
        /// </summary>
        public bool IsOpened { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Loaded += (s, e) => InitializePosition();
            _searchingViewModel = new SearchingViewModel();
            DataContext = _searchingViewModel;
            _searchingViewModel.OnSelectPlugin += 
                () => SearchPanel.SearchingInput.CaretIndex = SearchPanel.SearchingInput.Text.Length;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _hotkeyRegistrator = new HotkeyRegistrator(this);
            _hotkeyRegistrator.Add(Modifiers.Ctrl, Keys.Space, OpenWindowAction);
            _hotkeyRegistrator.Register();
            IsOpened = true;
        }

        private void OpenWindowAction()
        {
            if (!IsOpened)
            {
                WindowState = System.Windows.WindowState.Normal;
                Show();
                IsOpened = true;
            }
            else
            {
                Hide();
                IsOpened = false;
            }
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
        {
            WindowState = System.Windows.WindowState.Minimized;
            IsOpened = false;
        }

        private void AcrylicWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) => _searchingViewModel.OnCloseHandler();
    }
}
