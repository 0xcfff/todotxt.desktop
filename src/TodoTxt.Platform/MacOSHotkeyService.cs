using System;
using System.Collections.Generic;

namespace TodoTxt.Platform
{
    /// <summary>
    /// macOS-specific hotkey service implementation
    /// Note: This is a placeholder implementation. Full implementation would require
    /// native macOS APIs or a cross-platform library.
    /// </summary>
    public class MacOSHotkeyService : IHotkeyService
    {
#pragma warning disable CS0067 // Events are part of interface contract
        public event EventHandler<HotkeyPressedEventArgs>? HotkeyPressed;
#pragma warning restore CS0067

        public bool IsSupported => false; // TODO: Implement actual support

        public bool RegisterHotkey(Hotkey hotkey, int id)
        {
            // TODO: Implement macOS global hotkey registration using Carbon or Cocoa APIs
            // This would typically involve:
            // 1. Registering hotkeys with RegisterEventHotKey
            // 2. Setting up event handlers
            // 3. Managing hotkey lifecycle
            return false;
        }

        public bool UnregisterHotkey(int id)
        {
            // TODO: Implement macOS hotkey unregistration
            return false;
        }

        public void UnregisterAllHotkeys()
        {
            // TODO: Implement unregistering all macOS hotkeys
        }

        public bool IsHotkeyAvailable(Hotkey hotkey)
        {
            // TODO: Implement checking if hotkey is available on macOS
            return false;
        }

        public IEnumerable<int> GetRegisteredHotkeys()
        {
            // TODO: Return list of registered hotkey IDs
            return Array.Empty<int>();
        }
    }
}
