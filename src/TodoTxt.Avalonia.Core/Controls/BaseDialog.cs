using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace TodoTxt.Avalonia.Core.Controls
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
            return await base.ShowDialog<bool?>(this);
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
            return await base.ShowDialog<bool?>(owner ?? this);
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
