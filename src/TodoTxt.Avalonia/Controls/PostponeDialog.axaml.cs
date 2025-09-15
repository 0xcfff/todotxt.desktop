using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace TodoTxt.Avalonia.Controls
{
    public partial class PostponeDialog : BaseDialog
    {
        public int DaysToPostpone
        {
            get 
            { 
                if (int.TryParse(PostponeTextBox.Text, out int days))
                    return days;
                return 0;
            }
            set { PostponeTextBox.Text = value.ToString(); }
        }

        public PostponeDialog()
        {
            InitializeComponent();
            PostponeTextBox.Focus();
        }

        private void PostponeTextBox_KeyUp(object? sender, KeyEventArgs e)
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
