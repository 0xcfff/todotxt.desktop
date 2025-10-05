using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using System;

namespace TodoTxt.Avalonia.Controls
{
    /// <summary>
    /// Base class for all dialogs in the Avalonia application
    /// Provides common functionality and styling for dialog windows
    /// </summary>
    public abstract class BaseDialog : Window
    {
        public BaseDialog()
        {
            // Common dialog properties
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ShowInTaskbar = false;
            CanResize = false;
            
            // Common styling
            this.Classes.Add("dialog");
        }

        /// <summary>
        /// Shows the dialog as modal and returns the result
        /// </summary>
        /// <returns>True if OK was clicked, false if Cancel was clicked</returns>
        public virtual async Task<bool?> ShowDialog()
        {
            // Find the main window to use as owner
            var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow
                : null;
            
            if (mainWindow != null)
            {
                return await base.ShowDialog<bool?>(mainWindow);
            }
            else
            {
                // If no main window is available, show as non-modal
                Show();
                return true; // Assume OK for non-modal dialogs
            }
        }

        /// <summary>
        /// Shows the dialog as modal and returns the result
        /// </summary>
        /// <param name="owner">The owner window</param>
        /// <returns>True if OK was clicked, false if Cancel was clicked</returns>
        public new virtual async Task<bool?> ShowDialog(Window? owner)
        {
            if (owner != null)
                Owner = owner;
            var fallbackOwner = owner ?? (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow as Window ?? null;
            if (fallbackOwner != null)
            {
                return await base.ShowDialog<bool?>(fallbackOwner);
            }
            
            // If no owner is available, show non-modally as a safe fallback
            Show();
            return true;
        }

        /// <summary>
        /// Handles the OK button click
        /// </summary>
        protected virtual void OnOkClick(object? sender, RoutedEventArgs e)
        {
            Close(true);
        }

        /// <summary>
        /// Handles the Cancel button click
        /// </summary>
        protected virtual void OnCancelClick(object? sender, RoutedEventArgs e)
        {
            Close(false);
        }

        /// <summary>
        /// Handles the Escape key to cancel the dialog
        /// </summary>
        protected override void OnKeyDown(global::Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == global::Avalonia.Input.Key.Escape)
            {
                OnCancelClick(this, new RoutedEventArgs());
                e.Handled = true;
            }
            else
            {
                base.OnKeyDown(e);
            }
        }
    }
}
