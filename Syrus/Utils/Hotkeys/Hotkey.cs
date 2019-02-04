using System;
using System.Windows.Forms;

namespace Syrus.Utils.Hotkeys
{
    public enum Modifiers
    {
        Ctrl = 0x0002,
        Alt = 0x0001,
        Shift = 0x0004,
        Win = 0x0008
    }

    public class HotKey
    {

        public int Id { get; set; }
        public uint Modifier { get; }
        public uint Key { get; }
        public Action Action { get; }

        public HotKey(int id, Modifiers modifier, Keys key, Action action)
        {
            Id = id;
            Modifier = (uint)modifier;
            Key = (uint)key;
            Action = action;
        }
    }
}
