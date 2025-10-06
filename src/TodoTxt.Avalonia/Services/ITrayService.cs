using System;

namespace TodoTxt.Avalonia.Services
{
    /// <summary>
    /// Interface for system tray functionality across different platforms
    /// </summary>
    public interface ITrayService
    {
        /// <summary>
        /// Event fired when the tray icon is clicked
        /// </summary>
        event EventHandler? TrayIconClicked;

        /// <summary>
        /// Event fired when the tray icon is double-clicked
        /// </summary>
        event EventHandler? TrayIconDoubleClicked;

        /// <summary>
        /// Event fired when the tray icon is right-clicked
        /// </summary>
        event EventHandler? TrayIconRightClicked;

        /// <summary>
        /// Gets whether the tray icon is visible
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Gets or sets the tooltip text for the tray icon
        /// </summary>
        string ToolTipText { get; set; }

        /// <summary>
        /// Shows the tray icon
        /// </summary>
        void Show();

        /// <summary>
        /// Hides the tray icon
        /// </summary>
        void Hide();

        /// <summary>
        /// Shows a balloon tip notification
        /// </summary>
        /// <param name="title">The title of the notification</param>
        /// <param name="text">The text content of the notification</param>
        /// <param name="timeout">How long to show the notification (in milliseconds)</param>
        void ShowBalloonTip(string title, string text, int timeout = 5000);

        /// <summary>
        /// Disposes of the tray service and cleans up resources
        /// </summary>
        void Dispose();
    }
}
