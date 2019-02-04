using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Syrus.Utils.Hotkeys
{
    public class HotkeyRegistrator
    {
        private HwndSource _source;
        private readonly WindowInteropHelper _windowInteropHelper;
        private const int HotkeyId = 9000;
        private const int WmHotkey = 0x0312;
        private List<HotKey> _hotKeys;

        public HotkeyRegistrator(Window window)
        {
            _windowInteropHelper = new WindowInteropHelper(window);
            _source = HwndSource.FromHwnd(_windowInteropHelper.Handle);
            _source?.AddHook(HwndHook);
            _hotKeys = new List<HotKey>();
        }

        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey([In] IntPtr hWnd, [In] int id, [In] uint fsModifiers, [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey([In] IntPtr hWnd, [In] int id);


        public void Add(Modifiers modifier, Keys key, Action action)
            => _hotKeys.Add(new HotKey(HotkeyId + _hotKeys.Count, modifier, key, action));

        public void Register()
        {
            foreach (var hotKey in _hotKeys)
            {
                if (!RegisterHotKey(_windowInteropHelper.Handle, hotKey.Id, hotKey.Modifier, hotKey.Key))
                {
                    throw new Exception("Cannot register hotkey");
                }
            }
        }

        public void UnRegisterAll()
        {
            _source.RemoveHook(HwndHook);
            _source = null;
            foreach (var hotKey in _hotKeys)
            {
                UnregisterHotKey(_windowInteropHelper.Handle, hotKey.Id);
            }
            _hotKeys = null;
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WmHotkey)
                _hotKeys.FirstOrDefault(x => x.Id == wParam.ToInt32())?.Action.Invoke();
            return IntPtr.Zero;
        }
    }
}
