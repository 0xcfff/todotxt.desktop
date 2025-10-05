using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace TodoTxt.Avalonia.Controls
{
    public partial class AppendTextDialog : BaseDialog
    {
        public string TextToAppend
        {
            get { return TextToAppendTextBox.Text?.Trim() ?? ""; }
            set { TextToAppendTextBox.Text = string.IsNullOrEmpty(value) ? "" : value; }
        }

        public AppendTextDialog()
        {
            InitializeComponent();
            TextToAppendTextBox.Focus();
        }

        private void TextToAppendTextBox_KeyUp(object? sender, KeyEventArgs e)
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
