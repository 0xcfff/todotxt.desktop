using System;
using System.Collections.Generic;

namespace TodoTxt.Platform
{
    /// <summary>
    /// Represents a hotkey combination
    /// </summary>
    public struct Hotkey
    {
        public bool Ctrl { get; set; }
        public bool Alt { get; set; }
        public bool Shift { get; set; }
        public bool Win { get; set; }
        public int KeyCode { get; set; }

        public Hotkey(bool ctrl, bool alt, bool shift, bool win, int keyCode)
        {
            Ctrl = ctrl;
            Alt = alt;
            Shift = shift;
            Win = win;
            KeyCode = keyCode;
        }

        public override string ToString()
        {
            var parts = new List<string>();
            if (Ctrl) parts.Add("Ctrl");
            if (Alt) parts.Add("Alt");
            if (Shift) parts.Add("Shift");
            if (Win) parts.Add("Win");
            parts.Add($"Key{KeyCode}");
            return string.Join("+", parts);
        }
    }

    /// <summary>
    /// Interface for global hotkey functionality across different platforms
    /// </summary>
    public interface IHotkeyService
    {
        /// <summary>
        /// Event fired when a registered hotkey is pressed
        /// </summary>
        event EventHandler<HotkeyPressedEventArgs>? HotkeyPressed;

        /// <summary>
        /// Gets whether the service is supported on the current platform
        /// </summary>
        bool IsSupported { get; }

        /// <summary>
        /// Registers a global hotkey
        /// </summary>
        /// <param name="hotkey">The hotkey combination to register</param>
        /// <param name="id">Unique identifier for the hotkey</param>
        /// <returns>True if registration was successful</returns>
        bool RegisterHotkey(Hotkey hotkey, int id);

        /// <summary>
        /// Unregisters a global hotkey
        /// </summary>
        /// <param name="id">The unique identifier of the hotkey to unregister</param>
        /// <returns>True if unregistration was successful</returns>
        bool UnregisterHotkey(int id);

        /// <summary>
        /// Unregisters all hotkeys
        /// </summary>
        void UnregisterAllHotkeys();

        /// <summary>
        /// Checks if a hotkey combination is available for registration
        /// </summary>
        /// <param name="hotkey">The hotkey combination to check</param>
        /// <returns>True if the hotkey is available</returns>
        bool IsHotkeyAvailable(Hotkey hotkey);

        /// <summary>
        /// Gets a list of currently registered hotkey IDs
        /// </summary>
        /// <returns>List of registered hotkey IDs</returns>
        IEnumerable<int> GetRegisteredHotkeys();
    }

    /// <summary>
    /// Event arguments for hotkey press events
    /// </summary>
    public class HotkeyPressedEventArgs : EventArgs
    {
        public int HotkeyId { get; }
        public Hotkey Hotkey { get; }

        public HotkeyPressedEventArgs(int hotkeyId, Hotkey hotkey)
        {
            HotkeyId = hotkeyId;
            Hotkey = hotkey;
        }
    }
}

