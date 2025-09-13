using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace TodoTxt.Avalonia.Core.Controls
{
    public partial class SetDueDateDialog : BaseDialog
    {
        public DateTime? DueDate
        {
            get { return DueDatePicker.SelectedDate?.DateTime; }
            set { DueDatePicker.SelectedDate = value; }
        }

        public SetDueDateDialog()
        {
            InitializeComponent();
            DueDatePicker.Focus();
        }

        private void DueDatePicker_KeyUp(object? sender, KeyEventArgs e)
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
        }
    }
}
