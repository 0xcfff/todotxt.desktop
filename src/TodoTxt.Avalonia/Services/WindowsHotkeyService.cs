using System;
using System.Collections.Generic;

namespace TodoTxt.Avalonia.Services
{
    /// <summary>
    /// Windows-specific hotkey service implementation
    /// Note: This is a placeholder implementation. Full implementation would require
    /// Windows APIs or a cross-platform library.
    /// </summary>
    public class WindowsHotkeyService : IHotkeyService
    {
#pragma warning disable CS0067 // Events are part of interface contract
        public event EventHandler<HotkeyPressedEventArgs>? HotkeyPressed;
#pragma warning restore CS0067

        public bool IsSupported => false; // TODO: Implement actual support

        public bool RegisterHotkey(Hotkey hotkey, int id)
        {
            // TODO: Implement Windows global hotkey registration using RegisterHotKey
            return false;
        }

        public bool UnregisterHotkey(int id)
        {
            // TODO: Implement Windows hotkey unregistration using UnregisterHotKey
            return false;
        }

        public void UnregisterAllHotkeys()
        {
            // TODO: Implement unregistering all Windows hotkeys
        }

        public bool IsHotkeyAvailable(Hotkey hotkey)
        {
            // TODO: Implement checking if hotkey is available on Windows
            return false;
        }

        public IEnumerable<int> GetRegisteredHotkeys()
        {
            // TODO: Return list of registered hotkey IDs
            return Array.Empty<int>();
        }
    }
}
