using System;

namespace TodoTxt.Avalonia.Services
{
    /// <summary>
    /// Windows-specific tray service implementation
    /// This is a placeholder that will be replaced with actual Avalonia TrayIcon implementation
    /// </summary>
    public class WindowsTrayService : ITrayService
    {
#pragma warning disable CS0067 // Events are part of interface contract
        public event EventHandler? TrayIconClicked;
        public event EventHandler? TrayIconDoubleClicked;
        public event EventHandler? TrayIconRightClicked;
#pragma warning restore CS0067

        public bool IsVisible { get; private set; } = false;
        public string ToolTipText { get; set; } = string.Empty;

        public void Show()
        {
            // This will be implemented using Avalonia's TrayIcon in the main application
            IsVisible = true;
            System.Diagnostics.Debug.WriteLine("Windows tray icon shown (placeholder)");
        }

        public void Hide()
        {
            IsVisible = false;
            System.Diagnostics.Debug.WriteLine("Windows tray icon hidden (placeholder)");
        }

        public void ShowBalloonTip(string title, string text, int timeout = 5000)
        {
            // This will be implemented using Avalonia's notification system
            System.Diagnostics.Debug.WriteLine($"Windows notification: {title} - {text}");
        }

        public void Dispose()
        {
            Hide();
        }
    }
}
