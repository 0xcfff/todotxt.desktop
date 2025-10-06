using System;

namespace TodoTxt.Avalonia.Services
{
    /// <summary>
    /// Unsupported tray service implementation that provides no functionality
    /// </summary>
    public class UnsupportedTrayService : ITrayService
    {
#pragma warning disable CS0067 // Events are part of interface contract
        public event EventHandler? TrayIconClicked;
        public event EventHandler? TrayIconDoubleClicked;
        public event EventHandler? TrayIconRightClicked;
#pragma warning restore CS0067

        public bool IsVisible { get; set; } = false;
        public string ToolTipText { get; set; } = string.Empty;

        public void Show()
        {
            // No-op on unsupported platforms
        }

        public void Hide()
        {
            // No-op on unsupported platforms
        }

        public void ShowBalloonTip(string title, string text, int timeout = 5000)
        {
            // No-op on unsupported platforms
        }

        public void Dispose()
        {
            // No-op on unsupported platforms
        }
    }
}
