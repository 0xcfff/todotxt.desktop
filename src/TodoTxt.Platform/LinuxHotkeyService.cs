using System;
using System.Collections.Generic;

namespace TodoTxt.Platform
{
    /// <summary>
    /// Linux-specific hotkey service implementation
    /// Note: This is a placeholder implementation. Full implementation would require
    /// Linux desktop environment APIs or a cross-platform library.
    /// </summary>
    public class LinuxHotkeyService : IHotkeyService
    {
#pragma warning disable CS0067 // Events are part of interface contract
        public event EventHandler<HotkeyPressedEventArgs>? HotkeyPressed;
#pragma warning restore CS0067

        public bool IsSupported => false; // TODO: Implement actual support

        public bool RegisterHotkey(Hotkey hotkey, int id)
        {
            // TODO: Implement Linux global hotkey registration using X11 or Wayland APIs
            return false;
        }

        public bool UnregisterHotkey(int id)
        {
            // TODO: Implement Linux hotkey unregistration
            return false;
        }

        public void UnregisterAllHotkeys()
        {
            // TODO: Implement unregistering all Linux hotkeys
        }

        public bool IsHotkeyAvailable(Hotkey hotkey)
        {
            // TODO: Implement checking if hotkey is available on Linux
            return false;
        }

        public IEnumerable<int> GetRegisteredHotkeys()
        {
            // TODO: Return list of registered hotkey IDs
            return Array.Empty<int>();
        }
    }
}
