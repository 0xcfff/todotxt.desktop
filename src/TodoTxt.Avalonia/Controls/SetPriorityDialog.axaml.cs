using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace TodoTxt.Avalonia.Controls
{
    public partial class SetPriorityDialog : BaseDialog
    {
        public string Priority
        {
            get { return PriorityTextBox.Text?.Trim().ToUpper() ?? ""; }
            set { PriorityTextBox.Text = string.IsNullOrEmpty(value) ? "" : value.ToUpper(); }
        }

        public SetPriorityDialog()
        {
            InitializeComponent();
            PriorityTextBox.Focus();
        }

        private void PriorityTextBox_KeyUp(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnOkClick(this, new RoutedEventArgs());
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                OnCancelClick(this, new RoutedEventArgs());
                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                IncreasePriority();
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                DecreasePriority();
                e.Handled = true;
            }
        }

        private void IncreasePriority()
        {
            var currentPriority = Priority;
            if (string.IsNullOrEmpty(currentPriority))
            {
                Priority = "A";
            }
            else if (currentPriority[0] > 'A')
            {
                Priority = ((char)(currentPriority[0] - 1)).ToString();
            }
        }

        private void DecreasePriority()
        {
            var currentPriority = Priority;
            if (string.IsNullOrEmpty(currentPriority))
            {
                Priority = "Z";
            }
            else if (currentPriority[0] < 'Z')
            {
                Priority = ((char)(currentPriority[0] + 1)).ToString();
            }
        }
    }
}
