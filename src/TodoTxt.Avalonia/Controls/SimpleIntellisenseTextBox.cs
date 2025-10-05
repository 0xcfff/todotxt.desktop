using Avalonia;
using Avalonia.Controls;
using TodoTxt.Core;

namespace TodoTxt.Avalonia.Controls
{
    /// <summary>
    /// A simplified version of IntellisenseTextBox for debugging
    /// </summary>
    public class SimpleIntellisenseTextBox : TextBox
    {
        public static readonly StyledProperty<TaskList?> TaskListProperty =
            AvaloniaProperty.Register<SimpleIntellisenseTextBox, TaskList?>(nameof(TaskList));

        public TaskList? TaskList
        {
            get => GetValue(TaskListProperty);
            set => SetValue(TaskListProperty, value);
        }

        public static readonly StyledProperty<bool> CaseSensitiveProperty =
            AvaloniaProperty.Register<SimpleIntellisenseTextBox, bool>(nameof(CaseSensitive), false);

        public bool CaseSensitive
        {
            get => GetValue(CaseSensitiveProperty);
            set => SetValue(CaseSensitiveProperty, value);
        }

        public SimpleIntellisenseTextBox()
        {
            System.Diagnostics.Debug.WriteLine("SimpleIntellisenseTextBox constructor called");
            
            // Ensure the control is visible
            this.IsVisible = true;
            this.Opacity = 1.0;
            
            System.Diagnostics.Debug.WriteLine("SimpleIntellisenseTextBox constructor completed");
        }
    }
}
