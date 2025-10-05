using System;
using System.Collections.Generic;

namespace TodoTxt.Platform
{
    /// <summary>
    /// Unsupported hotkey service implementation that provides no functionality
    /// </summary>
    public class UnsupportedHotkeyService : IHotkeyService
    {
#pragma warning disable CS0067 // Events are part of interface contract
        public event EventHandler<HotkeyPressedEventArgs>? HotkeyPressed;
#pragma warning restore CS0067

        public bool IsSupported => false;

        public bool RegisterHotkey(Hotkey hotkey, int id)
        {
            return false;
        }

        public bool UnregisterHotkey(int id)
        {
            return false;
        }

        public void UnregisterAllHotkeys()
        {
            // No-op on unsupported platforms
        }

        public bool IsHotkeyAvailable(Hotkey hotkey)
        {
            return false;
        }

        public IEnumerable<int> GetRegisteredHotkeys()
        {
            return Array.Empty<int>();
        }
    }
}
